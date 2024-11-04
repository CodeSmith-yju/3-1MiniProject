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
        Debug.Log("Demon 생성");
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
                // 변신
                BattleManager.Instance.ui.GenerateLog(class_Portrait, "메타포모시스");

                ani.SetBool("isSkill", true);
            }
            else
            {
                if (isAttack)
                {
                    BattleManager.Instance.ui.GenerateLog(class_Portrait, "스킬 이름");

                    // 변신 후 스킬
                    ani.SetBool("isSkill", true);

                    // 스킬 효과
                    // 묵직한 한방 스킬을 사용할지? 여러번 사용 하도록 할지?

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
        isMadness = true; // 받는 피해 15% 증가 디버프
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