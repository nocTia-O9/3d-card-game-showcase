using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverPanel : MonoBehaviour
{
    [Header("广播")]
    public ObjectEventSO loadMenuEvent;
    public ObjectEventSO loadEndEvent;
    private Button backToStartButton;
    private void OnEnable()
    {
        GetComponent<UIDocument>().rootVisualElement.Q<Button>("BackToStartButton").clicked += BackToStart;
    }
    
    private void BackToStart()
    {
        if (GameManager.Instance.isEnd)
        {
            GameManager.Instance.isEnd = false;
            loadEndEvent.RaiseEvent(null,this);
        }
        else
        {
            loadMenuEvent.RaiseEvent(null,this);
        }
    }
}
