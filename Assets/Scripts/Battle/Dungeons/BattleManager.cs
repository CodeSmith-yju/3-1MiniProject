using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    private static BattleManager instance = null;
    public ObjectManager pool;
    public List<GameObject> party_List = new List<GameObject>();
    public List<GameObject> deloy_Player_List = new List<GameObject>();
    public List<GameObject> deloy_Enemy_List = new List<GameObject>();
    public GameObject popup_Bg;
    public GameObject vic_Popup;
    public GameObject def_Popup;
    private bool battleEnded = false;

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
        Start,
        Deploy,
        Battle,
        End
    }


    public BattlePhase _curphase;

    private int _mapId;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }

        ChangePhase(BattlePhase.Start); // �� üũ
    }

  


    private void Start()
    {
        /*if (_mapId != 0 || _mapId != 2)
        {
            ChangePhase(BattlePhase.Deploy); // �� üũ�ؼ� �������̸� ����
        }*/

        ChangePhase(BattlePhase.Deploy);
        GameObject[] enemy_Obj = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemy_Obj != null)
        {
            foreach (GameObject obj in enemy_Obj) 
            {
                deloy_Enemy_List.Add(obj);
            }
        }
        
    }

    public void BattleReady()
    {
        // ��Ƽ���� �ʱ� ��ġ�� ��ġ�ϴ� �޼��峪 �ڵ� �ۼ�

        BaseEntity[] entity = FindObjectsOfType<BaseEntity>(); // ���Ϳ� �÷��̾ ã��

        foreach (BaseEntity obj in entity)
        {
            NavMeshAgent nav = obj.GetComponent<NavMeshAgent>();
            EntityDrag drag = obj.GetComponent<EntityDrag>();

            if (nav != null)
            {
                nav.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            }
            if (drag != null)
            {
                drag.enabled = true;
            }
        }
    }

    private void Update()
    {
        if (_curphase == BattlePhase.Deploy)
        {
            BattleReady();
        }

        if (_curphase == BattlePhase.Battle && (deloy_Player_List.Count == 0 || deloy_Enemy_List.Count == 0))
        {
            Debug.Log("�� ����");
            ChangePhase(BattlePhase.End);
        }

        if (_curphase == BattlePhase.End)
        {
            EndBattle();
        }
    }


    public void ChangePhase(BattlePhase phase)
    {
        _curphase = phase;

        switch (phase)
        {
            case BattlePhase.Start:
                // ������ ���� ���� üũ �� �� �濡 �ش��ϴ� �޽Ĺ�
                break;
            case BattlePhase.Deploy:

                break;
            case BattlePhase.Battle:

                break;
            case BattlePhase.End:
                break;
        }
    }


    public void BattleStart()
    {
        if (deloy_Player_List.Count == 0)
        {
            Debug.Log("�ϳ� �̻��� �÷��̾ ��ġ�ϼ���");
            return;
            
        }
        else
        {
            Debug.Log("��Ʋ ����");
            ChangePhase(BattlePhase.Battle);

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
                        drag.enabled = false;
                    }
                }
            }
        }
    }

    private void EndBattle()
    {
        if (_curphase == BattlePhase.End && !battleEnded)
        {
            popup_Bg.SetActive(true);

            if (deloy_Player_List.Count == 0)
            {
                def_Popup.SetActive(true);
                vic_Popup.SetActive(false);
            }
            else if (deloy_Enemy_List.Count == 0)
            {
                def_Popup.SetActive(false);
                vic_Popup.SetActive(true);
            }
        }


        battleEnded = true;
    }


    private void CheckMap(int map)
    {
        switch (map) // �� �ڵ�
        {
            case 0: // �޽Ĺ�
                _mapId = 0;
                break;
            case 1: // ������
                _mapId = 1;
                break;
            case 2: // �������� ��
                _mapId = 2;
                break;
            case 3: // �߰����� ��
                _mapId = 3;
                break;
            case 4: // ���� ��
                _mapId = 4;
                break;
        }
    }


    public void ReturnToTown()
    {
        Debug.Log("������ �̵�");
        SceneManager.LoadScene("Scene1");
    }

}
