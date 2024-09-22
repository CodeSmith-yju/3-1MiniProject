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
        Start, // �� ���� ���� (���� ���� üũ)
        Deploy, // ������ ���� �� ��ġ �ܰ� ����
        Rest, // �������� �ƴ� �� 
        Battle, // ��ġ �ܰ迡�� ��Ʋ ���� ��ư�� ���� ��Ʋ�� ���۵� ����
        End // ���� �� �׾��ų�, �Ʊ��� �� ������� ��� <- ����� ��ġ�� ��Ƽ���� �� ������ ������ ����̶� ���� ��ġ�� ���� ���� ��Ƽ���� ������ �����? Ȥ�� �ƿ� �� ��ġ�ǵ��� �ؾ��� ��
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
            Debug.Log("NOW QUESTID 40 GOLD: "+ GameMgr.playerData[0].player_Gold);
            GameMgr.playerData[0].player_Gold = 1500;
        }

        // �Ҹ�ǰ ������ üũ �� ������ �ٿ� ����
        SetItem();
    }

    private void Start()
    {
        ChangePhase(BattlePhase.Start); // �� üũ
        AudioManager.single.PlayBgmClipChange(2);
    }

    public IEnumerator BattleReady() // ���� ���� �� ����Ǵ� �޼���
    {
        deploy_area = GameObject.FindGameObjectWithTag("Deploy");
        unit_deploy_area = GameObject.FindGameObjectWithTag("Wait");
        ui.OpenPopup(ui.battle_Ready_Banner);
        yield return StartCoroutine(ui.StartBanner(ui.battle_Ready_Banner));
        yield return new WaitForSeconds(0.15f);

        // PlacementUnit(); // ��Ƽ ����Ʈ�� �ִ� ���� ����

        Enemy[] entity = FindObjectsOfType<Enemy>(); // ���͸� ã��
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
            Debug.Log("�� ����");
            ChangePhase(BattlePhase.End);
        }
    }


    public void ChangePhase(BattlePhase phase)
    {
        _curphase = phase;

        switch (phase) // �� ���¿� ���� ���� �� ����Ǵ� switch��
        {
            case BattlePhase.Start:
                if (room.isMoveDone || isFirstEnter)
                {
                    CheckRoom();
                    isFirstEnter = false;
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
            ui.alert_Popup.GetComponent<TitleInit>().Init("�ּ� �Ѹ��� ��Ƽ���� ��ġ�� �ؾ� �մϴ�.");
            yield break;
        }
        else
        {
            AudioManager.single.PlaySfxClipChange(7);
            ui.OpenPopup(ui.battle_Start_Banner);
            yield return StartCoroutine(ui.StartBanner(ui.battle_Start_Banner));
            yield return new WaitForSeconds(0.15f);

            Debug.Log("��Ʋ ����");
            ChangePhase(BattlePhase.Battle);
            deploy_area = GameObject.FindGameObjectWithTag("Deploy");
            unit_deploy_area = GameObject.FindGameObjectWithTag("Wait");
            deploy_area.SetActive(false);
            unit_deploy_area.SetActive(false);

            if (_curphase == BattlePhase.Battle)
            {
                BaseEntity[] entity = FindObjectsOfType<BaseEntity>(); // Ȱ��ȭ �� �÷��̾ ���͸� ã�Ƽ� ����Ʈ�� ����

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
                        drag.enabled = false; // ��Ʋ ���� �ÿ��� �÷��̾���� �巡�װ� �ȵǵ��� ����.
                    }
                }

                // ��Ʋ ���� �� �� �ȿ� �ִ� ������ ������ ����ġ��, ���, ��� ������ ����ؼ� �ӽú����� ����;
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

                    Debug.Log("���� �� �ִ� ����ġ �� : " + exp_Cnt);
                    Debug.Log("���� �� �ִ� ��� : " + gold_Cnt);
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

                Debug.Log("���� ����ġ : " + exp_Cnt);
                RewardPopupInit popup = ui.reward_Popup.GetComponent<RewardPopupInit>();
                popup.Init("���� �¸�", false);


                GameObject gold_Obj = Instantiate(ui.reward_Prefab, popup.inner_Gold_Exp);
                gold_Obj.GetComponent<RewardInit>().Init(ui.reward_Icons[0], gold_Cnt + " Gold");
                total_Gold += gold_Cnt;

                GameObject exp_Obj = Instantiate(ui.reward_Prefab, popup.inner_Gold_Exp);
                exp_Obj.GetComponent<RewardInit>().Init(ui.reward_Icons[1], exp_Cnt + " Exp");
                total_Exp += exp_Cnt;


                // ������ �ڵ�� �ߺ� ����� ������ ã�� �׷�ȭ
                SetIconDropItem(popup, drop_Item);

                foreach (Item item in drop_Item)
                {
                    // ����� �����۵��� �� ��� ������ ����Ʈ�� �ֱ�
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

                // �� ���, ����ġ�� ����
                GameMgr.playerData[0].player_Gold += total_Gold;
                GameMgr.playerData[0].GetPlayerExp(total_Exp);

                // ���� �����۵��� �κ��丮�� �߰�
                foreach (Item item in total_Drop_Item) 
                {
                    Inventory.Single.AddItem(item);
                }


                Debug.Log("��ư ����");
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
            Debug.Log("�����");
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

    // �� ���� üũ �޼���
    public void CheckRoom()
    {

        // �� �濡 ��ġ�� ���ֵ� ����

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

        unit_deploy_area = GameObject.FindGameObjectWithTag("Wait");
        PlacementUnit(); // � ���̵� ������ ��ȯ ��Ű���� ��.

        if (room.cur_Room.tag == "Battle")
        {

            Debug.Log("���� ���Դϴ�.");

            if (room.FindRoom(room.cur_Room.gameObject).isBoss)
            {
                AudioManager.single.PlayBgmClipChange(3);
            }

            ChangePhase(BattlePhase.Deploy);
        }
        else
        {
            ChangePhase(BattlePhase.Rest);
            Debug.Log("�޽�");
        }
    }

    // ��ġ �ܰ��϶� ���� ��Ƽ���� ������ ������ ��Ƽ���� ��ġ
    private void PlacementUnit()
    {
        Debug.Log("�۵��ϳ� üũ");

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

                // �� �� ���ֵ��� ���� �ϵ��� ��.
                if (GameMgr.playerData[unit_Cnt].cur_Player_Hp > 0)
                {
                    Debug.Log("�۵��ϳ� üũ ���� ����" + unit_Cnt);
                    Vector3 worldPos = deployTilemap.GetCellCenterWorld(position);

                    GameObject unit = Instantiate(party_List[unit_Cnt], worldPos, Quaternion.identity);
                    unit.GetComponent<Ally>().InitStat(GameMgr.playerData[unit_Cnt].playerIndex);

                    deploy_Player_List.Add(unit);

                    unit_Cnt++; // ���� ���� ��ġ�ϱ� ���� �ε��� ��
                }
            }
        }
    }

    // ��ġ �������� üũ
    private bool CanPlace(Vector3Int position)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(unit_deploy_area.GetComponent<Tilemap>().GetCellCenterWorld(position));

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return false; // �̹� �ش� ��ġ�� ������ ������ ��ġ�� �� �����ϴ�.
            }
        }

        return true;
    }


    // ������ ������ ������ ���ư��� �޼���
    public void ReturnToTown()
    {
        /*Debug.Log("������ �̵�");

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

    // ���� �˾� ���빰 �ʱ�ȭ
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


    // �κ��丮 ������ �ҷ�����
    private void SetItem()
    {
        int index = 0;
        for (int i = 0; i < 10; i++)
        {
            ItemUse iia = Instantiate(ui.item_Slot_Prefabs, ui.item_Bar.transform.GetChild(0));

            // ������ ���� �ʱ�ȭ
            Item item = SetInnerItem(ref index);

            iia.Init(item);

            // ������ ������ ����Ʈ�� �߰�
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

    // ��� �����۵��� ������ �ڵ�� �׷�ȭ �Ͽ� �������� �����ϴ� �޼��� (LinQ ���)
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
            // �׷�ȭ �� �����۵��� ���������� ����
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
