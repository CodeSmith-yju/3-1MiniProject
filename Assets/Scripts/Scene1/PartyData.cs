using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
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
    //public string strName;

    public string type;

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

    public bool able_Skill;
    public bool isMelee;
    public GameObject obj_Data;
    public Ally player;

    public Ally.JobClass jobType;
    public Sprite jobIcon;
    public PartyData(GameObject prefab, int _Lvel)
    {
        obj_Data = prefab;
        this.player = prefab.GetComponent<Ally>();
        level = _Lvel;
        GenerateStat(player.job, _Lvel);//�÷��̾� �������� ��õ������ ���ϰ��ִ�(�������� ��������) �÷��̾� ����������, ������ ���� ������ ����
        //str name = RandomGenerateName();  

        type = prefab.name;// �����������Ʈ�� �̸�, JobClass enum���� ū ���̴� ����.
        cost = Random.Range(50 + _Lvel*10, 100+ _Lvel*50);
        Debug.Log("cost: "+cost);
        spPartyIcon = player.GetComponent<SpriteRenderer>().sprite;
        jobIcon = player.class_Icon;
    }


    public void GenerateStat(Ally.JobClass _Code, int _Lvel)
    {
        jobType = _Code;
        switch (jobType)
        {
            case Ally.JobClass.Ranger:
                Debug.Log("Type Ranger, Generate Code: "+_Code);
                partyHp = 15f;
                partyMp = 5f;
                partyAtk = 2f;
                partyAtkSpd = 1.0f;
                partyAtkRange = 7f;
                strPartyName = "Ranger";
                isMelee = false; //false �϶� ���Ÿ�����
                able_Skill = true;
                break;
            case Ally.JobClass.Wizard:
                Debug.Log("Type wizard, Generate Code: " + _Code);
                partyHp = 15f;
                partyMp = 3f;
                partyAtk = 3f;
                partyAtkSpd = 0.75f;
                partyAtkRange = 7f;
                strPartyName = "Wizard";
                isMelee = false;
                able_Skill = true;
                break;
            case Ally.JobClass.Knight:
                Debug.Log("Type 3, Generate Code: " + _Code);
                partyHp = 50f;
                partyMp = 5f;
                partyAtk = 2f;
                partyAtkSpd = 1.0f;
                partyAtkRange = 1.1f;
                strPartyName = "Knight";
                isMelee = true;
                able_Skill = true;
                break;
/*            case 0://Player
                break;*/
            default:
                type = "Default";
                Debug.Log("Type d, Generate Code: " + _Code);
                partyHp = 20f + (0.01f * _Lvel);
                partyMp = 5f;
                partyAtk = 2f + (0.1f * _Lvel);
                partyAtkSpd = 1.0f + (0.1f * _Lvel);
                partyAtkRange = 2f;
                strPartyName = "��ø";
                isMelee = true;
                able_Skill = false;
                break;
        }
    }

}
