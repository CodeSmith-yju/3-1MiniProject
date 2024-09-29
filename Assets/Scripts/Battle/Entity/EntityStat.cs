using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStat : MonoBehaviour
{
    public int index;
    public string entity_name;
    public int level;
    public float max_Hp;
    public float cur_hp;
    public float max_Mp;
    public float atkDmg;
    public float atkSpd;
    public float atkRange;
    public bool isMelee;
    public bool able_Skill;
    public float exp;
    public int gold;
    public Item item;

    public EntityStat(int index, string entity_name, int level, float max_Hp, float cur_hp, float max_Mp, float atkDmg, float atkSpd, float atkRange, bool isMelee, bool able_Skill)
    {
        this.index = index;
        this.entity_name = entity_name;
        this.level = level;
        this.max_Hp = max_Hp;
        this.cur_hp = cur_hp;  
        this.max_Mp = max_Mp;
        this.atkDmg = atkDmg;
        this.atkSpd = atkSpd;
        this.atkRange = atkRange;
        this.isMelee = isMelee;
        this.able_Skill = able_Skill;
    }


    public EntityStat(float max_Hp, float max_Mp, float atkDmg, float atkSpd, float atkRange, bool isMelee, bool able_Skill, float exp, int gold, Item item)
    {
        this.max_Hp = max_Hp;
        this.max_Mp = max_Mp;
        this.atkDmg = atkDmg;
        this.atkSpd = atkSpd;
        this.atkRange = atkRange;
        this.isMelee = isMelee;
        this.able_Skill = able_Skill;
        this.exp = exp;
        this.gold = gold;
        this.item = item;
    }

}
