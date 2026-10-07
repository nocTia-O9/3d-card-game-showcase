using UnityEngine;

public class Player : CharacterBase
{
    public IntVariable money;
    public IntVariable playerEnergy;

    public int startMoney;//初始金钱
    public int maxEnergy;
    public int CurrentEnergy { get => playerEnergy.currentValue; 
                               set => playerEnergy.SetValue(value);}
    public int CurrentMoney { get => money.currentValue;
                              set => money.SetValue(value);
    }

    private void OnEnable()
    {
        playerEnergy.maxValue = maxEnergy;
        CurrentEnergy=playerEnergy.maxValue;
    }

    /// <summary>
    /// 监听：每回合开始重置能量
    /// </summary>
    public void NewTurn()
    {        
        CurrentEnergy=maxEnergy;
    }

    /// <summary>
    /// 监听：消耗能量
    /// </summary>
    /// <param name="cost">需要消耗的能量值</param>
    public void UpadatEnergy(int cost)
    {
        CurrentEnergy-=cost;
        if (CurrentEnergy <= 0)
            CurrentEnergy = 0;
    }
    
    public void NewGame()
    {
        CurrentMoney = startMoney;        
        CurrentHP = MaxHP;
        isDead = false;
        buffRound.currentValue = buffRound.maxValue;
        NewTurn();
    }

    //public void OnMoneyChangedEvent(int amount)
    //{
    //    CurrentMoney += amount;
    //}
}
