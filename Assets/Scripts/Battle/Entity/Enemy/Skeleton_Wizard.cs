using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Wizard : Enemy
{
    [SerializeField] GameObject skill_Eff;


    protected override void Start()
    {
        base.Start();
        // 최대 체력, 최대 마나, 공격력, 공격속도, 사거리, 근접유무, 스킬유무, 경험치, 골드, 아이템 드랍
        InitStat(130, 10, 5, 0.6f, 50f, false, true, 20, SetRandomGold(400), ItemResources.instance.itemRS[4]);
        // 아이템 바꿔야됨

        if (BattleManager.Instance.dialogue != null && BattleManager.Instance.dialogue.isTutorial)
            item_Drop_Check = false;
        else
            item_Drop_Check = ShouldDropItem(15);

        Debug.Log("경험치 설정 : " + exp_Cnt);
        Debug.Log("골드 설정 : " + gold_Cnt);
        Debug.Log("아이템 드랍 유무 설정 : " + item_Drop_Check);
    }

    public override void RangeAttack(BaseEntity target)
    {
        base.RangeAttack(target);
        GameObject obj_Arrow = BattleManager.Instance.pool.GetObject(3);
        obj_Arrow.transform.position = transform.GetChild(0).position;
        Arrow arrow = obj_Arrow.GetComponent<Arrow>();
        arrow.Shoot(this, target);
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
                Ally[] players = FindObjectsOfType<Ally>();
                
                foreach (Ally enemy in players)
                {
                    if (enemy._curstate != State.Death)
                    {
                        Instantiate(skill_Eff, enemy.transform);
                        enemy.cur_Hp -= 10f;
                    }
                }
                cur_Mp = 0;
                ani.SetBool("isSkill", true); // 스킬 애니메이션
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
