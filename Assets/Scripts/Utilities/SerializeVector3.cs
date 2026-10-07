using UnityEngine;

/// <summary>
/// 1、用于解决Vector3无法被序列化的问题（建议加入Unity常用套餐）
/// 2、不要继承Mono，否则会报错
/// </summary>
[System.Serializable]//让该类能被序列化
public class SerializeVector3
{
    public float x, y, z;
    public SerializeVector3(Vector3 pos)
    {
        x=pos.x; 
        y=pos.y; 
        z=pos.z;
    }
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }
}
