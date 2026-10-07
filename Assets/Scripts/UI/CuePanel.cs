using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CuePanel : MonoBehaviour
{    
    private VisualElement rootElement;
    private Label cueTxt;
    private Button confirmBtn;

    private void OnEnable()
    {
        rootElement=GetComponent<UIDocument>().rootVisualElement;
        cueTxt = rootElement.Q<Label>("CueTxt");
        confirmBtn = rootElement.Q<Button>("ConfirmBtn");

        confirmBtn.clicked += () =>
        {
            GetComponent<UIDocument>().sortingOrder = 2;
        };
    }

    public void OnCueTxtChanged(object txt)
    {
        GetComponent<UIDocument>().sortingOrder = 4;
        cueTxt.text=txt.ToString();
    }
}
