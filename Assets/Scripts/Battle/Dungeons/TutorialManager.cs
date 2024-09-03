using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public bool isDeploy_Tutorial;
    public bool isItem_Tutorial;

    public void Tutorial(int quest_cnt)
    {
        Debug.Log(quest_cnt);
        switch (quest_cnt)
        {
            /*case 0: // 현재 사용하지 않는 튜토리얼
                Debug.Log("들어온 포탈 UI");
                BattleManager.Instance.ui.ui_Tutorial_Rest.SetActive(true);

                Canvas in_Portal = BattleManager.Instance.ui.in_Portal.AddComponent<Canvas>();
                in_Portal.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
                in_Portal.overrideSorting = true;
                in_Portal.sortingOrder = 1;

                if (BattleManager.Instance.ui.ui_Tutorial_Rest.activeSelf)
                {
                    BattleManager.Instance.ui.ui_Tutorial_Rest.transform.GetChild(quest_cnt).gameObject.SetActive(true);
                }
                BattleManager.Instance.dialogue.NextDialogue();
                break;*/
            case 1:
                Debug.Log("다음 방으로 가는 포탈 UI");
                //EndTutorial(quest_cnt - 1);
                BattleManager.Instance.ui.ui_Tutorial_Rest.SetActive(true);
                Canvas out_Portal = BattleManager.Instance.ui.out_Portal.AddComponent<Canvas>();
                out_Portal.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
                out_Portal.overrideSorting = true;
                out_Portal.sortingOrder = 1;

                if (BattleManager.Instance.ui.ui_Tutorial_Rest.activeSelf)
                {
                    BattleManager.Instance.ui.ui_Tutorial_Rest.transform.GetChild(quest_cnt - 1).gameObject.SetActive(false);
                    BattleManager.Instance.ui.ui_Tutorial_Rest.transform.GetChild(quest_cnt).gameObject.SetActive(true);
                }
                BattleManager.Instance.dialogue.NextDialogue();
                break;
            case 2:
                Debug.Log("상태창 UI");
                EndTutorial(quest_cnt - 1);

                Canvas stat_Bar = BattleManager.Instance.ui.player_Statbar.AddComponent<Canvas>();
                stat_Bar.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
                stat_Bar.overrideSorting = true;
                stat_Bar.sortingOrder = 1;

                if (BattleManager.Instance.ui.ui_Tutorial_Rest.activeSelf)
                {
                    BattleManager.Instance.ui.ui_Tutorial_Rest.transform.GetChild(quest_cnt - 1).gameObject.SetActive(false);
                    BattleManager.Instance.ui.ui_Tutorial_Rest.transform.GetChild(quest_cnt).gameObject.SetActive(true);
                }
                BattleManager.Instance.dialogue.NextDialogue();
                break;
            case 3:
                Debug.Log("미니맵 UI");
                EndTutorial(quest_cnt - 1);

                Canvas mini_Map = BattleManager.Instance.ui.mini_Map.AddComponent<Canvas>();
                mini_Map.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
                mini_Map.overrideSorting = true;
                mini_Map.sortingOrder = 1;

                if (BattleManager.Instance.ui.ui_Tutorial_Rest.activeSelf)
                {
                    BattleManager.Instance.ui.ui_Tutorial_Rest.transform.GetChild(quest_cnt - 1).gameObject.SetActive(false);
                    BattleManager.Instance.ui.ui_Tutorial_Rest.transform.GetChild(quest_cnt).gameObject.SetActive(true);
                }
                BattleManager.Instance.dialogue.NextDialogue();
                break;
            case 4:
                Debug.Log("아이템 창");
                EndTutorial(quest_cnt - 1);

                Canvas item_Bar = BattleManager.Instance.ui.item_Bar.AddComponent<Canvas>();
                item_Bar.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
                item_Bar.overrideSorting = true;
                item_Bar.sortingOrder = 1;
                

                BattleManager.Instance.ui.dialogue_Box.transform.position = new Vector3(BattleManager.Instance.ui.dialogue_Box.transform.position.x, 410f, 0);
                if (BattleManager.Instance.ui.ui_Tutorial_Rest.activeSelf)
                {
                    BattleManager.Instance.ui.ui_Tutorial_Rest.transform.GetChild(quest_cnt - 1).gameObject.SetActive(false);
                    BattleManager.Instance.ui.ui_Tutorial_Rest.transform.GetChild(quest_cnt).gameObject.SetActive(true);
                }
                BattleManager.Instance.dialogue.NextDialogue();
                break;
            case 5:
                EndTutorial(quest_cnt - 1);
                Debug.Log("UI 튜토리얼 끝");
                EndTutorial(quest_cnt);
                break;
            case 6:
                Debug.Log("아이템 사용 튜토리얼");

                GameMgr.playerData[0].cur_Player_Hp -= 5;
                isItem_Tutorial = true;
                Canvas item_Bar_Use = BattleManager.Instance.ui.item_Bar.AddComponent<Canvas>();
                item_Bar_Use.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
                item_Bar_Use.overrideSorting = true;
                item_Bar_Use.sortingOrder = 1;
                BattleManager.Instance.ui.item_Bar.AddComponent<GraphicRaycaster>();

                BattleManager.Instance.dialogue.ONOFF(false);
                BattleManager.Instance.ui.item_Tutorial.SetActive(true);

                break;
            case 7:
                EndTutorial(quest_cnt);
                break;
            case 8:
                Debug.Log("배치 단계 튜토리얼 시작");
                BattleManager.Instance.ui.ui_Tutorial_Deploy.SetActive(true);

                /*Canvas party_List = BattleManager.Instance.ui.party_List.AddComponent<Canvas>();
                party_List.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
                party_List.overrideSorting = true;
                party_List.sortingOrder = 1;*/

                if (BattleManager.Instance.ui.ui_Tutorial_Deploy.activeSelf)
                {
                    BattleManager.Instance.ui.ui_Tutorial_Deploy.transform.GetChild(quest_cnt - quest_cnt).gameObject.SetActive(true);
                }
                BattleManager.Instance.dialogue.NextDialogue();
                break;
            case 9:
                Debug.Log("유닛 생성 튜토리얼");
                if (BattleManager.Instance.ui.ui_Tutorial_Deploy.activeSelf)
                {
                    BattleManager.Instance.ui.ui_Tutorial_Deploy.transform.GetChild(quest_cnt - quest_cnt).gameObject.SetActive(false);
                    //BattleManager.Instance.ui.ui_Tutorial_Deploy.transform.GetChild((quest_cnt - quest_cnt) + 1).gameObject.SetActive(true);
                }
                BattleManager.Instance.dialogue.NextDialogue();
                break;
            case 10:
                EndTutorial(quest_cnt - 1);
                Debug.Log("배치 영역 튜토리얼");

                if (BattleManager.Instance.ui.ui_Tutorial_Deploy.activeSelf)
                {
                    BattleManager.Instance.ui.ui_Tutorial_Deploy.transform.GetChild((quest_cnt - quest_cnt) + 1).gameObject.SetActive(false);
                    BattleManager.Instance.ui.ui_Tutorial_Deploy.transform.GetChild((quest_cnt - quest_cnt) + 2).gameObject.SetActive(true);
                }
                BattleManager.Instance.dialogue.NextDialogue();
                break;
            case 11:
                EndTutorial(quest_cnt - 1);
                break;
            case 12:
                Debug.Log("전투 시작 버튼 튜토리얼");
                BattleManager.Instance.ui.dialogue_Box.transform.position = new Vector3(BattleManager.Instance.ui.dialogue_Box.transform.position.x, 410f, 0);

                Canvas battle_Start = BattleManager.Instance.ui.battleStart.AddComponent<Canvas>();
                battle_Start.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
                battle_Start.overrideSorting = true;
                battle_Start.sortingOrder = 1;

                if (BattleManager.Instance.ui.ui_Tutorial_Deploy.activeSelf)
                {
                    BattleManager.Instance.ui.ui_Tutorial_Deploy.transform.GetChild((quest_cnt - quest_cnt) + 3).gameObject.SetActive(true);
                }
                BattleManager.Instance.dialogue.NextDialogue();
                break;
            case 13:
                EndTutorial(quest_cnt - 1);
                BattleManager.Instance.dialogue.NextDialogue();
                break;
            case 14:
                // 사용하지 않는 튜토리얼 ( 파티 리스트에서 배치 )
                Debug.Log("유닛 배치 튜토리얼");
                isDeploy_Tutorial = true;
                Canvas party_List_Deploy = BattleManager.Instance.ui.party_List.AddComponent<Canvas>();
                BattleManager.Instance.ui.party_List.AddComponent<GraphicRaycaster>();
                party_List_Deploy.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
                party_List_Deploy.overrideSorting = true;
                party_List_Deploy.sortingOrder = 1;

                BattleManager.Instance.dialogue.ONOFF(false);
                if (BattleManager.Instance.ui.ui_Tutorial_Deploy.activeSelf)
                {
                    BattleManager.Instance.ui.ui_Tutorial_Deploy.transform.GetChild((quest_cnt - quest_cnt) + 4).gameObject.SetActive(true);
                }
                break;
            case 15:
                EndTutorial(quest_cnt);
                break;
            case 16:
                EndTutorial(quest_cnt);
                break;
            case 17:
                if(BattleManager.Instance.ui.out_Portal.activeSelf)
                {
                    BattleManager.Instance.ui.out_Portal.GetComponent<FadeEffect>().fadein = true;
                }
                BoxOpen[] box = FindObjectsOfType<BoxOpen>();
                foreach (BoxOpen boxs in box)
                {
                    if (boxs.tag == "Box")
                    {
                        boxs.SetTutorial(false);
                    }
                    else
                    {
                        boxs.SetTutorial(true);
                    }
                }

                BattleManager.Instance.ui.ui_Tutorial_Box.SetActive(true);
                if (BattleManager.Instance.ui.ui_Tutorial_Box.activeSelf)
                {
                    BattleManager.Instance.ui.ui_Tutorial_Box.transform.GetChild((quest_cnt - quest_cnt)).gameObject.SetActive(true);
                }

                BattleManager.Instance.ui.dialogue_Bg.SetActive(false);
                BattleManager.Instance.dialogue.ONOFF(false);
                break;
            case 18:
                BattleManager.Instance.ui.ui_Tutorial_Box.SetActive(true);
                if (BattleManager.Instance.ui.ui_Tutorial_Box.activeSelf)
                {
                    BattleManager.Instance.ui.ui_Tutorial_Box.transform.GetChild((quest_cnt - quest_cnt)).gameObject.SetActive(false);
                    BattleManager.Instance.ui.ui_Tutorial_Box.transform.GetChild((quest_cnt - quest_cnt) + 1).gameObject.SetActive(true);
                }
                BattleManager.Instance.ui.dialogue_Bg.SetActive(false);
                BattleManager.Instance.dialogue.ONOFF(false);

                BoxOpen mimic = GameObject.FindGameObjectWithTag("Mimic").GetComponent<BoxOpen>();
                mimic.SetTutorial(false);

                break;
            case 19:
                EndTutorial(quest_cnt);
                break;
            default:
                Debug.Log("아직 구현안함");
                break;
        }
    }


    public void EndTutorial(int quest_cnt)
    {
        Debug.Log(quest_cnt);
        switch (quest_cnt)
        {
            /*case 0: // 현재 사용하지 않는 튜토리얼
                Debug.Log("In_Portal 튜토리얼 끝");
                Destroy(BattleManager.Instance.ui.in_Portal.GetComponent<Canvas>());
                break;*/
            case 1:
                Debug.Log("out_Portal 튜토리얼 끝");
                Destroy(BattleManager.Instance.ui.out_Portal.GetComponent<Canvas>());
                break;
            case 2:
                Debug.Log("Stat_Bar 튜토리얼 끝");
                Destroy(BattleManager.Instance.ui.player_Statbar.GetComponent<Canvas>());
                break;
            case 3:
                Debug.Log("miniMap 튜토리얼 끝");
                Destroy(BattleManager.Instance.ui.mini_Map.GetComponent<Canvas>());
                break;
            case 4:
                Debug.Log("item_bar 튜토리얼 끝");
                Destroy(BattleManager.Instance.ui.item_Bar.GetComponent<Canvas>());
                break;
            case 5:
                Debug.Log("UI 튜토리얼 끝");
                BattleManager.Instance.ui.dialogue_Box.transform.position = new Vector3(BattleManager.Instance.ui.dialogue_Box.transform.position.x, 10f, 0);
                if (BattleManager.Instance.ui.ui_Tutorial_Rest.activeSelf)
                {
                    BattleManager.Instance.ui.ui_Tutorial_Rest.SetActive(false);
                }
                BattleManager.Instance.dialogue.NextDialogue();
                break;
            case 6:
                Debug.Log("아이템 튜토리얼 끝");
                isItem_Tutorial = false;
                Destroy(BattleManager.Instance.ui.item_Bar.GetComponent<GraphicRaycaster>());
                Destroy(BattleManager.Instance.ui.item_Bar.GetComponent<Canvas>());

                BattleManager.Instance.ui.item_Tutorial.SetActive(false);

                BattleManager.Instance.dialogue.ONOFF(true);
                BattleManager.Instance.dialogue.NextDialogue();
                break;
            case 7:
                Debug.Log("휴식방 튜토리얼 끝");
                BattleManager.Instance.ui.dialogue_Bg.SetActive(false);
                BattleManager.Instance.dialogue.ONOFF(false);
                break;
            case 9:
                // 사용하지 않는 튜토리얼
                Debug.Log("파티 리스트 UI 튜토리얼 끝");
                //Destroy(BattleManager.Instance.ui.party_List.GetComponent<Canvas>());
                break;
            case 10:
                Debug.Log("배치 영역 튜토리얼 끝");
                BattleManager.Instance.ui.ui_Tutorial_Deploy.transform.GetChild((quest_cnt - quest_cnt) + 2).gameObject.SetActive(false);
                BattleManager.Instance.dialogue.NextDialogue();
                break;
            case 12:
                Debug.Log("전투 시작 버튼 튜토리얼 끝");
                Destroy(BattleManager.Instance.ui.battleStart.GetComponent<Canvas>());
                BattleManager.Instance.ui.dialogue_Box.transform.position = new Vector3(BattleManager.Instance.ui.dialogue_Box.transform.position.x, 10f, 0);
                BattleManager.Instance.ui.ui_Tutorial_Deploy.transform.GetChild((quest_cnt - quest_cnt) + 3).gameObject.SetActive(false);
                break;
            case 14:
                // 사용하지 않는 튜토리얼
                Debug.Log("유닛 배치 튜토리얼 끝");
                isDeploy_Tutorial = false;
                Destroy(BattleManager.Instance.ui.party_List.GetComponent<GraphicRaycaster>());
                Destroy(BattleManager.Instance.ui.party_List.GetComponent<Canvas>());

                BattleManager.Instance.ui.ui_Tutorial_Deploy.transform.GetChild((quest_cnt - quest_cnt) + 4).gameObject.SetActive(false);

                BattleManager.Instance.dialogue.ONOFF(true);
                BattleManager.Instance.dialogue.NextDialogue();
                break;
            case 15:
                Debug.Log("전투 방 튜토리얼 끝");
                BattleManager.Instance.ui.ui_Tutorial_Deploy.SetActive(false);
                BattleManager.Instance.dialogue.ONOFF(false);
                BattleManager.Instance.ui.dialogue_Bg.SetActive(false);
                break;
            case 16:
                Debug.Log("전투 방 종료");
                BattleManager.Instance.dialogue.ONOFF(false);
                BattleManager.Instance.ui.dialogue_Bg.SetActive(false);
                break;
            case 17:
                Debug.Log("일반 상자 튜토리얼 끝");
                BattleManager.Instance.ui.dialogue_Bg.SetActive(true);
                BattleManager.Instance.ui.ui_Tutorial_Box.transform.GetChild((quest_cnt - quest_cnt)).gameObject.SetActive(false);

                BattleManager.Instance.dialogue.ONOFF(true);
                BattleManager.Instance.dialogue.NextDialogue();
                break;
            case 18:
                Debug.Log("미믹 튜토리얼 끝");
                BattleManager.Instance.ui.dialogue_Bg.SetActive(true);
                BattleManager.Instance.ui.ui_Tutorial_Box.transform.GetChild((quest_cnt - quest_cnt) + 1).gameObject.SetActive(false);
                BattleManager.Instance.ui.ui_Tutorial_Box.SetActive(false);

                BattleManager.Instance.dialogue.ONOFF(true);
                BattleManager.Instance.dialogue.NextDialogue();
                break;
            case 19:
                BattleManager.Instance.dialogue.ONOFF(false);
                BattleManager.Instance.ui.dialogue_Bg.SetActive(false);
                break;
            default:
                Debug.Log("아직 구현안함");
                break;
        }
    }
}
