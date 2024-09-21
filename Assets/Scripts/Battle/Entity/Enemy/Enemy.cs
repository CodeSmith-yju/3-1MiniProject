using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseEntity
{
    public float exp_Cnt;
    public int gold_Cnt;

    protected override void Update()
    {
        base.Update();
    }

    // �ִ� ü��, �ִ� ����, ���ݷ�, ���ݼӵ�, ��Ÿ�, ��������, ��ų����
    public void InitStat(float max_Hp, float max_Mp, float atkDmg, float atkSpd, float atkRange, bool isMelee, bool able_Skill, float exp, int gold)
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
            gold
            );

        this.max_Hp = stat.max_Hp;
        this.cur_Hp = this.max_Hp;
        this.max_Mp = stat.max_Mp;
        this.cur_Mp = 0f;
        this.atkDmg = stat.atkDmg;
        SetAttackSpeed(stat.atkSpd);
        this.atkRange = stat.atkRange;
        this.isMelee = stat.isMelee;
        this.able_Skill = stat.able_Skill;
        exp_Cnt = stat.exp;
        gold_Cnt = stat.gold;
    }

    // ���� ���� ����
    public void AttackSound(int index)
    {
        // AudioManager.single.EnemySound(index, index, 1);
    }


    public void DieSound(int index)
    {
        // AudioManager.single.EnemySound(index, index, 0);
    }

}