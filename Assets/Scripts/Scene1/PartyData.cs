using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

[System.Serializable]
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
    public bool moveInCk;
    public int cost = 128;
    public int index;
    public int level = 0;
    
    public Sprite spPartyIcon;

    //Party Stat .. hp, atk, ... 여기넣어야되나
    //public State myState;//플레이어블 프리펩을 결정할 상태변수

    // Ranger.cs에서 고유 id, 최대 HP, 최대 MP, 공격력, 공격속도, 공격사거리 순으로 초기화
    public int partyJobIndex;// 고유 id == 직업인덱스
    public float partyHp;
    public float partyMp;
    public float partyAtk;
    public float partyAtkSpd;
    public float partyAtkRange;

    public int partyDefense;

    public bool able_Skill;
    public bool isMelee;
    public GameObject obj_Data;
    public Ally player;

    public Ally.Job jobType;
    public Ally.Class jobClass;
    public Sprite portraitIcon;
    public Sprite jobIcon;
    public Sprite ElementalIcon;

    public List<float> defaultStats;//Hp, Mp, atk, atkspd, atkrng, def, spd
    public List<float> weightPerLevelStats;//hp, mp, atk, atkspd, atkrng

    public PartyData(GameObject prefab, int _Lvel)
    {
        moveInCk = false;
        obj_Data = prefab;
        this.player = prefab.GetComponent<Ally>();
        level = _Lvel;
        GenerateStat(player.job, _Lvel);//플레이어 프리펩이 선천적으로 지니고있는(수동으로 지정해줌) 플레이어 직업정보와, 레벨에 따라서 스텟을 생성
        //str name = RandomGenerateName();  
        type = prefab.name;// 프리펩오브젝트의 이름, JobClass enum값과 큰 차이는 없음.
        cost = (150 + (_Lvel * 20)) + Random.Range(0, _Lvel * 10);
        //Debug.Log("cost: "+cost);
        spPartyIcon = player.GetComponent<SpriteRenderer>().sprite;
        if (player.job == Ally.Job.Knight)
        {
            spPartyIcon = GameUiMgr.single.entityIconRS.GetStandingIcon(0);
        }
        else if (player.job == Ally.Job.Wizard)
        {
            spPartyIcon = GameUiMgr.single.entityIconRS.GetStandingIcon(1);
        }
        SetIcons();
    }


    public void GenerateStat(Ally.Job _Code, int _Lvel)
    {
        jobType = _Code;
        switch (jobType)
        {
            case Ally.Job.Ranger:
                jobClass = Ally.Class.Range;
                //Debug.Log("Type Ranger, Generate Code: "+_Code);
                partyHp = 15f + (_Lvel * 2f);
                partyMp = 5f;
                partyAtk = 2f + (_Lvel * 0.2f);
                partyAtkSpd = 1.0f +(_Lvel * 0.05f);
                //partyAtkSpd = Mathf.Clamp((1f + (_Lvel * 0.05f)), 1.0f, 2f);
                partyAtkRange = 5f;
                //strPartyName = "Ranger";
                strPartyName = GameUiMgr.single.partyNameSetting.GetRandomName(GameUiMgr.single.partyNameSetting.archerNames);
                isMelee = false;//false 일때 원거리공격
                able_Skill = true;
                Elemental = GetRandomElement(rangerAttributes);
                partyDefense = 5 + (_Lvel * 2);
                SetDefaultStats(15f, 5f, 2f, 1.0f, 5f, 5, 2.2f);
                SetWeightPerLevelStats(2f, 0f, 0.2f, 0.05f, 0f);
                break;
            case Ally.Job.Wizard:
                //Debug.Log("Type wizard, Generate Code: " + _Code);
                jobClass = Ally.Class.Range;
                partyHp = 20f + (_Lvel * 1f);
                partyMp = 3f;
                partyAtk = 3f + (_Lvel * 0.5f);
                partyAtkSpd = 0.7f +(_Lvel * 0.015f);
                //partyAtkSpd = Mathf.Clamp((0.7f + (_Lvel * 0.015f)), 0.7f, 1.4f);
                partyAtkRange = 7f;
                //strPartyName = "Wizard";
                strPartyName = GameUiMgr.single.partyNameSetting.GetRandomName(GameUiMgr.single.partyNameSetting.mageNames);
                isMelee = false;
                able_Skill = true;
                Elemental = GetRandomElement(wizardAttributes);
                partyDefense = 0 + (_Lvel * 1);
                SetDefaultStats(20f, 3f, 3f, 0.7f, 7f, 0, 1.8f);
                SetWeightPerLevelStats(1f, 0f, 0.5f, 0.015f, 0f);
                break;
            case Ally.Job.Knight:
                jobClass = Ally.Class.Tank;
                //Debug.Log("Type 3, Generate Code: " + _Code);
                partyHp = 50f + (_Lvel * 5f);
                partyMp = 5f;
                partyAtk = 2f + (_Lvel * 0.3f);
                partyAtkSpd = 1.0f +(_Lvel * 0.025f);
                //partyAtkRange = 1.2f;
                //partyAtkSpd = Mathf.Clamp( (1.0f+(_Lvel * 0.025f)),1.0f, 2f);
                partyAtkRange = 1.2f;
                //strPartyName = "Knight";
                strPartyName = GameUiMgr.single.partyNameSetting.GetRandomName(GameUiMgr.single.partyNameSetting.knightNames);
                isMelee = true;
                able_Skill = true;
                Elemental = GetRandomElement(knightAttributes);
                partyDefense = 20 + (_Lvel * 4);
                SetDefaultStats(50f, 5f, 2f, 1.0f, 1.2f, 20, 1.8f);
                SetWeightPerLevelStats(5f, 0f, 0.3f, 0.025f, 0f);
                break;
            case Ally.Job.Priest:
                jobClass = Ally.Class.Support;
                partyHp = 35f + (_Lvel + 3f);
                partyMp = 3f;
                partyAtk = 1f + (_Lvel * 0.2f);
                partyAtkSpd = 0.85f +(_Lvel * 0.015f);
                partyAtkRange = 5f;
                strPartyName = GameUiMgr.single.partyNameSetting.GetRandomName(GameUiMgr.single.partyNameSetting.priestNames);
                isMelee = false;
                able_Skill = true;
                Elemental = BaseEntity.Attribute.Light;
                partyDefense = 2 + (_Lvel * 1);
                SetDefaultStats(35f, 3f, 1f, 0.85f, 5f, 2, 2.0f);
                SetWeightPerLevelStats(3f, 0f, 0.2f, 0.015f, 0f);
                break;
            case Ally.Job.Demon:
                jobClass = Ally.Class.Melee;
                //스텟설정해야됨
                partyHp = 30f + (_Lvel * 2f);
                partyMp = 6f;
                partyAtk = 3 + (_Lvel * 0.5f);
                partyAtkSpd = 0.8f + (_Lvel * 0.05f);

                partyAtkRange = 1.5f;
                strPartyName = GameUiMgr.single.partyNameSetting.GetRandomName(GameUiMgr.single.partyNameSetting.demonNames);
                isMelee = true;
                able_Skill = true;
                Elemental = BaseEntity.Attribute.Dark;
                partyDefense = 5 + (_Lvel * 2);
                SetDefaultStats(30f, 6f, 3f, 0.8f, 1.5f, 5, 2.0f);
                SetWeightPerLevelStats(2f, 0f, 0.5f, 0.05f, 0f);
                break;
/*            case 0://Player
                break;*/
            default:
                //Debug.Log("플레이어캐릭터를위한 파티데이터가 생성될때 동작함");
                //type = "Default";
                //Debug.Log("Type d, Generate Code: " + _Code);
                // 스텟 상한 설정
                jobClass = Ally.Class.Melee;
                partyHp = 40f + (_Lvel * 3f);
                partyMp = 5f;
                partyAtk = 3f + (_Lvel * 0.6f);
                partyAtkSpd = 1.0f + (_Lvel * 0.05f);
                //partyAtkSpd = Mathf.Clamp((1.0f + (_Lvel * 0.05f)), 1.0f, 2f);
                partyAtkRange = 1.2f;
                strPartyName = GameMgr.playerData[0].GetPlayerName();
                isMelee = true;
                Elemental = BaseEntity.Attribute.Normal;
                partyDefense = 10 + (_Lvel * 3);
                SetDefaultStats(40f, 5f, 3f, 1.0f, 1.2f, 10, 2.0f);
                SetWeightPerLevelStats(3f, 0f, 0.6f, 0.05f, 0f);
                break;
        }
        //Debug.Log("party Name: " + strPartyName);

        partyAtkSpd = Mathf.Clamp(partyAtkSpd, 0.7f, 2f);
    }
    
    void SetIcons()
    {
        portraitIcon = GameUiMgr.single.entityIconRS.GetPortraitIcon(jobType);
        jobIcon = GameUiMgr.single.entityIconRS.GetJobIcon(jobClass);
        ElementalIcon = GameUiMgr.single.entityIconRS.GetElementIcon(Elemental);
    }
    BaseEntity.Attribute GetRandomElement(List<BaseEntity.Attribute> attributes)
    {
        int randomIndex = UnityEngine.Random.Range(0, attributes.Count);
        return attributes[randomIndex];
    }

    public int GetPlayerbleObjIndex()
    {
        int _index = -1;
        switch (jobType)
        {
            case Ally.Job.Ranger:
                _index = 0;
                break;
            case Ally.Job.Knight:
                _index = 1;
                break;
            case Ally.Job.Wizard:
                _index = 2;
                break;
            case Ally.Job.Priest:
                _index = 3;
                break;
            case Ally.Job.Demon:
                _index = 4;
            break;
        }
        return _index;
    }
    public void PartyDataSetMoveInCk(bool _bool)
    {
        moveInCk = _bool;
    }
    public void SetPartyCost(int _cost)
    {
        //Debug.Log("GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG Run SetPartyCost: Befor:"+cost +"/ After: "+_cost);
        cost = _cost;
    }

    void SetDefaultStats(float Hp, float Mp, float Atk, float AtkSpd, float AtkRng, int Def, float Spd)//Hp, Mp, atk, atkspd, atkrng, def, spd
    {
        defaultStats ??= new();
        defaultStats.Clear();

        defaultStats.Add(Hp);
        defaultStats.Add(Mp);
        defaultStats.Add(Atk);
        defaultStats.Add(AtkSpd);
        defaultStats.Add(AtkRng);
        defaultStats.Add(Def);
        defaultStats.Add(Spd);
    }

    void SetWeightPerLevelStats(float Hp, float Mp, float Atk, float AtkSpd, float AtkRng)//Hp, Mp, atk, atkspd, atkrng
    {
        weightPerLevelStats ??= new();
        weightPerLevelStats.Clear();

        weightPerLevelStats.Add(Hp);
        weightPerLevelStats.Add(Mp);
        weightPerLevelStats.Add(Atk);
        weightPerLevelStats.Add(AtkSpd);
        weightPerLevelStats.Add(AtkRng);
    }

    public void LoadAttribute(PartyData _pd)
    {
        Elemental = _pd.Elemental;
        ElementalIcon = _pd.ElementalIcon;
    }
}
