using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    public void SetupTooltip(int _DungeonLvel, bool _onoff)
    {
        if (_DungeonLvel == 3)
        {
            if (_onoff)
            {
                icon_element3.gameObject.SetActive(false);
            }
            else
            {
                icon_element3.gameObject.SetActive(true);
            }
        }
        else
        {
            icon_element3.gameObject.SetActive(true);
        }

        switch (_DungeonLvel)
        {
            /*case -1:
                break;*/
            case 0://쉬움: 고블린, 슬라임, 퍼플슬라임(바람,물,불)
                title.text = "쉬움";

                icon_element1.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Wind);
                icon_element2.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Water);
                icon_element3.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Fire);

                icon_enemy1.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Goblin);
                icon_enemy2.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Slime);
                icon_enemy3.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Purple_Slime);

                desc.text = "이 난이도에서는 \n적이 25%약화되지만 \n대신 보상도 약화됩니다.";
                break;
            case 1://보통: 퍼플슬라임, 기본골렘, 스컬전사(불,바람,어둠)
                title.text = "보통"; 
                
                icon_element1.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Fire);
                icon_element2.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Wind);
                icon_element3.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Dark);

                icon_enemy1.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Purple_Slime);
                icon_enemy2.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Skeleton);
                icon_enemy3.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Golem);

                desc.text = "일반적인 난이도로 \n기본적인 보상을 획득가능합니다.";
                break;
            case 2://어려움: 퍼핏휴먼, 불/얼 골렘(빛, 물, 불)
                title.text = "어려움";

                icon_element1.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Light);
                icon_element2.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Water);
                icon_element3.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Fire);

                icon_enemy1.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Puppet_Human);
                icon_enemy2.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.IceGolem);
                icon_enemy3.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.FireGolem);

                desc.text = "이 난이도에서는 \n적이 25%강화되지만 \n대신 보상도 강화됩니다.";
                break;
            case 3:
                if (_onoff)
                {
                    title.text = "악마의 미궁";

                    icon_element1.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Light);
                    icon_element2.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Dark);
                    icon_element3.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Normal);

                    icon_enemy1.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Puppet_Human);
                    icon_enemy2.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Skeleton);
                    icon_enemy3.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.SkeletonWizard);

                    desc.text = "악마의 미궁입니다. \n적이 50%강화됩니다.";
                }
                else
                {
                    title.text = "???";

                    icon_element1.sprite = GameUiMgr.single.question;
                    icon_element2.sprite = GameUiMgr.single.question;
                    icon_element3.sprite = GameUiMgr.single.question;

                    icon_enemy1.sprite = GameUiMgr.single.question;
                    icon_enemy2.sprite = GameUiMgr.single.question;
                    icon_enemy3.sprite = GameUiMgr.single.question;

                    desc.text = "미지의 던전입니다. \n어떤 적이 있을지 모르니 조심해야합니다.";
                }

                break;
            default://-1, 튜토리얼: 고블린, 슬라임(바람,물) 
                title.text = "튜토리얼";
                icon_element1.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Wind);
                icon_element2.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Water);
                icon_element3.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Dark);

                icon_enemy1.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Goblin);
                icon_enemy2.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Slime);
                icon_enemy3.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Skeleton);

                desc.text = "모의 전투를 통해 기본적인 전투를 체험할 수 있습니다.";
                //desc_elemnet.text = "이 난이도에서는 바람, 물 속성의 고블린, 슬라임이 등장하며, 근거리/원거리 공격을 합니다. 기본적인 전투를 경험할 수 있습니다.";
                //icon_enemy1.sprite = GameUiMgr.single.entityIconRS;
                break;
        }
    }

}
