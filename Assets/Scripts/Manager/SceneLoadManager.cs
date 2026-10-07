using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

/// <summary>
/// 该类的方法会添加到事件列表中来激活使用
/// </summary>
public class SceneLoadManager : MonoBehaviour
{
    [Header("广播")]
    public ObjectEventSO afterRoomLoadedEvent;
    public ObjectEventSO updateRoomEvent;
    public FadePanel fadePanel;

    private Room currentRoom;

    //当前场景
    private AssetReference currentScene;
    //地图场景
    public AssetReference map;
    public AssetReference menu;
    public AssetReference intro;
    public AssetReference end;

    public List<AssetReference> sceneList=new();
    private Dictionary<int, AssetReference> sceneDic=new();
    public int index;//房间序号

    private Vector2 currentRoomVector;

    //改为Awake？
    private void Awake()
    {
        for (int i = 0; i < sceneList.Count; i++)
        {
            sceneDic.Add(i, sceneList[i]);
        }        

        currentRoomVector = Vector2Int.one * -1;        
        LoadIntro();
    }    
    /// <summary>
    /// 在房间加载事件中监听
    /// </summary>
    /// <param name="data"></param>
    public async void OnLoadRoomEvent(object data)
    {
        if(data is Room)
        {
            currentRoom=data as Room;
            currentRoomVector =new(currentRoom.column,currentRoom.line);
            var currentData = currentRoom.roomData;          
            currentScene=currentData.sceneToLoad;             
        }

        //卸载房间
        await UnloadSceneTask();

        //加载房间
        //类似启动协程
        await LoadSceneTask();
          
        //场景加载完成后，做什么事情
        afterRoomLoadedEvent.RaiseEvent(currentRoom, this);
    }

    /// <summary>
    /// 异步操作加载场景
    /// </summary>
    /// <returns></returns>
    private async Awaitable LoadSceneTask()
    {
        //Awaitable可以返回所有异步操作的方法
        //s的类型为AsyncOperationHandle
        var s = currentScene.LoadSceneAsync(LoadSceneMode.Additive);
        //用await返回等待的内容，类似协程中的yield return
        await s.Task;

        //如果场景加载成功了
        if (s.Status == AsyncOperationStatus.Succeeded)
        {
            fadePanel.FadeOut(0.2f);
            //将加载的场景设置为激活状态
            SceneManager.SetActiveScene(s.Result.Scene);
        }
    }

    /// <summary>
    /// 卸载场景
    /// </summary>
    /// <returns></returns>
    private async Awaitable UnloadSceneTask()
    {
        fadePanel.FadeIn(0.4f);

        await Awaitable.WaitForSecondsAsync(0.45f);
        //卸载当前激活的场景
        await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    /// <summary>
    /// 监听：加载地图
    /// </summary>
    public async void LoadMap()
    {
        await UnloadSceneTask();

        //如果不等于这个结果，代表有房间，有房间则代表不是从主菜单回到地图
        if (currentRoomVector != Vector2Int.one * -1)
        {
            //因为Vector无法转化为object，所以改为传递SerializeVector3
            updateRoomEvent.RaiseEvent(new SerializeVector3(currentRoomVector), this);
        }
        currentScene = map;
        await LoadSceneTask();
    }

    public async void LoadMenu()
    {
        if(currentScene != null)
            await UnloadSceneTask();
        
        currentScene = menu;
        await LoadSceneTask();
    }

    public async void LoadIntro()
    {
        if (currentScene != null)
            await UnloadSceneTask();

        currentScene = intro;
        await LoadSceneTask();
    }
    
    public async void LoadEnd()
    {
        if (currentScene != null)
            await UnloadSceneTask();

        currentScene = end;
        await LoadSceneTask();
    }
}
