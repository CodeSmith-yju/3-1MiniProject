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

    public GameObject obj_Data;
    public PartyData(GameObject prefab, int _Lvel)
    {
        obj_Data = prefab;
        BaseEntity player = prefab.GetComponent<BaseEntity>();
        level = _Lvel;
        partyJobIndex = player.entity_id;
        Debug.Log(partyJobIndex);
        GenerateStat(partyJobIndex, _Lvel);

        //�� �Ʒ��� ������ ���� ����
        /*partyHp = player.max_Hp;
        partyMp = player.max_Mp;
        partyAtk = player.atkDmg;
        partyAtkSpd = player.atkSpd;
        partyAtkRange = player.atkRange;
        partyAbleSkill = player.able_Skill;*/
        Type = "Ranger";
        cost = Random.Range(50 + _Lvel*10, 100+ _Lvel*50);
        Debug.Log("cost: "+cost);
        spPartyIcon = player.GetComponent<SpriteRenderer>().sprite;
    }


    public void GenerateStat(int _Code, int _Lvel)
    {
        switch (_Code)
        {
            case 1:
                Debug.Log("Type Ranger, Generate Code: "+_Code);
                partyHp = 20f + (0.01f* _Lvel);
                partyMp = 5f + (0.01f * _Lvel);
                partyAtk = 2f;
                partyAtkSpd = 1.0f;
                partyAtkRange = 7f;
                Type = "Ranger";
                break;
            case 2:
                break;
            case 3:
                break;
            case 0://Player

                break;
            default: 
                break;
        }
    }

}
