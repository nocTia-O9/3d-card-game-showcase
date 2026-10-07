using System.Collections;
using UnityEngine;

public class Enemy : CharacterBase
{
    public EnemyActionDataSO enemyActionDataSO;
    public EnemyAction currentAction;
    [SerializeField]protected Player player;
       
    /// <summary>
    /// 监听：敌人随机获得一个意图
    /// </summary>
    public virtual void OnPlayerTurnBegin()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        var randomIndex=Random.Range(0, enemyActionDataSO.actions.Count);
        currentAction = enemyActionDataSO.actions[randomIndex];
    }

    /// <summary>
    /// 监听：敌人开始行动
    /// </summary>
    public virtual void OnEnemyTurnBegin()
    {
        ResetDefense();
        switch (currentAction.effect.targetType)
        {
            case CardEffectTargetType.Self:
                Skill();
                break;
            case CardEffectTargetType.Target:
                Attack();
                break;
            case CardEffectTargetType.All:
                break;
        }
    }

    public virtual void Skill()
    {        
        StartCoroutine(ProcessDelayAction("skill"));
    }
    public virtual void Attack()
    {        
        StartCoroutine(ProcessDelayAction("attack"));
    }

    /// <summary>
    /// 动画播放一会再执行效果
    /// </summary>
    /// <param name="actionName"></param>
    /// <returns></returns>
    IEnumerator ProcessDelayAction(string actionName)
    {
        animator.SetTrigger(actionName);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f > 0.55f
                                       && !animator.IsInTransition(0)
                                       && animator.GetCurrentAnimatorStateInfo(0).IsName(actionName));
        if (actionName == "attack")
            currentAction.effect.Execute(this,player);
        else
            currentAction.effect.Execute(this, this);
    }
}
