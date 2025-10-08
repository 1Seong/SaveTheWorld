using UnityEngine;

public interface ITransition
{
    public void InTransition();
    public void OutTransition();

    public void InLeftRoomTransition()
    {
        InTransition();
    }

    public void OutLeftRoomTransition()
    {
        OutTransition();
    }

    public void OutRightRoomTransition()
    {
        OutTransition();
    }

    public void InRightRoomTransition()
    {
        InTransition();
    }

    public void SceneLoadTransition(string sceneName);

    public void RoomLeftTransition();
    public void RoomRightTransition();
}
