using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PartyDetailTooltip : MonoBehaviour
{
    [Header("Object")]
    public GameObject[] tooltips;
    public PartyData _data;
    [Header("Text")]
    public TextMeshProUGUI class_name_text;
    public TextMeshProUGUI class_Icon_text;
    public TextMeshProUGUI nomal_Icon_text;
    public TextMeshProUGUI lightdark_Icon_text;
    public TextMeshProUGUI stat_text;
    //public TextMeshProUGUI textPower;
    [Header("Image")]
    public Image class_icon;
    public Image attribute_icon1;
    public Image attribute_icon2;
    public Image attribute_icon3;
    public Image stat_Icon;

    private float canvaseWidth;
    private RectTransform tooltipRect;

    public void SetUpToolTip(PartyIconState _iconState, PartyDesc _desc)//(string _name, string _title, string _desc,Sprite _img)
    {
        _data = _desc.GetPartyData();

        if (_iconState == PartyIconState.Class)
        {
            Debug.Log("파티 아이콘 툴팁 활성화");
            VeiwToolTip(0);
            ViewClassTip(_data);
        }
        else if (_iconState == PartyIconState.Attribute)
        {
            if (_data.Elemental == BaseEntity.Attribute.Fire || _data.Elemental == BaseEntity.Attribute.Wind || _data.Elemental == BaseEntity.Attribute.Water)
            {
                VeiwToolTip(1);
                switch (_data.Elemental)
                {
                    case BaseEntity.Attribute.Fire:
                        attribute_icon1.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Wind);
                        attribute_icon2.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Water);
                        break;
                    case BaseEntity.Attribute.Wind:
                        attribute_icon1.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Water);
                        attribute_icon2.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Fire);
                        break;
                    case BaseEntity.Attribute.Water:
                        attribute_icon1.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Fire);
                        attribute_icon2.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Wind);
                        break;
                    default:
                        Debug.LogWarning("Party PopUp is Warning");
                        break;
                }
            }
            else
            {
                VeiwToolTip(2);
                if (_data.Elemental == BaseEntity.Attribute.Normal)//원소 아니면 노멀 아니면 빛/암
                {
                    attribute_icon3.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Normal);
                    nomal_Icon_text.gameObject.SetActive(true);
                    lightdark_Icon_text.gameObject.SetActive(false);
                }
                else
                {
                    if (_data.Elemental == BaseEntity.Attribute.Light)
                    {
                        attribute_icon3.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Dark);
                    }
                    else if (_data.Elemental == BaseEntity.Attribute.Dark)
                    {
                        attribute_icon3.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Light);
                    }

                    nomal_Icon_text.gameObject.SetActive(false);
                    lightdark_Icon_text.gameObject.SetActive(true);
                }
            }
        }
        else if (_iconState == PartyIconState.Skill || _iconState == PartyIconState.Skill2)//스킬 아이콘
        {
            VeiwToolTip(0);
            
            ViewSkillTip(_data, _iconState, _desc);
            Debug.Log("Skill Icon 툴팁 활성");
        }
    }
    public void SetUpToolTip(StatIconState _statState)
    {
        VeiwToolTip(3);

        ViewStatTip(_statState);
    }
    void VeiwToolTip(int _veiwIndex)
    {
        if (_veiwIndex > -1)
        {
            for (int i = 0; i < tooltips.Length; i++)
            {
                if (i == _veiwIndex)
                {
                    tooltips[i].SetActive(true);
                    if (_veiwIndex == 3)
                    {
                        Debug.Log("now ViewIndex: " + _veiwIndex);
                    }

                    continue;
                }
                tooltips[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < tooltips.Length; i++)
            {
                tooltips[i].SetActive(false);
            }
        }

    }
    void ViewClassTip(PartyData _partyData)
    {
        class_icon.sprite = _partyData.jobIcon;
        switch (_partyData.jobClass)
        {
            case Ally.Class.Range:
                //class_Icon_text.text = "빠른 공격 속도로 먼 거리에서 적을 제압하는 원거리 공격 클래스";
                class_Icon_text.text = "먼 거리에서 적을 제압하는 원거리 공격 클래스";
                class_name_text.text = "원거리 딜러";
                break;
            /*
        case Ally.Job.Wizard:
            //class_Icon_text.text = "낮은 체력을 지녔지만 강력한 스킬을 자주 사용할 수 있는 마법 클래스";
            class_Icon_text.text = "먼 거리에서 적을 제압하는 원거리 공격 클래스";
            class_name_text.text = "마법사";
            break;
             */
            case Ally.Class.Tank:
                class_Icon_text.text = "공격력이 약하지만 체력이 높은 근접 전투 클래스";
                class_name_text.text = "탱커";
                break;
            case Ally.Class.Melee:
                class_Icon_text.text = "적절한 체력과 공격력을 지닌 균형 잡힌 클래스";
                class_name_text.text = "근접 딜러";
                break;
            case Ally.Class.Support:
                class_Icon_text.text = "동료를 지원하거나 적을 약화시키는 보조 클래스";
                class_name_text.text = "서포터";
                break;
            default:
                break;
        }
    }
    public void ViewStatTip(StatIconState _stat)//Hp, Mp, atk, atkspd, atkrng, def, spd
    {
        Debug.Log("스텟툴팁에 내용을 할당했다");
        switch (_stat)
        {
            case StatIconState.HP:
                stat_Icon.sprite = GameUiMgr.single.entityIconRS.spStatIcon[0];
                stat_text.text = "체력\n 체력이 모두 떨어지면 전투에 참여할 수 없습니다.";
                break;
            case StatIconState.MP:
                stat_Icon.sprite = GameUiMgr.single.entityIconRS.spStatIcon[1];
                stat_text.text = "마나\n 기본 공격 시 1씩 회복되며, 가득 차면 스킬을 사용할 수 있습니다.";
                break;
            case StatIconState.Atk:
                stat_Icon.sprite = GameUiMgr.single.entityIconRS.spStatIcon[2];
                stat_text.text = "공격력\n 높을수록 적에게 가하는 피해가 증가합니다.";
                break;
            case StatIconState.AtkSpd:
                stat_Icon.sprite = GameUiMgr.single.entityIconRS.spStatIcon[3];
                stat_text.text = "공격 속도\n 높을수록 적을 더 자주 공격할 수 있습니다.";
                break;
            case StatIconState.AtkRng:
                stat_Icon.sprite = GameUiMgr.single.entityIconRS.spStatIcon[4];
                stat_text.text = "공격 범위\n 높을수록 멀리 있는 적을 공격할 수 있습니다.";
                break;
            case StatIconState.Def:
                stat_Icon.sprite = GameUiMgr.single.entityIconRS.spStatIcon[5];
                stat_text.text = "방어력\n 높을수록 받는 피해가 줄어들며, 최대 60%까지 경감됩니다.";
                break;
            case StatIconState.Spd:
                stat_Icon.sprite = GameUiMgr.single.entityIconRS.spStatIcon[6];
                stat_text.text = "이동 속도\n 높을수록 빠르게 이동할 수 있습니다.";
                break;
            default:
                Debug.Log("족버그 실시간발생중");
                break;
        }
    }
    void ViewSkillTip(PartyData _partyData, PartyIconState _partyIconState, PartyDesc _partyDesc)
    {
        class_icon.sprite = GameUiMgr.single.entityIconRS.GetSkillIcon(_partyData.jobType);
        if (_partyDesc == null)
        {
            return;
        }

        string LastDmg = "";
        string oriDmg = "";
        switch (_partyData.jobType)
        {
            case Ally.Job.Hero:
                class_icon.sprite = GameUiMgr.single.entityIconRS.GetSkillIcon(Ally.Job.Hero);
                class_name_text.text = "섬광 베기";
                LastDmg = (_partyDesc.atk * 2).ToString("F3");
                oriDmg = (_partyDesc.atk).ToString();
                TextSetting(ref LastDmg);

                //class_Icon_text.text = "단일 대상에게 공격력의 2배 대미지("+ _partyDesc.atk * 2+")";
                class_Icon_text.text = "단일 대상에게 " + LastDmg + "(" + oriDmg + " * 2)피해";
                break;
            case Ally.Job.Knight:
                class_icon.sprite = GameUiMgr.single.entityIconRS.GetSkillIcon(Ally.Job.Knight);
                class_name_text.text = "돌진 찌르기";

                LastDmg = (_partyDesc.atk * 2).ToString("F3");
                oriDmg = (_partyDesc.atk).ToString();
                TextSetting(ref LastDmg);
                //class_Icon_text.text = "단일 대상에게 공격력의 1.3배 대미지("+ _partyDesc.atk * 1.3+ ")";
                class_Icon_text.text = "단일 대상에게 " + LastDmg + "(" + oriDmg + " * 1.3)피해";
                break;
            case Ally.Job.Ranger:
                class_icon.sprite = GameUiMgr.single.entityIconRS.GetSkillIcon(Ally.Job.Ranger);
                class_name_text.text = "가시 화살";

                LastDmg = (_partyDesc.atk * 2).ToString("F3");
                oriDmg = (_partyDesc.atk).ToString();
                TextSetting(ref LastDmg);
                //class_Icon_text.text = "단일 대상에게 공격력의 1.2배 대미지("+ _partyDesc.atk * 1.2 + ")";
                class_Icon_text.text = "단일 대상에게 " + LastDmg + "(" + oriDmg + " * 1.2)피해";
                break;
            case Ally.Job.Wizard:
                class_icon.sprite = GameUiMgr.single.entityIconRS.GetSkillIcon(Ally.Job.Wizard);
                class_name_text.text = "파이어 볼트";

                LastDmg = (_partyDesc.atk * 2).ToString("F3");
                oriDmg = (_partyDesc.atk).ToString();
                TextSetting(ref LastDmg);
                class_Icon_text.text = "단일 대상에게 "+ LastDmg + "("+ oriDmg + " * 3)피해";
                break;
            case Ally.Job.Priest:
                class_icon.sprite = GameUiMgr.single.entityIconRS.GetSkillIcon(Ally.Job.Priest);
                class_name_text.text = "치유의 빛";
                //class_Icon_text.text = "현재 체력이 가장 낮은 아군에게 15(5 + 10 //자신의 최대 체력의 10%만큼 회복 = 숫자)";

                LastDmg = (_partyDesc.hp * 0.1).ToString("F3");
                oriDmg = (_partyDesc.atk).ToString();
                TextSetting(ref LastDmg);
                class_Icon_text.text = "현재 체력이 가장 낮은 아군에게 "+ LastDmg + "(5 + "+ oriDmg + ")회복";
                break;
            case Ally.Job.Demon:
                if (_partyIconState == PartyIconState.Skill)
                {
                    class_icon.sprite = GameUiMgr.single.entityIconRS.GetSkillIcon(Ally.Job.Demon);
                    class_name_text.text = "메타모포시스";

                    LastDmg = (_partyDesc.atk * 0.3).ToString("F3");
                    oriDmg = (_partyDesc.atk).ToString();
                    TextSetting(ref LastDmg);
                    //class_Icon_text.text = "변신하여 기본 공격이 변화, 타격범위에 추가적인 피해를 입히고\n공격력,공격속도,공격범위가 증가 받는 피해 15% 증가한다.";
                    class_Icon_text.text = "변신하여 기본 공격이 강화되고 흡혈 효과가 추가되며 타겟과 인접한 적에게 " + LastDmg + "(" + oriDmg + "*0.3)피해" +
                        "\n공격력, 공격 속도, 공격 범위가 증가하지만, 받는 피해가 15% 상승합니다.";
                }
                else if(_partyIconState == PartyIconState.Skill2)
                {
                    class_icon.sprite = GameUiMgr.single.entityIconRS.GetSkillIcon(Ally.Job.Demon);
                    class_name_text.text = "존큰대";
                    class_Icon_text.text = "겁나쎄게때린다.";
                }
                break;
            default:
                break;
        }
    }
    void TextSetting(ref string _stat)
    {
        for (int i = 0; i < 3; i++)
        {
            if (_stat.EndsWith("0"))
                _stat = _stat.Substring(0, _stat.Length - 1);
        }
        if (_stat.EndsWith("."))
            _stat = _stat.Substring(0, _stat.Length - 1);
        _stat = "<color=green>" + _stat + "</color>";
    }

    public void TooltipSetting(float _canvasWidth, RectTransform _tooltipRect)
    {
        //ItemResources.instance.AfterIconSet();
        canvaseWidth = _canvasWidth;
        tooltipRect = _tooltipRect;
    }

    public void MoveTooltip()
    {
        transform.position = Input.mousePosition;
        // 04-15 ToolTip
        tooltipRect = GetComponent<RectTransform>();

        if (tooltipRect.anchoredPosition.x + tooltipRect.sizeDelta.x > canvaseWidth)
            tooltipRect.pivot = new Vector2(1, 0);
        else
            tooltipRect.pivot = new Vector2(0, 0);
    }
}
