using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseEntity
{
    protected override void Start()
    {
        base.Start();
        Debug.Log("Enemy ( " + name + " ) ����");

        // ���� ID, �ִ� HP, �ִ� MP, ���ݷ�, ���ݼӵ�, ���ݻ�Ÿ�, ��ų���� ������ �ʱ�ȭ
        stat = new EntityStat(12, 0, 1f, 0.8f, 1.0f);

        max_Hp = stat.max_Hp;
        cur_Hp = max_Hp;
        max_Mp = stat.max_Mp;
        cur_Mp = 0;
        atkDmg = stat.atkDmg;
        SetAttackSpeed(stat.atkSpd);
        atkRange = stat.atkRange;
        able_Skill = false;

        isMelee = true;
    }

    protected override void Update()
    {
        base.Update();
    }
}