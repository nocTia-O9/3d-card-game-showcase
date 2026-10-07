using UnityEngine;

/// <summary>
/// 卡牌位置数据
/// </summary>
public struct CardTransfom
{
    public Vector3 pos;
    public Quaternion rotation;
    public CardTransfom(Vector3 vector3, Quaternion quaternion)
    {
        pos = vector3;
        rotation = quaternion;
    }
}
