using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : BaseEntity
{
    public string player_Name;
    public int level;
    public Sprite class_Portrait;

    public enum Class
    {
        Tank, // 아이콘 - 방패 (탱커)
        Melee, // 아이콘 - 검 (근거리 딜러)
        Range, // 아이콘 - 활 (원거리 딜러)
        Support // 아이콘 -   (버퍼, 디버퍼, 힐러)
    }


    public enum Job
    {
        Hero,
        Knight,
        Ranger,
        Wizard,
        Priest,
        Demon
    }

    public Job job;
    public Class type;

    protected override void Start()
    {
        base.Start();
        isPlayer = true;
    }



    protected override void Update()
    {
        base.Update();
        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Deploy || BattleManager.Instance._curphase == BattleManager.BattlePhase.Battle)
        {
            UpdateCurrentHPMPToSingle();
        }

        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Rest)
        {
            cur_Hp = GameMgr.playerData[entity_index].cur_Player_Hp;
            UpdateResetMp();
        }
    }

    public void UpdateCurrentHPMPToSingle()
    {
        GameMgr.playerData[entity_index].cur_Player_Hp = cur_Hp;
        GameMgr.playerData[entity_index].cur_Player_Mp = cur_Mp;
    }

    private void UpdateResetMp()
    {
        cur_Mp = 0;
        GameMgr.playerData[entity_index].cur_Player_Mp = cur_Mp;
    }

    // 안쓰는 코드
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
            GameMgr.playerData[index].defensePoint,
            GameMgr.playerData[index].max_Player_Mp,
            GameMgr.playerData[index].base_atk_Dmg,
            GameMgr.playerData[index].atk_Speed,
            GameMgr.playerData[index].atk_Range,
            GameMgr.playerData[index].isMelee,
            GameMgr.playerData[index].skill_Able
            );

        entity_index = stat.index;
        player_Name = stat.entity_name;
        level = stat.level;
        max_Hp = stat.max_Hp;
        cur_Hp = stat.cur_hp;
        max_Mp = stat.max_Mp;
        cur_Mp = 0f;
        atkDmg = stat.atkDmg;
        SetAttackSpeed(Mathf.Clamp(stat.atkSpd, 0.1f, 2f)); // 0.1 ~ 2 까지 제한
        isMelee = stat.isMelee;
        if (isMelee)
            atkRange = Mathf.Clamp(stat.atkRange, 0.5f, 2.5f); // 근접 유닛은 0.5 ~ 2.5까지 제한
        else
            atkRange = Mathf.Clamp(stat.atkRange, 0.5f, 10f); // 원거리 유닛은 0.5 ~ 10까지 제한
        able_Skill = stat.able_Skill;

    }

    // 사운드 수정 예정
    public void AttackSound(int index)
    {
        AudioManager.single.PlayerSound(index, index, 1);
    }

    public void DieSound(int index)
    {
       AudioManager.single.PlayerSound(index, index, 0);
    }

    public void SkillSound(int index)
    {
        AudioManager.single.PlayerSound(index, index, 2);
    }

}
