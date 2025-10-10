using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static Dictionary<string, Dictionary<string, string>> Read(string file)
    {
        var dic = new Dictionary<string, Dictionary<string, string>>();
        TextAsset data = Resources.Load(file) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return dic;

        var header = Regex.Split(lines[0], SPLIT_RE);

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, string>();
            string key = "";
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                if (j == 0)
                {
                    key = value;
                }
                else
                {
                    string finalvalue = value;
                    entry[header[j]] = finalvalue;
                }
            }
            dic.Add(key, entry);
        }

        return dic;
    }

    public static List<string> ReadForTrans(string file, string pre_fix)
    {
        var result = new List<string>();
        TextAsset data = Resources.Load(file) as TextAsset;
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return result;

        var header = Regex.Split(lines[0], SPLIT_RE);
        List<int> hasValueLine = new List<int>();

        for (int i = 0; i < header.Length; i++)
        {
            if (header[i].StartsWith(pre_fix))
            {
                hasValueLine.Add(i);
            }
        }

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            foreach (int j in hasValueLine)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                result.Add(value);
            }
        }

        return result;
    }
}
