using UnityEngine;
using UnityEngine.UIElements;

public class GameWinPanel : MonoBehaviour
{
    [Header("广播")]
    public ObjectEventSO loadMapEvent;
    public ObjectEventSO pickCardEvent;
    public IntEventSO moneyChangedEvent;

    private VisualElement rootElement;
    private Button pickCardBtn, backToMapBtn, addGoldBtn;

    //点击方法
    private void OnEnable()
    {
        rootElement=GetComponent<UIDocument>().rootVisualElement;
        pickCardBtn = rootElement.Q<Button>("PickCardBtn");
        backToMapBtn = rootElement.Q<Button>("BackToMapBtn");
        addGoldBtn = rootElement.Q<Button>("AddGoldBtn");

        pickCardBtn.clicked += OnPickCardBtnClicked;
        backToMapBtn.clicked += OnBackToMapBtnClicked;
        addGoldBtn.clicked += OnAddGoldBtnClicked;
    }

    private void OnAddGoldBtnClicked()
    {
        moneyChangedEvent.RaiseEvent(50, this);
        addGoldBtn.SetEnabled(false);
    }

    #region 监听
    private void OnPickCardBtnClicked()
    {
        pickCardEvent.RaiseEvent(null,this);
    }

    private void OnBackToMapBtnClicked()
    {
        loadMapEvent.RaiseEvent(null,this);
    }

    public void OnFinishPickCardEvent()
    {
        pickCardBtn.SetEnabled(false);
    }
    #endregion
}
