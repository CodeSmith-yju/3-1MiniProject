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
    public string strPartyName;
    public string strName;

    public string Type;

    public int cost = 128;
    //public int index;

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

    public PartyData(GameObject prefab, int _Lvel)
    {
        BaseEntity player = prefab.GetComponent<BaseEntity>();

        partyJobIndex = player.entity_id;

        //GenerateStat(partyJobIndex, _Lvel, player);
        //�� �Ʒ��� ������ ���� ����
        partyHp = player.max_Hp;
        partyMp = player.max_Mp;
        partyAtk = player.atkDmg;
        partyAtkSpd = player.atkSpd;
        partyAtkRange = player.atkRange;
        partyAbleSkill = player.able_Skill;
        cost = Random.Range(50 + _Lvel*10, 100+ _Lvel*50);
        spPartyIcon = player.GetComponent<SpriteRenderer>().sprite;
    }


    public void GenerateStat(int _Code, int _Lvel, BaseEntity entity)
    {
        switch (_Code)
        {
            case 1:
                Debug.Log("Type Ranger, Generate Code: "+_Code);
                partyHp = entity.max_Hp + (0.01f* _Lvel);
                partyMp = entity.max_Mp + (0.01f * _Lvel);
                partyAtk = entity.atkDmg;
                partyAtkSpd = entity.atkSpd;
                partyAtkRange = entity.atkRange;
                break;
            case 2:
                break;
            case 3:
                break;
            default: 
                break;
        }
    }

}
