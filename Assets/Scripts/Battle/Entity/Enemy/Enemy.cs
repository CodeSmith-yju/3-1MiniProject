using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseEntity
{
    private EntityStat stat;

    protected override void Start()
    {
        base.Start();
        Debug.Log("Enemy ( " + name + " ) ����");

        // ���� ID, �ִ� HP, �ִ� MP, ���ݷ�, ���ݼӵ�, ���ݻ�Ÿ�, ��ų���� ������ �ʱ�ȭ
        stat = new EntityStat(50, 12, 0, 1f, 0.8f, 1.1f, false);

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
        if (_curstate == State.Skill)
        {
            Skill();
        }
    }


    public void Skill()
    {
        if (_curstate == State.Skill)
        {

            StopAllCoroutines();
            if (isAttack)
            {

                BaseEntity target = FindTarget().GetComponent<BaseEntity>();
                Debug.Log("Ÿ���� ������ 2���� �������� �ѹ� ����" + " " + (atkDmg * 2) + "������");
                target.cur_Hp -= atkDmg * 2;
                Debug.Log(target.cur_Hp + " " + target.name);
            }
            else
            {
                return;
            }
            cur_Mp = 0;
            ChangeState(State.Idle);
        }
    }
}