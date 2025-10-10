#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class TransExtractor : EditorWindow
{
    // 스캔할 함수 이름들 
    private static string functionNamesCsv = "TransMgr.GetText";
    private static string SearchCodePath = "Assets/Scripts";
    private static string ColumnTable = "t_";
    private static string SearchTablePath = "Assets/Resources/Tables";
    private static string outputCsvPath = "Assets/Trans/Result/TransExtraction.csv";
    private static StringTable TransTable = null;
    
    private class Row
    {
        public string Key;        // key if Table
        public string Path;
    }

    [MenuItem("Tools/번역 추출(값만)")]
    private static void RunValue()
    {
        Run(false);
    }

    [MenuItem("Tools/번역 추출(위치 포함)")]
    private static void RunValueAndLocation()
    {
        Run(true);
    }

    private static void Run(bool isIncludePath)
    {
        var rows = new List<Row>();
        var allCollections = LocalizationEditorSettings.GetStringTableCollections();
        foreach (var row in allCollections)
        {
            foreach (var col in row.StringTables)
            {
                TransTable = col;
            }
        }
        
        HarvestCode(rows, isIncludePath);
        HarvestTables(rows, isIncludePath);
        
        WriteCsv(rows, outputCsvPath, isIncludePath);
        AssetDatabase.Refresh();
        EditorUtility.RevealInFinder(Path.GetFullPath(outputCsvPath));
        Debug.Log($"[TransExtractor] Done. {rows.Count} rows → {outputCsvPath}");
    }

    // --- 1) 코드에서 특정 함수 호출의 문자열 리터럴 수집 ---
    private static void HarvestCode(List<Row> rows, bool isIncludePath)
    {
        // 함수명 중 하나 + 여는 괄호 + 공백 무시 + "문자열"
        // 그룹1: 함수명, 그룹2: 따옴표 안 문자열
        // verbatim 문자열(@"...")는 기본 지원 X (필요시 개선)
        string pattern = $@"\b({functionNamesCsv})\s*\(\s*""((?:[^""\\]|\\.)*)""";
        var regex = new Regex(pattern);

        var csGuids = AssetDatabase.FindAssets("t:TextAsset", new[] { SearchCodePath })
                                   .Where(g => AssetDatabase.GUIDToAssetPath(g).EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                                   .ToArray();

        foreach (var guid in csGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            if (textAsset == null)
                continue;

            var src = textAsset.text;
            foreach (Match m in regex.Matches(src))
            {
                string func = m.Groups[1].Value;
                string literalEscaped = m.Groups[2].Value;

                // 이스케이프 해제
                string literal = UnescapeCSharpString(literalEscaped);

                // 라인 번호 계산
                int line = 1 + src.Take(m.Index).Count(ch => ch == '\n');

                var entry = TransTable.GetEntry(literal);

                if (entry == null)
                {
                    if (isIncludePath)
                    {
                        rows.Add(new Row
                        {
                            Key = literal,
                            Path = string.Format("{0} {1}", path, line),
                        });
                    }
                    else
                    {
                        rows.Add(new Row
                        {
                            Key = literal,
                        });
                    }

                }
            }
        }

        EditorUtility.ClearProgressBar();
    }

    // 간단한 C# 문자열 이스케이프 해제
    private static string UnescapeCSharpString(string s)
    {
        // 기본적인 \" \\ \n \t 만 처리
        return s.Replace("\\n", "\n")
                .Replace("\\t", "\t")
                .Replace("\\\"", "\"")
                .Replace("\\\\", "\\");
    }

    // --- 2) Localization String Tables 수집 ---
    private static void HarvestTables(List<Row> rows, bool isIncludePath)
    {
        var csGuids = AssetDatabase.FindAssets("t:TextAsset", new[] { SearchTablePath })
                           .Where(g => AssetDatabase.GUIDToAssetPath(g).EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                           .ToArray();

        Dictionary<string, List<string>> needExtract = new Dictionary<string, List<string>>();

        foreach (string key in csGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(key);
            path = path.Replace("Assets/Resources/", "");
            path = path.Replace(".csv", "");
            needExtract.Add(path, CSVReader.ReadForTrans(path, ColumnTable));
        }

        foreach (string key in needExtract.Keys)
        {
            foreach (string value in needExtract[key])
            {
                var entry = TransTable.GetEntry(value);

                if (entry == null)
                {
                    if (isIncludePath)
                    {
                        rows.Add(new Row
                        {
                            Key = value,
                            Path = key,
                        });
                    }
                    else
                    {
                        rows.Add(new Row
                        {
                            Key = value,
                        });
                    }
                }
            }
        }

        EditorUtility.ClearProgressBar();
    }

    private static void WriteCsv(List<Row> rows, string path, bool isIncludePath)
    {
        var sb = new StringBuilder();
        if (isIncludePath)
        {
            sb.AppendLine("Key,File");
        }

        string Esc(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";
            s = s.Replace("\"", "\"\"");
            if (s.Contains(",") || s.Contains("\n"))
                return $"\"{s}\"";
            return s;
        }

        foreach (var r in rows)
        {
            if (isIncludePath)
            {
                sb.AppendLine(string.Join(",",
                    Esc(r.Key),
                    Esc(r.Path)
                ));
            }
            else
            {
                sb.AppendLine(string.Join(",",
                    Esc(r.Key)
                ));
            }
        }

        // BOM 포함 UTF-8
        File.WriteAllText(path, sb.ToString(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
    }
}
#endif