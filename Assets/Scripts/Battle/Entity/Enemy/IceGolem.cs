using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGolem : Enemy
{
    protected override void Start()
    {
        base.Start();
        // 최대 체력, 최대 마나, 방어력, 공격력, 공격속도, 사거리, 근접유무, 스킬유무, 경험치, 골드, 아이템 드랍
        InitStat(140, 3, 20, 6, 0.75f, 1.2f, true, true, GetExp(10), SetRandomGold(300), new Item().GenerateRandomItem(Random.Range(8, 12)), false);

        if (BattleManager.Instance.dialogue != null && BattleManager.Instance.dialogue.isTutorial)
            item_Drop_Check = false;
        else
            item_Drop_Check = ShouldDropItem(10);

        Debug.Log("경험치 설정 : " + exp_Cnt);
        Debug.Log("골드 설정 : " + gold_Cnt);
        Debug.Log("아이템 드랍 유무 설정 : " + item_Drop_Check);


    }

    // 나중에 코루틴으로 애니메이션을 체크하지 말고 애니메이션 클립 이벤트로 제작 예정 (애니메이션 재생이 다 완료되면 Idle 상태로 변경하도록)
    protected override void Skill()
    {
        base.Skill();
        if (_curstate == State.Skill)
        {
            StopCoroutine(SetAttack());

            ani.SetBool("isSkill", true); // 스킬 애니메이션

            // 3초간 데미지 무효
            StartCoroutine(SkillCasting());
            cur_Mp = 0;

        }
        else
        {
            return;
        }
    }


    // 3초간 데미지 무효
    private IEnumerator SkillCasting()
    {
        isInvulnerable = true;
        float temp_AtkSpd = atkSpd;
        atkSpd = 0f;
        if (isAttack)
            isAttack = false;
        

        yield return new WaitForSeconds(3f);

        atkSpd = temp_AtkSpd;
        isInvulnerable = false;

        ani.SetBool("isSkill", false);
        ChangeState(State.Idle);
        using_Skill = false;
    }
}
