using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Demon : Ally
{
    [SerializeField] RuntimeAnimatorController metamorphosis_Ani;
    bool isMeta = false;

    protected override void Start()
    {
        base.Start();
        Debug.Log("Demon ����");
        isMeta = false;
        type = Class.Melee;
        job = Job.Demon;
    }

    protected override void Skill()
    {
        base.Skill();
        if (_curstate == State.Skill)
        {
            StopCoroutine(SetAttack());

            if (!isMeta)
            {
                // ����
                BattleManager.Instance.ui.GenerateLog(class_Portrait, "��Ÿ����ý�");

                ani.SetBool("isSkill", true);
            }
            else
            {
                if (isAttack)
                {
                    BattleManager.Instance.ui.GenerateLog(class_Portrait, "��ų �̸�");

                    // ���� �� ��ų
                    ani.SetBool("isSkill", true);

                    // ��ų ȿ��
                    // ������ �ѹ� ��ų�� �������? ������ ��� �ϵ��� ����?

                    cur_Mp = 0;
                }
                else
                {
                    return;
                }
            }
        }
        else
        {
            return;
        }
    }

    private void MetaAfterStat()
    {
        isMadness = true; // �޴� ���� 15% ���� �����
        atkDmg += 1f;
        atkSpd += 0.1f;
        atkRange += 0.5f;

        max_Mp = 8;
        cur_Mp = 0;
    }

    public void Metamorphosis()
    {
        ani.SetBool("isSkill", false);
        ani.runtimeAnimatorController = metamorphosis_Ani;
        isMeta = true;
        MetaAfterStat();
        ChangeState(State.Idle);
        using_Skill = false;
    }

}