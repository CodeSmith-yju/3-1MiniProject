using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Priest : Ally
{
    [SerializeField] GameObject skillEff;
    [SerializeField] GameObject attackEff;

    protected override void Start()
    {
        base.Start();
        Debug.Log("Priest 생성");
        type = Class.Support;
        job = Job.Priest;
    }

    protected override void RangeAttack(BaseEntity target)
    {
        base.RangeAttack(target);

        GameObject obj_Eff = Instantiate(attackEff, target.transform);
        obj_Eff.transform.localPosition = Vector3.zero;

        RangeHit(target, atkDmg);
    }


    protected override void Skill()
    {
        base.Skill();
        if (_curstate == State.Skill)
        {
            StopCoroutine(SetAttack());
            if (isAttack)
            {
                BattleManager.Instance.ui.GenerateLog(class_Portrait, "치유의 빛");

                ani.SetBool("isSkill", true);
                Ally ally = FindMinCurHpPlayer(); // 현재 체력이 가장 낮은 플레이어 가져옴
                Instantiate(skillEff, ally.transform);

                float heal = 5f + max_Hp * 0.1f;

                if (ally.cur_Hp + heal < ally.max_Hp)
                    ally.cur_Hp += heal;
                else if (ally.cur_Hp + heal >= ally.max_Hp)
                    ally.cur_Hp = ally.max_Hp;

                cur_Mp = 0;
                Debug.Log("현재 체력이 가장 낮은 플레이어에게 기본 회복력 5 + 최대 체력의 10%만큼 회복합니다.");

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