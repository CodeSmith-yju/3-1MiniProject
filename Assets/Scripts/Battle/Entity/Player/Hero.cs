using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UI;

public class Hero : BaseEntity
{

    protected override void Start()
    {
        base.Start();
        Debug.Log("Player 생성");
        foreach (GameObject _player in BattleManager.Instance.party_List)
        {
            BaseEntity player_Stat = _player.GetComponent<BaseEntity>();

            if (player_Stat.entity_index == GetComponent<BaseEntity>().entity_index)
            {
                InitStat(entity_index);
            }
        }

        /*        
        */
        /*
                // 고유 id, 최대 HP, 최대 MP, 공격력, 공격속도, 공격사거리 순으로 초기화
                stat = new EntityStat
                    (0, GameMgr.playerData.max_Player_Hp, GameMgr.playerData.max_Player_Mp, GameMgr.playerData.base_atk_Dmg,
                    GameMgr.playerData.atk_Speed, GameMgr.playerData.atk_Range, GameMgr.playerData.skill_Able);*//*

                entity_id = stat.id;
                max_Hp = stat.max_Hp;
                cur_Hp = GameMgr.playerData.cur_Player_Hp;
                max_Mp = stat.max_Mp;
                cur_Mp = 0;
                atkDmg = stat.atkDmg;
                SetAttackSpeed(stat.atkSpd);
                atkRange = stat.atkRange;
                able_Skill = stat.isSkill;
                isMelee = true;*/

        //SetAttackSpeed(atkSpd);

    }

    protected override void Update()
    {
        base.Update();

        /*if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Battle)
        {
            GameMgr.playerData[entity_index].cur_Player_Hp = cur_Hp;
        }*/

        if (_curstate == State.Skill)
        {
            Skill();
        }
    }


    public void Skill()
    {
        if (_curstate == State.Skill)
        {
            
            StopAllCoroutines();
            if (isAttack)
            {
           
                BaseEntity target = FindTarget().GetComponent<BaseEntity>();
                Debug.Log("타겟의 적에게 2배의 데미지로 한번 공격" + " " + (atkDmg * 2) + "데미지");
                target.cur_Hp -= atkDmg * 2;
                Debug.Log(target.cur_Hp + " " + target.name);
            }
            else
            {
                return;
            }
            cur_Mp = 0;
            ChangeState(State.Idle);
        }
    }
}