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
        int cnt = _partySlots.Count;//2이들어갔어
        
        //여기서 0이랑 1이 돌아갔고 앞으로 2랑 3은 꺼져야해.
        for (int i = 0; i < _partySlots.Count; i++)
        {
            PartyDetailDescs[i].Init(_partySlots[i].partyData, this);
            PartyDetailDescs[i].SetIndex(i);
            PartyDetailDescs[i].gameObject.SetActive(true);
        }

        if (cnt < 4)//2개잖아
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

        txtHp.text = "HP: " + GameMgr.playerData[_desc.GetIndex()].max_Player_Hp; //_desc.str_Hp;
        txtMp.text = "MP: " + GameMgr.playerData[_desc.GetIndex()].max_Player_Mp;//_desc.str_Mp;
        txtAtk.text = "Atk: " + GameMgr.playerData[_desc.GetIndex()].base_atk_Dmg;//_desc.str_Atk ;
        txtAtkSpd.text = "AtkSpd: " + GameMgr.playerData[_desc.GetIndex()].atk_Speed;//_desc.str_AtkSpd;
        txtAtkRange.text = "AtkRng: " + GameMgr.playerData[_desc.GetIndex()].atk_Range;//_desc.str_AtkRange;
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
