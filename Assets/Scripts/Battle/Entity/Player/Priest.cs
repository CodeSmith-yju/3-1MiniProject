using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Priest : Ally
{
    [SerializeField] GameObject SkillEff;

    protected override void Start()
    {
        base.Start();
        Debug.Log("Priest ����");
        type = Class.Support;
        job = Job.Priest;
    }

    protected override void MeleeAttack(BaseEntity target)
    {
        base.MeleeAttack(target);

        GameObject obj_Eff = BattleManager.Instance.pool.GetObject(4, isPlayer);
        obj_Eff.transform.SetParent(target.transform);
        obj_Eff.transform.localPosition = Vector3.zero;
    }


    protected override void Skill()
    {
        base.Skill();
        if (_curstate == State.Skill)
        {
            StopCoroutine(SetAttack());
            if (isAttack)
            {
                BattleManager.Instance.ui.GenerateLog(class_Portrait, "ġ���� ��");

                ani.SetBool("isSkill", true);
                Ally ally = FindMinCurHpPlayer(); // ���� ü���� ���� ���� �÷��̾� ������

                ally.cur_Hp += (5f + atkDmg * 0.5f);
                Debug.Log("���� ü���� ���� ���� �÷��̾�� �⺻ ȸ���� 5 + ���ݷ��� 50%��ŭ ȸ���մϴ�.");

                // ���⿡ ġ�� ����Ʈ�� �����ϴ� �������� �ұ�? �ִϸ��̼� ����� ������ �ı� �ǵ��� �ϰ�

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


    private Ally FindMinCurHpPlayer()
    {
        Ally temp_Player = null;
        float lowerHp = float.MaxValue; 


        foreach (GameObject player in BattleManager.Instance.deploy_Player_List)
        {
            if (player != null)
            {
                Ally ally = player.GetComponent<Ally>();

                if (ally != null && ally.cur_Hp > 0)
                {
                    if (ally.cur_Hp < lowerHp) 
                    {
                        lowerHp = ally.cur_Hp;
                        temp_Player = ally;
                    }
                }
            }
        }

        return temp_Player;
    }

}