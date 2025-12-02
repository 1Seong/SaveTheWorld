
public interface ISaveable
{
    // 이 매니저 고유 키 (SaveManager가 데이터 매핑에 사용)
    string SaveKey { get; }

    // 현재 상태를 JSON(string)으로 반환 (직렬화는 각 매니저가 책임)
    string Save();

    // Load용 JSON(string)을 받아 상태 복원
    void Load(string json);
}
