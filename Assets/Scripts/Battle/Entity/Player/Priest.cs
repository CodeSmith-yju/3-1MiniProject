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

            BattleManager.Instance.ui.GenerateLog(GameUiMgr.single.entityIconRS.GetPortraitIcon(job), "치유의 빛");

            ani.SetBool("isSkill", true);
            Ally ally = FindMinCurHpPlayer(); // 최대 체력 대비 가장 낮은 현재 체력을 가진 플레이어 찾기
            Instantiate(skillEff, ally.transform);

            float heal = 5f + max_Hp * 0.15f;

            if (ally.cur_Hp + heal < ally.max_Hp)
                ally.cur_Hp += heal;
            else if (ally.cur_Hp + heal >= ally.max_Hp)
                ally.cur_Hp = ally.max_Hp;

            cur_Mp = 0;
            Debug.Log("현재 체력이 가장 낮은 플레이어에게 기본 회복력 5 + 최대 체력의 15%만큼 회복합니다.");
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

                if (ally != null && ally.cur_Hp > 0 && ally.max_Hp > 0)
                {
                    float hpRatio = ally.cur_Hp / ally.max_Hp;

                    if (hpRatio < lowerHp) 
                    {
                        lowerHp = hpRatio;
                        temp_Player = ally;
                    }
                }
            }
        }

        return temp_Player;
    }

}