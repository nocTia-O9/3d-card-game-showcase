using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float offsetSpeed=0.2f;
    private void Update()
    {
        //先判断有没有这个组件
        if (lineRenderer != null)
        {
            //获得当前纹理偏移
            var offset = lineRenderer.material.mainTextureOffset;
            offset.x += offsetSpeed * Time.deltaTime;

            lineRenderer.material.mainTextureOffset = offset;
        }
    }
}
