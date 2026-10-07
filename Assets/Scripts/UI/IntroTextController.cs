using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * @description: 介绍文本控制器，管理序列文本的淡入淡出效果
 */
public class IntroTextController : MonoBehaviour
{
    public ObjectEventSO loadMenuEvent;
    public List<string> textList;
    public TextMeshProUGUI text;
    public float fadeSpeed = 1.0f;
    public float textDisplayTime = 1f;

    private enum TextState
    {
        FadingIn,
        Displaying,
        FadingOut,
        Waiting
    }

    private Coroutine currentCoroutine;
    private int textIndex = 0;
    private bool isStop = false;
    private TextState currentState = TextState.Waiting;

    /**
     * @description: 初始化文本控制器
     */
    private void Start()
    {
        // 参数验证
        if (text == null)
        {
            Debug.LogError("文本组件未赋值！");
            isStop = true;
            return;
        }
        
        if (textList == null || textList.Count == 0)
        {
            Debug.LogWarning("文本列表为空！");
            isStop = true;
            if (loadMenuEvent != null)
            {
                loadMenuEvent.RaiseEvent(null, this);
            }
            return;
        }
        
        // 初始化文本
        text.alpha = 0;
        StartNextText();
    }

    /**
     * @description: 开始显示下一条文本
     */
    private void StartNextText()
    {
        if (textIndex >= textList.Count)
        {
            // 所有文本显示完毕
            isStop = true;
            if (loadMenuEvent != null)
            {
                loadMenuEvent.RaiseEvent(null, this);
            }
            return;
        }

        // 设置文本内容
        text.text = textList[textIndex];
        textIndex++;
        
        // 开始淡入效果
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(TextSequence());
    }

    /**
     * @description: 文本显示序列（淡入->显示->淡出->下一条）
     */
    private IEnumerator TextSequence()
    {
        // 淡入阶段
        currentState = TextState.FadingIn;
        yield return StartCoroutine(FadeIn(fadeSpeed));
        
        // 显示阶段
        currentState = TextState.Displaying;
        yield return new WaitForSeconds(textDisplayTime);
        
        // 淡出阶段
        currentState = TextState.FadingOut;
        yield return StartCoroutine(FadeOut(fadeSpeed));
        
        // 切换到下一条文本
        currentState = TextState.Waiting;
        StartNextText();
    }

    /**
     * @description: 文本淡入效果
     * @param {float} speed - 淡入速度
     */
    private IEnumerator FadeIn(float speed)
    {
        while (text.alpha < 1)
        {
            text.alpha += Time.deltaTime * speed;
            if (text.alpha > 1) text.alpha = 1;
            yield return null;
        }
    }
    
    /**
     * @description: 文本淡出效果
     * @param {float} speed - 淡出速度
     */
    private IEnumerator FadeOut(float speed)
    {
        while (text.alpha > 0)
        {
            text.alpha -= Time.deltaTime * speed;
            if (text.alpha < 0) text.alpha = 0;
            yield return null;
        }
    }
}
