using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Hero : Ally
{
    bool using_Skill = false;


    protected override void Start()
    {
        base.Start();
        Debug.Log("Hero ����");
        job = JobClass.Hero;
    }

    protected override void Update()
    {
        base.Update();

        if (_curstate == State.Skill && using_Skill == false)
        {
            Skill();
        }
    }


    // ���߿� �ڷ�ƾ���� �ִϸ��̼��� üũ���� ���� �ִϸ��̼� Ŭ�� �̺�Ʈ�� ���� ���� (�ִϸ��̼� ����� �� �Ϸ�Ǹ� Idle ���·� �����ϵ���)
    private void Skill()
    {
        using_Skill = true;
        if (_curstate == State.Skill)
        {
            StopCoroutine(SetAttack());
            if (isAttack)
            {
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