using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class CardDeck : MonoBehaviour
{
    [Header("广播")]    
    public IntEventSO drawAmountChangedEvent;
    public IntEventSO discardAmountChangedEvent;

    public CardManager cardManager;

    //抽牌堆和弃牌堆不需要展示真实的卡牌，所以列表类型为卡牌数据CardDataSO
    //而手牌区必须展示真实的卡牌，所以列表类型为Card
    private List<CardDataSO> drawDeck = new();//抽牌堆
    private List<CardDataSO> discardDeck = new();//弃牌堆
    private List<Card>handCardObjectList = new();//当前手牌(每回合)

    public Vector3 deckPosition;//卡牌的初始位置

    public CardLayoutManager cardLayoutManager;    

    //private void Start()
    //{
    //    InitializeDeck();        
    //}

    /// <summary>
    /// 监听：每次进入房间 初始化抽牌堆
    /// </summary>
    public void InitializeDeck()
    {
        drawDeck.Clear();
        //entry中包含每种卡牌的Data和数量
        foreach(var entry in cardManager.currentLibrary.cardLibraryList)
        {
            drawDeck.Add(entry.cardData);
            //遍历数量，再把卡牌装到抽牌堆里
            //for(int i = 0; i < entry.amount; i++)
            //{
            //    drawDeck.Add(entry.cardData);
            //}
        }

        //洗牌
        ShuffleDeck();
    }
    
    //监听：新回合抽卡
    public void NewTurnDrawCard()
    {
        DrawCard(4);
    }

    /// <summary>
    /// 抽卡
    /// </summary>
    /// <param name="amount"></param>
    public void DrawCard(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            if (drawDeck.Count == 0)
            {
                foreach (var item in discardDeck)
                {
                    drawDeck.Add(item);
                }
                ShuffleDeck();
            }

            //抽取 抽牌堆最上面的 一张牌(的数据)
            CardDataSO currentCardDataSO = drawDeck[0];            
            //抽完 从抽牌堆 移除
            drawDeck.RemoveAt(0);

            //更新UI显示
            drawAmountChangedEvent.RaiseEvent(drawDeck.Count, this);

            //从对象池生成一张卡牌(生成一张白板卡)
            var card=cardManager.GetCard().GetComponent<Card>();
            //把抽取的卡牌数据 赋值给 生成的卡牌 让其初始化(印卡)
            card.Init(currentCardDataSO);

            //卡牌的生成位置
            card.transform.position = deckPosition;

            //添加到手牌中
            handCardObjectList.Add(card);

            var delay = i * 0.2f;
            //设置手牌位置，这项操作会在每次抽卡时执行
            SetCardLayout(delay);
        }
    }

    /// <summary>
    /// 设置卡牌布局
    /// 在每次卡牌数量变化时执行
    /// </summary>
    private void SetCardLayout(float delay)
    {
        for(int i = 0;i<handCardObjectList.Count;i++)
        {
            Card currentCard= handCardObjectList[i];
            CardTransfom cardTransfom=cardLayoutManager.GetCardTransfom(i,handCardObjectList.Count);

            //currentCard.transform.SetPositionAndRotation(cardTransfom.pos,cardTransfom.rotation);

            //判断卡牌能否被打出
            currentCard.UpdateCardState();            
            currentCard.isAnimating = true;

            //因为之前生成的卡牌缩放已经是1了，所以没有效果
            //参数一：目标值 参数二：所需时间 参数三：生成间隔的延迟时间
            currentCard.transform.DOScale(Vector3.one, 0.2f).SetDelay(delay).onComplete = () =>
            {
                //缩放动画结束后播放移动和旋转动画
                currentCard.transform.DOMove(cardTransfom.pos, 0.4f).onComplete= ()=> currentCard.isAnimating = false;
                currentCard.transform.DORotateQuaternion(cardTransfom.rotation, 0.4f);
            };

            //设置卡牌排序
            currentCard.GetComponent<SortingGroup>().sortingOrder = handCardObjectList.Count-1-i;
            //注意此处因为用到DOTween移动，所以赋值直接使用currentCard会有问题，所以使用cardTransfom
            currentCard.UpdatePosRotation(cardTransfom.pos, cardTransfom.rotation);            
        }
    }

    //洗牌
    private void ShuffleDeck()
    {
        //弃牌堆清空
        discardDeck.Clear();

        //更新UI显示
        drawAmountChangedEvent.RaiseEvent(drawDeck.Count,this);
        discardAmountChangedEvent.RaiseEvent(discardDeck.Count,this);

        //打乱排序，实现洗牌效果
        for(int i = 0;i<drawDeck.Count;i++)
        {
            CardDataSO tempCard = drawDeck[i];
            int randomIndex=Random.Range(i,drawDeck.Count);
            drawDeck[i] = drawDeck[randomIndex];
            drawDeck[randomIndex] = tempCard;
        }
    }

    /// <summary>
    /// 监听：打出卡牌后回收
    /// </summary>
    /// <param name="card"></param>
    public void DiscardCard(object obj)
    {
        Card card=obj as Card;
        //从手牌中移除，添加到弃牌堆
        discardDeck.Add(card.cardData);

        //更新UI显示
        discardAmountChangedEvent.RaiseEvent(discardDeck.Count, this);

        handCardObjectList.Remove(card);

        //回收
        cardManager.DiscardCard(card.gameObject);

        //重新设置卡牌布局
        SetCardLayout(0f);
    }

    /// <summary>
    /// 监听：玩家回合结束 弃掉所有手牌
    /// </summary>
    public void OnPlayerTurnEnd()
    {
        for(int i = 0; i < handCardObjectList.Count; i++)
        {
            discardDeck.Add(handCardObjectList[i].cardData);
            cardManager.DiscardCard(handCardObjectList[i].gameObject);
        }
        handCardObjectList.Clear();
        //更新UI
        discardAmountChangedEvent.RaiseEvent(discardDeck.Count, this);
    }

    /// <summary>
    /// 监听：战斗结束(胜利或失败) 回收卡牌
    /// </summary>
    /// <param name="obj"></param>
    public void ReleaseAllCards(object obj)
    {
        foreach(Card card in handCardObjectList)
        {
            cardManager.DiscardCard(card.gameObject);
        }

        handCardObjectList.Clear();
        InitializeDeck();
    }
}
