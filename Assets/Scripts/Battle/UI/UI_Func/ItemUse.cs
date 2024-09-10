using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemUse : MonoBehaviour
{
    StatManager[] party_stat;
    public TMP_Text item_Cnt_Text;
    [SerializeField] int item_Cnt;
    [SerializeField] Item myItem;
    [SerializeField] Image itemImg;


    /*private void SetItem()
    {
        for (int i = 0; i < Inventory.Single.items.Count; i++)
        {
            if (Inventory.Single.items[i].itemType == Item.ItemType.Consumables)
            {
                ItemUse iia = Instantiate(Prefab, BattleManager.Instance.ui.item_Bar.transform.GetChild(0));

                // 생성된 슬롯 초기화
                iia.Init(Inventory.Single.items[i]);

                // 생성된 슬롯을 리스트에 추가
                Inner.IiaList.Add(iia);
            }
        }

    }*/


    public void Init(Item _item)
    {
        myItem = _item;
        itemImg.sprite = myItem.itemImage;
        item_Cnt = myItem.itemStack;
        item_Cnt_Text.text = item_Cnt.ToString();
    }


    public void ShowPostionUI()
    {
        party_stat = GameObject.FindObjectsOfType(typeof(StatManager)) as StatManager[];


        if (item_Cnt != 0)
        {
            Canvas statbar = BattleManager.Instance.ui.player_Statbar.AddComponent<Canvas>();
            BattleManager.Instance.ui.player_Statbar.AddComponent<GraphicRaycaster>();
            statbar.overrideSorting = true;
            statbar.sortingOrder = 1;
            statbar.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;

            if (BattleManager.Instance.dialogue != null && BattleManager.Instance.tutorial != null) 
            {
                if (BattleManager.Instance.dialogue.isTutorial && BattleManager.Instance.tutorial.isItem_Tutorial)
                {
                    BattleManager.Instance.ui.item_Tutorial.SetActive(false);
                }
            }
            BattleManager.Instance.ui.item_Use_UI.SetActive(true);

            
            foreach (StatManager stat in party_stat)
            {
                Button membnt = stat.gameObject.AddComponent<Button>();
                Debug.Log(membnt.gameObject.name);
                membnt.onClick.AddListener(() => Postion(stat));
            }
        }
        else
        {
            BattleManager.Instance.ui.OpenPopup(BattleManager.Instance.ui.alert_Popup);
            BattleManager.Instance.ui.alert_Popup.GetComponent<TitleInit>().Init("사용 할 아이템의 갯수가 부족합니다.");
            return;
        }
        
    }


    private void Postion(StatManager player)
    {
        if (!player.isDead)
        {
            if (BattleManager.Instance.tutorial == null || BattleManager.Instance.dialogue == null)
            {
                if (player.player.cur_Player_Hp == player.player.max_Player_Hp)
                {
                    BattleManager.Instance.ui.OpenPopup(BattleManager.Instance.ui.alert_Popup);
                    BattleManager.Instance.ui.alert_Popup.GetComponent<TitleInit>().Init("회복할 체력이 없습니다.");
                    HidePostionUI();
                    return;
                }
            }
            

            foreach (PlayerData player_index in GameMgr.playerData)
            {
                if (player.player.playerIndex == player_index.playerIndex)
                {
                    if ((player.player.cur_Player_Hp + 5f) <= player.player.max_Player_Hp)
                    {
                        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Deploy)
                        {
                            foreach (GameObject ally in BattleManager.Instance.deploy_Player_List)
                            {
                                Ally ally_player = ally.GetComponent<Ally>();

                                if (player_index.playerIndex == ally_player.entity_index)
                                {
                                    ally_player.cur_Hp += 5f;
                                    break;
                                }
                            }
                        }
                        else if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Rest)
                        {
                            player_index.cur_Player_Hp += 5f;
                        }
                    }
                    else if ((player.player.cur_Player_Hp + 5f) > player.player.max_Player_Hp)
                    {
                        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Deploy)
                        {
                            foreach (GameObject ally in BattleManager.Instance.deploy_Player_List)
                            {
                                Ally ally_player = ally.GetComponent<Ally>();

                                if (player_index.playerIndex == ally_player.entity_index)
                                {
                                    ally_player.cur_Hp = player_index.max_Player_Hp;
                                    break;
                                }
                            }
                        }
                        else if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Rest)
                        {
                            player_index.cur_Player_Hp = player_index.max_Player_Hp;
                        }
                    }
                }
            }
            
            item_Cnt -= 1;
            item_Cnt_Text.text = item_Cnt.ToString();

            HidePostionUI();

            if (BattleManager.Instance.tutorial != null && BattleManager.Instance.dialogue != null)
            {
                if (BattleManager.Instance.dialogue.isTutorial && BattleManager.Instance.tutorial.isItem_Tutorial)
                {
                    BattleManager.Instance.tutorial.EndTutorial(6);
                }
            }
        }
        else
        {
            BattleManager.Instance.ui.OpenPopup(BattleManager.Instance.ui.alert_Popup);
            BattleManager.Instance.ui.alert_Popup.GetComponent<TitleInit>().Init("죽은 파티원에게는 \n사용 할 수 없습니다.");
            HidePostionUI();
            return;
        }
    }

    private void HidePostionUI()
    {
        Destroy(BattleManager.Instance.ui.player_Statbar.GetComponent<GraphicRaycaster>());
        Destroy(BattleManager.Instance.ui.player_Statbar.GetComponent<Canvas>());
        foreach (StatManager stat in party_stat)
        {
            Button membnt = stat.gameObject.GetComponent<Button>();
            Destroy(membnt);
        }
        BattleManager.Instance.ui.item_Use_UI.SetActive(false);
    }
}
