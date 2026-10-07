using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Card : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [Header("广播")]
    public ObjectEventSO discardCardEvent;
    public IntEventSO costEvent;

    [Header("组件")]
    public SpriteRenderer sp;
    //注意此处是TextMeshPro，而非UGUI
    public TextMeshPro txtName,txtCost, txtDescription, txtType;    
    public CardDataSO cardData;//卡牌数据
    private CardLayoutManager cardLayoutManager;//卡牌布局管理

    [Header("卡牌位置")]
    private Vector3 originalPosititon;
    private Quaternion originalRotation;
    private int originalLayerOrder;
    [HideInInspector]public bool isAnimating;//是否在播放动画
    [HideInInspector]public Player player;//角色身上的CharacterBase

    public bool isAvailable;//卡牌能否被打出

    public void Init(CardDataSO data)
    {
        cardData = data;
        sp.sprite=cardData.cardImage;
        txtName.text=cardData.cardName;
        txtCost.text = cardData.cost.ToString();
        txtDescription.text = cardData.description;
        txtType.text = cardData.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Skill => "技能",
            CardType.Ability => "能力",
            _ => throw new System.NotImplementedException(),
        };

        player=GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();  
        cardLayoutManager=GameObject.Find("CardLayout Manager").GetComponent<CardLayoutManager>();
    }  

    public void OnPointerEnter(PointerEventData eventData)
    {
        //卡牌动画效果结束后方法才可以生效
        if (isAnimating) return;        
        if (cardLayoutManager.isHorizontal)
        {
            transform.position = originalPosititon + Vector3.up;
        }
        else
        {
            transform.position = new Vector3(originalPosititon.x, -3.5f, 0);
        }        
        transform.rotation = Quaternion.identity;
        GetComponent<SortingGroup>().sortingOrder = 20;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isAnimating) return;
        ResetCardTransform();
    }

    //保存卡牌位置
    public void UpdatePosRotation(Vector3 pos, Quaternion rotation)
    {
        originalPosititon = pos;
        originalRotation = rotation;
        originalLayerOrder = GetComponent<SortingGroup>().sortingOrder;
    }

    //还原卡牌位置
    public void ResetCardTransform()
    {
        transform.SetPositionAndRotation(originalPosititon, originalRotation);
        GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;
    }

    /// <summary>
    /// 执行卡牌效果
    /// </summary>
    /// <param name="from">释放对象</param>
    /// <param name="target">目标对象</param>
    //public void ExecuteCardEffects(CharacterBase from, CharacterBase target)
    //{
    //    //减少能量
    //    costEvent.RaiseEvent(cardData.cost, this);        
    //    //通知回收卡牌
    //    discardCardEvent.RaiseEvent(this, this);
    //    //执行卡牌效果
    //    foreach (var effect in cardData.effects)
    //    {
    //        effect.Execute(from, target);
    //    }
    //}

    public void ExecuteCardEffects(CharacterBase from, CharacterBase target)
    {
        //减少能量
        costEvent.RaiseEvent(cardData.cost, this);
        //角色动画播放一会后再执行效果
        player.GetComponent<PlayerAnimation>().OnPlayCardEvent(this);
        StartCoroutine(ProcessDelayAction(from, target));        
    }

    IEnumerator ProcessDelayAction(CharacterBase from, CharacterBase target)
    {
        yield return new WaitForSeconds(0.5f);
        discardCardEvent.RaiseEvent(this, this);
        //执行卡牌效果
        foreach (var effect in cardData.effects)
        {
            effect.Execute(from, target);
        }
    }

    /// <summary>
    /// 设置卡牌能否被打出
    /// </summary>
    public void UpdateCardState()
    {        
        isAvailable = cardData.cost <= player.CurrentEnergy;
        txtCost.color=isAvailable?Color.green:Color.red;
    }
}
