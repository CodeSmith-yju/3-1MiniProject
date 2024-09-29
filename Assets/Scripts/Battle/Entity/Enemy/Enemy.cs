using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseEntity
{
    public float exp_Cnt;
    public int gold_Cnt;
    public bool item_Drop_Check = false;
    private Item drop_Item;

    protected override void Update()
    {
        base.Update();
    }

    // 최대 체력, 최대 마나, 공격력, 공격속도, 사거리, 근접유무, 스킬유무, 경험치, 골드, 드랍아이템
    public void InitStat(float max_Hp, float max_Mp, float atkDmg, float atkSpd, float atkRange, bool isMelee, bool able_Skill, float exp, int gold, Item item)
    {
        stat = new(
            max_Hp,
            max_Mp,
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
        this.cur_Hp = this.max_Hp * BattleManager.Instance.dungeon_Level_Scale;
        this.max_Mp = stat.max_Mp;
        this.cur_Mp = 0f;
        this.atkDmg = stat.atkDmg * BattleManager.Instance.dungeon_Level_Scale;
        SetAttackSpeed(stat.atkSpd);
        this.atkRange = stat.atkRange;
        this.isMelee = stat.isMelee;
        this.able_Skill = stat.able_Skill;
        exp_Cnt = stat.exp * BattleManager.Instance.dungeon_Level_Scale;
        gold_Cnt = (int)(stat.gold * BattleManager.Instance.dungeon_Level_Scale);
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
        return randomDrop;
    }


    // 사운드 수정 예정
    public void AttackSound(int index)
    {
        // AudioManager.single.EnemySound(index, index, 1);
    }


    public void DieSound(int index)
    {
        // AudioManager.single.EnemySound(index, index, 0);
    }

}