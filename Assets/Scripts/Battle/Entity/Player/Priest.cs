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
        Debug.Log("Priest 생성");
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
                BattleManager.Instance.ui.GenerateLog(class_Portrait, "치유의 빛");

                ani.SetBool("isSkill", true);
                Ally ally = FindMinCurHpPlayer(); // 현재 체력이 가장 낮은 플레이어 가져옴

                ally.cur_Hp += (5f + atkDmg * 0.5f);
                Debug.Log("현재 체력이 가장 낮은 플레이어에게 기본 회복력 5 + 공격력의 50%만큼 회복합니다.");

                // 여기에 치유 이펙트를 생성하는 방향으로 할까? 애니메이션 재생이 끝나면 파괴 되도록 하고

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