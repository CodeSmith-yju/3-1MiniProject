using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Wizard : Enemy
{
    [SerializeField] GameObject skill_Eff;


    protected override void Start()
    {
        base.Start();
        // �ִ� ü��, �ִ� ����, ���ݷ�, ���ݼӵ�, ��Ÿ�, ��������, ��ų����, ����ġ, ���, ������ ���
        InitStat(200, 10, 5, 0.7f, 50f, false, true, 20, SetRandomGold(400), ItemResources.instance.itemRS[15]);
        // ������ �ٲ�ߵ�

        if (BattleManager.Instance.dialogue != null && BattleManager.Instance.dialogue.isTutorial)
            item_Drop_Check = false;
        else
            item_Drop_Check = ShouldDropItem(5);

        Debug.Log("����ġ ���� : " + exp_Cnt);
        Debug.Log("��� ���� : " + gold_Cnt);
        Debug.Log("������ ��� ���� ���� : " + item_Drop_Check);
    }

    public override void RangeAttack(BaseEntity target)
    {
        base.RangeAttack(target);
        GameObject obj_Arrow = BattleManager.Instance.pool.GetObject(1, !isPlayer);
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
                
                foreach (GameObject enemys in players)
                {
                    Ally enemy = enemys.GetComponent<Ally>();

                    if (enemy._curstate != State.Death)
                    {
                        Instantiate(skill_Eff, enemy.transform);
                        enemy.cur_Hp -= 10f;
                    }
                }
                cur_Mp = 0;
                ani.SetBool("isSkill", true); // ��ų �ִϸ��̼�
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
