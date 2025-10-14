#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;

public class KoRangeExtractor : EditorWindow
{
    [MenuItem("Tools/사용하는 한글 범위 추출")]
    public static void ExtractHangulFromTables()
    {
        // Localization Settings에서 등록된 모든 테이블 컬렉션 가져오기
        var stringTableCollections = LocalizationEditorSettings.GetStringTableCollections();

        HashSet<char> hangulSet = new HashSet<char>();

        foreach (var collection in stringTableCollections)
        {
            // 각 Locale 테이블 확인
            foreach (var table in collection.StringTables)
            {
                foreach (var entry in table.SharedData.Entries)
                {
                    // 각 Key의 실제 번역 값(Localized String)
                    var localizedEntry = table.GetEntry(entry.Id);
                    if (localizedEntry == null)
                        continue;

                    string text = localizedEntry.LocalizedValue;
                    if (string.IsNullOrEmpty(text))
                        continue;

                    foreach (char c in text)
                    {
                        if (c >= 0xAC00 && c <= 0xD7A3) // 완성형 한글 범위
                            hangulSet.Add(c);

                        // 숫자, 알파벳, 기본 기호도 같이 넣고 싶다면:
                        if (char.IsLetterOrDigit(c) || char.IsPunctuation(c) || char.IsSymbol(c))
                            hangulSet.Add(c);
                    }
                }
            }
        }

        // 결과 정렬
        char[] result = hangulSet.OrderBy(c => c).ToArray();

        StringBuilder builder = new StringBuilder();
        
        foreach (var entry in result) {
            builder.Append(((int)entry).ToString("X4"));
            builder.Append(",");
        }
        
        // 결과 파일 저장
        string path = Path.Combine(Application.dataPath, "Trans/Result/Hangul_FromLocalization.txt");
        File.WriteAllText(path, builder.ToString(), Encoding.UTF8);

        Debug.Log($"한글 추출 완료! 총 {hangulSet.Count}자 → {path}");
        EditorUtility.RevealInFinder(path);
    }
}
#endif