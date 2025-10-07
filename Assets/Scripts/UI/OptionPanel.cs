using UnityEngine;

public class OptionPanel : MonoBehaviour
{
    private static OptionPanel _instance;
    public static OptionPanel Instance
    {
        get
        {
            if (_instance == null)
            {
                // ������ ã��
                _instance = FindFirstObjectByType<OptionPanel>();

                // ������ ���� ����
                if (_instance == null)
                {
                    GameObject singletonObj = new GameObject("OptionPanel");
                    _instance = singletonObj.AddComponent<OptionPanel>();
                    DontDestroyOnLoad(singletonObj);
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        // �ߺ� ����
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
