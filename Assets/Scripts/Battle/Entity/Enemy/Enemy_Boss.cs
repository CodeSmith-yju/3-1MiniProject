using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : BaseEntity
{
    private EntityStat stat;

    protected override void Start()
    {
        base.Start();
        Debug.Log("Enemy ( " + name + " ) ����");

        // ���� ID, �ִ� HP, �ִ� MP, ���ݷ�, ���ݼӵ�, ���ݻ�Ÿ�, ��ų���� ������ �ʱ�ȭ
        stat = new EntityStat(51, 45, 0, 5f, 0.5f, 1.8f, false);

        entity_id = stat.id;
        max_Hp = stat.max_Hp;
        cur_Hp = max_Hp;
        max_Mp = stat.max_Mp;
        cur_Mp = 0;
        atkDmg = stat.atkDmg;
        SetAttackSpeed(stat.atkSpd);
        atkRange = stat.atkRange;
        able_Skill = stat.isSkill;
        isMelee = true;
    }

    protected override void Update()
    {
        base.Update();
    }
}
