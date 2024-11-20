using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lumina : Enemy
{
    [SerializeField] GameObject attack_Eff;


    protected override void Start()
    {
        base.Start();
        // 최대 체력, 최대 마나, 방어력, 공격력, 공격속도, 사거리, 근접유무, 스킬유무, 경험치, 골드, 아이템 드랍
        InitStat(350, 5, 45, 5, 0.6f, 99f, false, true, GetExp(35), SetRandomGold(600), new Item().GenerateRandomItem(15), false);
        // 아이템 바꿔야됨

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
        GameObject obj_Arrow = Instantiate(attack_Eff);
        obj_Arrow.transform.position = transform.GetChild(0).position;

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

                BaseEntity target = FindMaxCurHpPlayer();

                GameObject obj_Arrow = BattleManager.Instance.pool.GetObject(2, isPlayer);
                obj_Arrow.transform.position = transform.GetChild(0).position;
                Arrow arrow = obj_Arrow.GetComponent<Arrow>();
                arrow.Shoot(this, target, DamageCalc(target, atkDmg) * 3f); // 현재 체력이 가장 많은 적에게 공격력의 3배 데미지의 투사체를 발사
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

    private BaseEntity FindMaxCurHpPlayer()
    {
        Ally temp_Player = null;
        float upperHp = float.MinValue;


        foreach (GameObject player in BattleManager.Instance.deploy_Player_List)
        {
            if (player != null)
            {
                Ally ally = player.GetComponent<Ally>();

                if (ally != null && ally.cur_Hp > 0)
                {
                    if (ally.cur_Hp > upperHp)
                    {
                        upperHp = ally.cur_Hp;
                        temp_Player = ally;
                    }
                }
            }
        }

        return temp_Player;
    }

}
