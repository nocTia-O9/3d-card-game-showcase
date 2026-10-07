using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡牌库数据：CardLibrarySO
/// </summary>
[CreateAssetMenu(fileName = "CardLibrarySO",menuName = "Card/CardLibrarySO")]
public class CardLibrarySO : ScriptableObject
{
    public List<CardLibraryEntry> cardLibraryList=new();
}

[System.Serializable]
public class CardLibraryEntry
{    
    //某张卡牌的数据
    public CardDataSO cardData;
    //某张卡牌的数量
    //public int amount;
}
