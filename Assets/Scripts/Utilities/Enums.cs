using System;

[Flags]
public enum RoomType//房间类型
{
    //如何让枚举可以多选？
    //1、加Flags特性
    //2、将枚举项的值设置为2的次方（涉及到2进制的计算）
    MinorEnemy=1,//普通敌人
    EliteEnemy=2,//精英敌人
    Store=4,
    Treasure=8,
    RestRoom=16,
    Boss=32
}

public enum RoomState//房间状态
{
    Locked,//锁定
    Visited,//已访问
    Attainable//可访问
}
public enum CardType
{
    Attack,
    Skill,
    Ability
}

//目标类型
public enum CardEffectTargetType
{
    Self,//自身
    Target,//单体目标
    All//群体
}
