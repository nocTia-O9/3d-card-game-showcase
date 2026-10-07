using System.Collections;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [Header("广播")]
    public ObjectEventSO characterDeadEvent;

    public int maxHp;
    protected Animator animator;
    public IntVariable hp;
    public IntVariable defense;

    public IntVariable buffRound;
    public float baseStrength=1f;
    private float strengthEffect=0.5f;

    public GameObject buff;
    public GameObject deBuff;

    public bool isDead;
    
    public int CurrentHP { get => hp.currentValue; set => hp.SetValue(value); }
    public int MaxHP { get => hp.maxValue; }

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    protected virtual void Start()
    {        
        hp.maxValue = maxHp;

        //即 使用SetValue方法设置IntVariable的currentValue
        CurrentHP = MaxHP;
        buffRound.currentValue = 0;
        ResetDefense();
    }

    protected virtual void Update()
    {
        animator.SetBool("isDead", isDead);
    }
    public void TakeDamage(int damage)
    {
        var currentDamage=(damage-defense.currentValue)>=0? (damage - defense.currentValue):0;
        var currentDefense = (damage - defense.currentValue) >= 0 ? 0 : (defense.currentValue-damage);
        defense.SetValue(currentDefense);
        if (CurrentHP>currentDamage)
        {
            CurrentHP-=currentDamage;
            animator.SetTrigger("hit");
        }
        else
        {
            CurrentHP = 0;
            //角色死亡
            isDead = true;
            characterDeadEvent.RaiseEvent(this, this);
            StartCoroutine(DelayCharacterDisable());
        }
    }

    /// <summary>
    /// 等待一会再让人物消失
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayCharacterDisable()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 更新防御值
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateDefense(int amount)
    {
        var value = defense.currentValue + amount;
        defense.SetValue(value);
    }

    /// <summary>
    /// 防御值清零
    /// </summary>
    public void ResetDefense()
    {
        defense.SetValue(0);
    }

    public void HealHP(int amount)
    {
        CurrentHP += amount;
        CurrentHP=Mathf.Min(CurrentHP, MaxHP);
        buff.SetActive(true);        
    }

    public void SetupStrength(int round,bool isPositive)
    {
        if (isPositive)
        {
            buff.SetActive(true);
            float newStrength = baseStrength + strengthEffect;
            baseStrength = Mathf.Min(newStrength, 1.5f);
        }
        else
        {
            deBuff.SetActive(true);
            baseStrength=1-strengthEffect;
        }

        var currentRound = buffRound.currentValue + round;

        //降低攻击又增加攻击后，抵消回合
        if (baseStrength == 1)
            buffRound.SetValue(0);
        else
            buffRound.SetValue(currentRound);
    }

    /// <summary>
    /// 更新buff回合
    /// </summary>
    public void UpdateStrengthRound()
    {
        buffRound.SetValue(buffRound.currentValue-1);
        if (buffRound.currentValue <= 0)
        {
            buffRound.SetValue(0);
            baseStrength = 1;
        }
    }
}
