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
        type = Class.Melee;
        job = Job.Hero;
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

                BattleManager.Instance.ui.GenerateLog(GameUiMgr.single.entityIconRS.GetPortraitIcon(job), "���� ����");

                BaseEntity target = FindTarget().GetComponent<BaseEntity>();
                Debug.Log("Ÿ���� ������ 2���� �������� ����" + " " + (AttributeDamageCalc(target, DamageCalc(target, atkDmg) * 2f)) + "������");
                target.cur_Hp -= AttributeDamageCalc(target, DamageCalc(target, atkDmg) * 2f);
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
}