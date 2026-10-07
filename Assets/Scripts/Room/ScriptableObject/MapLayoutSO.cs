using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于存储地图布局的数据
/// </summary>
[CreateAssetMenu(fileName = "MapLayoutSO",menuName = "Map/MapLayoutSO")]
public class MapLayoutSO : ScriptableObject
{
    public List<MapRoomData>mapRoomDataList=new ();
    public List<LinePosition>linePositionList=new ();
}

[System.Serializable]//序列化
public class MapRoomData
{    
    [HideInInspector]public string strRoomData;//序列化后的SO房间数据
    public float posX,posY;//房间位置的x和y
    public int column, line;//房间所在的行和列
    public RoomDataSO roomData;
    public RoomState roomState;
    //能够连线房间的行列信息
    //public List<Vector2Int> linkTo=new();
    public List<SerializeVector3> linkTo=new();

    public void Save()
    {
        strRoomData=JsonUtility.ToJson(roomData);        
    }

    #region 第一种方法
    //public void Load()
    //{
    //    //创建一个空的SO文件，并且是RoomDataSO类型，用于覆盖
    //    var newSO = ScriptableObject.CreateInstance<RoomDataSO>();

    //    //反序列化一个json文件，再把它覆盖给另一个object文件
    //    //参数一：json文件 参数二：Object文件
    //    JsonUtility.FromJsonOverwrite(strRoomData, newSO);
    //    roomData =newSO;
    //    //异步加载房间图片
    //    ResourceRequest rq = Resources.LoadAsync<Sprite>("Map Icons/"+roomData.roomType.ToString());
    //    rq.completed += LoadOver;
    //}

    //private void LoadOver(AsyncOperation operation)
    //{
    //    roomData.roomIcon=(operation as ResourceRequest).asset as Sprite;
    //}
    #endregion

    public void Load()
    {
        //创建一个空的SO文件，并且是RoomDataSO类型，用于覆盖
        var newSO = ScriptableObject.CreateInstance<RoomDataSO>();
        //反序列化
        JsonUtility.FromJsonOverwrite(strRoomData, newSO);
        //异步加载资源
        ResourceRequest rq=Resources.LoadAsync<RoomDataSO>("RoomData/"+newSO.roomType.ToString());
        rq.completed += LoadOver;        
    }

    private void LoadOver(AsyncOperation operation)
    {
        roomData=(operation as ResourceRequest).asset as RoomDataSO;
    }
}

[System.Serializable]//序列化
public class LinePosition
{
    //连线的起点和终点
    public SerializeVector3 startPos, endPos;
}
