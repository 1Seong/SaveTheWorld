using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private ITransition[] transitions;

    public enum TransitionType { Fade }; // IMPORTANT ; ordering should be matched with the gameObject children

    [SerializeField] private TransitionType _type = TransitionType.Fade;
    public TransitionType Type
    {
        get { return _type; }
        set
        {
            switch(value)
            {
                case TransitionType.Fade:
                    _type = value;
                    transition = transitions[(int)value];
                    break;
            }
        }
    }

    private ITransition transition;

    private static SceneTransition _instance;
    public static SceneTransition Instance
    {
        get
        {
            if (_instance == null)
            {
                // 씬에서 찾기
                _instance = FindFirstObjectByType<SceneTransition>();

                // 없으면 새로 생성
                if (_instance == null)
                {
                    GameObject singletonObj = new GameObject("TransitionCanvasParent");
                    _instance = singletonObj.AddComponent<SceneTransition>();
                    DontDestroyOnLoad(singletonObj);
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        // 중복 방지
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        transitions = GetComponentsInChildren<ITransition>();
        Type = _type;
    }

    public void LoadScene(string sceneName)
    {
        transition.SceneLoadTransition(sceneName);
    }

    public void RoomLeftTransition()
    {
        transition.RoomLeftTransition();
    }

    public void RoomRightTransition()
    {
        transition.RoomRightTransition();
    }

    public void ChangeTransition(TransitionType t)
    {
        Type = t;
    }
}
