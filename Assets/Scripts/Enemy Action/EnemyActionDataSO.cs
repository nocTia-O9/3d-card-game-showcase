using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyActionDataSO",menuName = "Enemy/EnemyActionDataSO")]
public class EnemyActionDataSO : ScriptableObject
{
    public List<EnemyAction> actions;
}

[System.Serializable]
public struct EnemyAction
{
    public Sprite intentSprite;
    public Effect effect;
}
