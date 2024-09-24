using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    private static BattleManager instance = null;
    [Header("Manager")]
    public ObjectManager pool;
    public MapManager room;
    public UIManager ui;
    public Dialogue dialogue;
    public TutorialManager tutorial;


    [Header("Battle")]
    public List<GameObject> party_List = new List<GameObject>();
    public List<GameObject> deploy_Player_List = new List<GameObject>();
    public List<GameObject> deploy_Enemy_List = new List<GameObject>();
    public GameObject deploy_area;
    public GameObject unit_deploy_area;
    private bool isFirstEnter;
    private bool battleEnded = false;

    [Header("Stage")]
    public float level_Scale = 1;

    [Header("Reward")]
    public float exp_Cnt;
    public int gold_Cnt;
    public List<Item> drop_Item = new List<Item>();

    [Header("Total_Reward")]
    public int total_Gold;
    public float total_Exp;
    public List<Item> total_Drop_Item = new List<Item>();

    public static BattleManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }

    }


    public enum BattlePhase
    {
        Start, // 방 진입 상태 (방의 종류 체크)
        Deploy, // 전투방 입장 후 배치 단계 상태
        Rest, // 전투방이 아닐 때 
        Battle, // 배치 단계에서 배틀 시작 버튼을 눌러 배틀이 시작된 상태
        End // 적이 다 죽었거나, 아군이 다 사망했을 경우 <- 현재는 배치한 파티원이 다 죽으면 끝나는 방식이라 만약 배치를 하지 않은 파티원이 있으면 재시작? 혹은 아예 다 배치되도록 해야할 듯
    }


    public BattlePhase _curphase;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }

        room = FindObjectOfType<MapManager>();
        isFirstEnter = true;

        for (int i = 0; i < GameUiMgr.single.lastDeparture.Count; i++)
        {
            party_List.Add(GameUiMgr.single.lastDeparture[i].partyData.obj_Data);
        }

        if (GameUiMgr.single.questMgr.questId == 40)
        {
            Debug.Log("NOW QUESTID 40 GOLD: " + GameMgr.playerData[0].player_Gold);
            GameMgr.playerData[0].player_Gold = 1500;
        }

        // 소모품 아이템 체크 후 아이템 바에 생성
        SetItem();
    }

    private void Start()
    {
        ChangePhase(BattlePhase.Start); // 방 체크
        AudioManager.single.PlayBgmClipChange(2);
    }

    public IEnumerator BattleReady() // 전투 방일 시 실행되는 메서드
    {
        deploy_area = GameObject.FindGameObjectWithTag("Deploy");
        unit_deploy_area = GameObject.FindGameObjectWithTag("Wait");
        ui.OpenPopup(ui.battle_Ready_Banner);
        yield return StartCoroutine(ui.StartBanner(ui.battle_Ready_Banner));
        yield return new WaitForSeconds(0.15f);

        // PlacementUnit(); // 파티 리스트에 있는 유닛 생성

        Enemy[] entity = FindObjectsOfType<Enemy>(); // 몬스터를 찾음
        battleEnded = false;

        //ui.party_List.SetActive(true);
        deploy_area.SetActive(true);
        unit_deploy_area.SetActive(true);

        foreach (GameObject ally in deploy_Player_List)
        {
            EntityDrag drag = ally.GetComponent<EntityDrag>();

            drag.enabled = true;
        }


        foreach (Enemy obj in entity)
        {
            NavMeshAgent nav = obj.GetComponent<NavMeshAgent>();

            if (obj.gameObject != null)
            {
                deploy_Enemy_List.Add(obj.gameObject);
            }

            if (nav != null)
            {
                nav.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            }
        }
    }

    private void Update()
    {
        if (_curphase == BattlePhase.Battle && (deploy_Player_List.Count == 0 || deploy_Enemy_List.Count == 0))
        {
            Debug.Log("다 죽음");
            ChangePhase(BattlePhase.End);
        }
    }


    public void ChangePhase(BattlePhase phase)
    {
        _curphase = phase;

        switch (phase) // 각 상태에 진입 했을 때 실행되는 switch문
        {
            case BattlePhase.Start:
                if (room.isMoveDone || isFirstEnter)
                {
                    StartCoroutine(CheckRoom());
                }
                break;
            case BattlePhase.Rest:
                if (!ui.out_Portal.activeSelf)
                {
                    ui.out_Portal.SetActive(true);
                    ui.out_Portal.GetComponent<FadeEffect>().fadeout = true;
                }

                foreach (GameObject ally in deploy_Player_List)
                {
                    EntityDrag drag = ally.GetComponent<EntityDrag>();

                    drag.enabled = false;
                }

                break;
            case BattlePhase.Deploy:
                if (ui.out_Portal.activeSelf)
                    ui.out_Portal.GetComponent<FadeEffect>().fadein = true;
                StartCoroutine(BattleReady());
                break;
            case BattlePhase.Battle:
                break;
            case BattlePhase.End:
                StartCoroutine(EndBattle());
                break;
        }
    }

    public void BattleStartButton()
    {
        StartCoroutine(BattleStart());
    }


    public IEnumerator BattleStart()
    {
        if (ui.party_List.activeSelf)
            ui.party_List.SetActive(false);

        if (deploy_Player_List.Count == 0)
        {
            ui.OpenPopup(ui.alert_Popup);
            ui.alert_Popup.GetComponent<TitleInit>().Init("최소 한명의 파티원을 배치를 해야 합니다.");
            yield break;
        }
        else
        {
            AudioManager.single.PlaySfxClipChange(7);
            ui.OpenPopup(ui.battle_Start_Banner);
            yield return StartCoroutine(ui.StartBanner(ui.battle_Start_Banner));
            yield return new WaitForSeconds(0.15f);

            Debug.Log("배틀 시작");
            ChangePhase(BattlePhase.Battle);
            deploy_area = GameObject.FindGameObjectWithTag("Deploy");
            unit_deploy_area = GameObject.FindGameObjectWithTag("Wait");
            deploy_area.SetActive(false);
            unit_deploy_area.SetActive(false);

            if (_curphase == BattlePhase.Battle)
            {
                BaseEntity[] entity = FindObjectsOfType<BaseEntity>(); // 활성화 된 플레이어나 몬스터를 찾아서 리스트에 넣음

                foreach (BaseEntity obj in entity)
                {
                    NavMeshAgent nav = obj.GetComponent<NavMeshAgent>();
                    EntityDrag drag = obj.GetComponent<EntityDrag>();

                    if (nav != null)
                    {
                        nav.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
                    }
                    if (drag != null)
                    {
                        drag.enabled = false; // 배틀 시작 시에는 플레이어들이 드래그가 안되도록 방지.
                    }
                }

                // 배틀 시작 시 방 안에 있는 적들의 정보의 경험치량, 골드, 드랍 유무를 계산해서 임시변수에 넣음;
                foreach (GameObject enemy in deploy_Enemy_List)
                {
                    float enemy_Exp = enemy.GetComponent<Enemy>().exp_Cnt;
                    int enemy_gold = enemy.GetComponent<Enemy>().gold_Cnt;
                    bool enemy_Item_Drop = enemy.GetComponent<Enemy>().item_Drop_Check;

                    exp_Cnt += enemy_Exp;
                    gold_Cnt += enemy_gold;

                    if (enemy_Item_Drop)
                    {
                        drop_Item.Add(enemy.GetComponent<Enemy>().GetItemDropTable());
                    }

                    Debug.Log("얻을 수 있는 경험치 량 : " + exp_Cnt);
                    Debug.Log("얻을 수 있는 골드 : " + gold_Cnt);
                }
            }
        }
    }

    private IEnumerator EndBattle()
    {
        if (_curphase == BattlePhase.End && !battleEnded)
        {
            if (deploy_Player_List.Count == 0)
            {
                AudioManager.single.PlaySfxClipChange(10);
                ui.OpenPopup(ui.def_Banner);
                yield return StartCoroutine(ui.Def_Banner());
                yield return new WaitForSeconds(0.15f);
                ui.OpenPopup(ui.def_Popup);
            }
            else if (deploy_Enemy_List.Count == 0)
            {
                AudioManager.single.PlaySfxClipChange(9);
                ui.OpenPopup(ui.vic_Banner);
                yield return StartCoroutine(ui.StartBanner(ui.vic_Banner));
                yield return new WaitForSeconds(0.15f);

                if (!ui.out_Portal.activeSelf)
                {
                    ui.out_Portal.SetActive(true);
                    ui.out_Portal.GetComponent<FadeEffect>().fadeout = true;
                }

                ui.OpenPopup(ui.reward_Popup);

                Debug.Log("얻은 경험치 : " + exp_Cnt);
                RewardPopupInit popup = ui.reward_Popup.GetComponent<RewardPopupInit>();
                popup.Init("전투 승리", false);


                GameObject gold_Obj = Instantiate(ui.reward_Prefab, popup.inner_Gold_Exp);
                gold_Obj.GetComponent<RewardInit>().Init(ui.reward_Icons[0], gold_Cnt + " Gold");
                total_Gold += gold_Cnt;

                GameObject exp_Obj = Instantiate(ui.reward_Prefab, popup.inner_Gold_Exp);
                exp_Obj.GetComponent<RewardInit>().Init(ui.reward_Icons[1], exp_Cnt + " Exp");
                total_Exp += exp_Cnt;


                // 아이템 코드로 중복 드랍된 아이템 찾고 그룹화
                SetIconDropItem(popup, drop_Item);

                foreach (Item item in drop_Item)
                {
                    // 드랍된 아이템들을 총 드랍 아이템 리스트에 넣기
                    total_Drop_Item.Add(item);
                }

                if (!room.FindRoom(room.cur_Room.gameObject).isBoss)
                {
                    ChangePhase(BattlePhase.Rest);
                }
            }


            if (deploy_Enemy_List.Count == 0 && room.FindRoom(room.cur_Room.gameObject).isBoss)
            {
                if (!ui.out_Portal.activeSelf)
                {
                    ui.out_Portal.SetActive(true);
                    ui.out_Portal.GetComponent<FadeEffect>().fadeout = true;
                }

                // 총 골드, 경험치를 얻음
                GameMgr.playerData[0].player_Gold += total_Gold;
                GameMgr.playerData[0].GetPlayerExp(total_Exp);

                // 얻은 아이템들을 인벤토리에 추가
                foreach (Item item in total_Drop_Item)
                {
                    Inventory.Single.AddItem(item);
                }


                Debug.Log("버튼 생성");
                DestroyImmediate(ui.out_Portal.GetComponent<Button>());
                ui.out_Portal.AddComponent<Button>().onClick.AddListener(() => TotalReward());
            }

            BaseEntity[] unit = FindObjectsOfType<BaseEntity>();

            foreach (BaseEntity obj in unit)
            {
                Ally ally = obj as Ally;
                if (ally != null)
                    ally.UpdateCurrentHPToSingle();
                Destroy(obj.gameObject);

                foreach (Transform arrow_Obj in pool.obj_Parent)
                {
                    Destroy(arrow_Obj.gameObject);
                }

                pool.Poolclear();
            }

            deploy_Player_List.Clear();
            deploy_Enemy_List.Clear();

        }

        exp_Cnt = 0;
        gold_Cnt = 0;
        drop_Item.Clear();
        battleEnded = true;
    }


    public void TotalReward()
    {
        if (_curphase == BattlePhase.End)
        {
            Debug.Log("실행됨");
            if (deploy_Enemy_List.Count == 0 && room.FindRoom(room.cur_Room.gameObject).isBoss)
            {
                if (!ui.out_Portal.activeSelf)
                {
                    ui.out_Portal.SetActive(true);
                    ui.out_Portal.GetComponent<FadeEffect>().fadeout = true;
                }

                ui.OpenPopup(ui.vic_Popup);
                RewardPopupInit popup = ui.vic_Popup.GetComponent<RewardPopupInit>();

                DestroyRewardPopup();

                GameObject gold_Obj = Instantiate(ui.reward_Prefab, popup.inner_Gold_Exp);
                gold_Obj.GetComponent<RewardInit>().Init(ui.reward_Icons[0], total_Gold + " Gold");

                GameObject exp_Obj = Instantiate(ui.reward_Prefab, popup.inner_Gold_Exp);
                exp_Obj.GetComponent<RewardInit>().Init(ui.reward_Icons[1], total_Exp + " Exp");

                SetIconDropItem(popup, total_Drop_Item);

            }
        }
    }

    // 방 종류 체크 메서드
    public IEnumerator CheckRoom()
    {
        yield return null;
        // 전 방에 배치된 유닛들 제거
        if (deploy_Player_List != null)
        {
            Ally[] unit = FindObjectsOfType<Ally>();

            if (unit != null)
            {
                foreach (Ally obj in unit)
                {
                    if (obj != null)
                        obj.UpdateCurrentHPToSingle();
                    Destroy(obj.gameObject);

                    foreach (Transform arrow_Obj in pool.obj_Parent)
                    {
                        if (arrow_Obj != null)
                            Destroy(arrow_Obj.gameObject);
                    }

                    pool.Poolclear();
                }
            }
        }

        deploy_Player_List.Clear();
        deploy_Enemy_List.Clear();

        if (isFirstEnter)
        {
            isFirstEnter = false;
            yield return null;
            unit_deploy_area = GameObject.FindGameObjectWithTag("Wait");
            PlacementUnit(); // 어떤 방이든 유닛을 소환 시키도록 함.
        }
        else
        {
            unit_deploy_area = GameObject.FindGameObjectWithTag("Wait");
            PlacementUnit(); // 어떤 방이든 유닛을 소환 시키도록 함.
        }

        if (room.cur_Room.tag == "Battle")
        {

            Debug.Log("전투 방입니다.");

            if (room.FindRoom(room.cur_Room.gameObject).isBoss)
            {
                AudioManager.single.PlayBgmClipChange(3);
            }

            ChangePhase(BattlePhase.Deploy);
            yield break;
        }
        else
        {
            ChangePhase(BattlePhase.Rest);
            Debug.Log("휴식");
            yield break;
        }
    }

    // 배치 단계일때 죽은 파티원을 제외한 나머지 파티원을 배치
    private void PlacementUnit()
    {
        Debug.Log("작동하나 체크");

        Tilemap deployTilemap = unit_deploy_area.GetComponent<Tilemap>();

        BoundsInt bounds = deployTilemap.cellBounds;
        int unit_Cnt = 0;

        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            if (unit_Cnt >= party_List.Count)
            {
                break;
            }

            if (deployTilemap.HasTile(position) && CanPlace(position))
            {
                if (GameMgr.playerData[unit_Cnt].cur_Player_Hp <= 0)
                {
                    unit_Cnt++;
                    continue;
                }

                // 그 외 유닛들은 생성 하도록 함.
                if (GameMgr.playerData[unit_Cnt].cur_Player_Hp > 0)
                {
                    Debug.Log("작동하나 체크 유닛 생성" + unit_Cnt);
                    Vector3 worldPos = deployTilemap.GetCellCenterWorld(position);

                    GameObject unit = Instantiate(party_List[unit_Cnt], worldPos, Quaternion.identity);
                    unit.GetComponent<Ally>().InitStat(GameMgr.playerData[unit_Cnt].playerIndex);

                    deploy_Player_List.Add(unit);

                    unit_Cnt++; // 다음 유닛 배치하기 위한 인덱스 값
                }
            }
        }
    }

    // 배치 가능한지 체크
    private bool CanPlace(Vector3Int position)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(unit_deploy_area.GetComponent<Tilemap>().GetCellCenterWorld(position));

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return false; // 이미 해당 위치에 유닛이 있으면 배치할 수 없습니다.
            }
        }

        return true;
    }


    // 던전이 끝나면 마을로 돌아가는 메서드
    public void ReturnToTown()
    {
        /*Debug.Log("마을로 이동");

    total_Gold = 0;
    total_Exp = 0;

    GameMgr.playerData[0].cur_Player_Sn -= 5;
    GameMgr.playerData[0].cur_Player_Hp = GameMgr.playerData[0].max_Player_Hp;

    GameUiMgr.single.GameSave();

    SceneManager.LoadScene("Town");*/

        Debug.Log("Now_cur - exp" + GameMgr.playerData[0].player_cur_Exp);
        Debug.Log("Now_max - exp" + GameMgr.playerData[0].player_max_Exp);

        //GameMgr.playerData[0].GetPlayerExp(40);

        Debug.Log("PlayerData - QID: " + GameUiMgr.single.questMgr.questId);
        Debug.Log("PlayerData - AID: " + GameUiMgr.single.questMgr.questActionIndex);
        Debug.Log("NowGold: " + GameMgr.playerData[0].player_Gold);
        Debug.Log("Now_SN" + GameMgr.playerData[0].cur_Player_Sn);
        Debug.Log("Now_HP" + GameMgr.playerData[0].cur_Player_Hp);

        Debug.Log("Now_Lv" + GameMgr.playerData[0].player_level);
        Debug.Log("Now_cur - exp" + GameMgr.playerData[0].player_cur_Exp);
        Debug.Log("Now_max - exp" + GameMgr.playerData[0].player_max_Exp);

        Debug.Log("Load Type: " + GameMgr.single.LoadChecker());

        GameMgr.playerData[0].cur_Player_Sn -= 15;
        //GameMgr.playerData[0].cur_Player_Hp -= 15;
        GameMgr.playerData[0].cur_Player_Hp = GameMgr.playerData[0].max_Player_Hp;

        GameUiMgr.single.GameSave();
        GameMgr.single.IsGameLoad(true);
        Debug.Log("Load Type: " + GameMgr.single.LoadChecker());

        LoadingSceneController.LoadScene("Town");
    }

    // 보상 팝업 내용물 초기화
    public void DestroyRewardPopup()
    {
        RewardPopupInit popup = ui.reward_Popup.GetComponent<RewardPopupInit>();

        foreach (Transform child in popup.inner_Gold_Exp.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform item_Child in popup.inner_Item.transform)
        {
            Destroy(item_Child.gameObject);
        }

    }


    // 인벤토리 아이템 불러오기
    private void SetItem()
    {
        int index = 0;
        for (int i = 0; i < 10; i++)
        {
            ItemUse iia = Instantiate(ui.item_Slot_Prefabs, ui.item_Bar.transform.GetChild(0));

            // 생성된 슬롯 초기화
            Item item = SetInnerItem(ref index);

            iia.Init(item);

            // 생성된 슬롯을 리스트에 추가
            //Inner.IiaList.Add(iia);
        }

    }

    Item SetInnerItem(ref int _index)
    {
        for (int i = _index; i < Inventory.Single.items.Count; i++)
        {
            if (Inventory.Single.items[i].itemType == Item.ItemType.Consumables)
            {
                _index = i + 1;
                return Inventory.Single.items[i];
            }
        }
        return null;
    }

    // 드랍 아이템들을 아이템 코드로 그룹화 하여 아이콘을 생성하는 메서드 (LinQ 사용)
    public void SetIconDropItem(RewardPopupInit popup, List<Item> drop_List)
    {
        var item_Cnt = drop_List
                    .GroupBy(item => item.itemCode)
                    .Select(group => new { item = group.First(), count = group.Count() })
                    .ToList();

        if (drop_List.Count == 0)
        {
            popup.null_Item_Text.gameObject.SetActive(true);
            return;
        }
        else
        {
            popup.null_Item_Text.gameObject.SetActive(false);
            // 그룹화 된 아이템들을 아이콘으로 생성
            foreach (var drop in item_Cnt)
            {
                if (drop != null)
                {
                    GameObject item_Obj = Instantiate(ui.reward_Item_Prefab, popup.inner_Item);
                    item_Obj.GetComponent<SetDropItem>().Init(drop.item, drop.count);

                }
            }
        }
    }
}
