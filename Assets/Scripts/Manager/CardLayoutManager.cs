using System.Collections.Generic;
using UnityEngine;

public class CardLayoutManager : MonoBehaviour
{
    [HideInInspector]public bool isHorizontal;//是否水平布局
    public float maxWidth = 10f;//默认最大宽度
    public float cardSpacing = 2f;//默认卡牌间距
    [HideInInspector]public Vector3 centerPoint;//卡牌生成位置的中心点

    [Header("扇形布局参数")]
    public float angleBetweenCards = 7f;//扇形间距
    public float radius = 17f;//扇形半径    

    [SerializeField] private List<Vector3> cardPositions = new();
    private List<Quaternion> cardRotations = new();

    private void Awake()
    {
        //屏幕三分之一的宽度，在屏幕中间差不多
        //maxWidth = Mathf.RoundToInt(Camera.main.orthographicSize * 2 * Camera.main.aspect / 3);
        //print(maxWidth);
        //centerPoint = isHorizontal ? Vector3.up * -4.5f : Vector3.up * -21.5f;
    }

    /// <summary>
    /// 返回卡牌布局
    /// </summary>
    /// <param name="index"></param>
    /// <param name="totalCards"></param>
    /// <returns></returns>
    public CardTransfom GetCardTransfom(int index,int totalCards)
    {
        CalculatePosition(totalCards);
        return new CardTransfom(cardPositions[index], cardRotations[index]);
    }

    /// <summary>
    /// 计算卡牌布局
    /// 根据手牌数量计算卡牌布局
    /// </summary>
    /// <param name="numberOfCards"></param>
    /// <param name="isHorizontal"></param>
    private void CalculatePosition(int numberOfCards)
    {
        //每次计算布局都要清除上一次的数据
        cardPositions.Clear();
        cardRotations.Clear();

        //手牌数量大于6为水平布局，否则扇形布局
        if (numberOfCards > 6)
        {
            isHorizontal = true;
            centerPoint = Vector3.up * -4.5f;
            //当前宽度：如果只有一张牌，则为0
            float currentWidth = cardSpacing * (numberOfCards - 1);

            //实际宽度
            float totalWidth = Mathf.Min(currentWidth, maxWidth);

            //实际间距
            float currentSpacing = totalWidth > 0 ? totalWidth / (numberOfCards - 1) : 0;

            for (int i = 0; i < numberOfCards; i++)
            {
                //float posX=0-(totalWidth/2)+(i*currentSpacing);
                //本游戏中第一张牌在最右边
                //根据当前的默认宽度为7参考(手牌再多也不能超过这个宽度)，在手牌数量充足的情况下
                //并且间距也一直在改变
                //所以最右边的牌的x值固定为3,5，最左边的牌固定为-3.5，也就是3.5*=7                
                //在这条规则下，后生成的卡牌会让手牌区越来越密(都向中间区域靠拢)，卡牌越多，贴的越近
                float posX = 0 + (totalWidth / 2) - (i * currentSpacing);

                var pos = new Vector3(posX, centerPoint.y, 0);
                var rotation = Quaternion.identity;

                cardPositions.Add(pos);
                cardRotations.Add(rotation);
            }
        }
        else
        {
            isHorizontal = false;
            centerPoint = Vector3.up * -21.5f;
            //得到一个中间部分的扇面(取负代表从左边开始)
            //TODO:未实现将扇形限制在一定范围内
            float cardAngle = -((numberOfCards - 1) * angleBetweenCards / 2);

            for (int i = 0; i < numberOfCards; i++)
            {
                var pos = FanCardPosition(cardAngle + i * angleBetweenCards);
                var rotation = Quaternion.Euler(0, 0, cardAngle + i * angleBetweenCards);
                cardPositions.Add(pos);
                cardRotations.Add(rotation);
            }
        }
        #region 老版
        //if (isHorizontal)
        //{
        //    centerPoint = isHorizontal ? Vector3.up * -4.5f : Vector3.up * -21.5f;
        //    //当前宽度：如果只有一张牌，则为0
        //    float currentWidth = cardSpacing * (numberOfCards - 1);

        //    //实际宽度
        //    float totalWidth=Mathf.Min(currentWidth, maxWidth);

        //    //实际间距
        //    float currentSpacing = totalWidth > 0 ? totalWidth / (numberOfCards - 1) : 0;

        //    for(int i = 0; i < numberOfCards; i++)
        //    {
        //        //float posX=0-(totalWidth/2)+(i*currentSpacing);
        //        //本游戏中第一张牌在最右边
        //        //根据当前的默认宽度为7参考(手牌再多也不能超过这个宽度)，在手牌数量充足的情况下
        //        //并且间距也一直在改变
        //        //所以最右边的牌的x值固定为3,5，最左边的牌固定为-3.5，也就是3.5*=7                
        //        //在这条规则下，后生成的卡牌会让手牌区越来越密(都向中间区域靠拢)，卡牌越多，贴的越近
        //        float posX =0+(totalWidth/2)-(i*currentSpacing);

        //        var pos = new Vector3(posX, centerPoint.y, 0);
        //        var rotation = Quaternion.identity;

        //        cardPositions.Add(pos);
        //        cardRotations.Add(rotation);
        //    }
        //}
        //else
        //{
        //    //得到一个中间部分的扇面(取负代表从左边开始)
        //    //TODO:未实现将扇形限制在一定范围内
        //    float cardAngle = -((numberOfCards - 1) * angleBetweenCards/2);

        //    for (int i = 0; i < numberOfCards; i++)
        //    {
        //        var pos = FanCardPosition(cardAngle + i * angleBetweenCards);
        //        var rotation = Quaternion.Euler(0, 0, cardAngle + i * angleBetweenCards);
        //        cardPositions.Add(pos);
        //        cardRotations.Add(rotation);
        //    }
        //}
        #endregion
    }

    //计算扇形布局坐标
    private Vector3 FanCardPosition(float angle)
    {
        return new Vector3(
            centerPoint.x-Mathf.Sin(Mathf.Deg2Rad*angle)*radius,
            centerPoint.y+Mathf.Cos(Mathf.Deg2Rad*angle)*radius,
            0
            );
    }
}
