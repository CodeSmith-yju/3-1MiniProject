using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public Transform[] rooms;
    public GameObject popup;

    private Transform currentRoom;

    void Start()
    {
        // �ʱ� �� ����
        currentRoom = rooms[0];

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
        }


    }

    public void ChangeRoom(int roomIndex)
    {
        // ���ο� ������ ����
        currentRoom = rooms[roomIndex];
        BattleManager.Instance.ChangePhase(BattleManager.BattlePhase.Deploy);
        popup.SetActive(false);

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
        }

        

        Camera.main.transform.position = currentRoom.position;

        // ���� ������ ������ Deloy ������� ���ư�
        
    }
}
