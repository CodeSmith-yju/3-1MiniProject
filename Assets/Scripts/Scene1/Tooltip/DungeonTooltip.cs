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
            case 0://����: ���, ������, ���ý�����(�ٶ�,��,��)
                title.text = "����";

                icon_element1.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Wind);
                icon_element2.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Water);
                icon_element3.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Fire);

                icon_enemy1.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Goblin);
                icon_enemy2.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Slime);
                icon_enemy3.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Purple_Slime);

                desc.text = "�� ���̵������� \n���� 25%��ȭ������ \n��� ���� ��ȭ�˴ϴ�.";
                break;
            case 1://����: ���ý�����, �⺻��, ��������(��,�ٶ�,���)
                title.text = "����"; 
                
                icon_element1.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Fire);
                icon_element2.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Wind);
                icon_element3.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Dark);

                icon_enemy1.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Purple_Slime);
                icon_enemy2.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Skeleton);
                icon_enemy3.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Golem);

                desc.text = "�Ϲ����� ���̵��� \n�⺻���� ������ ȹ�氡���մϴ�.";
                break;
            case 2://�����: �����޸�, ��/�� ��(��, ��, ��)
                title.text = "�����";

                icon_element1.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Light);
                icon_element2.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Water);
                icon_element3.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Fire);

                icon_enemy1.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Puppet_Human);
                icon_enemy2.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.IceGolem);
                icon_enemy3.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.FireGolem);

                desc.text = "�� ���̵������� \n���� 25%��ȭ������ \n��� ���� ��ȭ�˴ϴ�.";
                break;
            case 3:
                if (_onoff)
                {
                    title.text = "�Ǹ��� �̱�";

                    icon_element1.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Light);
                    icon_element2.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Dark);
                    icon_element3.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Normal);

                    icon_enemy1.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Puppet_Human);
                    icon_enemy2.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Skeleton);
                    icon_enemy3.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.SkeletonWizard);

                    desc.text = "�Ǹ��� �̱��Դϴ�. \n���� 50%��ȭ�˴ϴ�.";
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

                    desc.text = "������ �����Դϴ�. \n� ���� ������ �𸣴� �����ؾ��մϴ�.";
                }

                break;
            default://-1, Ʃ�丮��: ���, ������(�ٶ�,��) 
                title.text = "Ʃ�丮��";
                icon_element1.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Wind);
                icon_element2.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Water);
                icon_element3.sprite = GameUiMgr.single.entityIconRS.GetElementIcon(BaseEntity.Attribute.Dark);

                icon_enemy1.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Goblin);
                icon_enemy2.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Slime);
                icon_enemy3.sprite = GameUiMgr.single.entityIconRS.GetEnemyIcon(IconEnemy.Skeleton);

                desc.text = "���� ������ ���� �⺻���� ������ ü���� �� �ֽ��ϴ�.";
                //desc_elemnet.text = "�� ���̵������� �ٶ�, �� �Ӽ��� ���, �������� �����ϸ�, �ٰŸ�/���Ÿ� ������ �մϴ�. �⺻���� ������ ������ �� �ֽ��ϴ�.";
                //icon_enemy1.sprite = GameUiMgr.single.entityIconRS;
                break;
        }
    }

}
