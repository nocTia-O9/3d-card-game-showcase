using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{    
    [Header("面板")]
    public GameObject gamePlayerPanel;
    public GameObject gameWinPanel;
    public GameObject gameOverPanel;
    public GameObject pickCardPanel;
    public GameObject restRoomPanel;    
    public GameObject statePanel;    
    public GameObject storePanel;    
    public GameObject cuePanel;   
    
    #region 监听
    public void AfterRoomLoadedEvent(object data)
    {        
        //进入房间中 默认关闭 状态面板
        statePanel.SetActive(false);
        Room currentRoom = (Room)data;        
        switch (currentRoom.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                gamePlayerPanel.SetActive(true);                
                break;
            case RoomType.RestRoom:
                restRoomPanel.SetActive(true);                
                break;
            case RoomType.Store:
                statePanel.SetActive(true);
                storePanel.SetActive(true);
                cuePanel.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// 监听：加载地图或加载菜单，关闭所有面板
    /// </summary>
    public void HideAllPanels()
    {
        gamePlayerPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        restRoomPanel.SetActive(false);
        pickCardPanel.SetActive(false);
        statePanel.SetActive(false);
        storePanel.SetActive(false);
        cuePanel.SetActive(false);
    }

    //在地图中显示 状态面板
    public void LoadMap()
    {
        gamePlayerPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        restRoomPanel.SetActive(false);
        pickCardPanel.SetActive(false);
        storePanel.SetActive(false);
        cuePanel.SetActive(false);

        StartCoroutine(DelayStatePanelEmerge());
    }    
    IEnumerator DelayStatePanelEmerge()
    {
        yield return new WaitForSeconds(0.5f);
        statePanel.SetActive(true);
    }

    public void OnGameWinEvent()
    {
        gamePlayerPanel.SetActive(false);
        gameWinPanel.SetActive(true);
        //游戏胜利时 打开状态面板
        statePanel.SetActive(true);
    }

    public void OnGameOverEvent()
    {
        gamePlayerPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void OnPickCardEvent()
    {
        pickCardPanel.SetActive(true);
    }

    public void OnFinishPickCardEvent()
    {
        pickCardPanel.SetActive(false);
    }
    #endregion
}
