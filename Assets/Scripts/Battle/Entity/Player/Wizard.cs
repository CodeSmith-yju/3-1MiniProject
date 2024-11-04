using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Wizard : Ally
{
    protected override void Start()
    {
        base.Start();
        Debug.Log("Wizard ����");
        job = Job.Wizard;
    }

    public override void RangeAttack(BaseEntity target)
    {
        base.RangeAttack(target);
        GameObject obj_Arrow = BattleManager.Instance.pool.GetObject(2, isPlayer); // ��Ʈ ������
        obj_Arrow.transform.position = transform.GetChild(0).position;
        Arrow arrow = obj_Arrow.GetComponent<Arrow>();
        arrow.Shoot(this, target, atkDmg);
    }


    protected override void Skill()
    {
        base.Skill();
        if (_curstate == State.Skill)
        {
            StopCoroutine(SetAttack());
            if (isAttack)
            {
                BattleManager.Instance.ui.GenerateLog(class_Portrait, "���̾� ��Ʈ");

                ani.SetBool("isSkill", true);
                BaseEntity target = FindTarget().GetComponent<BaseEntity>();
                Debug.Log("Ÿ���� ������ 3���� �������� ����" + " " + (atkDmg * 3) + "������");
                GameObject obj_Arrow = BattleManager.Instance.pool.GetObject(3, isPlayer);
                obj_Arrow.transform.position = transform.GetChild(0).position;
                Arrow arrow = obj_Arrow.GetComponent<Arrow>();
                arrow.Shoot(this, target, atkDmg * 3f);
                cur_Mp = 0;
                Debug.Log("��ų ��� ( " + name + " -> " + target.name + " )");
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }
}