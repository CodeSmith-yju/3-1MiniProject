using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lumina : Enemy
{
    [SerializeField] GameObject attack_Eff;


    protected override void Start()
    {
        base.Start();
        // �ִ� ü��, �ִ� ����, ����, ���ݷ�, ���ݼӵ�, ��Ÿ�, ��������, ��ų����, ����ġ, ���, ������ ���
        InitStat(350, 5, 45, 5, 0.6f, 99f, false, true, GetExp(35), SetRandomGold(600), new Item().GenerateRandomItem(15), false);
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
        GameObject obj_Arrow = Instantiate(attack_Eff, transform.GetChild(0));
        obj_Arrow.transform.localPosition = new Vector3(-7.5f, 0);

        List<GameObject> players = BattleManager.Instance.deploy_Player_List;

        foreach (GameObject enemys in players)
        {
            Ally enemy = enemys.GetComponent<Ally>();

            if (enemy._curstate != State.Death)
            {
                RangeHit(enemy, atkDmg);
            }
        }
    }


    protected override void Skill()
    {
        base.Skill();
        if (_curstate == State.Skill)
        {
            StopCoroutine(SetAttack());
            if (isAttack)
            {
                ani.SetBool("isSkill", true);
                BaseEntity target = FindMinCurHpPlayer();

                GameObject obj_Arrow = BattleManager.Instance.pool.GetObject(2, isPlayer);
                obj_Arrow.transform.position = transform.GetChild(0).position;
                Arrow arrow = obj_Arrow.GetComponent<Arrow>();
                arrow.Shoot(this, target, DamageCalc(target, atkDmg) * 3f); // ���� ü���� ���� ���� ������ 3�� ������
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

    private BaseEntity FindMinCurHpPlayer()
    {
        Ally temp_Player = null;
        float lowerHp = float.MaxValue;


        foreach (GameObject player in BattleManager.Instance.deploy_Player_List)
        {
            if (player != null)
            {
                Ally ally = player.GetComponent<Ally>();

                if (ally != null && ally.cur_Hp > 0 && ally.max_Hp > 0)
                {

                    float hpRatio = ally.cur_Hp / ally.max_Hp;

                    if (hpRatio < lowerHp)
                    {
                        lowerHp = hpRatio;
                        temp_Player = ally;
                    }
                }
            }
        }

        return temp_Player;
    }

}
