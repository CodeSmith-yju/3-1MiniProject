using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BattleManager : MonoBehaviour
{
    private static BattleManager instance = null;
    public List<GameObject> deloy_Player_List = new List<GameObject>();

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
        if (_mapId != 0 || _mapId != 2)
        {
            ChangePhase(BattlePhase.Deploy); // �� üũ�ؼ� �������̸� ����
        }
    }

    public void BattleReady()
    {
        // ��Ƽ���� �ʱ� ��ġ�� ��ġ�ϴ� �޼��峪 �ڵ� �ۼ�

        BaseEntity[] entity = FindObjectsOfType<BaseEntity>(); // ���͸� ����

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


}
