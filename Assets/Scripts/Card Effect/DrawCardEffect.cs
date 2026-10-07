using UnityEngine;

[CreateAssetMenu(fileName = "DrawCardEffect", menuName = "Card Effect/DrawCardEffect")]
public class DrawCardEffect : Effect
{
    public IntEventSO drawCardEvent;
    //此效果只有玩家使用
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        drawCardEvent?.RaiseEvent(value,this);
    }
}
