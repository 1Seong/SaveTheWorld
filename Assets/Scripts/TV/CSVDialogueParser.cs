using System.Collections.Generic;
using UnityEngine;

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

            string[] cols = line.Split(',');

            // 컬럼 개수가 다르면 skip
            if (cols.Length < 6) continue;

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
}
