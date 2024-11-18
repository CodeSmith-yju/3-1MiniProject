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
            Debug.Log("��Ƽ ������ ���� Ȱ��ȭ");
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
                if (_data.Elemental == BaseEntity.Attribute.Normal)//���� �ƴϸ� ��� �ƴϸ� ��/��
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
        else if (_iconState == PartyIconState.Skill || _iconState == PartyIconState.Skill2)//��ų ������
        {
            VeiwToolTip(0);
            
            ViewSkillTip(_data, _iconState, _desc);
            Debug.Log("Skill Icon ���� Ȱ��");
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
                //class_Icon_text.text = "���� ���� �ӵ��� �� �Ÿ����� ���� �����ϴ� ���Ÿ� ���� Ŭ����";
                class_Icon_text.text = "�� �Ÿ����� ���� �����ϴ� ���Ÿ� ���� Ŭ����";
                class_name_text.text = "���Ÿ� ����";
                break;
            /*
        case Ally.Job.Wizard:
            //class_Icon_text.text = "���� ü���� �������� ������ ��ų�� ���� ����� �� �ִ� ���� Ŭ����";
            class_Icon_text.text = "�� �Ÿ����� ���� �����ϴ� ���Ÿ� ���� Ŭ����";
            class_name_text.text = "������";
            break;
             */
            case Ally.Class.Tank:
                class_Icon_text.text = "���ݷ��� �������� ü���� ���� ���� ���� Ŭ����";
                class_name_text.text = "��Ŀ";
                break;
            case Ally.Class.Melee:
                class_Icon_text.text = "������ ü�°� ���ݷ��� ���� ���� ���� Ŭ����";
                class_name_text.text = "���� ����";
                break;
            case Ally.Class.Support:
                class_Icon_text.text = "���Ḧ �����ϰų� ���� ��ȭ��Ű�� ���� Ŭ����";
                class_name_text.text = "������";
                break;
            default:
                break;
        }
    }
    public void ViewStatTip(StatIconState _stat)//Hp, Mp, atk, atkspd, atkrng, def, spd
    {
        Debug.Log("���������� ������ �Ҵ��ߴ�");
        switch (_stat)
        {
            case StatIconState.HP:
                stat_Icon.sprite = GameUiMgr.single.entityIconRS.spStatIcon[0];
                stat_text.text = "ü��\n ü���� ��� �������� ������ ������ �� �����ϴ�.";
                break;
            case StatIconState.MP:
                stat_Icon.sprite = GameUiMgr.single.entityIconRS.spStatIcon[1];
                stat_text.text = "����\n �⺻ ���� �� 1�� ȸ���Ǹ�, ���� ���� ��ų�� ����� �� �ֽ��ϴ�.";
                break;
            case StatIconState.Atk:
                stat_Icon.sprite = GameUiMgr.single.entityIconRS.spStatIcon[2];
                stat_text.text = "���ݷ�\n �������� ������ ���ϴ� ���ذ� �����մϴ�.";
                break;
            case StatIconState.AtkSpd:
                stat_Icon.sprite = GameUiMgr.single.entityIconRS.spStatIcon[3];
                stat_text.text = "���� �ӵ�\n �������� ���� �� ���� ������ �� �ֽ��ϴ�.";
                break;
            case StatIconState.AtkRng:
                stat_Icon.sprite = GameUiMgr.single.entityIconRS.spStatIcon[4];
                stat_text.text = "���� ����\n �������� �ָ� �ִ� ���� ������ �� �ֽ��ϴ�.";
                break;
            case StatIconState.Def:
                stat_Icon.sprite = GameUiMgr.single.entityIconRS.spStatIcon[5];
                stat_text.text = "����\n �������� �޴� ���ذ� �پ���, �ִ� 60%���� �氨�˴ϴ�.";
                break;
            case StatIconState.Spd:
                stat_Icon.sprite = GameUiMgr.single.entityIconRS.spStatIcon[6];
                stat_text.text = "�̵� �ӵ�\n �������� ������ �̵��� �� �ֽ��ϴ�.";
                break;
            default:
                Debug.Log("������ �ǽð��߻���");
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
                class_name_text.text = "���� ����";
                LastDmg = (_partyDesc.atk * 2).ToString("F3");
                oriDmg = (_partyDesc.atk).ToString();
                TextSetting(ref LastDmg);

                //class_Icon_text.text = "���� ��󿡰� ���ݷ��� 2�� �����("+ _partyDesc.atk * 2+")";
                class_Icon_text.text = "���� ��󿡰� " + LastDmg + "(" + oriDmg + " * 2)����";
                break;
            case Ally.Job.Knight:
                class_icon.sprite = GameUiMgr.single.entityIconRS.GetSkillIcon(Ally.Job.Knight);
                class_name_text.text = "���� ���";

                LastDmg = (_partyDesc.atk * 2).ToString("F3");
                oriDmg = (_partyDesc.atk).ToString();
                TextSetting(ref LastDmg);
                //class_Icon_text.text = "���� ��󿡰� ���ݷ��� 1.3�� �����("+ _partyDesc.atk * 1.3+ ")";
                class_Icon_text.text = "���� ��󿡰� " + LastDmg + "(" + oriDmg + " * 1.3)����";
                break;
            case Ally.Job.Ranger:
                class_icon.sprite = GameUiMgr.single.entityIconRS.GetSkillIcon(Ally.Job.Ranger);
                class_name_text.text = "���� ȭ��";

                LastDmg = (_partyDesc.atk * 2).ToString("F3");
                oriDmg = (_partyDesc.atk).ToString();
                TextSetting(ref LastDmg);
                //class_Icon_text.text = "���� ��󿡰� ���ݷ��� 1.2�� �����("+ _partyDesc.atk * 1.2 + ")";
                class_Icon_text.text = "���� ��󿡰� " + LastDmg + "(" + oriDmg + " * 1.2)����";
                break;
            case Ally.Job.Wizard:
                class_icon.sprite = GameUiMgr.single.entityIconRS.GetSkillIcon(Ally.Job.Wizard);
                class_name_text.text = "���̾� ��Ʈ";

                LastDmg = (_partyDesc.atk * 2).ToString("F3");
                oriDmg = (_partyDesc.atk).ToString();
                TextSetting(ref LastDmg);
                class_Icon_text.text = "���� ��󿡰� "+ LastDmg + "("+ oriDmg + " * 3)����";
                break;
            case Ally.Job.Priest:
                class_icon.sprite = GameUiMgr.single.entityIconRS.GetSkillIcon(Ally.Job.Priest);
                class_name_text.text = "ġ���� ��";
                //class_Icon_text.text = "���� ü���� ���� ���� �Ʊ����� 15(5 + 10 //�ڽ��� �ִ� ü���� 10%��ŭ ȸ�� = ����)";

                LastDmg = (_partyDesc.hp * 0.1).ToString("F3");
                oriDmg = (_partyDesc.atk).ToString();
                TextSetting(ref LastDmg);
                class_Icon_text.text = "���� ü���� ���� ���� �Ʊ����� "+ LastDmg + "(5 + "+ oriDmg + ")ȸ��";
                break;
            case Ally.Job.Demon:
                if (_partyIconState == PartyIconState.Skill)
                {
                    class_icon.sprite = GameUiMgr.single.entityIconRS.GetSkillIcon(Ally.Job.Demon);
                    class_name_text.text = "��Ÿ�����ý�";

                    LastDmg = (_partyDesc.atk * 0.3).ToString("F3");
                    oriDmg = (_partyDesc.atk).ToString();
                    TextSetting(ref LastDmg);
                    //class_Icon_text.text = "�����Ͽ� �⺻ ������ ��ȭ, Ÿ�ݹ����� �߰����� ���ظ� ������\n���ݷ�,���ݼӵ�,���ݹ����� ���� �޴� ���� 15% �����Ѵ�.";
                    class_Icon_text.text = "�����Ͽ� �⺻ ������ ��ȭ�ǰ� ���� ȿ���� �߰��Ǹ� Ÿ�ٰ� ������ ������ " + LastDmg + "(" + oriDmg + "*0.3)����" +
                        "\n���ݷ�, ���� �ӵ�, ���� ������ ����������, �޴� ���ذ� 15% ����մϴ�.";
                }
                else if(_partyIconState == PartyIconState.Skill2)
                {
                    class_icon.sprite = GameUiMgr.single.entityIconRS.GetSkillIcon(Ally.Job.Demon);
                    class_name_text.text = "��ū��";
                    class_Icon_text.text = "�̳���Զ�����.";
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
