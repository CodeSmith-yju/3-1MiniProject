using System.Collections.Generic;
using UnityEngine;

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
    public BaseEntity.Attribute Elemental;
    List<BaseEntity.Attribute> rangerAttributes = new() { BaseEntity.Attribute.Wind, BaseEntity.Attribute.Water };
    List<BaseEntity.Attribute> wizardAttributes = new() { BaseEntity.Attribute.Fire, BaseEntity.Attribute.Dark };
    List<BaseEntity.Attribute> knightAttributes = new() { BaseEntity.Attribute.Light, BaseEntity.Attribute.Wind };
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
    public Sprite ElementalIcon;

    public PartyData(GameObject prefab, int _Lvel)
    {
        obj_Data = prefab;
        this.player = prefab.GetComponent<Ally>();
        level = _Lvel;
        GenerateStat(player.job, _Lvel);//�÷��̾� �������� ��õ������ ���ϰ��ִ�(�������� ��������) �÷��̾� ����������, ������ ���� ������ ����
        //str name = RandomGenerateName();  

        type = prefab.name;// �����������Ʈ�� �̸�, JobClass enum���� ū ���̴� ����.
        cost = (150 + (_Lvel * 20)) + Random.Range(0, _Lvel * 10);
        Debug.Log("cost: "+cost);
        spPartyIcon = player.GetComponent<SpriteRenderer>().sprite;
        SetClassAndAttributeIcon(player.job);
    }


    public void GenerateStat(Ally.JobClass _Code, int _Lvel)
    {
        jobType = _Code;
        switch (jobType)
        {
            case Ally.JobClass.Ranger:
                Debug.Log("Type Ranger, Generate Code: "+_Code);
                partyHp = 15f + (_Lvel * 2f);
                partyMp = 5f;
                partyAtk = 2f + (_Lvel * 0.5f);
                partyAtkSpd = 1.0f;
                partyAtkRange = 7f +(_Lvel * 0.1f);
                //strPartyName = "Ranger";
                strPartyName = GameUiMgr.single.partyNameSetting.GetRandomName(GameUiMgr.single.partyNameSetting.archerNames);
                isMelee = false;//false �϶� ���Ÿ�����
                able_Skill = true;
                break;
            case Ally.JobClass.Wizard:
                Debug.Log("Type wizard, Generate Code: " + _Code);
                partyHp = 15f + (_Lvel * 1f);
                partyMp = 3f;
                partyAtk = 3f + (_Lvel * 0.5f);
                partyAtkSpd = 0.75f;
                partyAtkRange = 7f + (_Lvel * 0.1f);
                //strPartyName = "Wizard";
                strPartyName = GameUiMgr.single.partyNameSetting.GetRandomName(GameUiMgr.single.partyNameSetting.mageNames);
                isMelee = false;
                able_Skill = true;
                break;
            case Ally.JobClass.Knight:
                Debug.Log("Type 3, Generate Code: " + _Code);
                partyHp = 50f + (_Lvel * 5f);
                partyMp = 5f;
                partyAtk = 2f + (_Lvel * 0.3f);
                partyAtkSpd = 1.0f;
                partyAtkRange = 1.1f;
                //strPartyName = "Knight";
                strPartyName = GameUiMgr.single.partyNameSetting.GetRandomName(GameUiMgr.single.partyNameSetting.knightNames);
                isMelee = true;
                able_Skill = true;
                break;
/*            case 0://Player
                break;*/
            default:
                Debug.Log("�÷��̾�ĳ���͸����� ��Ƽ�����Ͱ� �����ɶ� ������");
                type = "Default";
                Debug.Log("Type d, Generate Code: " + _Code);
                partyHp = 20f + (0.01f * _Lvel);
                partyMp = 5f;
                partyAtk = 2f + (0.1f * _Lvel);
                partyAtkSpd = 1.0f + (0.1f * _Lvel);
                partyAtkRange = 2f;
                strPartyName = "��ø";
                isMelee = true;
                break;
        }
        Debug.Log("party Name: " + strPartyName);
    }
    
    void SetClassAndAttributeIcon(Ally.JobClass _jobClass)
    {
        switch (_jobClass)
        {
            case Ally.JobClass.Ranger:
                Elemental = GetRandomElement(rangerAttributes);
                jobIcon = GameUiMgr.single.partyIconRS.dictn_jobIcon[_jobClass];
                ElementalIcon = GameUiMgr.single.partyIconRS.dictn_ElementIcon[Elemental];
                break;
            case Ally.JobClass.Wizard:
                Elemental = GetRandomElement(wizardAttributes);
                jobIcon = GameUiMgr.single.partyIconRS.dictn_jobIcon[_jobClass];
                ElementalIcon = GameUiMgr.single.partyIconRS.dictn_ElementIcon[Elemental];
                break;
            case Ally.JobClass.Knight:
                Elemental = GetRandomElement(knightAttributes);
                jobIcon = GameUiMgr.single.partyIconRS.dictn_jobIcon[_jobClass];
                ElementalIcon = GameUiMgr.single.partyIconRS.dictn_ElementIcon[Elemental];
                break;
            default:
                Elemental = BaseEntity.Attribute.Normal;
                jobIcon = GameUiMgr.single.partyIconRS.dictn_jobIcon[Ally.JobClass.Hero];
                ElementalIcon = GameUiMgr.single.partyIconRS.dictn_ElementIcon[Elemental];
                break;
        }
    }
    BaseEntity.Attribute GetRandomElement(List<BaseEntity.Attribute> attributes)
    {
        int randomIndex = UnityEngine.Random.Range(0, attributes.Count);
        return attributes[randomIndex];
    }

}
