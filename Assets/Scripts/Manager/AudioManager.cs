using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public void AfterRoomLoadedEvent(object data)
    {
        //进入房间中 默认关闭 状态面板        
        Room currentRoom = (Room)data;
        switch (currentRoom.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                GetComponent<AudioSource>().enabled=true;
                break;            
        }
    }
    public void LoadMap()
    {
        GetComponent<AudioSource>().enabled = false;
    }
}
