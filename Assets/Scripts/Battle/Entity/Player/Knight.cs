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
        job = JobClass.Knight;
    }

    protected override void Skill()
    {
        base.Skill();
        if (_curstate == State.Skill)
        {
            StopCoroutine(SetAttack());
            if (isAttack)
            {

                BattleManager.Instance.ui.GenerateLog(class_Portrait, "���� ���");

                BaseEntity target = FindTarget().GetComponent<BaseEntity>();
                Debug.Log("Ÿ���� ������ 1.3���� �������� ����" + " " + (atkDmg * 1.3) + "������");
                target.cur_Hp -= atkDmg * 1.3f;
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