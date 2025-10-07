using UnityEngine;

public interface ITransition
{
    public void InTransition();
    public void OutTransition();

    public void FullTransition()
    {
        InTransition();
        OutTransition();
    }

    public void SceneLoadTransition(string sceneName);
}
