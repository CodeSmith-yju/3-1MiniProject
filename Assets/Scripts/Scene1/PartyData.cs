using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

/*public enum State { 
    Ranger, 
    Magician,
    Knight
}*/

public class PartyData
{
    //Party Panel Act
    /*public string strPartyName;
    public string strName;*/

    public string Type;

    public int cost = 128;
    public int index;
    public int level = 0;

    public Sprite spPartyIcon;

    //Party Stat .. hp, atk, ... ����־�ߵǳ�
    //public State myState;//�÷��̾�� �������� ������ ���º���

    // Ranger.cs���� ���� id, �ִ� HP, �ִ� MP, ���ݷ�, ���ݼӵ�, ���ݻ�Ÿ� ������ �ʱ�ȭ
    public int partyJobIndex;// ���� id == �����ε���
    public float partyHp;
    public float partyMp;
    public float partyAtk;
    public float partyAtkSpd;
    public float partyAtkRange;

    public bool partyAbleSkill;
    public bool isMelee;
    public GameObject obj_Data;
    public BaseEntity player;
    public PartyData(GameObject prefab, int _Lvel)
    {
        obj_Data = prefab;
        this.player = prefab.GetComponent<BaseEntity>();
        level = _Lvel;
        GenerateStat(player.job, _Lvel);

        //�� �Ʒ��� ������ ���� ����
        /*partyHp = player.max_Hp;
        partyMp = player.max_Mp;
        partyAtk = player.atkDmg;
        partyAtkSpd = player.atkSpd;
        partyAtkRange = player.atkRange;
        partyAbleSkill = player.able_Skill;*/
        Type = prefab.name;
        cost = Random.Range(50 + _Lvel*10, 100+ _Lvel*50);
        Debug.Log("cost: "+cost);
        spPartyIcon = player.GetComponent<SpriteRenderer>().sprite;
    }


    public void GenerateStat(BaseEntity.JobClass _Code, int _Lvel)
    {
        switch (_Code)
        {
            case BaseEntity.JobClass.Ranger:
                Debug.Log("Type Ranger, Generate Code: "+_Code);
                partyHp = 20f + (0.01f* _Lvel);
                partyMp = 5f + (0.01f * _Lvel);
                partyAtk = 2f;
                partyAtkSpd = 1.0f;
                partyAtkRange = 7f;
                Type = "Ranger";
                isMelee = false;
                break;
            case BaseEntity.JobClass.Wizard:
                Debug.Log("Type wizard, Generate Code: " + _Code);
                partyHp = 20f + (0.01f * _Lvel);
                partyMp = 5f + (0.01f * _Lvel);
                partyAtk = 2f;
                partyAtkSpd = 1.0f;
                partyAtkRange = 7f;
                Type = "wizard";
                isMelee = false;
                break;
            case BaseEntity.JobClass.Knight:
                Debug.Log("Type 3, Generate Code: " + _Code);
                partyHp = 20f + (0.01f * _Lvel);
                partyMp = 5f + (0.01f * _Lvel);
                partyAtk = 2f;
                partyAtkSpd = 1.0f;
                partyAtkRange = 2f;
                isMelee = true;
                break;
/*            case 0://Player
                break;*/
            default:
                Debug.Log("Type d, Generate Code: " + _Code);
                partyHp = 20f + (0.01f * _Lvel);
                partyMp = 5f + (0.01f * _Lvel);
                partyAtk = 2f;
                partyAtkSpd = 1.0f;
                partyAtkRange = 2f;
                isMelee = true;
                break;
        }
    }

}
