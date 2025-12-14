using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public class DialogueData
{
    public int id;
    public string leftCharacter;
    public string rightCharacter;
    public bool? isSpeakerLeft;
    public string dialogue;
}


public static class CSVDialogueParser
{
    public static List<DialogueData> Parse(TextAsset csvFile)
    {
        var result = new List<DialogueData>();

        if (csvFile == null)
        {
            Debug.LogError("CSV file is null");
            return result;
        }

        string[] lines = csvFile.text.Split('\n');

        // 첫 줄은 header니까 skip
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;
            var cols = ParseCsvLine(line);

            for (int j = 0; j < cols.Count; j++)
            {
                cols[j] = cols[j].Replace("\\n", "\n");
            }

            // 컬럼 개수가 다르면 skip
            if (cols.Count < 5) continue;

            var data = new DialogueData()
            {
                id = int.Parse(cols[0].Trim()),
                leftCharacter = cols[1].Trim(),
                rightCharacter = cols[2].Trim(),
                isSpeakerLeft = cols[3].Trim() == "" ? null : (cols[3].Trim() == "1" ? true : false),
                dialogue = cols[4].Trim()
            };

            result.Add(data);
        }

        return result;
    }

    public static List<string> ParseCsvLine(string line)
    {
        List<string> result = new List<string>();
        bool inQuotes = false;
        string current = "";

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                // "" → 내부 큰따옴표 하나
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current += '"';
                    i++; // 다음 " 건너뛰기
                }
                else
                {
                    // 큰따옴표 시작/종료
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                // 필드 종료
                result.Add(current);
                current = "";
            }
            else
            {
                current += c;
            }
        }

        // 마지막 필드 추가
        result.Add(current);

        return result;
    }
}
