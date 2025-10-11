using UnityEngine;

public interface IRoomTransition
{
    public void RoomLeftTransition();
    public void RoomRightTransition();

    public void RoomCeilingTransition();
    public void RoomSideReturnTransition();
}
