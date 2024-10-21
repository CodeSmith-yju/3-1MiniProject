using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGolem : Enemy
{
    protected override void Start()
    {
        base.Start();
        // �ִ� ü��, �ִ� ����, ���ݷ�, ���ݼӵ�, ��Ÿ�, ��������, ��ų����, ����ġ, ���, ������ ���
        InitStat(140, 0, 5, 0.75f, 1.3f, true, false, 10, SetRandomGold(300), ItemResources.instance.itemRS[19]);

        if (BattleManager.Instance.dialogue != null && BattleManager.Instance.dialogue.isTutorial)
            item_Drop_Check = false;
        else
            item_Drop_Check = ShouldDropItem(30);

        Debug.Log("����ġ ���� : " + exp_Cnt);
        Debug.Log("��� ���� : " + gold_Cnt);
        Debug.Log("������ ��� ���� ���� : " + item_Drop_Check);


    }

    // ���߿� �ڷ�ƾ���� �ִϸ��̼��� üũ���� ���� �ִϸ��̼� Ŭ�� �̺�Ʈ�� ���� ���� (�ִϸ��̼� ����� �� �Ϸ�Ǹ� Idle ���·� �����ϵ���)
    protected override void Skill()
    {
        base.Skill();
        if (_curstate == State.Skill)
        {
            StopCoroutine(SetAttack());

            ani.SetBool("isSkill", true); // ��ų �ִϸ��̼�

            // 3�ʰ� ������ ��ȿ
            StartCoroutine(SkillCasting());
            cur_Mp = 0;

        }
        else
        {
            return;
        }
    }


    // 3�ʰ� ������ ��ȿ
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