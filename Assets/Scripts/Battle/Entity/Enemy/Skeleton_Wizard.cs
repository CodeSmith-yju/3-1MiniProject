using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Wizard : Enemy
{
    [SerializeField] GameObject skill_Eff;


    protected override void Start()
    {
        base.Start();
        // �ִ� ü��, �ִ� ����, ����, ���ݷ�, ���ݼӵ�, ��Ÿ�, ��������, ��ų����, ����ġ, ���, ������ ���, ��������
        InitStat(70, 4, 20, 3.5f, 0.7f, 7.5f, false, true, GetExp(10), SetRandomGold(250), new Item().GenerateRandomItem(19), false);

        if (BattleManager.Instance.dialogue != null && BattleManager.Instance.dialogue.isTutorial)
            item_Drop_Check = false;
        else
            item_Drop_Check = ShouldDropItem(20);

        Debug.Log("����ġ ���� : " + exp_Cnt);
        Debug.Log("��� ���� : " + gold_Cnt);
        Debug.Log("������ ��� ���� ���� : " + item_Drop_Check);
    }

    protected override void RangeAttack(BaseEntity target)
    {
        base.RangeAttack(target);
        GameObject obj_Arrow = BattleManager.Instance.pool.GetObject(1, isPlayer);
        obj_Arrow.transform.position = transform.GetChild(0).position;
        Arrow arrow = obj_Arrow.GetComponent<Arrow>();
        arrow.Shoot(this, target, atkDmg);
    }


    // ���߿� �ڷ�ƾ���� �ִϸ��̼��� üũ���� ���� �ִϸ��̼� Ŭ�� �̺�Ʈ�� ���� ���� (�ִϸ��̼� ����� �� �Ϸ�Ǹ� Idle ���·� �����ϵ���)
    protected override void Skill()
    {
        base.Skill();
        if (_curstate == State.Skill)
        {
            StopCoroutine(SetAttack());
            if (isAttack)
            {
                List<GameObject> players = BattleManager.Instance.deploy_Player_List;

                ani.SetBool("isSkill", true); // ��ų �ִϸ��̼�

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
