using System.Collections.Generic;
using System.IO;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class SaveManager : MonoBehaviour
{
    [System.Serializable]
    private class SaveEntry
    {
        public string key;
        public string json;
    }

    [System.Serializable]
    private class SaveFile
    {
        public List<SaveEntry> entries;
    }

    public static SaveManager Instance { get; private set; }

    public bool doSave = true;

    private readonly List<ISaveable> saveables = new();

    private string saveFileName = "save.dat";

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Register(ISaveable s)
    {
        if (!saveables.Contains(s)) saveables.Add(s);
    }

    public void Unregister(ISaveable s)
    {
        if (saveables.Contains(s)) saveables.Remove(s);
    }

    // 즉시 저장
    public void SaveAll()
    {
        if (!doSave) return;

        var file = new SaveFile();
        file.entries = new List<SaveEntry>();

        foreach (var s in saveables)
        {
            var json = s.Save() ?? string.Empty;
            file.entries.Add(new SaveEntry { key = s.SaveKey, json = json });
        }

        var plain = JsonUtility.ToJson(file);
        var encrypted = SecureStorage.EncryptToBase64(plain);

        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        File.WriteAllText(path, encrypted);
        Debug.Log($"SaveManager: saved ({path})");
    }

    // 로드
    public void LoadAll()
    {
        if (!doSave) return;

        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        if (!File.Exists(path))
        {
            Debug.Log("SaveManager: no save file found.");
            return;
        }

        var encrypted = File.ReadAllText(path);
        var plain = SecureStorage.DecryptFromBase64(encrypted);
        if (string.IsNullOrEmpty(plain))
        {
            Debug.LogWarning("SaveManager: failed to decrypt or empty save.");
            return;
        }

        var file = JsonUtility.FromJson<SaveFile>(plain);
        if (file == null || file.entries == null) return;

        // 매니저들에 데이터 전달
        var map = new Dictionary<string, string>();
        foreach (var e in file.entries) map[e.key] = e.json ?? string.Empty;

        foreach (var s in saveables)
        {
            if (map.TryGetValue(s.SaveKey, out var json))
                s.Load(json);
            else
                s.Load(string.Empty); // 데이터 없음 -> 매니저가 초기화 처리
        }

        Debug.Log("SaveManager: loaded.");
    }

    // 테스트용 삭제
    public void DeleteSave()
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        if (File.Exists(path)) File.Delete(path);
    }

    public bool HasSave()
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        
        return File.Exists(path);
    }
}
