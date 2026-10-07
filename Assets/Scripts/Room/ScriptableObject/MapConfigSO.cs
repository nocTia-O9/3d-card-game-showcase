using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图配置文件
/// </summary>
[CreateAssetMenu(fileName = "MapConfigSO",menuName = "Map/MapConfigSO")]
public class MapConfigSO : ScriptableObject
{
    public List<RoomBlueprint> roomBlueprints;
}

[System.Serializable]
public class RoomBlueprint
{
    public int min, max;//房间的最小最大数量
    public RoomType roomType;//房间类型
}
