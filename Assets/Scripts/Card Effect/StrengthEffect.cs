using UnityEngine;

[CreateAssetMenu(fileName = "StrengthEffect", menuName = "Card Effect/StrengthEffect")]
public class StrengthEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        switch(targetType)
        {
            case CardEffectTargetType.Self://给自己用肯定是加攻击力
                from.SetupStrength(value, true);
                break;
            case CardEffectTargetType.Target://给敌人就是减攻击力
                target.SetupStrength(value, false);
                break;
            case CardEffectTargetType.All:                
                break;
        }
    }
}
