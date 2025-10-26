using UnityEngine;

public interface ITransition
{

    public void SceneLoadTransition(string sceneName, bool isAdditive);


    public void SceneUnloadTransition();
}
