# 데이터 기반 대사와 회상 연출

**담당자:** 최성원 — 대사 데이터 분리와 초상화·장면 연출 구현

## 코드

**CSVDialogueParser:** CSV를 발화자·초상화·대사 데이터로 변환합니다. **PortraitController:** 화자에 맞춰 좌우 초상화를 강조합니다. **TVManager:** 대사와 회상 장면을 재생합니다.

```csharp
// CSVDialogueParser.ParseCsvLine: 따옴표 안의 쉼표와 "" 이스케이프 구분
public static List<string> ParseCsvLine(string line)
{
    var result = new List<string>();
    var field = new System.Text.StringBuilder();
    bool quoted = false;
    for (int i = 0; i < line.Length; ++i)
    {
        char c = line[i];
        if (c == '"')
        {
            if (quoted && i + 1 < line.Length && line[i + 1] == '"')
            {
                // 따옴표 안의 연속된 두 따옴표는 구분자가 아니라 문자 하나다.
                field.Append('"');
                ++i;
            }
            else quoted = !quoted;
        }
        else if (c == ',' && !quoted)
        {
            result.Add(field.ToString());
            field.Clear();
        }
        else field.Append(c);
    }
    result.Add(field.ToString());
    return result;
}

// CSV 한 행을 발화자·초상화 경로·대사 데이터로 변환
private static DialogueData ReadDialogue(string line)
{
    var columns = ParseCsvLine(line);
    if (columns.Count < 5) throw new FormatException("대사 컬럼이 부족합니다.");
    return new DialogueData
    {
        id = int.Parse(columns[0].Trim()),
        leftCharacter = columns[1].Trim(),
        rightCharacter = columns[2].Trim(),
        isSpeakerLeft = columns[3].Trim() == "" ? (bool?)null
            : columns[3].Trim() == "1",
        // CSV에 문자로 적힌 \\n을 화면에서 사용할 실제 줄바꿈으로 바꾼다.
        dialogue = columns[4].Replace("\\n", "\n").Trim()
    };
}
```

```csharp
// TVManager.ShowDialogue: 데이터로 초상화와 대사를 함께 갱신
private void ShowDialogue(int sequenceIndex)
{
    var lines = sequences[sequenceIndex].dialogues;
    if (dialogueIndex >= lines.Count) return;
    var data = lines[dialogueIndex];
    portraitController.UpdatePortrait(
        data.isSpeakerLeft, data.leftCharacter, data.rightCharacter);
    StartCoroutine(TypeDialogue(data.dialogue, sequenceIndex));
}

// TVManager.dialogueCoroutine: 타이핑 스킵과 다음 대사 대기 분리
private IEnumerator TypeDialogue(string line, int sequenceIndex)
{
    dialogueText.text = "";
    skipPressed = false;
    for (int i = 0; i < line.Length && !skipPressed; ++i)
    {
        dialogueText.text += line[i];
        yield return new WaitForSeconds(textAppendTime);
    }
    dialogueText.text = line;
    // 타이핑 스킵 입력을 소비해 같은 입력이 다음 대사까지 넘기지 않게 한다.
    skipPressed = false;
    float elapsed = 0f;
    while (elapsed < nextTextWaitTime && !skipPressed)
    {
        elapsed += Time.deltaTime;
        yield return null;
    }
    NextDialogue(sequenceIndex);
}
```

## 생략 및 재구성한 내용

- 원본: `Assets/Scripts/TV/CSVDialogueParser.cs`, `PortraitController.cs`, `TVManager.cs`, `Assets/Scripts/EndingManager.cs`·`EndingManager2.cs`. 기준 커밋: `8a05e4251547a6c7cdecbcd1f97b84af051603eb`.
- CSV 필드 누적은 문자열 덧셈에서 StringBuilder로 바꾸고 행 매핑을 별도 함수로 분리했습니다. 헤더·빈 행 순회는 생략했습니다. 
- TVManager는 초상화 갱신과 스킵 가능한 타이핑만 발췌했습니다. 배경·음악 페이드, 다음 회상 전환, 완료 이벤트는 원본 NextDialogue가 담당합니다.
- 블록은 서로 다른 원본 클래스의 발췌입니다. DialogueData·시퀀스 데이터와 field, Unity·TMP·DOTween·코루틴 설정이 필요하며 독립 실행되지 않습니다.
