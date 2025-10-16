using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private ITransition[] transitions;
    private IRoomTransition[] roomTransitions;

    public enum TransitionType { Fade }; // IMPORTANT ; ordering should be matched with the gameObject children
    public enum RoomTransitionType { Fade };

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

    [SerializeField] private RoomTransitionType _rType = RoomTransitionType.Fade;
    public RoomTransitionType RType
    {
        get { return _rType; }
        set
        {
            switch (value)
            {
                case RoomTransitionType.Fade:
                    _rType = value;
                    roomTransition = roomTransitions[(int)value];
                    break;
            }
        }
    }

    private ITransition transition;
    private IRoomTransition roomTransition;

    private static SceneTransition _instance;
    public static SceneTransition Instance
    {
        get
        {
            if (_instance == null)
            {
                // ������ ã��
                _instance = FindFirstObjectByType<SceneTransition>();

                // ������ ���� ����
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

        transitions = GetComponentsInChildren<ITransition>();
        roomTransitions = GetComponentsInChildren<IRoomTransition>();

        Type = _type;
        RType = _rType;
    }

    public void LoadScene(string sceneName)
    {
        if(ItemManager.Instance != null)
        {
            if (sceneName == "MainScene")
            {
                ItemManager.Instance.gameObject.SetActive(true);
            }
            else
            {
                ItemManager.Instance.gameObject.SetActive(false);
                NoteManager.Instance.gameObject.SetActive(false);
            }
        }
        transition.SceneLoadTransition(sceneName);
    }

    public void UnloadScene()
    {
        Invoke("enableMainUI", 0.6f);
        transition.SceneUnloadTransition();
    }

    private void enableMainUI()
    {
        ItemManager.Instance.gameObject.SetActive(true);
        NoteManager.Instance.gameObject.SetActive(true);
    }

    public void RoomLeftTransition()
    {
        roomTransition.RoomLeftTransition();
    }

    public void RoomRightTransition()
    {
        roomTransition.RoomRightTransition();
    }

    public void RoomCeilingTransition()
    {
        roomTransition.RoomCeilingTransition();
    }

    public void RoomSideReturnTransition()
    {
        roomTransition.RoomSideReturnTransition();
    }

    public void ChangeTransition(TransitionType t)
    {
        Type = t;
    }
}
