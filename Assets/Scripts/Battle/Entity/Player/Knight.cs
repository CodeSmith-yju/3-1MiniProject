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

                BattleManager.Instance.ui.GenerateLog(class_Portrait, "돌진 찌르기");

                ani.SetBool("isSkill", true);
                BaseEntity target = FindTarget().GetComponent<BaseEntity>();
                Debug.Log("타겟의 적에게 1.3배의 데미지로 공격" + " " + (atkDmg * 1.3) + "데미지");
                target.cur_Hp -= atkDmg * 1.3f;
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