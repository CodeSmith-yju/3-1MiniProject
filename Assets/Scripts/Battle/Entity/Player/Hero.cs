using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Hero : Ally
{
    protected override void Start()
    {
        base.Start();
        Debug.Log("Hero ����");
        job = JobClass.Hero;
    }


    // ���߿� �ڷ�ƾ���� �ִϸ��̼��� üũ���� ���� �ִϸ��̼� Ŭ�� �̺�Ʈ�� ���� ���� (�ִϸ��̼� ����� �� �Ϸ�Ǹ� Idle ���·� �����ϵ���)
    protected override void Skill()
    {
        base.Skill();
        if (_curstate == State.Skill)
        {
            StopCoroutine(SetAttack());
            if (isAttack)
            {

                BattleManager.Instance.ui.GenerateLog(class_Portrait, "���� ����");

                BaseEntity target = FindTarget().GetComponent<BaseEntity>();
                Debug.Log("Ÿ���� ������ 2���� �������� �ѹ� ����" + " " + (atkDmg * 2) + "������");
                target.cur_Hp -= atkDmg * 2;
                cur_Mp = 0;
                Debug.Log("��ų ��� ( " + name + " -> " + target.name + " )");

                // �ִϸ��̼� �ֱ�
                ani.SetBool("isSkill", true); // �ӽ� �ִϸ��̼�
                Debug.Log("��ų �ִϸ��̼� ��");
                
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

    public void SkillAnimationDone()
    {
        ani.SetBool("isSkill", false);
        ChangeState(State.Idle);
        using_Skill = false;
    }

}