﻿//using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUiMgr : MonoBehaviour/*, IBeginDragHandler, IDragHandler, IEndDragHandler*/
{
    public static GameUiMgr single;
    private GameObject scanObject;//상호작용중인 오브젝트의 정보를 받아오 변수, PlayerAction에서 스캔할 오브젝트정보를 받아오기때문에 얘는 인스펙터에서 안 보여도된다.
    
    [Header("TalkPanel")]
    [SerializeField] private Image imgTalkPnel;// 대화창 표시유무를 위해 변수 선언
    public bool isActionTalk;// 대화창의 활성화 유무를 판별하기위한 변수
    public Image imgPortrait;// 초상화 이미지를 관리할 변수
    public TextMeshProUGUI talkName;
    [Header("TalkMgr")]
    public TalkMgr talkMgr;// 대화 매니저를 변수로 선언하여 대화매니저의 함수에 접근 할 수 있게함.
    public int talkIndex;

    [Header("TextEffect")]
    public TypeEffect typeTextEffect;

    [Header("Player DESC")]
    //GameMgr.PlayerData.NAME/Level 을 인계받아 화면에 출력할 TMP변수
    public TextMeshProUGUI tmp_PlayerName;
    public TextMeshProUGUI tmp_PlayerLevle;
    public TextMeshProUGUI tmp_PlayerGold;

    public Slider s_HP;
    public Slider s_SN;
    public Slider s_EXP;

    [Header("Button")]
    public GameObject mainButton;// 클릭할 메인 버튼
    public GameObject[] subButtons;// 클릭하면 펼쳐질 서브버튼들.
    private bool areSubButtonsVisible = false;// 메인버튼을 클릭해서 얘가 true가 되면 서브버튼이 보여짐 
    //[SerializeField] private Image img_Portrait;
    //[SerializeField] private Sprite[] ary_sp_Portrait;
    [SerializeField] private GameObject objSubButtonFrame;
    
    [Header("VideoOption")]
    [SerializeField] private VideoOption videoOption_S1;

    //Minimap
    private bool bigMinimapChek;
    [Header("Minimap")]
    [SerializeField] private Camera miniCamera;
    [SerializeField] private RenderTexture minimapRanderTexture;
    [SerializeField] private RawImage rimgSmall;
    [SerializeField] private RawImage rimgBig;
    [SerializeField] private Image smallMap;
    [SerializeField] private Image bigMap;

    //Player Desc
    [Header("Desc_Player")]
    [SerializeField] private Image PlayerDesc;
    private bool DescCheck;

    //MenuSet
    [Header("Mnue Set")]
    public GameObject menuSet;

    [Header("Player Options")]
    public GameObject player;

    [Header("Quest Manager")]
    public QuestMgr questMgr;//퀘스트 번호를 가져올 퀘스트 매니저 변수 생성
    public TextMeshProUGUI questDesc;

    [Header("Invnetory")]
    [SerializeField] private GameObject inventory_panel;
    private Inventory inventory;
    [HideInInspector] public bool activeInventory = false;
    //03-31 variable Inventoty - try.4LocalDataStore
    public Slot[] slots;
    public Transform slotHolder;
    //04-21 Inventory Slot Drag items
    public Image dragIcon;
    public Slot nowSlot;
    public Slot[] targetSlots;
    public int dragIndex;
    //04-22
    public Image addEquipPanel;
    public Button btn_YesEquipAdd;
    public Button btn_NoEquipAdd;
    public bool equipmnet;
    //Add
    public TextMeshProUGUI textEquipPanel;
    
    

    [Header("ToolTip")]
    public GameObject tooltip;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textTitle;
    public TextMeshProUGUI textDesc;
    //public TextMeshProUGUI textPower;
    public Image imgIcon;
    public Canvas canvas_Tooltip;

    private float canvaseWidth;
    private RectTransform tooltipRect;


    //Vector3 lodingPosition;// player Position

    [Header("player State")]
    public float player_Max_HP;
    public float player_Cur_HP;
    public float player_Max_SN;
    public float player_Cur_SN;
    private float player_Max_MP;
    private float player_Cur_MP;

    public int playerGold;
    public int playerLevel;

    private float player_Atk_Speed;
    private float player_Atk_Range;
    private float player_Base_Atk_Dmg;
    private float player_Max_EXP;
    private float player_Cur_EXP;

    //04-25 Tutorial Quest ActionIndex
    private bool wearEquipment = false;

    //05-12 PartyList
    [Header("Party Bord")]
    public GameObject panelPartyBoard;// 파티 게시판오브젝트
    [SerializeField] private List<PartySlot> poolPartySlot = new(); // 파티게시판의 Body에 해당하는 고용가능한 파티원 리스트 이거수정해야할수도있음
    [SerializeField] private List<PartyData> listPartyData = new();// 실제파티원들 정보가 저장되어야함
    
    //05-21 ClickedPartySlot -> Add Buttom PartySlot 
    public List<PartyData> partyData;// 얘로 파티원데이터생성해서 집어넣음 왜인지 아직 파악못했는데 Slot자체로하려니까 오류가남
    public List<PartySlot> poolMoveInSlot = new(); // 파티게시판의 Buttom에 해당하는 고용파티원 명단 리스트

    public GameObject partyPrefab; // 새로운 슬롯을 생성할 때 사용할 프리팹, 부모 transform은 transfrom.parent를 사용하는것으로 사용안함
    public GameObject playerPrefab;//플레이어 프리펩넣을곳 이걸로 파티고용리스트 0번 요소에 상호작용 불가능한 플레이어이미지를 고정으로 넣어 줄 것.
    //05-14
    public List<GameObject> objListPlayable;// 파티보드에서 출력될 실제 플레이어블 캐릭터 리소스 데이터를 여기에 임시로 등록

    //파티보드 하단에 출력되는 파티원 인원수 카운팅 / 고용할 파티원목록에 포함된 파티원 들 몸값 텍스트 반영
    public TextMeshProUGUI textPartyCount;
    public TextMeshProUGUI textPartyPrice;
    public int partyPrice;

// PoolMoveInSlot에 PartyData가 있을경우 여기에 담아서 고용완료 목록에 추가되어 Battle씬의 PartyList가 얘를 참조하게됨 || 슬롯이가지고있는 PartyData에 포함된 Prefab을 가져가는형식
    public List<PartySlot> lastDeparture; 
    private void Awake()
    {
        single = this;
    }
    public void AddItemTest()
    {
        Debug.Log("AddItem");

        Item newItem = ItemResources.instance.itemRS[UnityEngine.Random.Range(1,6)]; // 새로운 아이템 생성
        inventory.AddItem(newItem); // 인벤토리에 아이템 추가,
        RedrawSlotUI();
    }
    public void ValueUpdate()
    {
        int ran = UnityEngine.Random.Range(0, 2);
        if (ran == 0)
        {
            playerGold += 15;
            player_Max_HP += 15;
            player_Max_SN += 15;
            player_Cur_EXP += 2;

            Debug.Log("player gold: " + playerGold);
            Debug.Log("player max hp: " + player_Max_HP);
            Debug.Log("player max sn: " + player_Max_SN);
            Debug.Log("player cur sn: " + player_Cur_EXP);
        }
        else
        {
            playerGold -= 1;
            player_Cur_HP -= 1;
            player_Cur_SN -= 1;

            Debug.Log("player gold: " + playerGold);
            Debug.Log("player cur hp: " + player_Cur_HP);
            Debug.Log("player cur sn: " + player_Cur_SN);
        }

        SliderChange();
    }
    

    private void Start()
    {
        //06-14 BGM
        AudioManager.single.PlayBgmChange(1);

        imgTalkPnel.gameObject.SetActive(false);// NPC대화창 시작할때 꺼줌
        objSubButtonFrame.SetActive(true);//서브버튼 목록 시작할때 켜줌
        HideSubButtons();//서브버튼 하위 목록 시작할때 꺼줌
        OffVideoOption_S1();//게임옵션 설정창 시작할때 꺼줌
        panelPartyBoard.SetActive(false);//05-12 파티창 게임시작할때 꺼줌

        bigMinimapChek = true;
        smallMap.gameObject.SetActive(true);
        bigMap.gameObject.SetActive(false);

        PlayerDesc.gameObject.SetActive(true);
        DescCheck = true;


        //03-31 Start Inventory - try.4
        inventory = Inventory.single;

        inventory_panel.SetActive(activeInventory);
        inventory.onSlotCountChange += SlotChange;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inventory.onChangeItem += RedrawSlotUI;

        AddSlot();//인벤토리 칸 세팅할때 나는 설정 안 만져서 그런지 이걸로 인벤토리 한번 활성화 시켜주지않으면 이상하게 동작하는거 확인.

        EquipSlotSetting();// 씬 실행 시 각 장비 슬롯에 해당하는 아이템 타입을 직접 지정해줌

        //05-21 
        RefreshiPartyBord();

        //04-22
        addEquipPanel.gameObject.SetActive(false);
        // Yes 버튼에 클릭 이벤트 리스너 추가
        btn_YesEquipAdd.onClick.AddListener(OnYesButtonClick);

        // No 버튼에 클릭 이벤트 리스너 추가
        btn_NoEquipAdd.onClick.AddListener(OnNoButtonClick);

        if (GameMgr.single.LoadChecker() == true)
        {
            GameLoad();
            Debug.Log("Load Success");
            Debug.Log(GameMgr.playerData[0].GetPlayerName());
        }
    
        questDesc.text = questMgr.CheckQuest();

        SetPlayerDatas();

        SliderChange();

        //Tooltip
        canvaseWidth = canvas_Tooltip.GetComponent<CanvasScaler>().referenceResolution.x * 0.5f;
    }

    //03-31 Method Inventory - try.4
    public void SlotChange(int val)// slotChange 에서 slot의 slotNum을 차례대로 부여함
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].slotnum = i;

            if (i < inventory.items.Count) // 인벤토리에 아이템이 있을 때만 버튼을 활성화
                slots[i].GetComponent<Button>().interactable = true;
            else
                slots[i].GetComponent<Button>().interactable = false;
        }
    }

    public void AddSlot()
    {
        inventory.SlotCnt += 5;
    }
    public void RedrawSlotUI()
    {
        for(int i = 0; i< slots.Length;i++)
        {
            slots[i].RemoveSlot();
        }
        for(int i = 0; i< inventory.items.Count; i++)
        {
            slots[i].item = inventory.items[i];
            slots[i].item.itemIndex = i;
            //Debug.Log(slots[i].item.itemIndex);

            if (slots[i].name == ItemResources.instance.itemRS[i].itemName)
            {
                slots[i].item.itemCode = ItemResources.instance.itemRS[i].itemCode;
            }
            slots[i].UpdateSloutUI();
        }
    }

    private void Update()
    {
        // Minimap 
        if (Input.GetKeyDown(KeyCode.M))
        {
            ChangeRanderTextur();
            MinimapInteraction();
        }
        // Player Desc
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (DescCheck == true)
            {
                PlayerDesc.gameObject.SetActive(false);
                DescCheck = false;
            }
            else
            {
                PlayerDesc.gameObject.SetActive(true);
                DescCheck = true;
            }

        }
        
        // Sub Menu Set
        if (Input.GetButtonDown("Cancel") )
        {
            if (menuSet.activeSelf)
            {
                menuSet.SetActive(false);
            }
            else
            {
                menuSet.SetActive(true);
            }
        }

        //Inventory
        if (Input.GetKeyDown(KeyCode.I) && questMgr.questId >= 20)
        {
            ActiveInventory();
        }
        //Tooltip
        MoveTooltip();

        /*Debug.Log("x:" + player.transform.position.x);시발시발시발
        Debug.Log("y:" + player.transform.position.y);*/

        //PartyPanel
        if (Input.GetKeyDown(KeyCode.P) && questMgr.questId >= 30)
        {
            ActiveParty();
        }
    }
    #region MinimapMethod
    private void ChangeRanderTextur()
    {
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        if (bigMinimapChek)
        {
            minimapRanderTexture.Release(); // 기존 텍스처 해제
            minimapRanderTexture.width = screenWidth; // 너비 변경
            minimapRanderTexture.height = screenHeight; // 높이 변경
            minimapRanderTexture.Create(); // 변경된 크기로 새로 생성
        }
        else
        {
            minimapRanderTexture.Release(); // 기존 텍스처 해제
            minimapRanderTexture.width = 1024; // 너비 변경
            minimapRanderTexture.height = 1024; // 높이 변경
            minimapRanderTexture.Create(); // 변경된 크기로 새로 생성
        }

    }
    private void MinimapInteraction()
    {
        if (bigMinimapChek)
        {
            smallMap.gameObject.SetActive(false);
            bigMap.gameObject.SetActive(bigMinimapChek);
            RawAlphaChange(rimgSmall, 0f);
            RawAlphaChange(rimgBig, 1f);


            Rect newRect = new Rect(0f, 0f, 1.2f, 1.2f);
            miniCamera.rect = newRect;

            bigMinimapChek = false;
        }
        else
        {
            smallMap.gameObject.SetActive(true);
            bigMap.gameObject.SetActive(bigMinimapChek);
            RawAlphaChange(rimgSmall, 1f);
            RawAlphaChange(rimgBig, 0f);

            Rect newRect = new Rect(0f, 0f, 2f, 2f);
            miniCamera.rect = newRect;

            bigMinimapChek = true;
        }
    }
    private void RawAlphaChange(RawImage rimg, float alpha)
    {
        Color color = rimg.color;
        color.a = alpha;
        rimg.color = color;
    }
    #endregion
    #region ToggleBtn
    public void ToggleSubButtons()
    {
        areSubButtonsVisible = !areSubButtonsVisible;

        if (areSubButtonsVisible)
        {
            ShowSubButtons();
        }
        else
        {
            HideSubButtons();
        }
    }
    private void ShowSubButtons()
    {
        foreach (GameObject subButton in subButtons)
        {
            subButton.gameObject.SetActive(true);
        }
    }
    private void HideSubButtons()
    {
        foreach (GameObject subButton in subButtons)
        {
            subButton.gameObject.SetActive(false);
        }
    }
    #endregion
    private void SetPlayerDatas()
    {
        tmp_PlayerName.text = GameMgr.playerData[0].NAME;
        playerGold = GameMgr.playerData[0].player_Gold;
        playerLevel = GameMgr.playerData[0].player_level;

        tmp_PlayerLevle.text = "Lv." + GameMgr.playerData[0].player_level .ToString();
        tmp_PlayerGold.text = GameMgr.playerData[0].player_Gold .ToString();

        this.player_Max_HP = GameMgr.playerData[0].max_Player_Hp;
        this.player_Cur_HP = GameMgr.playerData[0].cur_Player_Hp;

        this.player_Max_SN = GameMgr.playerData[0].max_Player_Sn;
        this.player_Cur_SN = GameMgr.playerData[0].cur_Player_Sn;

        this.player_Max_MP = GameMgr.playerData[0].max_Player_Mp;
        this.player_Cur_MP = GameMgr.playerData[0].cur_Player_Mp;

        this.player_Atk_Speed = GameMgr.playerData[0].atk_Speed;
        this.player_Atk_Range = GameMgr.playerData[0].atk_Range;
        this.player_Base_Atk_Dmg = GameMgr.playerData[0].base_atk_Dmg;
        this.player_Max_EXP = GameMgr.playerData[0].player_max_Exp;
        this.player_Cur_EXP = GameMgr.playerData[0].player_cur_Exp;
        

        s_HP.value = this.player_Cur_HP / this.player_Max_HP;
        s_SN.value = this.player_Cur_SN / this.player_Max_SN;
        s_EXP.value = this.player_Cur_EXP / this.player_Max_EXP;

    }
    public void SliderChange()
    {
        s_HP.value = this.player_Cur_HP / this.player_Max_HP;
        s_SN.value = this.player_Cur_SN / this.player_Max_SN;
        s_EXP.value = this.player_Cur_EXP / this.player_Max_EXP;
        tmp_PlayerGold.text = this.playerGold.ToString();

        if (playerGold <= 0)
        {
            tmp_PlayerGold.text = "0";
        }
    }

    public void TalkAction(GameObject scanObj)
    {
        
        scanObject = scanObj;
        ObjectData objectData = scanObject.GetComponent<ObjectData>();// Ray가 스캔했을때  LayerMask가 Obejct인 오브젝트가 부착중인 ObecjtData를  Ray가 오브젝트를 스캔 했을 때만 추출해서 TossTalkData메서드의 매개변수로 사용함.
        TossTalkData(objectData.id, objectData.isNpc);

        //Debug.Log(objectData.id.ToString());// 04-23 Debug

        imgTalkPnel.gameObject.SetActive(isActionTalk);// isActionTalk의 true/false 상태를 따라가기때문에 이렇게 작성해주면 코드 깔끔해짐 
    }

    private void TossTalkData(int scanObj_ID, bool scanObj_isNpc)
    {
        //Set Talk Data
        int questTalkIndex = 0;
        string talkData = "";

        // isAnim
        if (typeTextEffect.isAnim)
        {
            typeTextEffect.SetMsg("");
            return;
        }
        else
        {
            questTalkIndex = questMgr.GetQuestTalkIndex(scanObj_ID);
            talkData = talkMgr.GetTalk(scanObj_ID + questTalkIndex, talkIndex);
        }

        //End Talk
        if (talkData == null)
        {
            //Debug.Log("NulltalkData // ToosTalkData: " + scanObj_ID); // 04 -23 Debug
            /*if (AllEquipChek())
            {
                questMgr.CheckQuest(scanObj_ID);
                return;
            }*/

            isActionTalk = false;
            talkIndex = 0;
            questDesc.text = questMgr.CheckQuest(scanObj_ID);

            if (scanObj_ID == 8000)
            {
                Debug.Log("8000 실행");
                if (questMgr.questId >= 30)
                {
                    Debug.Log("던전 입장");
                    SceneManager.LoadScene("Battle");//아니면여기에 던전에입장하시겠습니까? 예, 아니오, Wall, 값을 넣고 던져서 예누르면 wall로 텔포,아니오누르면 그냥 retrun하게하는식으로하면~ 야매 맵이동구현 뚝딲
                    GameSave();
                }
                else
                {
                    // ToDo: CallBack Img.SetActive(true);
                    Debug.Log("튜토리얼 던전에 진입 할 수 없습니다.");
                    return;
                }
            }
                /*if (questMgr.questId ==10 && questMgr.questActionIndex == 0)
                {
                    questMgr.receptionist[0].SetActive(false);
                    questMgr.receptionist[1].SetActive(true);
                }*/

                return;
        }

        //Continue Talk
        if (scanObj_isNpc)
        {
            //Debug.Log("ContinueTalk // ToosTalkData: " + scanObj_ID);// 04-23 Debug
            typeTextEffect.SetMsg(talkData.Split(':')[0]);// .Split()  ':' 구분자 : 를 통하여 문자열을 배열로 나눠주는 함수

            //show Portrait
            imgPortrait.sprite = talkMgr.GetPortrait(scanObj_ID, int.Parse(talkData.Split(':')[1]));
            imgPortrait.color = new Color(1, 1, 1, 1);// npc가 맞으면 초상화이미지 활성화
            SetTalkName(imgPortrait.sprite);
        }
        else
        {
            //Debug.Log("else ContinueTalk // ToosTalkData: " + scanObj_ID); // 04 -23 Debug
            typeTextEffect.SetMsg(talkData);
            imgPortrait.color = new Color(1, 1, 1, 0);// npc가 아니면 초상화이미지 비활성화
            talkName.gameObject.SetActive(false);
        }

        isActionTalk = true;
        talkIndex++;
    }
    //06- 11 Add
    private void SetTalkName(Sprite _sp)
    {
        talkName.gameObject.SetActive(true);
        talkName.text = talkMgr.dictTalkName[_sp];
    }

    public void GameSave()
    {
        if (menuSet.activeSelf)
        {
            menuSet.SetActive(false);
        }

        List<Item> saveInventoryItem = new();
        List<Item> saveWearItem = new();

        foreach (Item item in inventory.items)
        {
            saveInventoryItem.Add(item);
        }

        for (int i = 0; i < targetSlots.Length; i++)
        {
            if (targetSlots[i].wearChek == true)
            {
                saveWearItem.Add(targetSlots[i].item);
            }
        }

        SaveData gameSaveData = new SaveData(GameMgr.playerData[0].GetPlayerName(), playerLevel, playerGold, questMgr.questId, questMgr.questActionIndex, player_Max_HP, player_Cur_HP, player_Max_SN, player_Cur_SN, player_Max_MP, player_Cur_MP, player_Atk_Speed, player_Atk_Range, player_Base_Atk_Dmg, player_Max_EXP, player_Cur_EXP, saveInventoryItem, saveWearItem);
        SaveSystem.Save(gameSaveData, "save");

        //  Player DayCount, Player Inventory, Player Desc (Stat, Name, Job, Gold ... ect)
    }
    public void GameLoad()
    {
        SaveData loadData = SaveSystem.Load("save");

        //Load Player Data => save_001.x, save_001.y, save_001.questId, save_001.QuestActionIndex 

        GameMgr.single.OnSelectPlayer(loadData.playerName);

        /*Vector3 lodingPosition = new Vector3(loadData.playerX, loadData.playerY);
        player.transform.position = lodingPosition;*/
        //Debug.Log("load x, y: "+loadData.playerX +", "+ loadData.playerY);
        //SetNowPosition(loadData.playerX, loadData.playerY);

        GameMgr.playerData[0].max_Player_Hp = loadData.p_max_hp;
        GameMgr.playerData[0].cur_Player_Hp = loadData.p_cur_hp;

        GameMgr.playerData[0].max_Player_Sn = loadData.p_max_sn;
        GameMgr.playerData[0].cur_Player_Sn = loadData.p_cur_sn;

        GameMgr.playerData[0].max_Player_Mp = loadData.p_max_mp;
        GameMgr.playerData[0].cur_Player_Mp = loadData.p_cur_mp;

        GameMgr.playerData[0].player_max_Exp = loadData.p_max_Exp;
        GameMgr.playerData[0].player_cur_Exp = loadData.p_cur_Exp;


        GameMgr.playerData[0].player_Gold = loadData.p_gold;
        GameMgr.playerData[0].player_level = loadData.p_level;


        GameMgr.playerData[0].atk_Speed = loadData.p_atk_speed;
        GameMgr.playerData[0].atk_Range = loadData.p_atk_range;
        GameMgr.playerData[0].base_atk_Dmg = loadData.p_base_atk_Dmg;

        GameMgr.playerData[0].listInventory = loadData.listInven;
        GameMgr.playerData[0].listEquipment = loadData.listEquip;

        LoadInventory(loadData.listInven);
        LoadEquipment(loadData.listEquip);

        questMgr.questId = loadData.questId;
        questMgr.questActionIndex = loadData.questActionIndex;
        questMgr.ControlQuestObejct();
        //GetNowPositon();

    }
    #region PlayerPosition
    public void SetNowPosition(float x, float y)
    {
        //Debug.Log("set x, y: " + x + ", " + y);
        //lodingPosition.x = x;
        //lodingPosition.y = y;
    }
    public void GetNowPositon()
    {
        //Debug.Log("get x, y: " + lodingPosition.x + ", " + lodingPosition.y);
        //player.transform.position = lodingPosition;
        //Debug.Log("P x, y: " + player.transform.position.x + ", " + player.transform.position.y);
    }
    #endregion
    public void OnVideoOption_S1()
    {
        if (videoOption_S1.gameObject.activeSelf)
        {
            videoOption_S1.gameObject.SetActive(false);
        }
        else
        {
            videoOption_S1.gameObject.SetActive(true);
        }
    }
    public void OffVideoOption_S1()
    {
        videoOption_S1.gameObject.SetActive(false);
    }
    public void ActiveInventory()
    {
        activeInventory = !activeInventory;
        inventory_panel.SetActive(activeInventory);
        if (activeInventory == false)
        {
            for (int i = 0; i < inventory.items.Count; i++)
            {
                tooltip.SetActive(false);
            }
        }
    }
    public void SetupTooltip(string _name, string _title, string _desc, Sprite _img)
    {
        textName.text = _name;

        textTitle.text = _title;

        textDesc.text = _desc;

        imgIcon.sprite = _img;
    }
    private void MoveTooltip()
    {
        tooltip.transform.position = Input.mousePosition;
        // 04-15 ToolTip
        tooltipRect = tooltip.GetComponent<RectTransform>();

        if (tooltipRect.anchoredPosition.x + tooltipRect.sizeDelta.x > canvaseWidth)
            tooltipRect.pivot = new Vector2(1, 0);
        else
            tooltipRect.pivot = new Vector2(0, 0);
    }

    public void OnClickedQuite()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }


    /*public void GetMoseItem(Slot _slot)
    {
        this.nowSlot = _slot;
        this.dragIcon = _slot.itemIcon;
    }*/
    /*public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Start");
        nowSlot = slots[dragIndex];// 슬롯 등록

        dragIcon.transform.position = eventData.position;
        dragIcon = nowSlot.itemIcon;

        dragIcon.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Now ing...");
        dragIcon.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Drag End");
        dragIcon.gameObject.SetActive(false);
    }*/

    public void OnYesButtonClick()
    {
        if (nowSlot.wearChek)
        {
            
            TakeOffItem(nowSlot);
            addEquipPanel.gameObject.SetActive(false);
            return;
        }

        equipmnet = true;
        Debug.Log("AddEquip Name: " + nowSlot.item.itemName);
        Debug.Log("AddEquip Type: " + nowSlot.item.itemType);

        WearEquipment();
        if (AllEquipChek())
        {
            questMgr.questActionIndex = 1;
        }
    }
    public void OnNoButtonClick()
    {
        equipmnet = false;
        addEquipPanel.gameObject.SetActive(false);
    }

    /*public void WearEquipMent()
    {
        for (int i = 0; i < targetSlots.Length; i++)
        {
            if (targetSlots[i].item.itemType == nowSlot.item.itemType)
            {
                Debug.Log("Sucess Equip Add: "+ nowSlot.item.itemName);
                
                targetSlots[i].slotnum = nowSlot.slotnum;
                targetSlots[i].item = nowSlot.item;
                targetSlots[i].itemIcon = nowSlot.itemIcon;
            }
            else
            {
                continue;
            }
        }
    }*/

    public void EquipSlotSetting()
    {
        for (int i = 0; i < targetSlots.Length; i++)
        {
            switch (i)
            {
                case 0:
                    targetSlots[i].item.itemType = Item.ItemType.Equipment_Helmet;
                    break;
                case 1:
                    targetSlots[i].item.itemType = Item.ItemType.Equipment_Arrmor;
                    break;
                case 2:
                    targetSlots[i].item.itemType = Item.ItemType.Equipment_Weapon;
                    break;
                case 3:
                    targetSlots[i].item.itemType = Item.ItemType.Equipment_Boots;
                    break;
                default:
                    break;
            }
        }
    }
    public void WearEquipment()
    {
        int index = 0;
        // 현재 선택된 슬롯의 아이템을 복제하여 대상 슬롯에 추가
        for (int i = 0; i < targetSlots.Length; i++)
        {
            if (targetSlots[i].item.itemType == nowSlot.item.itemType)
            {
                Debug.Log("Success Equip Add: " + nowSlot.item.itemName);
                index = nowSlot.slotnum;
                // 아이템 복제
                Item clonedItem = nowSlot.item;

                // 아이템 인덱스 설정
                clonedItem.itemIndex = nowSlot.slotnum;

                // 아이콘 설정
                targetSlots[i].itemIcon.sprite = nowSlot.itemIcon.sprite;
                targetSlots[i].itemIcon.gameObject.SetActive(true);
                targetSlots[i].wearChek = true;
                // 아이템 설정
                targetSlots[i].item = clonedItem;

                ApplyEquipPower(targetSlots[i].wearChek, nowSlot.item);// == ApplyEquipPower(targetSlots[i], clonedItem);
            }
        }
        // 사용한 아이템 제거 
        inventory.RemoveItem(slots[index].slotnum);
        RedrawSlotUI();

        nowSlot = null;

        addEquipPanel.gameObject.SetActive(false);
    }
    //06- 12
    public void ApplyEquipPower(bool _onoff, Item _equip)
    {
        float equipPower;

        if (_onoff == true)
        {
            equipPower = _equip.itemPower;
            Debug.Log("장착: " + equipPower + _onoff);
        }
        else
        {
            equipPower = -1 * (_equip.itemPower);
            Debug.Log("장착해제: " + equipPower + _onoff);
        }

        Debug.Log("Now EquipItem Power: " + _equip.itemPower);
        switch (_equip.itemType)
        {
            case Item.ItemType.Equipment_Helmet:
                Debug.Log("장착전 HP: " + GameMgr.playerData[0].max_Player_Hp);
                GameMgr.playerData[0].max_Player_Hp += equipPower;

                Debug.Log("장착후 HP: "+GameMgr.playerData[0].max_Player_Hp);
                break;
            case Item.ItemType.Equipment_Arrmor:
                Debug.Log("장착전 Range: " + GameMgr.playerData[0].atk_Range);
                GameMgr.playerData[0].atk_Range += equipPower;

                Debug.Log("장착후 Range: " + GameMgr.playerData[0].atk_Range);
                break;
            case Item.ItemType.Equipment_Weapon:
                Debug.Log("장착전 Dmg: " + GameMgr.playerData[0].base_atk_Dmg);
                GameMgr.playerData[0].base_atk_Dmg += equipPower;

                Debug.Log("장착후 Dmg: " + GameMgr.playerData[0].base_atk_Dmg);
                break;
            case Item.ItemType.Equipment_Boots:
                Debug.Log("장착전 SPD: " + GameMgr.playerData[0].atk_Speed);
                GameMgr.playerData[0].atk_Speed += equipPower;

                Debug.Log("장착후 SPD: " + GameMgr.playerData[0].atk_Speed);
                break;
            /*case Item.ItemType.Consumables:
                break;
            case Item.ItemType.Ect:
                break;*/
            default:
                break;
        }
    }

    public bool AllEquipChek()
    {
        int sum = 0;
        for (int i = 0; i < targetSlots.Length; i++)
        {
            if (targetSlots[i].wearChek == true)
            {
                sum++;
            }
        }

        if (sum == (targetSlots.Length))
        {
            Debug.Log("장비 장착 완료");
            Receptionist_1();
            //tutorial Quest - wearEquipment
            if (wearEquipment != true)
            {
                wearEquipment = true;
                return true;
            }
            return false;
        }
        return false;
    }

    public void TutorialDungeonClear()
    {
        Debug.Log("튜토리얼 던전 클리어");
        Receptionist_1();
    }
    private void Receptionist_1()
    {
        questMgr.receptionist[0].SetActive(false);
        questMgr.receptionist[1].SetActive(true);
    }
    public void LoadInventory(List<Item> _items)
    {
        if (_items == null)
            return;
        for (int i = 0; i < _items.Count; i++)
        {
            inventory.items.Add(_items[i]);
        }
        RedrawSlotUI();
    }
    public void LoadEquipment(List<Item> _items)
    {
        if (_items == null || _items.Count == 0 || _items.Count != targetSlots.Length)
        {
            // 만약 _items 리스트가 null이거나 비어있거나 targetSlots과의 길이가 일치하지 않으면 로드를 진행하지 않고 종료합니다.
            Debug.Log("exeption");
            return;
        }

        // 현재 선택된 슬롯의 아이템을 복제하여 대상 슬롯에 추가
        for (int i = 0; i < targetSlots.Length; i++)
        {
            if (targetSlots[i].item.itemType == _items[i].itemType)
            {
                Debug.Log("Success Equip Add: " + _items[i].itemName);

                // 아이콘 설정
                targetSlots[i].itemIcon.sprite = _items[i].itemImage;
                targetSlots[i].itemIcon.gameObject.SetActive(true);
                targetSlots[i].wearChek = true;
                // 아이템 설정
                targetSlots[i].item = _items[i];
            }
        }
        // 사용한 아이템 제거 
        RedrawSlotUI();
    }

    //06-09 InventoryAdd
    public void TakeOffItem(Slot _Slot)
    {
        Item livingItem = _Slot.item;
        livingItem = ItemResources.instance.itemRS[_Slot.item.itemCode];//매개변수로 넘겨받은 슬롯의 아이템 코드 값으로 새 아이템을 생성하여.

        //일단 장착해제
        _Slot.wearChek = false;//슬롯의 장비가 빠졌으니 fasle로 바꿔줌
        ApplyEquipPower(_Slot.wearChek, nowSlot.item);

        //현재 슬롯의 아이템 지우기
        _Slot.item = new();
        _Slot.itemIcon.sprite = ItemResources.instance.iconRS[0];
        //_Slot.usability = true;//까먹을까봐 넣어둠 내가 클릭한 슬롯의 주소값을 참조하고있을(확실하진 않은데 그간 경험상 맞을거임) _Slot의 item들과 wearChek만 수정해주면되서 얘는 시작할때 건드려둔거 안 건드려도됨.

        //다시 장착할때 필요한 기본설정 초기화
        _Slot.item.itemType = livingItem.itemType;
        _Slot.GetComponent<Button>().interactable = false;

        //인벤토리에 장착 해제한 아이템 추가 후 인벤토리 새로그리기
        inventory.AddItem(livingItem);
        RedrawSlotUI();

        nowSlot = null;
    }

    //05-12 PartyPanel
    public void ActiveParty()
    {
        if (panelPartyBoard.activeSelf == false)
        {
            panelPartyBoard.SetActive(true);
        }
        else
        {
            panelPartyBoard.SetActive(false);
        }
    }

    public void RefreshiPartyBord()
    {
        //활성화된 슬롯 비 활성화
        foreach (var _slot in poolPartySlot)
        {
            _slot.gameObject.SetActive(false);
            _slot.text_Cost.gameObject.SetActive(true);
            _slot.text_Name.gameObject.SetActive(false);
            //Debug.Log("PartyBordSlots Active: False");
        }
        foreach (var _slot in poolMoveInSlot)
        {
            _slot.gameObject.SetActive(false);
            _slot.text_Cost.gameObject.SetActive(false);
            _slot.text_Name.gameObject.SetActive(true);
            //Debug.Log("MoveInSlots Active: False");
        }
        //MoveInSlot 초기화
        PartyListInPlayer();

        //05-23 고용리스트 텍스트 관리
        RefreshiEmploy();

        //06-13 PartyAdd
        OnClickCreateParty();//파티보드 초기화 될 때마다 목록 생성

        // 세이브된 기존의 파티 보드의 데이터가 존재한다면 해당 데이터를 슬롯에 추가해서 활성화, 이거는 다른곳으로 옮기는게 나을듯?
        /*foreach (var nowPartyBord in listPartyData)
        {
            CreatePartySlot(nowPartyBord);
        }*/

    }
    public void CreatePartySlot(PartyData _partyData)
    {
        int activeCount = poolPartySlot.FindAll(s => s.gameObject.activeSelf).Count;
        if (activeCount >= 16)
        {
            return;
        }

        PartySlot partySlot = poolPartySlot.Find(s => !s.gameObject.activeSelf); // 비활성화된 오브젝트 있으면 반환하는 코드
        if (partySlot == null)
        {
            GameObject go = Instantiate(partyPrefab, poolPartySlot[0].transform.parent);
            partySlot = go.GetComponent<PartySlot>();
            poolPartySlot.Add(partySlot);
        }

        partySlot.Init(_partyData);

        partySlot.partySlotIndex = (activeCount +1);
        partySlot.partyData.index = partySlot.partySlotIndex;

        Debug.Log("생성 번호: "+activeCount);
        partySlot.gameObject.SetActive(true);//활성화
    }

    public void OnClickCreateParty()// 모집가능파티원리스트 생성 메서드
    {
        int random = Random.Range(4, 17);

        for (int i = 0; i < random; i++)
        {
            // 0부터 10 사이의 정수 난수 생성 (10은 포함되지 않음)
            int ran = Random.Range(1, 10);
            PartyData newParty = new(objListPlayable[Random.Range(0, objListPlayable.Count)], ran);

            Debug.Log("Btn 파티 영입가능인원 생성 ");

            CreatePartySlot(newParty);
            listPartyData.Add(newParty);// 고용가능 파티원목록리스트를 저장, 여기에서 저장했으니까 씬 넘어갈때 Clier해서 비워줘야겠지??
        }
    }

    /*public bool ClickedPartySlot(PartyData _partyData)
    {
        int activeCount = poolMoveInSlot.FindAll(s => s.gameObject.activeSelf).Count;
        if (activeCount >= 4)
        {
            return false;
        }

        // 비활성화된 오브젝트 있으면 반환하는 코드
        PartySlot partySlot = poolMoveInSlot.Find(s => !s.gameObject.activeSelf);
        if (partySlot == null)
        {
            GameObject go = Instantiate(partyPrefab, poolMoveInSlot[0].transform.parent);
            partySlot = go.GetComponent<PartySlot>();
            poolMoveInSlot.Add(partySlot);
        }

        partySlot.Init(_partyData);
        
        partySlot.partySlotIndex = _partyData.index;// 06-04 여기수정
        partySlot.moveInChek = true;
        partySlot.btnMy.interactable = true;

        partySlot.gameObject.SetActive(true);//활성화
        partySlot.text_Cost.gameObject.SetActive(false);
        partySlot.text_Name.gameObject.SetActive(true);

        RefreshiEmploy();
        //listPartyData.Add(_partyData); 시발 이거 왜 안 보임?

        return true;
    }


    public void RestorePartySlot(int _index)
    {
        Debug.Log("Rrestore Index: " + _index);
        foreach (var _slot in poolMoveInSlot)
        {
            if (_slot.partySlotIndex == _index)
            {
                Debug.Log("equal Index: " + _slot.partySlotIndex);
                _slot.gameObject.SetActive(false);

                poolPartySlot[_index].block.SetActive(false);
                poolPartySlot[_index].moveInChek = false;
                poolPartySlot[_index].btnMy.interactable = true;
                
                _slot.ReSetPartySlot();
                //listPartyData.Remove(poolPartySlot[_index].partyData); 시발
                RefreshiEmploy();
                *//*poolMoveInSlot[_index].gameObject.SetActive(false);//비활성화 
                poolPartySlot[_index].block.SetActive(false);
                poolPartySlot[_index].moveInChek = false;*//*
            }
        }
    }*/
    public bool ClickedPartySlot(PartyData _partyData)
    {
        int activeCount = poolMoveInSlot.FindAll(s => s.gameObject.activeSelf).Count;
        if (activeCount >= 4)
        {
            return false;
        }

        // 비활성화된 오브젝트 있으면 반환하는 코드
        PartySlot partySlot = poolMoveInSlot.Find(s => !s.gameObject.activeSelf);
        if (partySlot == null)
        {
            GameObject go = Instantiate(partyPrefab, poolMoveInSlot[0].transform.parent);
            partySlot = go.GetComponent<PartySlot>();
            poolMoveInSlot.Add(partySlot);
        }

        partySlot.Init(_partyData);

        partySlot.partySlotIndex = _partyData.index; // 여기 수정
        partySlot.moveInChek = true;
        partySlot.btnMy.interactable = true;

        partySlot.gameObject.SetActive(true); // 활성화
        partySlot.text_Cost.gameObject.SetActive(false);
        partySlot.text_Name.gameObject.SetActive(true);

        RefreshiEmploy();

        return true;
    }

    public void RestorePartySlot(int _index)
    {
        Debug.Log("Restore Index: " + _index);
        foreach (var _slot in poolMoveInSlot)
        {
            if (_slot.partySlotIndex == _index)
            {
                Debug.Log("Equal Index: " + _slot.partySlotIndex);
                _slot.gameObject.SetActive(false);

                var correspondingSlot = poolPartySlot.Find(s => s.partySlotIndex == _index);
                if (correspondingSlot != null)
                {
                    correspondingSlot.block.SetActive(false);
                    correspondingSlot.moveInChek = false;
                    correspondingSlot.btnMy.interactable = true;
                }

                _slot.ReSetPartySlot();
                RefreshiEmploy();
            }
        }
    }
    public void RefreshiEmploy()
    {
        int countEmploy = 0;
        int sum = 0;
        foreach (PartySlot _slot in poolMoveInSlot)
        {
            if (_slot.partyData != null)
            {
                countEmploy++;
                sum += _slot.intPartyCost;
            }
        }
        textPartyCount.text = "파티원\n " + countEmploy + " / 4"; // countEmploy +" / 4";
        textPartyPrice.text = "금액\n "+ sum + "\n골드";//금액\n 121\n골드
        partyPrice = sum;
    }

    public void EmploymentCompleted()
    {
        PartyListInPlayer();

        int battleIndex = 1;
        playerGold = 999;
        if ((playerGold - partyPrice) < 0 )
        {
            Debug.Log("골드 부족");
            return; //버튼눌렀는데 골드가 부족하면 실행안됨
        }

        foreach (PartySlot _slot in poolMoveInSlot)
        {
            if (_slot.partySlotIndex != 0 &&_slot.partyData != null)
            {
                _slot.partyData.partyJobIndex = battleIndex++;
                lastDeparture.Add(_slot);

                PlayerData _pd = new(
                    _slot.partyData.partyJobIndex,

                    _slot.partyData.partyHp,
                    _slot.partyData.partyMp,

                    _slot.partyData.partyAtkSpd,
                    _slot.partyData.partyAtkRange,
                    _slot.partyData.partyAtk,

                    _slot.partyData.level,
                    _slot.partyData.strPartyName,

                    _slot.partyData.able_Skill,
                    _slot.partyData.isMelee,

                    _slot.partyData.jobType
                    );
                //_pd.partySlotData = _slot.partyData;// 06-05 수정

                GameMgr.playerData.Add(_pd);

                //_slot.partyData.obj_Data.GetComponent<Ally>().Init(_pd.playerIndex, _pd);
                Debug.Log("최종파티원LV: " + _slot.partyData.level + ", 파티인덱스 :" + _slot.partyData.partyJobIndex);
            }
            
        }
    }

    public void PartyListInPlayer()
    {
        lastDeparture.Clear();

        //게임이 최초로 시작될때, lastDepatuar[0]에 PlayerPartyData를 가진 MoveInSlot[0]에 || PartyData를 Player[0]의 Data로 채워서 Slot을만들어 MoveInSlot에 Add하고 
        PartyData pd = new(playerPrefab, GameMgr.playerData[0].player_level)//개체 초기화 단순화 하는 코드 
        {
            partyHp = GameMgr.playerData[0].max_Player_Hp,
            partyMp = GameMgr.playerData[0].max_Player_Mp,
            partyAtk = GameMgr.playerData[0].base_atk_Dmg,
            partyAtkSpd = GameMgr.playerData[0].atk_Speed,
            partyAtkRange = GameMgr.playerData[0].atk_Range
        };

        poolMoveInSlot[0].partyData = pd;
        poolMoveInSlot[0].gameObject.SetActive(true);
        poolMoveInSlot[0].partyIcon.sprite = playerPrefab.GetComponent<SpriteRenderer>().sprite;
        poolMoveInSlot[0].text_Name.text = "Player";

        poolMoveInSlot[0].text_Lv.text = GameMgr.playerData[0].player_level.ToString();
        poolMoveInSlot[0].classIcon.sprite = playerPrefab.GetComponent<Ally>().class_Icon;
        //poolMoveInSlot[0].partyData.obj_Data.GetComponent<Ally>().Init(GameMgr.playerData[0].playerIndex, GameMgr.playerData[0]);
        listPartyData.Add(poolMoveInSlot[0].partyData);
        lastDeparture.Add(poolMoveInSlot[0]);

        poolMoveInSlot[0].btnMy.interactable = false;
        /*        PartySlot nSlot = new();
                nSlot.Init(pd);*/

        
    }

}
