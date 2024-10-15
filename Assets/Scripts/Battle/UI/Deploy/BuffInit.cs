using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BattleManager;

public class BuffInit : MonoBehaviour
{
    [SerializeField] private Sprite[] buff_Icons;
    private int buff_Index;
    private Dictionary<int, PlayerData> playerDataMapping = new Dictionary<int, PlayerData>();
    [SerializeField] private GameObject[] players;
    private Dictionary<Ally, PlayerStats> temp_Stats = new Dictionary<Ally, PlayerStats>();
    private HashSet<Ally> buffedPlayers = new HashSet<Ally>();

    // ���� ������Ʈ ���� (���� �����ϴ� ���� ������Ʈ�� ����)
    [SerializeField] private BuffTooltip buffTooltip;


    private void Start()
    {
        buffTooltip = FindObjectOfType<BuffTooltip>();
        players = GameObject.FindGameObjectsWithTag("Player");

        // �ʱ�ȭ: Ally�� PlayerData ����
        foreach (PlayerData data in GameMgr.playerData)
        {
            playerDataMapping[data.playerIndex] = data;
        }
    }

    public void Init(int index)
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = buff_Icons[index];
        this.buff_Index = index;
    }



    private void Update()
    {
        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Deploy)
        {
            CheckPlayerDistances();
        }

        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Battle)
        {
            if (buffTooltip.transform.GetChild(0).gameObject.activeSelf)
                HideTooltip();
        }
    }


    // �� �����Ӹ��� �÷��̾����� �Ÿ� üũ
    private void CheckPlayerDistances()
    {
        foreach (GameObject playerObj in players)
        {
            Ally player = playerObj.GetComponent<Ally>();
            if (player == null) continue;

            // �ش� �÷��̾��� PlayerData ��������
            if (playerDataMapping.TryGetValue(player.entity_index, out PlayerData playerStat))
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);

                if (distance < 0.1f) // �÷��̾ ���� Ÿ�� ���� ���� ��
                {
                    if (!buffedPlayers.Contains(player))
                    {
                        Buff(buff_Index, player, playerStat);
                        buffedPlayers.Add(player);
                        BattleManager.Instance.buffedPlayers.Add(player); // �÷��̾ ������ ����Ʈ�� �߰�
                    }
                }
                else // �÷��̾ Ÿ�Ͽ��� ����� ��
                {
                    if (buffedPlayers.Contains(player))
                    {
                        RemoveBuff(player, playerStat);
                        buffedPlayers.Remove(player);
                        BattleManager.Instance.buffedPlayers.Remove(player); // �÷��̾ ������ ����Ʈ���� ����
                    }
                }
            }
        }
    }

    private void Buff(int index, Ally player, PlayerData player_Data)
    {
        if (!temp_Stats.ContainsKey(player))
        {
            temp_Stats[player] = new PlayerStats(player_Data.base_atk_Dmg, player_Data.max_Player_Hp, player_Data.max_Player_Mp);
            Instance.temp_Stats.Add(player, temp_Stats[player]);
        }


        switch (index) 
        {
            case 0: // ���ݷ� ���� +10%
                player.atkDmg *= 1.1f;
                player_Data.base_atk_Dmg = player.atkDmg;
                Debug.Log($"{player.name}�� ���ݷ� ���� ����: {player.atkDmg}");
                break;
            case 1: // �ִ� ü�� ���� +20%
                float originalMaxHp = player.max_Hp;
                player.max_Hp *= 1.2f;
                float healthRatio = (originalMaxHp > 0) ? player.cur_Hp / originalMaxHp : 0;
                player.cur_Hp = Mathf.Clamp(healthRatio * player.max_Hp, 0, player.max_Hp);
                player_Data.max_Player_Hp = player.max_Hp;
                player_Data.cur_Player_Hp = player.cur_Hp;
                Debug.Log($"{player.name}�� �ִ�ü�� ���� ����: {player.max_Hp}");
                break;
            case 2: // �ִ� ���� ���� -1 (��ų ��Ÿ�� ����)
                player.max_Mp -= 1;
                player_Data.max_Player_Mp = player.max_Mp;
                Debug.Log($"{player.name}�� ���� ���� ����: {player.max_Mp}");
                break;
        }
    }

    private void RemoveBuff(Ally player, PlayerData player_Data)
    {
        if (temp_Stats.ContainsKey(player))
        {
            PlayerStats stats = temp_Stats[player];

            float healthRatio = (player.max_Hp > 0) ? player.cur_Hp / player.max_Hp : 1;

            player.atkDmg = stats.temp_Dmg;
            player.max_Hp = stats.temp_MaxHp;
            player.max_Mp = stats.temp_MaxMp;

            // ü�� ������ ���� ���� ü�� ���� �� Ŭ����
            player.cur_Hp = Mathf.Clamp(healthRatio * stats.temp_MaxHp, 0, stats.temp_MaxHp);

            player_Data.base_atk_Dmg = player.atkDmg;
            player_Data.max_Player_Mp = stats.temp_MaxMp;
            player_Data.max_Player_Hp = player.max_Hp;
            player_Data.cur_Player_Hp = player.cur_Hp;
            

            temp_Stats.Remove(player);
            BattleManager.Instance.temp_Stats.Remove(player);
        }
    }

    // ���콺�� �ش� ������Ʈ�� ������ ��
    private void OnMouseEnter()
    {
        ShowTooltip();
    }

    // ���콺�� �ش� ������Ʈ���� ������ ��
    private void OnMouseExit()
    {
        HideTooltip();
    }

    private void OnMouseOver()
    {
        if (buffTooltip.transform.GetChild(0).gameObject.activeSelf)
            UpdateTooltipPosition();
    }

    private void ShowTooltip()
    {
        string description = GetBuffDescription(buff_Index);
        Sprite icon = buff_Icons[buff_Index];
        buffTooltip.Init(description, icon); // ���� �ʱ�ȭ
        buffTooltip.transform.GetChild(0).gameObject.SetActive(true); // ���� ǥ��
    }

    private void UpdateTooltipPosition()
    {
        if (buffTooltip.transform.GetChild(0).gameObject.activeSelf)
        {
            // �⺻ ���콺 ��ġ�� ������ �߰� (������ ���� �̵�)
            Vector3 tooltipPosition = Input.mousePosition + new Vector3(20, -20, 0);

            // ������ RectTransform ��������
            RectTransform tooltipRect = buffTooltip.transform.GetChild(0).GetComponent<RectTransform>();

            // �ǹ��� ������ ���� ����
            tooltipRect.pivot = new Vector2(0, 1);

            // ȭ�� ��踦 ����� ��ġ ����
            float clampedX = Mathf.Clamp(tooltipPosition.x, 0, Screen.width - tooltipRect.rect.width);
            float clampedY = Mathf.Clamp(tooltipPosition.y, 0, Screen.height - tooltipRect.rect.height);

            // ������ ��ġ�� ���� ��ġ
            buffTooltip.transform.position = new Vector3(clampedX, clampedY, 0);
        }
    }


    private void HideTooltip()
    {
        buffTooltip.transform.GetChild(0).gameObject.SetActive(false);
    }

    private string GetBuffDescription(int index)
    {
        switch (index)
        {
            case 0: return "���ݷ� +10%";
            case 1: return "�ִ� ü�� +20%";
            case 2: return "�ִ� ���� -1";
            default: return "�� �� ���� �����Դϴ�.";
        }
    }



}



