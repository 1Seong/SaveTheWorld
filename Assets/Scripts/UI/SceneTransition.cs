using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private ITransition transition;

    private void Awake()
    {
        // init transition
    }

    public void LoadScene(string sceneName)
    {
        transition.SceneLoadTransition(sceneName);
    }
}
