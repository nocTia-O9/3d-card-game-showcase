using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardManager : MonoBehaviour
{
    public PoolTool poolTool;
    public List<CardDataSO> cardDataList;//游戏中所有可能出现的卡牌

    [Header("卡牌库")]
    public CardLibrarySO newGameCardLibrary;
    public CardLibrarySO currentLibrary;    

    private int previousIndex;

    public void NewGameEvent()
    {
        InitializeCardDataList();

        currentLibrary.cardLibraryList.Clear();
        //每次游戏只初始化一次卡牌库，可根据需求调整
        foreach(var item in newGameCardLibrary.cardLibraryList)
        {
            currentLibrary.cardLibraryList.Add(item);
        }
    }
    private void OnDisable()
    {
        currentLibrary.cardLibraryList.Clear();
    }

    #region 初始化卡牌资源列表
    /// <summary>
    /// 初始化卡牌资源列表cardDataList，用代码就不需要手动拖了
    /// </summary>
    private void InitializeCardDataList()
    {
        //异步加载 找到所有Label为CardData的 CardDataSO类型资源
        //此处的第一个参数即为Label的关键字
        Addressables.LoadAssetsAsync<CardDataSO>("CardData",null).Completed += OnCardDataLoaded;
    }

    /// <summary>
    /// 回调函数
    /// </summary>
    /// <param name="handle"></param>
    private void OnCardDataLoaded(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if(handle.Status==AsyncOperationStatus.Succeeded)
        {
            cardDataList = new List<CardDataSO>(handle.Result);
        }
        else
        {
            Debug.LogError("No CardData Found!");
        }
    }
    #endregion

    //将从对象池获得和弃掉对象的方法做了封装
    //获得卡牌
    public GameObject GetCard()
    {
        //将获得的卡牌对象缩放设置为0，再返回
        var cardObj = poolTool.GetObjectFromPool();
        cardObj.transform.localScale = Vector3.zero;
        return cardObj;
    }

    //弃掉卡牌
    public void DiscardCard(GameObject cardObj)
    {
        poolTool.ReturnObjectToPool(cardObj);
    }

    public CardDataSO GetNewCardDataSO()
    {
        var randomIndex = 0;
        do
        {
            randomIndex = Random.Range(0, cardDataList.Count);
        } while (previousIndex == randomIndex);

        previousIndex = randomIndex;
        return cardDataList[randomIndex];
    }

    /// <summary>
    /// 解锁新卡牌
    /// </summary>
    /// <param name="newCardData"></param>
    public void UnlockCard(CardDataSO newCardData)
    {
        if(newCardData != null)
        {
            var newCard = new CardLibraryEntry { cardData = newCardData };

            //有数量添加bug，暂时直接添加卡牌
            currentLibrary.cardLibraryList.Add(newCard);
        }        
    }
}
