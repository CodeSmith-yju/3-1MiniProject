using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestMgr : MonoBehaviour
{
    public int questId;
    public int questActionIndex;// 퀘스트 대화순서
    private Dictionary<int, QuestData> dict_questList;

    [Header("Quest Object List")]
    public GameObject[] aryQuestObj;

    [Header("Quest Icons")]
    public GameObject[] questIcons;
    public Sprite[] spQuestIcons;

    [Header("NPC list")]
    public GameObject[] receptionist;// 모험가길드에서 튜토리얼을진행할 접수원을 분할하여 퀘스트기능을 구현하는데 용이하도록함

    //04-26 Quest Potion Ev
    //private static bool oneTimeEv = false;

    private void Awake()
    {
        dict_questList = new Dictionary<int, QuestData>();
        GenerateQuestData();

        foreach (var icon in questIcons)
        {
            icon.gameObject.SetActive(false);
        }
        questIcons[0].SetActive(true);
    }
    private void GenerateQuestData()
    {
        dict_questList.Add(0, new QuestData("모험의 시작", new int[] { 1000 }));
        // Add메서드로 questID, questData를 데이터사전(= dict_questList)에 저장. 구조체 매개변수 생성자의 int배열에는 첫 마을 방문 퀘스트에 연관된 npcID를 입력
        dict_questList.Add(10, new QuestData("모험가 길드 직원에게 말을 걸어보자", new int[] { 1000, 2000 }));
        dict_questList.Add(20, new QuestData("장비를 착용하고 다시 말을 걸어보자", new int[] { 1000, 2000 }));

        dict_questList.Add(30, new QuestData("파티원을 모집하자", new int[] { 1000, 2000 }));
        //dict_questList.Add(40, new QuestData("체력이 줄었다. 받은 물약을 먹자.", new int[] { 1000, 2000 }));
        dict_questList.Add(40, new QuestData("모의 전투에서 승리하자", new int[] { 1000, 2000 }));
        dict_questList.Add(50, new QuestData("모험가 등록 완료", new int[] { 1000, 2000 }));

        //dict_questList.Add(30, new QuestData("마을의 전설 듣기 퀘스트 클리어!", new int[] { 10000, 4000 }));
    }

    public int GetQuestTalkIndex(int id_Npc) // Npc의 Id를 매개변수로 받아서 퀘스트번호를 반환하는 메서드
    {

        return questId + questActionIndex;
    }

    public string CheckQuest(int id_Npc)
    {
        //순서에 맞게 대화했을때만 퀘스트 대화순서를 올림
        if (id_Npc == dict_questList[questId].npcId[questActionIndex])
        {
            questActionIndex++;
        }

        //Control Quest Objcet
        ControlQuestObejct();

        //Talk Complete & Next Quest
        if (questActionIndex == dict_questList[questId].npcId.Length)
        {
            NextQuest();
        }
        //Quest Name return
        return dict_questList[questId].questName;
    }
    public string CheckQuest()
    {
        return dict_questList[questId].questName;///Quest Name return
    }

    private void NextQuest()
    {
        //새로운 퀘스트로 이어졌다면 바꾸고 초기화
        questId += 10;
        questActionIndex = 0;
    }

    public void ControlQuestObejct()
    {
        //Item questItem;
        switch (questId)
        {
            case 10:// Start Tutorial
                if (questActionIndex == 0)
                {
                    //TutorialEquip();
                }
                if (questActionIndex == 1)
                {
                    GameUiMgr.single.tmp_PlayerRating.text = "견습 모험가";
                    questIcons[0].GetComponent<SpriteRenderer>().sprite = spQuestIcons[1];

                    receptionist[0].SetActive(false);
                    receptionist[1].SetActive(true);
                }
                if (questActionIndex == 2)
                {
                    questIcons[0].GetComponent<SpriteRenderer>().sprite = spQuestIcons[0];

                    receptionist[0].SetActive(true);
                    receptionist[1].SetActive(false);

                    TutorialEquip();
                }
                break;
            case 20:// wear EquipMent Event
                //questItem = ItemResources.instance.itemRS[0];

                if (questActionIndex == 0)
                {
                    Debug.Log("Case 20");
                    //questIcons[0].GetComponent<SpriteRenderer>().sprite = spQuestIcons[1];
                }

                if (questActionIndex == 1)
                {
                    Debug.Log("Case 21");
                    GameUiMgr.single.AllEquipChek();
                    questIcons[0].GetComponent<SpriteRenderer>().sprite = spQuestIcons[0];

                }
                else if (questActionIndex == 2)
                {
                    Debug.Log("Case 22");
                    receptionist[0].SetActive(true);
                    receptionist[1].SetActive(false);
                }
                break;
            case 30:// Dungeon Event
                if (questActionIndex == 0)
                {
                    Debug.Log("Case 30");
                }
                if (questActionIndex == 1)
                {
                    Debug.Log("Case31");
                }
                else if (questActionIndex == 2)
                {
                    Debug.Log("Case32");
                    for (int i = 0; i < 5; i++)
                    {
                        Debug.Log("Make Tutorial Potion");
                        Inventory.Single.AddItem(ItemResources.instance.itemRS[6]);
                    }
                    receptionist[0].SetActive(true);
                    receptionist[1].SetActive(false);
                }
                break;
            case 40:// Using Potion Event
                if (questActionIndex == 0)
                {
                    Debug.Log("Case 40");
                }
                if (questActionIndex == 1)
                {
                    Debug.Log("Case 41");
                }
                else if (questActionIndex == 2)
                {
                    Debug.Log("Case 42");
                    GameUiMgr.single.tmp_PlayerRating.text = "9급 모험가";
                    /*GameMgr.single.IsGameLoad(true);
                    GameUiMgr.single.GameSave();
                    SceneManager.LoadScene("Title");*/

                    /*receptionist[0].SetActive(true);
                    receptionist[1].SetActive(false);*/

                }
                break;
            case 50:
                if (questActionIndex == 0)
                {
                    Debug.Log("Case 50");
                    
                }

                if (questActionIndex == 1)
                {
                    Debug.Log("Case 51");
                }
                break;
        }
    }
    /*    Item questItem2;
    if (questActionIndex == 0){Debug.Log("Case 40");}
    if (questActionIndex == 1)
    {
        if (oneTimeEv == true)
        {
            questItem2 = ItemResources.instance.itemRS[6];
            Inventory.single.AddItem(questItem2);
            GameUiMgr.single.slots[questItem2.itemIndex].wearChek = true;

            Debug.Log(questItem2.itemName);
            oneTimeEv = false;
        }
        Debug.Log("Case 41");
    }
    else if (questActionIndex == 2)
    {
        Debug.Log("Case 42");
        receptionist[0].SetActive(true);
        receptionist[1].SetActive(false);

    }
    break;*/

    public void TutorialEquip()
    {
        questIcons[0].GetComponent<SpriteRenderer>().sprite = spQuestIcons[1];

        Inventory.Single.AddItem(ItemResources.instance.itemRS[2]);
        Inventory.Single.AddItem(ItemResources.instance.itemRS[3]);
        Inventory.Single.AddItem(ItemResources.instance.itemRS[4]);
        Inventory.Single.AddItem(ItemResources.instance.itemRS[5]);

        GameUiMgr.single.RedrawSlotUI();
    }

}
