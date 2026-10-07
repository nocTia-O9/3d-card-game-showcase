using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public bool isEnd;
    
    [Header("地图布局")]
    public MapLayoutSO mapLayout;

    [Header("广播")]
    public ObjectEventSO gameWinEvent;
    public ObjectEventSO gameOverEvent;

    public List<Enemy> aliveEnemyList=new ();
    
    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 监听：更新地图布局信息
    /// </summary>
    /// <param name="roomVector"></param>
    public void UpdateMapLayoutData(object value)//参数一定得是object
    {
        //因为Vector无法转化为object，所以改为传递SerializeVector3
        var roomVector = value as SerializeVector3;

        if (mapLayout.mapRoomDataList.Count <= 0)
            return;

        //注意这里的Room全部是MapLayoutSO里的MapRoomData对象，因为这个脚本修改的是地图布局信息
        //从MapRoomData列表中找到符号行列数据的对象并生成出来
        var currentRoom = mapLayout.mapRoomDataList.Find(r => r.column == roomVector.x &&
                                                         r.line == roomVector.y);        
        currentRoom.roomState = RoomState.Visited;        

        //更新同列房间的数据
        var sameColumnRooms = mapLayout.mapRoomDataList.FindAll(r => r.column == currentRoom.column);

        foreach (var room in sameColumnRooms)
        {
            //区分当前列进入和没进入的房间
            if (room.line != roomVector.y)
                room.roomState = RoomState.Locked;            
        }

        foreach(var link in currentRoom.linkTo)
        {
            var linkedRoom=mapLayout.mapRoomDataList.Find(r=>r.column==link.x&&r.line==link.y);
            linkedRoom.roomState = RoomState.Attainable;
        }    

        //走出房间后清出敌人列表
        aliveEnemyList.Clear();
    }

    /// <summary>
    /// 监听：填充敌人列表
    /// </summary>
    /// <param name="obj"></param>
    public void OnRoomLoadedEvent(object obj)
    {
        //参数一：是否包含未激活对象 参数二：是否排序
        var enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach(var enemy in enemies)
        {
            aliveEnemyList.Add(enemy);
        }        
    }

    /// <summary>
    /// 监听：战斗结束
    /// </summary>
    /// <param name="character"></param>
    public void OnCharacterDeadEvent(object character)
    {
        if(character is Player)
        {
            //发出失败通知
            StartCoroutine(EventDelayAction(gameOverEvent));
        }

        if (character is Boss)
        {
            isEnd = true;
            //发出失败通知
            StartCoroutine(EventDelayAction(gameOverEvent));
        }
        else if (character is Enemy)
        {            
            aliveEnemyList.Remove(character as Enemy);

            if (aliveEnemyList.Count == 0)
            {
                //发出获胜通知
                StartCoroutine(EventDelayAction(gameWinEvent));
            }
        }
        aliveEnemyList.Clear();
    }

    IEnumerator EventDelayAction(ObjectEventSO eventSO)
    {
        yield return new WaitForSeconds(1.5f);
        eventSO.RaiseEvent(null, this);
    }

    /// <summary>
    /// 监听：新的游戏开始清除房间数据
    /// </summary>
    public void OnNewGameEvent()
    {
        mapLayout.mapRoomDataList.Clear();
        mapLayout.linePositionList.Clear();
    }
}
