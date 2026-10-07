using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public int value;
    public CardEffectTargetType targetType;//目标类型

    public abstract void Execute(CharacterBase from, CharacterBase target);
}
