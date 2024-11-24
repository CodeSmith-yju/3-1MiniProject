using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PartyDetails : MonoBehaviour
{
    public List<PartyDesc> PartyDetailDescs = new();
    List<RectTransform> PartyDetailRects = new();
    public List<SetPartyDetailTolltip> setPartyTooltips = new();
    int activeStack;

    [Header("ToolTip")]
    [SerializeField] PartyDetailTooltip detailTooltip;

    [Header("Img")]
    public Image portrait;
    public Image job;
    public Image attribute;
    public Image skill;
    public Image DoubleSkill;

    [Header("Desc")]
    public TextMeshProUGUI textStatsDesc;

    [Header("LeftViewStats")]
    public TextMeshProUGUI txtHp;
    public TextMeshProUGUI txtMp;
    public TextMeshProUGUI textDef;
    public TextMeshProUGUI textSpeed;

    [Header("RightViewStats")]
    public TextMeshProUGUI txtAtk; 
    public TextMeshProUGUI txtAtkSpd;
    public TextMeshProUGUI txtAtkRange;

    public List<Ally> players;

    public void Init(List<PartySlot> _partySlots)//GuiMgr의 List<PartySlot>인 lastDefartual의 partyData를통해 목록을만들것.
    {
        if (_partySlots == null)
        {
            Debug.Log("Null Error");
            return;
        }
        SetPartyDetailRect();

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

        activeStack = 0;

    }
    public void OpenPartyDetail()
    {
        int cnt = GameMgr.playerData.Count;

        for (int i = 0; i < cnt; i++)
        {
            if (GameMgr.playerData[i].cur_Player_Hp < 0 && !PartyDetailDescs[i].unActive.gameObject.activeSelf)
            {
                PartyDetailDescs[i].UnActiveBtn(true);
                activeStack++;
            }
        }

        if (activeStack >= cnt) 
        {
            gameObject.SetActive(false);
            return;
        }

        for (int i = 0; i < cnt; i++)
        {
            if (!PartyDetailDescs[i].unActive.gameObject.activeSelf)
            {
                ShowDetals(PartyDetailDescs[i]);
                break;
            }
        }

    }
    public void SetTooltipIndex(PartyDesc _partyDesc)
    {
        for (int i = 0; i < setPartyTooltips.Count; i++)
        {
            setPartyTooltips[i].SetPartyDesc(_partyDesc);
        }
    }
    public void ShowDetals(PartyDesc _desc)//Hp, Mp, atk, atkspd, atkrng, def, spd
    {
        int _index = _desc.GetIndex();
        string hp;
        string mp;
        string def;
        string speed;

        string atk;
        string atkspd;
        string atkrange;

        /*if (_desc.GetPartyData().jobType == Ally.Job.Demon)
        {
            DoubleSkill.gameObject.transform.parent.gameObject.SetActive(true);
            DoubleSkill.sprite = GameUiMgr.single.entityIconRS.GetSkillIcon(Ally.Job.Demon);
        }
        else
        {
            DoubleSkill.gameObject.transform.parent.gameObject.SetActive(false);
        }*/
        DoubleSkill.gameObject.transform.parent.gameObject.SetActive(false);

        textStatsDesc.text = "최대 체력(+레벨로 인한 추가스탯)\n" + "스탯은 소모 아이템, 던전 내 버프/디버프, 장비 및 레벨업을 통해 변동될 수 있습니다.";
        portrait.sprite = _desc.img_Portrait;
        job.sprite = _desc.img_Job;
        attribute.sprite = _desc.img_Attribute;

        skill.sprite = _desc.img_Skill;

        if (_index == 0)
        {
            textStatsDesc.text = "최대 체력(+레벨과 장비로 인한 추가스탯\n" + "스탯은 소모 아이템, 던전 내 버프/디버프, 장비 및 레벨업을 통해 변동될 수 있습니다.";
        }

        if (SceneManager.GetActiveScene().name == "Town")
        {
            hp = (GameMgr.playerData[_index].max_Player_Hp - _desc.tempDefaultStats[0]).ToString("F3");
            mp = (GameMgr.playerData[_index].max_Player_Mp - _desc.tempDefaultStats[1]).ToString("F0");
            def = (GameMgr.playerData[_index].defensePoint - _desc.tempDefaultStats[5]).ToString("F0");
            speed = "0";

            atk = (GameMgr.playerData[_index].base_atk_Dmg - _desc.tempDefaultStats[2]).ToString("F3");
            atkspd = (GameMgr.playerData[_index].atk_Speed - _desc.tempDefaultStats[3]).ToString("F3");
            atkrange = "0";
            TextSetting(ref hp, ref atk, ref atkspd);

            _desc.SetHpAtk(GameMgr.playerData[_index].max_Player_Hp, GameMgr.playerData[_index].base_atk_Dmg);

            txtHp.text = "HP: " + GameMgr.playerData[_index].max_Player_Hp + "\n(+" + hp + ")"; //_desc.str_Hp;
            txtMp.text = "MP: " + GameMgr.playerData[_index].max_Player_Mp + "\n(+" + mp + ")";
            textDef.text = "Def: " + GameMgr.playerData[_index].defensePoint + "\n(+" + def + ")";
            textSpeed.text = "Spd: " + (_desc.tempDefaultStats[6] / 2).ToString("F1") + "\n(+" + speed + ")";

            txtAtk.text = "Atk: " + GameMgr.playerData[_index].base_atk_Dmg + "\n(+" + atk + ")";//_desc.str_Atk ;
            txtAtkSpd.text = "AtkSpd: " + GameMgr.playerData[_index].atk_Speed + "\n(+" + atkspd + ")";//_desc.str_AtkSpd;
            txtAtkRange.text = "AtkRng: " + GameMgr.playerData[_index].atk_Range + "\n(+" + atkrange + ")";
        }
        else //Battle, Tutorial Scean에서 여기만 수정해주면 될듯?
        {
            hp = (GameMgr.playerData[_index].max_Player_Hp - BattleManager.Instance.base_Stats[GameMgr.playerData[_index]].temp_MaxHp).ToString("F3");
            mp = (GameMgr.playerData[_index].max_Player_Mp - BattleManager.Instance.base_Stats[GameMgr.playerData[_index]].temp_MaxMp).ToString("F0");
            def = (GameMgr.playerData[_index].defensePoint - BattleManager.Instance.base_Stats[GameMgr.playerData[_index]].temp_Dp).ToString(); // 방어력 버프를 만들까말까 고민중
            speed = "0";

            atk = (GameMgr.playerData[_index].base_atk_Dmg - BattleManager.Instance.base_Stats[GameMgr.playerData[_index]].temp_Dmg).ToString("F3");
            atkspd = (GameMgr.playerData[_index].atk_Speed - BattleManager.Instance.base_Stats[GameMgr.playerData[_index]].temp_AtkSpd).ToString("F3");
            atkrange = "0";

            hp = ApplySign(hp);
            atk = ApplySign(atk);
            atkspd = ApplySign(atkspd);
            mp = ApplySign(mp);
            def = ApplySign(def);

            TextSetting(ref hp, ref atk, ref atkspd);

            _desc.SetHpAtk(GameMgr.playerData[_index].max_Player_Hp, GameMgr.playerData[_index].base_atk_Dmg);

            txtHp.text = "HP: " + GameMgr.playerData[_index].max_Player_Hp.ToString("F3") + "\n(" + hp + ")"; //_desc.str_Hp;
            txtMp.text = "MP: " + GameMgr.playerData[_index].max_Player_Mp.ToString("F0") + "\n(" + mp + ")";
            textDef.text = "Def: " + GameMgr.playerData[_index].defensePoint.ToString() + "\n(" + def + ")";
            textSpeed.text = "Spd: " + (_desc.tempDefaultStats[6] / 2).ToString("F1") + "\n(+" + speed + ")";

            txtAtk.text = "Atk: " + GameMgr.playerData[_index].base_atk_Dmg.ToString("F3") + "\n(" + atk + ")";//_desc.str_Atk ;
            txtAtkSpd.text = "AtkSpd: " + GameMgr.playerData[_index].atk_Speed.ToString("F3") + "\n(" + atkspd + ")";//_desc.str_AtkSpd;
            txtAtkRange.text = "AtkRng: " + GameMgr.playerData[_index].atk_Range.ToString("F3") + "\n(+" + atkrange + ")";
        }

        BtnResize(_index);
        SetTooltipIndex(PartyDetailDescs[_index]);
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

    private string ApplySign(string value) // 숫자에 따라 부호 표시
    {
        if (float.TryParse(value, out float numericValue))
        {
            if (numericValue > 0)
                return "+" + numericValue.ToString(); // 양수일 때 + 추가
            else if (numericValue < 0)
                return numericValue.ToString(); // 음수는 그대로 반환
        }
        return "+" + value; // 숫자로 변환되지 않으면 원래 값 반환
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

    public void BtnResize(int _index)//150,80 / 120,60
    {
        for (int i = 0; i < PartyDetailRects.Count; i++)
        {
            if (i == _index)
            {
                PartyDetailRects[i].sizeDelta = new Vector2(150, 80);
            }
            else
            {
                PartyDetailRects[i].sizeDelta = new Vector2(120, 60);
            }
        }
    }

    public void Test()
    {
        gameObject.SetActive(false);
        //GameUiMgr.single.partyDetails.gameObject.SetActive(false);
        //GameUiMgr.partyDetails.gameObject.SetActive(false);
        Debug.Log("외부영역 클릭");
    }

    public void SetPartyDetailRect()
    {
        PartyDetailDescs ??= new();
        PartyDetailRects ??= new();

        if (PartyDetailRects == null)
        {
            return;
        }
        else if (PartyDetailRects.Count == 0)
        {
            for (int i = 0; i < PartyDetailDescs.Count; i++)
            {
                PartyDetailRects.Add(PartyDetailDescs[i].gameObject.GetComponent<RectTransform>());
            }
        }
    }

    public void OnClickedClosePartyDetails()
    {
        if (SceneManager.GetActiveScene().name == "Town")
        {
            GameUiMgr.single.UnActivePartyDetail();
        }
        else
        {
            BattleManager.Instance.ui.UnActiveBattlePartyDetail();
        }
    }
}
