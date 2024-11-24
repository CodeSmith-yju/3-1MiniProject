using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Knight : Ally
{
    protected override void Start()
    {
        base.Start();
        Debug.Log("Knight ����");
        type = Class.Tank;
        job = Job.Knight;
    }

    protected override void Skill()
    {
        base.Skill();
        if (_curstate == State.Skill)
        {
            StopCoroutine(SetAttack());
            if (isAttack)
            {
                // ���� �ø��� ���� ��ų�� �ϴ� �� ���?
                BattleManager.Instance.ui.GenerateLog(GameUiMgr.single.entityIconRS.GetPortraitIcon(job), "���� ���");

                ani.SetBool("isSkill", true);
                BaseEntity target = FindTarget().GetComponent<BaseEntity>();
                Debug.Log("Ÿ���� ������ 1.3���� �������� ����" + " " + (AttributeDamageCalc(target, DamageCalc(target, atkDmg) * 1.3f)) + "������");
                target.cur_Hp -= AttributeDamageCalc(target,DamageCalc(target, atkDmg) * 1.3f);
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