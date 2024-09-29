using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Hero : Ally
{
    protected override void Start()
    {
        base.Start();
        Debug.Log("Hero 생성");
        job = JobClass.Hero;
    }


    // 나중에 코루틴으로 애니메이션을 체크하지 말고 애니메이션 클립 이벤트로 제작 예정 (애니메이션 재생이 다 완료되면 Idle 상태로 변경하도록)
    protected override void Skill()
    {
        base.Skill();
        if (_curstate == State.Skill)
        {
            StopCoroutine(SetAttack());
            if (isAttack)
            {

                BattleManager.Instance.ui.GenerateLog(class_Portrait, "섬광 베기");

                BaseEntity target = FindTarget().GetComponent<BaseEntity>();
                Debug.Log("타겟의 적에게 2배의 데미지로 한번 공격" + " " + (atkDmg * 2) + "데미지");
                target.cur_Hp -= atkDmg * 2;
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

    public void SkillAnimationDone()
    {
        ani.SetBool("isSkill", false);
        ChangeState(State.Idle);
        using_Skill = false;
    }

}