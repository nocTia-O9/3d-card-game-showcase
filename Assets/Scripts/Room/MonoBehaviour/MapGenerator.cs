using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("地图配置表")]
    public MapConfigSO mapConfig;
    [Header("地图布局")]
    public MapLayoutSO mapLayout;
    [Header("预制体")]    
    public Room roomPrefab;//房间预制体
    public LineRenderer linePrefab;

    //屏幕的宽高
    private float screenHeight;
    private float screenWidth;
    //房间宽度
    private float columnWidth;
    //生成位置
    private Vector3 generatePoint;
    //边界
    public float border;

    //统一管理生成的房间
    private List<Room> rooms=new();//省略写法
    //统一管理生成的连线
    private List<LineRenderer> lines=new();

    public List<RoomDataSO>roomDataList=new();
    private Dictionary<RoomType,RoomDataSO> roomDataDic = new();

    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        //用高度乘宽高比得到宽度
        screenWidth = screenHeight * Camera.main.aspect;

        //房间宽度=屏幕宽度/(地图中房间列数+1)
        //可以多加1来空出一些宽度来进行位置的调整
        //columnWidth = screenWidth / (mapConfig.roomBlueprints.Count + 1);
        columnWidth = screenWidth / (mapConfig.roomBlueprints.Count);

        //将列表中的房间数据存到字典中
        foreach(var roomData in roomDataList)
        {
            roomDataDic.Add(roomData.roomType, roomData);
        }
    }
    
    private void OnEnable()
    {
        //如果布局数据中的房间数量大于0，说明有保存好的数据，可以加载地图布局
        if(mapLayout.mapRoomDataList.Count > 0)
        {
            LoadMap();
        }
        //否则就创建地图
        else
        {
            CreatMap();
        }
    }
    
    public void CreatMap()
    {
        //上一列
        List<Room> previousColumnRooms = new();        

        //以列为单位，一列一列地生成房间
        for (int column = 0; column< mapConfig.roomBlueprints.Count; column++)
        {
            //首先得到某一列的数据
            var bluePrint = mapConfig.roomBlueprints[column];
            //再求出需要生成的房间数量
            var amount =UnityEngine.Random.Range(bluePrint.min, bluePrint.max+1);

            //第一个房间的位置大概在画面左上角部分
            //画面的中心位置为原点，所以
            //x值为屏幕宽度的一半，由于是左半边所以取负，然后再加上一个边界值进行偏移
            //y值为屏幕高度的一半，由于是上半边所以为正，然后用该值减去屏幕高度除以（每列房间数量 + 1）的结果即可，
            //可以看作是减去房间高度。注：此处如果不加1，则会导致Boss房显示在屏幕画面最下面

            //可以看作是房间高度：屏幕高度/(每列房间数量+1)
            //此处如果amount不加1，则会导致Boss房显示在屏幕最下面
            //初始高度
            var startHeight = screenHeight / 2 - screenHeight / (amount + 1);                            
            generatePoint = new Vector3(-screenWidth / 2 + border + columnWidth * column, startHeight, 0);
            var newPosition = generatePoint;

            //当前列
            List<Room>currentColumnRooms= new ();           

            //每列房间的行间距
            var roomGapY = screenHeight / (amount + 1);

            //循环当前列中的所有房间数量生成房间
            for (int i = 0; i < amount; i++)
            {
                //让每列的房间都左右偏移一点距离
                //如果是最后一列，让其在一个固定位置
                if (column == mapConfig.roomBlueprints.Count - 1)
                {
                    //屏幕最右边减一个距离
                    newPosition.x = screenWidth / 2 - border * 2;
                }
                //如果不是第一列也不是最后一列
                else if (column != 0)
                {
                    newPosition.x = generatePoint.x + UnityEngine.Random.Range(-border/3, (border)/3);
                }

                //第一个房间不需要再向下平移
                newPosition.y=startHeight- roomGapY*i;

                //生成房间：
                //最后一个参数代表生成的物体以该物体为父对象
                var room = Instantiate(roomPrefab,newPosition,Quaternion.identity ,transform);
                //获得一个随机房间类型
                RoomType tempRoomType = GetRandomRoomType(bluePrint.roomType);

                //设置只能从第0列来进入其它房间
                if (column == 0)
                {
                    room.roomState = RoomState.Attainable;
                }
                else
                {
                    room.roomState = RoomState.Locked;
                }
                //初始化生成的房间
                room.SetUpRoom(column, i, GetRoomData(tempRoomType));


                //添加到列表中
                rooms.Add(room); 
                //添加到当前列表中                
                currentColumnRooms.Add(room);
            }
            //判断当前列是否为第一列，如果不是则连接到上一列
            if (previousColumnRooms.Count > 0)
            {
                //为两个列表的房间创建联系
                CreateConnections(previousColumnRooms, currentColumnRooms);                
            }

            previousColumnRooms = currentColumnRooms;
        }

        //生成完所有数据后，保存地图布局
        SaveMap();
    }

    //重新生成地图
    //右键脚本可以找到该方法
    [ContextMenu("ReGenerateRoom")]
    public void ReGenerateRoom()
    {
        foreach (var room in rooms)
        {
            Destroy(room.gameObject);
        }
        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }
        rooms.Clear();
        lines.Clear();
        CreatMap();
    }

    #region 创建连线
    /// <summary>
    /// 建立两列房间之间的联系
    /// </summary>
    /// <param name="column1"></param>
    /// <param name="column2"></param>
    private void CreateConnections(List<Room> column1, List<Room> column2)
    {        
        //HashSet不存储重复项，可以用来存储已经连线的房间
        HashSet<Room>connectedColumn2Rooms=new HashSet<Room>();

        foreach (Room room in column1)
        {          
            //正向连接传true
            var targetRoom = ConnectToRandomRoom(room, column2,true);
            connectedColumn2Rooms.Add(targetRoom);
        }
        foreach (Room room in column2)
        {
            //如果有没连线的，反向与第一列的房间连线
            if (!connectedColumn2Rooms.Contains(room))
            {
                //反向连接传false
                ConnectToRandomRoom(room, column1,false);
            }
        }
    }
    /// <summary>
    /// 创建连线
    /// </summary>
    /// <param name="room"></param>
    /// <param name="column2"></param>
    /// <returns></returns>
    private Room ConnectToRandomRoom(Room room,List<Room> column2,bool check)
    {
        Room targetRoom;

        //从第二列中随机出一个房间用于和第一列中的房间连线
        targetRoom = column2[UnityEngine.Random.Range(0,column2.Count)];

        //如果是正向连接，即从左向右连接
        if (check)
        {
            room.linkTo.Add(new SerializeVector3(new Vector3(targetRoom.column, targetRoom.line, 0)));
        }
        //否则为反向连接
        else
        {
            targetRoom.linkTo.Add(new SerializeVector3(new Vector3(room.column, room.line, 0)));
        }

        //创建房间之间的连线
        var line = Instantiate(linePrefab, transform);
        //设置第一个点和第二个点的位置
        line.SetPosition(0,room.transform.position);
        line.SetPosition(1,targetRoom.transform.position);  
        lines.Add(line);
        return targetRoom;
    }
    #endregion

    #region 生成房间
    /// <summary>
    /// 根据房间类型返回房间数据
    /// </summary>
    /// <param name="roomType"></param>
    /// <returns></returns>
    private RoomDataSO GetRoomData(RoomType roomType)
    {
        return roomDataDic[roomType];
    }

    /// <summary>
    /// 得到一个随机房间类型
    /// </summary>
    /// <param name="flags"></param>
    /// <returns></returns>
    private RoomType GetRandomRoomType(RoomType flags)
    {
        //因为得到的类型是混合型的并且是用逗号隔开的，所以可以将得到的类型拆分
        string[]options=flags.ToString().Split(',');
        //得到一个随机类型
        string randomOption = options[UnityEngine.Random.Range(0, options.Length)];
        //由于将字符串转为枚举需要引用System命名空间的方法，这会导致两个命名空间的Random引用不明确
        //所以需要加上命名空间进行单独声明
        RoomType roomType=(RoomType)Enum.Parse(typeof(RoomType),randomOption);
        return roomType;
    }
    #endregion

    #region 地图布局
    //保存地图布局
    public void SaveMap()
    {
        //添加所有已经生成的房间
        //遍历rooms列表，通过里面的数据生成MapRoomData对象，再添加到MapRoomData列表中，实现保存
        mapLayout.mapRoomDataList = new();
        for(int i = 0;i<rooms.Count;i++)
        {
            var room = new MapRoomData
            {
                posX = rooms[i].transform.position.x,
                posY = rooms[i].transform.position.y,
                column = rooms[i].column,
                line= rooms[i].line,
                roomData = rooms[i].roomData,
                roomState = rooms[i].roomState,                
                linkTo = rooms[i].linkTo
            };

            mapLayout.mapRoomDataList.Add(room);
        }

        //添加所有连线，原理同保存房间
        mapLayout.linePositionList = new();
        for (int i = 0; i < lines.Count; i++)
        {
            var line = new LinePosition
            {
                startPos = new SerializeVector3(lines[i].GetPosition(0)),
                endPos = new SerializeVector3(lines[i].GetPosition(1))
            };

            mapLayout.linePositionList.Add(line);
        }
    }

    //读取地图布局
    public void LoadMap()
    {
        //读取房间数据生成房间
        //遍历MapRoomData列表，通过里面的数据生成room对象，再添加到rooms列表中，实现读取
        for (int i = 0;i<mapLayout.mapRoomDataList.Count;i++)
        {
            var indexRoom = mapLayout.mapRoomDataList[i];//读取出来的地图房间数据
            var newPos = new Vector3(indexRoom.posX, indexRoom.posY, 0);
            var room=Instantiate(roomPrefab,newPos,Quaternion.identity,transform);
            //一定先设置状态再初始化房间
            room.roomState = indexRoom.roomState;
            room.SetUpRoom(indexRoom.column, indexRoom.line, indexRoom.roomData);
            room.linkTo = indexRoom.linkTo;

            rooms.Add(room);
        }

        //读取连线数据生成连线，原理同读取房间
        for (int i = 0; i < mapLayout.linePositionList.Count; i++)
        {
            var line = Instantiate(linePrefab, transform);
            line.SetPosition(0, mapLayout.linePositionList[i].startPos.ToVector3());
            line.SetPosition(1, mapLayout.linePositionList[i].endPos.ToVector3());

            lines.Add(line);
        }
    }
    #endregion
}
