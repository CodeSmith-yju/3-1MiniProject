using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseEntity
{
    [Header("Enemy_Item")]
    public float exp_Cnt;
    public int gold_Cnt;
    public bool item_Drop_Check = false;


    private Item drop_Item;

    [Header("Enemy_Sound")]
    public int sfx_Index;

    protected override void Start()
    {
        base.Start();
        isPlayer = false;
    }



    // 최대 체력, 최대 마나, 공격력, 공격속도, 사거리, 근접유무, 스킬유무, 경험치, 골드, 드랍아이템
    public void InitStat(float max_Hp, float max_Mp, int def_Point, float atkDmg, float atkSpd, float atkRange, bool isMelee, bool able_Skill, float exp, int gold, Item item)
    {
        stat = new(
            max_Hp,
            max_Mp,
            def_Point,
            atkDmg,
            atkSpd,
            atkRange,
            isMelee,
            able_Skill,
            exp,
            gold,
            item
            );

        this.max_Hp = stat.max_Hp * BattleManager.Instance.dungeon_Level_Scale;
        this.cur_Hp = this.max_Hp;
        this.def_Point = stat.def_Point;
        this.max_Mp = stat.max_Mp;
        this.cur_Mp = 0f;
        this.atkDmg = stat.atkDmg * BattleManager.Instance.dungeon_Level_Scale;
        SetAttackSpeed(stat.atkSpd);
        this.atkRange = stat.atkRange;
        this.isMelee = stat.isMelee;
        this.able_Skill = stat.able_Skill;
        exp_Cnt = stat.exp;
        gold_Cnt = stat.gold;
        drop_Item = stat.item;
    }

    public Item GetItemDropTable()
    {
        return drop_Item;
    }

    protected bool ShouldDropItem(int value)
    {
        int randomDrop = Random.Range(0, 100);
        return randomDrop < (int)(value * BattleManager.Instance.dungeon_Level_Scale);
    }

    protected int SetRandomGold(int count)
    {
        int randomDrop = Random.Range(count, count + 20);
        return (int)(randomDrop * BattleManager.Instance.dungeon_Level_Scale);
    }

    protected float GetExp(float exp)
    {
        return exp * BattleManager.Instance.dungeon_Level_Scale;
    }


    // 사운드 수정 예정
    public void AttackSound(int index)
    {
        AudioManager.single.EnemySound(sfx_Index, index, 1);
    }

    public void DieSound(int index)
    {
        AudioManager.single.EnemySound(sfx_Index, index, 0);
    }

    public void SkillSound(int index)
    {
        AudioManager.single.EnemySound(sfx_Index, index, 2);
    }

}