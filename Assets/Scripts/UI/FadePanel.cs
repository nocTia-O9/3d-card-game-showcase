using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class FadePanel : MonoBehaviour
{
    private VisualElement background;
    private void Awake()
    {
        background=GetComponent<UIDocument>().rootVisualElement;
    }

    public void FadeIn(float duration)
    {
        //DOTween不能直接实现UIDocument的渐入渐出效果
        //所以这里使用DOVirtual.Float改变background的opacity
        DOVirtual.Float(0, 1, duration, value =>
        {
            background.style.opacity = value;
        }).SetEase(Ease.InOutSine);//变化的曲线
    }

    public void FadeOut(float duration)
    {
        DOVirtual.Float(1, 0, duration, value =>
        {
            background.style.opacity = value;
        }).SetEase(Ease.InOutSine);
    }
}
