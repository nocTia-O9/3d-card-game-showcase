using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{    
    [Header("广播")]
    public ObjectEventSO loadRoomEvent;

    public int column;//列
    public int line;//行
    private SpriteRenderer sp;
    public RoomDataSO roomData;
    public RoomState roomState;

    //能够连线房间的行列信息
    //public List<Vector2Int>linkTo=new();
    public List<SerializeVector3>linkTo=new();
    private void Awake()
    {
        sp = GetComponentInChildren<SpriteRenderer>();
    }
    
    private void OnMouseDown()
    {     
        if(roomState==RoomState.Attainable)
            loadRoomEvent.RaiseEvent(this, this);
    }

    /// <summary>
    /// 用于外部调用时创建房间
    /// </summary>
    /// <param name="column"></param>
    /// <param name="line"></param>
    /// <param name="roomData"></param>
    public void SetUpRoom(int column,int line,RoomDataSO roomData)
    {
        this.column = column;
        this.line = line;
        this.roomData = roomData;
        sp.sprite = roomData.roomIcon;
        
        sp.color = roomState switch
        {
            RoomState.Visited => new Color(0.5f, 0.8f, 0.5f, 0.5f),
            RoomState.Locked => new Color(0.5f, 0.5f, 0.5f, 1f),
            RoomState.Attainable => Color.white,
            _ => throw new System.NotImplementedException(),
        };
    }
}
