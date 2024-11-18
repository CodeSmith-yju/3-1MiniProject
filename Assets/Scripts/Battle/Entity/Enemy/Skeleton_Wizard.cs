using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Wizard : Enemy
{
    [SerializeField] GameObject skill_Eff;


    protected override void Start()
    {
        base.Start();
        // �ִ� ü��, �ִ� ����, ����, ���ݷ�, ���ݼӵ�, ��Ÿ�, ��������, ��ų����, ����ġ, ���, ������ ���
        InitStat(250, 6, 30, 7, 0.85f, 99f, false, true, GetExp(20), SetRandomGold(400), new Item().GenerateRandomItem(15), false);
        // ������ �ٲ�ߵ�

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
                foreach (GameObject enemys in players)
                {
                    Ally enemy = enemys.GetComponent<Ally>();

                    if (enemy._curstate != State.Death)
                    {
                        GameObject skill = Instantiate(skill_Eff, enemy.transform);
                        skill.transform.localPosition = new Vector3(0, 1.4f, 0);
                        enemy.cur_Hp -= 10f;
                    }
                }
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
