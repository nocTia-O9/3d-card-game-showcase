using UnityEngine;

[CreateAssetMenu(fileName = "DefenseEffect", menuName = "Card Effect/DefenseEffect")]
public class DefenseEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {        
        if (targetType == CardEffectTargetType.Self)
        {
            from.UpdateDefense(value);
        }

        //TODO：敌人可以给其它敌人加护盾
        if (targetType == CardEffectTargetType.Target)
        {
            target.UpdateDefense(value);
        }
    }
}
