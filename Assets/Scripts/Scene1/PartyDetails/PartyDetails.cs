using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyDetails : MonoBehaviour
{
    public List<PartyDesc> PartyDetailDescs = new();

    [Header("Img")]
    public Image portrait;
    public Image job;
    public Image attribute;
    public Image skill;

    [Header("Text")]
    public TextMeshProUGUI txtHp;
    public TextMeshProUGUI txtMp;
    public TextMeshProUGUI txtAtk;
    public TextMeshProUGUI txtAtkSpd;
    public TextMeshProUGUI txtAtkRange;
    public void Init(List<PartySlot> _partySlots)//GuiMgr의 List<PartySlot>인 lastDefartual의 partyData를통해 목록을만들것.
    {
        if (_partySlots == null)
        {
            Debug.Log("Null Error");
            return;
        }
        PartyDetailDescs ??= new();
        int cnt = _partySlots.Count;
        for (int i = 0; i < _partySlots.Count; i++)
        {
            PartyDetailDescs[i].Init(_partySlots[i].partyData, this);
            PartyDetailDescs[i].SetIndex(i);
            PartyDetailDescs[i].gameObject.SetActive(true);
        }
        if (cnt < 4)
        {
            for (int i = cnt; i < 4; i++)
            {
                PartyDetailDescs[i].gameObject.SetActive(false);
            }
        }

        ShowDetals(PartyDetailDescs[0]);
    }

    public void ShowDetals(PartyDesc _desc)
    {
        portrait.sprite = _desc.img_Portrait;
        job.sprite = _desc.img_Job;
        attribute.sprite = _desc.img_Attribute;
        skill.sprite = _desc.img_Skill;

        string hp = (GameMgr.playerData[_desc.GetIndex()].max_Player_Hp - _desc.tempDefaultStats[0]).ToString("F3");
        string atk = (GameMgr.playerData[_desc.GetIndex()].base_atk_Dmg - _desc.tempDefaultStats[2]).ToString("F3");
        string atkspd = (GameMgr.playerData[_desc.GetIndex()].atk_Speed - _desc.tempDefaultStats[3]).ToString("F3");
        TextSetting(ref hp, ref atk, ref atkspd);

        txtHp.text = "HP: " + GameMgr.playerData[_desc.GetIndex()].max_Player_Hp + "( +" + hp + ")"; //_desc.str_Hp;
        txtMp.text = "MP: " + GameMgr.playerData[_desc.GetIndex()].max_Player_Mp;
        txtAtk.text = "Atk: " + GameMgr.playerData[_desc.GetIndex()].base_atk_Dmg + "( +" + atk + ")";//_desc.str_Atk ;
        txtAtkSpd.text = "AtkSpd: " + GameMgr.playerData[_desc.GetIndex()].atk_Speed + "( +" + atkspd + ")";//_desc.str_AtkSpd;
        txtAtkRange.text = "AtkRng: " + GameMgr.playerData[_desc.GetIndex()].atk_Range;


    }

    void TextSetting(ref string hp, ref string atk, ref string atkspd)
    {
        for (int i = 0; i < 3; i++)
        {
            if (hp.EndsWith("0"))
                hp = hp.Substring(0, hp.Length - 1);

            if (atk.EndsWith("0"))
                atk = atk.Substring(0, atk.Length - 1);

            if (atkspd.EndsWith("0"))
                atkspd = atkspd.Substring(0, atkspd.Length - 1);
        }
        if (hp.EndsWith("."))
            hp = hp.Substring(0, hp.Length - 1);

        if (atk.EndsWith("."))
            atk = atk.Substring(0, atk.Length - 1);

        if (atkspd.EndsWith("."))
            atkspd = atkspd.Substring(0, atkspd.Length - 1);
    }

    /*void SetDetail(PartyData _pd)
    {
        //Default_ Img
        //img_Portrait = 이거없어서 곳에 만들고 추가하거나 해야할듯..
        //img_Skill.sprite = _pd. 이것도없어서 추가해야됨수구
        img_Entity.sprite = _pd.ElementalIcon;
        img_Job.sprite = _pd.jobIcon;
        
        //Default_ Text
        str_Lv = _pd.level.ToString();
        str_Hp = _pd.partyHp.ToString();
        str_Mp = _pd.partyMp.ToString();
        str_Atk = _pd.partyAtk.ToString();
        str_AtkSpd = _pd.partyAtkSpd.ToString();
        str_AtkRange = _pd.partyAtkRange.ToString();

        //Player Only???
        str_Name = _pd.strPartyName;

        str_max_Sn = GameMgr.playerData[0].max_Player_Sn.ToString();
        str_cur_Sn = GameMgr.playerData[0].cur_Player_Sn.ToString();

        str_max_Exp = GameMgr.playerData[0].player_max_Exp.ToString();
        str_cur_Exp = GameMgr.playerData[0].player_cur_Exp.ToString();

        Debug.Log("Now Details: "+str_Lv + str_Name);
    }*/

    public void Test()
    {
        gameObject.SetActive(false);
        Debug.Log("외부영역 클릭");
    }
}
