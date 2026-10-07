using UnityEngine;

/// <summary>
/// 特效动画 播放控制
/// </summary>
public class VFXController : MonoBehaviour
{
    public GameObject buff;
    public GameObject deBuff;
    private float timeCounter;

    private void Update()
    {
        if (buff.activeInHierarchy)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= 0.67f)
            {
                timeCounter = 0;
                buff.SetActive(false);
            }
        }
        if (deBuff.activeInHierarchy)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= 0.667f)
            {
                timeCounter = 0;
                deBuff.SetActive(false);
            }
        }
    }
}
