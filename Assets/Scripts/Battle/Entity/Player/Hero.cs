using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Hero : Ally
{
    bool using_Skill = false;


    protected override void Start()
    {
        base.Start();
        Debug.Log("Hero 생성");
        job = JobClass.Hero;
    }

    protected override void Update()
    {
        base.Update();

        if (_curstate == State.Skill && !using_Skill)
        {
            StartCoroutine(Skill());
        }
    }


    private IEnumerator Skill()
    {
        using_Skill = true;
        if (_curstate == State.Skill)
        {
            StopCoroutine(SetAttack());
            if (isAttack)
            {
                // 애니메이션 넣기
                ani.SetTrigger("isAtk"); // 임시 애니메이션

                BaseEntity target = FindTarget().GetComponent<BaseEntity>();
                Debug.Log("타겟의 적에게 2배의 데미지로 한번 공격" + " " + (atkDmg * 2) + "데미지");
                target.cur_Hp -= atkDmg * 2;
                cur_Mp = 0;
                Debug.Log("스킬 사용 ( " + name + " -> " + target.name + " )");

                yield return new WaitForSeconds(ani.GetCurrentAnimatorStateInfo(0).length);
                ChangeState(State.Idle);
                using_Skill = false;
            }
            else
            {
                yield break;
            }
        }
        else
        {
            yield break;
        }
    }



}