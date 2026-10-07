using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject arrowPrefab;//箭头预设体
    private GameObject currentArrow;

    private Card currentCard;
    private CharacterBase targetCharacter;//目标对象

    private bool canMove;    
    private bool canExecute;//卡牌效果是否可以执行    

    private void Awake()
    {
        currentCard = GetComponent<Card>();
    }

    /// <summary>
    /// 使用完卡牌后 重置状态
    /// </summary>
    private void OnDisable()
    {
        canMove = false;
        canExecute = false;
        targetCharacter = null;
        currentCard.isAvailable = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //卡牌能量不足，不能被打出，直接返回
        if (!currentCard.isAvailable)
            return;

        switch (currentCard.cardData.cardType)
        {
            case CardType.Attack: 
                //此处与教程不同，将箭头设置为卡牌子物体
                currentArrow=Instantiate(arrowPrefab,transform.position,Quaternion.identity,transform);
                break;
            case CardType.Skill:
            case CardType.Ability:
                canMove = true;
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!currentCard.isAvailable)
            return;

        if (canMove)
        {
            //不希望卡牌此时有选中上滑效果，所以isAnimating设为true
            //即拖拽卡牌的时候，划入划出效果不生效
            currentCard.isAnimating = true;            
            Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);            
            currentCard.transform.position = worldPos;

            //鼠标拖拽非攻击牌，使其位置到了某一个值，代表可以执行卡牌效果
            canExecute = worldPos.y > 0;
        }
        else
        {
            //检测鼠标指针是否有移入到某个对象中(注意设置这个对象的Layer)
            if (eventData.pointerEnter == null) return;
            //如果有 则判断这个对象的标签            
            if (eventData.pointerEnter.CompareTag("Enemy"))
            {
                canExecute = true;
                targetCharacter = eventData.pointerEnter.GetComponent<CharacterBase>();
                return;
            }
            canExecute = false;
            targetCharacter = null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!currentCard.isAvailable)
            return;

        //结束拖拽后，如果有箭头就销毁
        if(currentArrow != null)
            Destroy(currentArrow);

        if(canExecute)
        {                 
            currentCard.ExecuteCardEffects(currentCard.player, targetCharacter);
        }
        else
        {
            currentCard.isAnimating = false;
            currentCard.ResetCardTransform();
        }        
    }

    private void Update()
    {
        //修改攻击牌最后没有停留在敌人身上也能生效的bug
        if (currentCard.cardData.cardType == CardType.Attack && canExecute)
        {
            if (currentCard.GetComponentInChildren<LineRenderer>())
            {
                var tempPos = currentCard.GetComponentInChildren<LineRenderer>().GetPosition(19);
                if (!(tempPos.y <= 0.3 && tempPos.y >= -1.6 && tempPos.x >= 6 && tempPos.x <= 8))
                {
                    canExecute = false;
                    targetCharacter = null;
                }
            }
        }
    }
}
