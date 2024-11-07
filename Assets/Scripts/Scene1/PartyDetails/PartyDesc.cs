using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyDesc : MonoBehaviour
{
    [SerializeField] PartyDetails partyDetails;
    [SerializeField] Button btnMy;
    [SerializeField] int descIndex;

    [Header("My Icon")]
    public Image myPortrait;
    public Image myAttribute;

    [Header("PlayerOnly")]
    [HideInInspector]
    public string str_Name;
    public string str_max_Sn;
    public string str_cur_Sn;
    public string str_max_Exp;
    public string str_cur_Exp;

    [Header("Detail_Icons")]
    public Sprite img_Portrait;
    public Sprite img_Attribute;
    public Sprite img_Job;
    public Sprite img_Skill;

    [Header("Detail_Stats")]
    public string str_Lv;
    public string str_Hp;
    public string str_Mp;
    public string str_Atk;
    public string str_AtkSpd;
    public string str_AtkRange;

    public List<float> tempDefaultStats;
    public List<float> tempWeightStats;

    public void Init(PartyData _partyData, PartyDetails _partyDetails)
    {
        btnMy = GetComponent<Button>();
        RefreshDesc();
        this.partyDetails = _partyDetails;
        SetData(_partyData);

        myPortrait.sprite = img_Portrait;
        myAttribute.sprite = img_Attribute;

        tempDefaultStats.AddRange(_partyData.defaultStats);
        tempWeightStats.AddRange(_partyData.weightPerLevelStats);
    }

    public void OK()
    {
        partyDetails.ShowDetals(this);
    }
    void RefreshDesc()
    {
        descIndex = -1;
        btnMy.onClick.AddListener(OK);
    }
    void SetData(PartyData _partyData)
    {
        if (descIndex < 1) //is Player
        {
            str_Name = _partyData.strPartyName;
            str_max_Sn = GameMgr.playerData[0].max_Player_Sn.ToString();
            str_cur_Sn = GameMgr.playerData[0].cur_Player_Sn.ToString();
            str_max_Exp = GameMgr.playerData[0].player_max_Exp.ToString();
            str_cur_Exp = GameMgr.playerData[0].player_cur_Exp.ToString();
        }

        // img
        img_Portrait = _partyData.portraitIcon;
        //img_Attribute = _partyData.ElementalIcon;
        img_Attribute = GameUiMgr.single.entityIconRS.GetElementIcon(_partyData.Elemental);
        img_Job = _partyData.jobIcon;
        img_Skill = GameUiMgr.single.entityIconRS.GetSkillIcon(_partyData.jobType);

        // str
        str_Lv = _partyData.level.ToString();
        str_Hp = _partyData.partyHp.ToString();
        str_Mp = _partyData.partyMp.ToString();
        str_Atk = _partyData.partyAtk.ToString();
        str_AtkRange = _partyData.partyAtkRange.ToString();
        str_AtkSpd = _partyData.partyAtkSpd.ToString();
    }
    public void SetIndex(int _index)
    {
        //Debug.Log("@@@@@@@@@@@@@@@@@@@@@ Run SetIndex Index: " + _index);
        descIndex = _index;
        //Debug.Log("@@@@@@@@@@@@@@@@@@@@@ Run SetIndex Index: " + descIndex);
    }
    public int GetIndex() { return  descIndex; }

}
