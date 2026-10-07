using UnityEngine;

public class TurnBaseManager : MonoBehaviour
{
    [Header("广播")]
    public ObjectEventSO playerTurnBegin;
    public ObjectEventSO enemyTurnBegin;
    public ObjectEventSO enemyTurnEnd;

    public GameObject playerObj;

    private bool isPlayerTurn = false;
    private bool isEnemyTurn = false;
    public bool batteleEnd = true;

    private float timeCounter;
    public float enemyTurnDuration;
    public float playerTurnDuration;
    
    private void Update()
    {
        if (batteleEnd)
            return;

        if (isEnemyTurn)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= enemyTurnDuration)
            {
                timeCounter = 0f;
                //敌人回合结束
                EnemyTurnEnd();
                //玩家回合开始                
                isPlayerTurn = true;
            }
        }
        if (isPlayerTurn)
        {
            timeCounter += Time.deltaTime;
            if(timeCounter >= playerTurnDuration)
            {
                timeCounter = 0f;                
                //玩家回合开始                
                PlayerTurnBegin();
                //改为false，否则会一直执行
                isPlayerTurn = false;
            }
        }
    }
    
    //监听：游戏开始
    public void GameStart()
    {
        isPlayerTurn=true;
        isEnemyTurn=false;
        batteleEnd=false;
        timeCounter= 0f;
    }

    public void PlayerTurnBegin()
    {
        playerTurnBegin.RaiseEvent(null, this);
    }

    //玩家回合结束也就是敌人回合开始
    public void EnemyTurnBegin()
    {
        isEnemyTurn = true;
        enemyTurnBegin.RaiseEvent(null, this);
    }
    public void EnemyTurnEnd()
    {
        isEnemyTurn = false;
        enemyTurnEnd.RaiseEvent(null, this);
    }   
    
    /// <summary>
    /// 监听：在房间加载完成后，加载人物以及让游戏开始
    /// </summary>
    /// <param name="obj"></param>
    public void OnRoomLoadedEvent(object obj)
    {
        Room room = (Room)obj;
        switch (room.roomData.roomType)
        {
            case RoomType.MinorEnemy:                
            case RoomType.EliteEnemy:              
            case RoomType.Boss:
                playerObj.SetActive(true);
                playerObj.GetComponent<Player>().buffRound.currentValue = 0;
                GameStart();
                break;
            case RoomType.Store:
                break;
            case RoomType.Treasure:
                break;
            case RoomType.RestRoom:
                playerObj.SetActive(true);
                //playerObj.GetComponent<PlayerAnimation>().PlaySleepAnim();
                break;
        }
    }

    /// <summary>
    /// 监听：返回地图
    /// </summary>
    public void StopTurnBaseEvent()
    {
        batteleEnd=true;
        playerObj.SetActive(false);
    }

    /// <summary>
    /// 监听：由于人物会被关闭，无法触发，所以在此处触发
    /// </summary>
    public void NewGame()
    {
        playerObj.GetComponent<Player>().NewGame();
    }
}
