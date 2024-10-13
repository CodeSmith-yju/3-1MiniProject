using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInit : MonoBehaviour
{
    [SerializeField] private Sprite[] buff_Icons;
    private int buff_Index;
    private Dictionary<Ally, PlayerStats> temp_Stats = new Dictionary<Ally, PlayerStats>();
    private List<Ally> buffedPlayers = new List<Ally>(); // ������ ����� �÷��̾� ����

    // ���� ������Ʈ ���� (���� �����ϴ� ���� ������Ʈ�� ����)
    [SerializeField] private BuffTooltip buffTooltip;


    private void Start()
    {
        buffTooltip = FindObjectOfType<BuffTooltip>();
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
    }

    // �� �����Ӹ��� �÷��̾����� �Ÿ� üũ
    private void CheckPlayerDistances()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerObj in players)
        {
            Ally player = playerObj.GetComponent<Ally>();
            if (player == null) continue;

            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance < 0.1f) // �÷��̾ ���� Ÿ�� ���� ���� ��
            {
                if (!buffedPlayers.Contains(player))
                {
                    Buff(buff_Index, player); // ���� ����
                    buffedPlayers.Add(player);
                }
            }
            else // �÷��̾ ���� Ÿ���� ����� ��
            {
                if (buffedPlayers.Contains(player))
                {
                    RemoveBuff(player); // ���� ����
                    buffedPlayers.Remove(player);
                }
            }
        }
    }



    private void Buff(int index, Ally player)
    {
        if (!temp_Stats.ContainsKey(player))
        {
            temp_Stats[player] = new PlayerStats(player.atkDmg, player.max_Hp, player.max_Mp);
        }


        switch (index) 
        {
            case 0: // ���ݷ� ���� +10%
                player.atkDmg *= 1.1f;
                break;
            case 1: // �ִ� ü�� ���� +20%
                float originalMaxHp = player.max_Hp;
                player.max_Hp *= 1.2f;
                player.cur_Hp = player.cur_Hp / originalMaxHp * player.max_Hp;  // ���� ���� ���� ü�� ����
                break;
            case 2: // �ִ� ���� ���� -1 (��ų ��Ÿ�� ����)
                player.max_Mp -= 1;
                break;
        }
    }

    private void RemoveBuff(Ally player)
    {
        if (temp_Stats.ContainsKey(player))
        {
            PlayerStats stats = temp_Stats[player];

            player.atkDmg = stats.temp_Dmg;
            player.max_Hp = stats.temp_MaxHp;
            player.max_Mp = stats.temp_MaxMp;

            player.cur_Hp = player.cur_Hp / player.max_Hp * stats.temp_MaxHp;

            player.cur_Hp = Mathf.Min(player.cur_Hp, player.max_Hp);

            temp_Stats.Remove(player);
        }
    }

    private class PlayerStats
    {
        public float temp_Dmg;
        public float temp_MaxHp;
        public float temp_MaxMp;

        public PlayerStats(float atkDmg, float maxHp, float maxMp)
        {
            temp_Dmg = atkDmg;
            temp_MaxHp = maxHp;
            temp_MaxMp = maxMp;
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



