
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RoomManager : MonoBehaviour
{
    public Transform[] rooms;
    public int room_Count = 0;
    public GameObject popup;
    public bool isMoveDone = false;

    public Transform currentRoom;
    public Transform previousRoom;
    private Vector3 velocity = Vector3.zero;

    void Awake()
    {
        // �ʱ� �� ����
        currentRoom = rooms[room_Count];

        foreach (Transform obj in rooms) 
        {
            if (currentRoom == obj)
            {
                obj.gameObject.SetActive(true);
                BattleManager.Instance.deploy_area = GameObject.FindGameObjectWithTag("Deploy");
                BattleManager.Instance.unit_deploy_area = GameObject.FindGameObjectWithTag("Wait");
                foreach (Transform child in obj)
                {
                    if (child.CompareTag("Enemy"))
                    {
                        child.gameObject.SetActive(true);
                    }
                }

            }
            else
            {
                obj.gameObject.SetActive(false);
            }
        }


    }

    public void ChangeRoom()
    {
        // ���ο� ������ ����
        if ((rooms.Length - 1) == room_Count) 
        {
            SceneManager.LoadScene("Town");
        }
        else
        {
            previousRoom = currentRoom;
            currentRoom = rooms[++room_Count];
            isMoveDone = false;
            StartCoroutine(MoveCamera());
        }
        popup.SetActive(false);
    }

    private IEnumerator MoveCamera()
    {
        Vector3 targetPosition = new Vector3(currentRoom.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);

        foreach (Transform obj in rooms)
        {
            if (currentRoom == obj)
            {
                obj.gameObject.SetActive(true);
            }
            else
            {
                obj.gameObject.SetActive(false);
            }

            if (previousRoom == obj)
            {
                obj.gameObject.SetActive(true);
            }

        }
        BattleManager.Instance.deploy_area = GameObject.FindGameObjectWithTag("Deploy");
        BattleManager.Instance.unit_deploy_area = GameObject.FindGameObjectWithTag("Wait");

        if (BattleManager.Instance.deploy_area != null && BattleManager.Instance.unit_deploy_area != null)
        {
            BattleManager.Instance.deploy_area.SetActive(false);
            BattleManager.Instance.unit_deploy_area.SetActive(false);
        }
        else
        {
            Debug.Log("���� üũ");
        }

        while (Vector3.Distance(Camera.main.transform.position, targetPosition) > 0.1f)
        {
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, targetPosition, ref velocity, 0.3f);
            yield return null; // ���� �����ӱ��� ���
        }

        // ��ǥ ��ġ�� ��Ȯ�� ����
        Camera.main.transform.position = targetPosition;

        foreach (Transform obj in rooms)
        {
            if (currentRoom == obj) 
            {
                foreach (Transform child in obj)
                {
                    if (child.CompareTag("Enemy"))
                    {
                        child.gameObject.SetActive(true);
                    }
                }
            }

            if (previousRoom == obj)
            {
                obj.gameObject.SetActive(false);
            }

        }


        isMoveDone = true;
        // ���� ����
        BattleManager.Instance.ChangePhase(BattleManager.BattlePhase.Start);
        
        yield break;

    }


}
