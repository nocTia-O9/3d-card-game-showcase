using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StorePanel : MonoBehaviour
{
    [Header("广播")]    
    public ObjectEventSO loadMapEvent;
    public ObjectEventSO cueTxtChangedEvent;

    public IntVariable money;

    public CardManager cardManager;

    private VisualElement rootElement;
    public VisualTreeAsset cardTemplate;
    private VisualElement cardContainer;

    [SerializeField]private CardDataSO currentCardData;//当前选择的那一张卡牌Data
    private Button confirmButton,backToMapButton;//确认按钮

    private List<Button>cardButtons=new List<Button>();
    private void OnEnable()
    {
        rootElement=GetComponent<UIDocument>().rootVisualElement;
        cardContainer = rootElement.Q<VisualElement>("Container");
        confirmButton = rootElement.Q<Button>("ConfirmButton");
        backToMapButton=rootElement.Q<Button>("BackToMapButton");

        confirmButton.clicked += OnConfirmBtnClicked;
        backToMapButton.clicked += OnBackToMapBtnClicked;

        for (int i = 0; i < 3; i++)
        {
            //生成一份卡牌UI白板
            var card = cardTemplate.Instantiate();
            //从CardManager那里随机获得一份CardData
            var data = cardManager.GetNewCardDataSO();
            //初始化
            Init(card, data);
            var cardButton = card.Q<Button>("Card");
            cardContainer.Add(card);
            cardButtons.Add(cardButton);

            cardButton.clicked += ()=> OnCardClicked(cardButton,data);
        }
    }

    /// <summary>
    /// 点击事件：返回地图
    /// </summary>
    private void OnBackToMapBtnClicked()
    {
        currentCardData = null;
        loadMapEvent.RaiseEvent(null, this);
    }

    /// <summary>
    /// 点击事件：选择卡牌
    /// </summary>
    private void OnConfirmBtnClicked()
    {
        if(currentCardData==null)
            return;
        if (money.currentValue >= 30)
        {
            money.currentValue -= 30;
            cardManager.UnlockCard(currentCardData);
            cueTxtChangedEvent.RaiseEvent("购买成功", this);
            //currentCardData = null;
        }
        else
        {
            cueTxtChangedEvent.RaiseEvent("金币不足", this);
        }              
    }

    private void OnCardClicked(Button cardButton,CardDataSO data)
    {
        currentCardData=data;
        for (int i = 0;i < cardButtons.Count;i++)
        {
            //效果：点的那一张不可选，未点的两张可选
            if (cardButtons[i] == cardButton)
                cardButtons[i].SetEnabled(false);
            else
                cardButtons[i].SetEnabled(true);
        }        
    }

    public void Init(VisualElement card,CardDataSO cardData)
    {        
        //也可以数据绑定在按钮的dataSource上
        //card.Q<Button>("Card").dataSource = cardData;
        var cardSpriteElement = card.Q<VisualElement>("CardSprite");
        var cardName = card.Q<Label>("CardName");
        var cardDescription = card.Q<Label>("CardDescription");
        var cardCost = card.Q<Label>("EnergyCost");
        var cardType = card.Q<Label>("CardType");

        cardSpriteElement.style.backgroundImage = new StyleBackground(cardData.cardImage);
        cardName.text = cardData.cardName;
        cardDescription.text = cardData.description;
        cardCost.text = cardData.cost.ToString();
        cardType.text = cardData.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Skill => "技能",
            CardType.Ability => "能力",
            _ => throw new System.NotImplementedException(),
        };
    }
}
