using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect",menuName = "Card Effect/DamageEffect")]
public class DamageEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        var damage = (int)Mathf.Round(value * from.baseStrength);
        if (target == null) return;
        switch(targetType)
        {
            case CardEffectTargetType.Target:
                //var damage = (int)Mathf.Round(value * from.baseStrength);
                target.TakeDamage(damage);                                
                break;
            case CardEffectTargetType.All:
                //遍历场景中所有标签为敌人的对象，去调用它们受伤的方法
                //var damage = (int)Mathf.Round(value * from.baseStrength);
                foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {                    
                    enemy.GetComponent<CharacterBase>().TakeDamage(damage);
                }
                break;
        }
    }    
}
