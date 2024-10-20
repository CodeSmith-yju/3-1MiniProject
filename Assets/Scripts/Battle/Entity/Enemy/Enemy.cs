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

    // �ִ� ü��, �ִ� ����, ���ݷ�, ���ݼӵ�, ��Ÿ�, ��������, ��ų����, ����ġ, ���, ���������
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
        this.cur_Hp = this.max_Hp;
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


    // ���� ���� ����
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