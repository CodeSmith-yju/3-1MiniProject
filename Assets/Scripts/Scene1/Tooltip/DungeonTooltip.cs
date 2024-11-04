using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonTooltip : MonoBehaviour
{
    [Header("Attribute Icon")]
    [SerializeField] Image icon_element1;
    [SerializeField] Image icon_element2;
    [SerializeField] Image icon_element3;

    [Header("Enemy Icon")]
    [SerializeField] Image icon_enemy1;
    [SerializeField] Image icon_enemy2;
    [SerializeField] Image icon_enemy3;
    [Header("Text")]
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI desc;


    public void SetupTooltip(int _DungeonLvel)
    {
        Icon3OnOff(false);
        switch (_DungeonLvel)
        {
            /*case -1:
                break;*/
            case 0:
                Icon3OnOff(true);
                title.text = "����";
                desc.text = "�� ���̵������� ���� 25%��ȭ��, ��� ���� ��ȭ��";
                break;
            case 1:
                Icon3OnOff(true);
                title.text = "����";
                desc.text = "�Ϲ����� ���̵��� �⺻���� ������ ȹ�氡����";
                break;
            case 2:
                Icon3OnOff(true);
                title.text = "�����";
                desc.text = "�� ���̵������� ���� 25%��ȭ��, ��� ���� ��ȭ��";
                break;
            default://-1
                title.text = "Ʃ�丮��";
                desc.text = "���� ������ ���� �⺻���� ������ ü���� �� �ֽ��ϴ�.";
                //desc_elemnet.text = "�� ���̵������� �ٶ�, �� �Ӽ��� ���, �������� �����ϸ�, �ٰŸ�/���Ÿ� ������ �մϴ�. �⺻���� ������ ������ �� �ֽ��ϴ�.";
                //icon_enemy1.sprite = GameUiMgr.single.entityIconRS;
                break;
        }
    }

    public void Icon3OnOff(bool _bool)
    {
        icon_enemy3.gameObject.SetActive(_bool);
        icon_element3.gameObject.SetActive(_bool);
    }

}
