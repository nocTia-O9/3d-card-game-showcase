using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuPanel : MonoBehaviour
{
    [Header("广播")]
    public ObjectEventSO newGameEvent;    

    private VisualElement rootElement;
    private Button newGameBtn, quitGameBtn;
    private void OnEnable()
    {
        rootElement=GetComponent<UIDocument>().rootVisualElement;
        newGameBtn = rootElement.Q<Button>("NewGame");
        quitGameBtn = rootElement.Q<Button>("QuitGame");        

        newGameBtn.clicked += OnNewGameBtnClicked;
        quitGameBtn.clicked += OnQuitGameBtnClicked;        
    }

    private void OnQuitGameBtnClicked()
    {
        Application.Quit();
    }

    private void OnNewGameBtnClicked()
    {
        newGameEvent.RaiseEvent(null, this);
    }
}
