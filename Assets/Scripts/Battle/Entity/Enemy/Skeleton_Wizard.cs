using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Wizard : Enemy
{
    [SerializeField] GameObject skill_Eff;


    protected override void Start()
    {
        base.Start();
        // 최대 체력, 최대 마나, 방어력, 공격력, 공격속도, 사거리, 근접유무, 스킬유무, 경험치, 골드, 아이템 드랍, 광역공격
        InitStat(70, 4, 20, 3.5f, 0.7f, 7.5f, false, true, GetExp(10), SetRandomGold(250), new Item().GenerateRandomItem(19), false);

        if (BattleManager.Instance.dialogue != null && BattleManager.Instance.dialogue.isTutorial)
            item_Drop_Check = false;
        else
            item_Drop_Check = ShouldDropItem(20);

        Debug.Log("경험치 설정 : " + exp_Cnt);
        Debug.Log("골드 설정 : " + gold_Cnt);
        Debug.Log("아이템 드랍 유무 설정 : " + item_Drop_Check);
    }

    protected override void RangeAttack(BaseEntity target)
    {
        base.RangeAttack(target);
        GameObject obj_Arrow = BattleManager.Instance.pool.GetObject(1, isPlayer);
        obj_Arrow.transform.position = transform.GetChild(0).position;
        Arrow arrow = obj_Arrow.GetComponent<Arrow>();
        arrow.Shoot(this, target, atkDmg);
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
                List<GameObject> players = BattleManager.Instance.deploy_Player_List;

                ani.SetBool("isSkill", true); // 스킬 애니메이션

                BaseEntity target = FindTarget().GetComponent<BaseEntity>();

                GameObject skill = Instantiate(skill_Eff, target.transform);
                skill.transform.localPosition = new Vector3(0, 1.4f, 0);
                target.cur_Hp -= AttributeDamageCalc(target, DamageCalc(target, atkDmg) * 2f);


                cur_Mp = 0;
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
