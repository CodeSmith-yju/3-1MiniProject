using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : BaseEntity
{
    public string player_Name;
    public int level;
    public Sprite class_Icon;

    public enum JobClass
    {
        Hero,
        Knight,
        Ranger,
        Wizard
    }

    public JobClass job;

    protected override void Start()
    {
        base.Start();
    }



    protected override void Update()
    {
        base.Update();
        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Deploy || BattleManager.Instance._curphase == BattleManager.BattlePhase.Battle)
        {
            UpdateCurrentHPToSingle();
        }
    }

    public void UpdateCurrentHPToSingle()
    {
        GameMgr.playerData[entity_index].cur_Player_Hp = cur_Hp; 
    }


    /*public void Init(int index, PlayerData player)
    {
        entity_index = index;
        player_Name = player.GetPlayerName();
        level = player.player_level;
        max_Hp = player.max_Player_Hp;
        cur_Hp = max_Hp;
        max_Mp = player.max_Player_Mp;
        cur_Mp = 0;
        atkDmg = player.base_atk_Dmg;
        atkSpd = player.atk_Speed;
        atkRange = player.atk_Range;
        isMelee = player.isMelee;
        able_Skill = player.skill_Able;
    }*/

    public void InitStat(int index)
    {
        stat = new(
            GameMgr.playerData[index].playerIndex,
            GameMgr.playerData[index].GetPlayerName(),
            GameMgr.playerData[index].player_level,
            GameMgr.playerData[index].max_Player_Hp,
            GameMgr.playerData[index].cur_Player_Hp,
            GameMgr.playerData[index].max_Player_Mp,
            GameMgr.playerData[index].base_atk_Dmg,
            GameMgr.playerData[index].atk_Speed,
            GameMgr.playerData[index].atk_Range,
            GameMgr.playerData[index].isMelee,
            GameMgr.playerData[index].skill_Able
            );

        entity_index = stat.index;
        player_Name = stat.name;
        level = stat.level;
        max_Hp = stat.max_Hp;
        cur_Hp = stat.cur_hp;
        max_Mp = stat.max_Mp;
        cur_Mp = 0f;
        atkDmg = stat.atkDmg;
        SetAttackSpeed(stat.atkSpd);
        atkRange = stat.atkRange;
        isMelee = stat.isMelee;
        able_Skill = stat.able_Skill;
    }


    public void AttackSound(int index)
    {
        AudioManager.single.PlayerSound(index, index, 1);
    }

    public void DieSound(int index)
    {
        AudioManager.single.EnemySound(index, index, 0);
    }


}
