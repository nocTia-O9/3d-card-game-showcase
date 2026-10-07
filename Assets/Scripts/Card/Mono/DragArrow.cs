using UnityEngine;

public class DragArrow : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 mousePos;

    [Tooltip("点数")]
    public int pointCount;
    [Tooltip("曲度")]
    public float arcModifier;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        
        //生成贝塞尔曲线
        SetArrowPosition();
    }
    
    public void SetArrowPosition()
    {
        Vector3 cardPosition = transform.position;//卡牌位置
        Vector3 direction = mousePos - cardPosition;//从卡牌指向鼠标的方向
        Vector3 normalizedDirection = direction.normalized;//归一化方向

        //计算垂直于卡牌到鼠标方向的向量
        Vector3 perpendicular = new(-normalizedDirection.y,
            normalizedDirection.x, normalizedDirection.z);

        //设置控制点的偏移量
        Vector3 offset = perpendicular * arcModifier;//你可以调整这个值来改变曲线的形状
        Vector3 controlPoint = (cardPosition + mousePos) / 2 + offset;//控制点
        lineRenderer.positionCount = pointCount;//设置LineRenderer的点的数量

        for(int i=0; i<pointCount; i++)
        {
            float t = i / (float)(pointCount - 1);
            Vector3 point = CalculateQuadraticBezierPoint(t, cardPosition, controlPoint, mousePos);
            lineRenderer.SetPosition(i,point);
            lineRenderer.SetPosition(i,new Vector3(point.x,point.y,-7f));
        }
    }

    //计算二次贝塞尔曲线点
    Vector3 CalculateQuadraticBezierPoint(float t,Vector3 p0,Vector3 p1,Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;//第一项
        p += 2 * u * t * p1;//第二项
        p += tt * p2;//第三项

        return p;
    }
}
