using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Player player;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        player =GetComponent<Player>();
    }

    private void OnEnable()
    {
        anim.Play("sleep");
        anim.SetBool("isSleep", true);
    }

    public void PlayerTurnBeginAnimation()
    {
        anim.SetBool("isSleep", false);
        anim.SetBool("isParry", false);
    }

    public void PlayerTurnEndAnimation()
    {
        //如果回合结束有防御值就处于枕头防御，没有就算了
        if (player.defense.currentValue > 0)
        {
            anim.SetBool("isParry", true);
            anim.SetBool("isSleep", false);
        }
        else
        {
            anim.SetBool("isSleep", true);
            anim.SetBool("isParry", false);
        }
    }

    public void OnPlayCardEvent(object obj)
    {
        Card card = obj as Card;
        switch (card.cardData.cardType)
        {
            case CardType.Attack:
                anim.SetTrigger("attack");
                break;
            case CardType.Skill:
            case CardType.Ability:
                anim.SetTrigger("skill");
                break;
        }
    }

    public void PlaySleepAnim()
    {
        anim.Play("death");
    }
}
