using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDataSO",menuName = "Card/CardDataSO")]
public class CardDataSO : ScriptableObject
{
    public string cardName;
    public Sprite cardImage;
    public int cost;
    public CardType cardType;
    [TextArea]
    public string description;

    //执行的实际效果
    public List<Effect> effects;
    //public List<string> strEffects;
    //public string[] strEffect;

    //public void Save()
    //{
    //    foreach (var item in effects)
    //    {
    //        strEffects.Add(JsonUtility.ToJson(item));
    //    }
    //    //for (int i = 0; i < effects.Count; i++)
    //    //{
    //    //    strEffect[i] = JsonUtility.ToJson(effects[i]);
    //    //}
    //}

    //public void Load()
    //{
    //    foreach (var item in strEffects)
    //    {
    //        var newSO = ScriptableObject.CreateInstance<Effect>();
    //        JsonUtility.FromJsonOverwrite(item, newSO);
    //        ResourceRequest rq = Resources.LoadAsync<Effect>("Card Effect/" + newSO.name);
    //        rq.completed += LoadOver;
    //    }
    //    //for(int i = 0;i<strEffect.Length; i++)
    //    //{
    //    //    var newSO = ScriptableObject.CreateInstance<Effect>();
    //    //    JsonUtility.FromJsonOverwrite(strEffect[i], newSO);
    //    //    ResourceRequest rq = Resources.LoadAsync<Effect>("Card Effect/" + newSO.name);
    //    //    rq.completed += LoadOver;
    //    //}
    //}

    //private void LoadOver(AsyncOperation operation)
    //{
    //    for (int i = 0; i < effects.Count; i++)
    //    {
    //        effects[i] = (operation as ResourceRequest).asset as Effect;
    //    }
    //}
}
