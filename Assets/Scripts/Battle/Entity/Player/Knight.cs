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
        Debug.Log("Knight 생성");
        type = Class.Tank;
        job = Job.Knight;
    }

    protected override void Skill()
    {
        base.Skill();
        if (_curstate == State.Skill)
        {
            StopCoroutine(SetAttack());
            if (isAttack)
            {
                // 방어력 올리는 버프 스킬로 하는 건 어떨까?
                BattleManager.Instance.ui.GenerateLog(GameUiMgr.single.entityIconRS.GetPortraitIcon(job), "돌진 찌르기");

                ani.SetBool("isSkill", true);
                BaseEntity target = FindTarget().GetComponent<BaseEntity>();
                Debug.Log("타겟의 적에게 1.3배의 데미지로 공격" + " " + (AttributeDamageCalc(target, DamageCalc(target, atkDmg) * 1.3f)) + "데미지");
                target.cur_Hp -= AttributeDamageCalc(target,DamageCalc(target, atkDmg) * 1.3f);
                cur_Mp = 0;
                Debug.Log("스킬 사용 ( " + name + " -> " + target.name + " )");

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