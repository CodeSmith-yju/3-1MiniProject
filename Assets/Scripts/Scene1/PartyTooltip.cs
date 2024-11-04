using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PartyIconState
{
    None,
    Attribute,
    Class
}
public class PartyTooltip : MonoBehaviour
{
    [Header("Object")]
    public GameObject[] tooltips;

    [Header("Text")]
    public TextMeshProUGUI class_name_text;
    public TextMeshProUGUI class_Icon_text;
    public TextMeshProUGUI nomal_Icon_text;
    public TextMeshProUGUI lightdark_Icon_text;
    //public TextMeshProUGUI textPower;
    [Header("Image")]
    public Image class_icon;
    public Image attribute_icon1;
    public Image attribute_icon2;
    public Image attribute_icon3;

    private float canvaseWidth;
    private RectTransform tooltipRect;

    public void SetupTooltip(PartyIconState _iconState, PartySlot _data)//(string _name, string _title, string _desc,Sprite _img)
    {
        if (_iconState == PartyIconState.Class)
        {
            Debug.Log("파티 아이콘 툴팁 활성화");
            VeiwToolTip(0);
            ViewClassTip(_data.partyData);
        }
        else if (_iconState == PartyIconState.Attribute)
        {
            if (_data.partyData.Elemental == BaseEntity.Attribute.Fire || _data.partyData.Elemental == BaseEntity.Attribute.Wind || _data.partyData.Elemental == BaseEntity.Attribute.Water)
            {
                VeiwToolTip(1);
                switch (_data.partyData.Elemental)
                {
                    case BaseEntity.Attribute.Fire:
                        attribute_icon1.sprite = GameUiMgr.single.entityIconRS.dictn_ElementIcon[BaseEntity.Attribute.Wind];
                        attribute_icon2.sprite = GameUiMgr.single.entityIconRS.dictn_ElementIcon[BaseEntity.Attribute.Water];
                        break;
                    case BaseEntity.Attribute.Wind:
                        attribute_icon1.sprite = GameUiMgr.single.entityIconRS.dictn_ElementIcon[BaseEntity.Attribute.Water];
                        attribute_icon2.sprite = GameUiMgr.single.entityIconRS.dictn_ElementIcon[BaseEntity.Attribute.Fire];
                        break;
                    case BaseEntity.Attribute.Water:
                        attribute_icon1.sprite = GameUiMgr.single.entityIconRS.dictn_ElementIcon[BaseEntity.Attribute.Fire];
                        attribute_icon2.sprite = GameUiMgr.single.entityIconRS.dictn_ElementIcon[BaseEntity.Attribute.Wind];
                        break;
                    default:
                        Debug.LogWarning("Party PopUp is Warning");
                        break;
                }
            }
            else
            {
                VeiwToolTip(2);
                if (_data.partyData.Elemental == BaseEntity.Attribute.Normal)//원소 아니면 노멀 아니면 빛/암
                {
                    attribute_icon3.sprite = GameUiMgr.single.entityIconRS.dictn_ElementIcon[BaseEntity.Attribute.Normal];
                    nomal_Icon_text.gameObject.SetActive(true);
                    lightdark_Icon_text.gameObject.SetActive(false);
                }
                else
                {
                    if (_data.partyData.Elemental == BaseEntity.Attribute.Light)
                    {
                        attribute_icon3.sprite = GameUiMgr.single.entityIconRS.dictn_ElementIcon[BaseEntity.Attribute.Dark];
                    }
                    else if (_data.partyData.Elemental == BaseEntity.Attribute.Dark)
                    {
                        attribute_icon3.sprite = GameUiMgr.single.entityIconRS.dictn_ElementIcon[BaseEntity.Attribute.Light];
                    }

                    nomal_Icon_text.gameObject.SetActive(false);
                    lightdark_Icon_text.gameObject.SetActive(true);
                }
            }
        }
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
