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
                title.text = "쉬움";
                desc.text = "이 난이도에서는 적이 25%약화된, 대신 보상도 약화된";
                break;
            case 1:
                Icon3OnOff(true);
                title.text = "보통";
                desc.text = "일반적인 난이도로 기본적인 보상을 획득가능한";
                break;
            case 2:
                Icon3OnOff(true);
                title.text = "어려움";
                desc.text = "이 난이도에서는 적이 25%강화된, 대신 보상도 강화된";
                break;
            default://-1
                title.text = "튜토리얼";
                desc.text = "모의 전투를 통해 기본적인 전투를 체험할 수 있습니다.";
                //desc_elemnet.text = "이 난이도에서는 바람, 물 속성의 고블린, 슬라임이 등장하며, 근거리/원거리 공격을 합니다. 기본적인 전투를 경험할 수 있습니다.";
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
