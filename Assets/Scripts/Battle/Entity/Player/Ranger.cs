using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Ranger : Ally
{
    protected override void Start()
    {
        base.Start();
        job = JobClass.Ranger;
        Debug.Log("Ranger 생성");
    }


    public override void RangeAttack(BaseEntity target)
    {
        base.RangeAttack(target);
        GameObject obj_Arrow = BattleManager.Instance.pool.GetObject(0);
        obj_Arrow.transform.position = transform.GetChild(0).position;
        Arrow arrow = obj_Arrow.GetComponent<Arrow>();
        arrow.Shoot(this, target, atkDmg);
    }


    protected override void Skill()
    {
        base.Skill();
        if (_curstate == State.Skill)
        {
            StopCoroutine(SetAttack());
            if (isAttack)
            {

                BattleManager.Instance.ui.GenerateLog(class_Portrait, "가시 화살");

                BaseEntity target = FindTarget().GetComponent<BaseEntity>();
                Debug.Log("타겟의 적에게 1.2배의 데미지로 공격" + " " + (atkDmg * 1.2) + "데미지");
                GameObject obj_Arrow = BattleManager.Instance.pool.GetObject(1);
                obj_Arrow.transform.position = transform.GetChild(0).position;
                Arrow arrow = obj_Arrow.GetComponent<Arrow>();
                arrow.Shoot(this, target, atkDmg * 1.2f);
                cur_Mp = 0;
                Debug.Log("스킬 사용 ( " + name + " -> " + target.name + " )");

                // 애니메이션 넣기
                ani.SetBool("isSkill", true); // 임시 애니메이션
                Debug.Log("스킬 애니메이션 끝");

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