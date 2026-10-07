using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Card Effect/HealEffect")]
public class HealEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (targetType == CardEffectTargetType.Self)
        {
            from.HealHP(value);
        }

        //敌人给队友回血，或给其它角色回血
        if(targetType==CardEffectTargetType.Target)
        {
            target.HealHP(value);
        }
    }
}
