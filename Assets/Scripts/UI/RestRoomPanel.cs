using System;
using UnityEngine;
using UnityEngine.UIElements;

public class RestRoomPanel : MonoBehaviour
{
    [Header("广播")]
    public ObjectEventSO loadMapEvent;
    private VisualElement rootElement;
    private Button backToMapBtn;
    private Button restBtn;

    public Effect restEffect;
    private CharacterBase player;
    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        backToMapBtn = rootElement.Q<Button>("BackToMapBtn");
        restBtn = rootElement.Q<Button>("RestBtn");
        restBtn.clicked += OnRestBtnClicked;
        backToMapBtn.clicked += OnBackToMapBtnClicked;

        //即使未激活，也能获得
        player = FindAnyObjectByType<Player>(FindObjectsInactive.Include);
    }

    private void OnBackToMapBtnClicked()
    {
        loadMapEvent.RaiseEvent(null, this);
    }

    private void OnRestBtnClicked()
    {        
        restEffect.Execute(player,null);
        restBtn.SetEnabled(false);
    }
}
