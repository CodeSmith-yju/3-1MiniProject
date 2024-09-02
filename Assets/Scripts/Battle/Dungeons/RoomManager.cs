
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RoomManager : MonoBehaviour
{
    public Transform[] rooms;
    public List<GameObject> map_List;
    public int room_Count = 0;
    public bool isMoveDone = false;
    public GameObject maps_Prefab;
    public Transform map_Pos;
    public Camera map_Camera;
    public Camera map_Big_Camera;

    Transform cur_Map;
    public Transform currentRoom;
    public Transform previousRoom;
    private Vector3 velocity = Vector3.zero;

    void Awake()
    {
        map_List = new List<GameObject>();
        isMoveDone = true;
        // 초기 방 설정
        currentRoom = rooms[room_Count];

        for (int i = 0; i < rooms.Length; i++) 
        {
            Vector3 map = map_Pos.position + new Vector3(i * 5f, 0, 0); 
            GameObject maps = Instantiate(maps_Prefab, map, Quaternion.identity);

            maps.transform.SetParent(map_Pos);
            map_List.Add(maps);
        }

        cur_Map = map_List[room_Count].transform;

        foreach (GameObject minimap in map_List) 
        {
            if (cur_Map.gameObject == minimap)
            {
                foreach (Transform child in minimap.transform)
                {
                    child.gameObject.SetActive(false);
                }
                map_Camera.transform.position = new Vector3(minimap.transform.position.x, minimap.transform.position.y, map_Camera.transform.position.z);
            }
        }


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
        if (isMoveDone)
        {
            AudioManager.single.PlaySfxClipChange(4);
            isMoveDone = false;
            BattleManager.Instance.ui.out_Portal.GetComponent<FadeEffect>().fadein = true;
            previousRoom = currentRoom;
            currentRoom = rooms[++room_Count];
            cur_Map = map_List[room_Count].transform;
            StartCoroutine(MoveCamera());
        }
    }

    private IEnumerator MoveCamera()
    {
        
        Vector3 targetPosition = new Vector3(currentRoom.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        Vector3 targetMap = new Vector3(cur_Map.position.x, cur_Map.position.y, map_Camera.transform.position.z);

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
            Debug.Log("버그 체크");
        }

        while (Vector3.Distance(Camera.main.transform.position, targetPosition) > 0.1f)
        {
            if (Vector3.Distance(Camera.main.transform.position, targetPosition) > 0.1f)
            {
                Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, targetPosition, ref velocity, 0.2f);
            }

            if (Vector3.Distance(map_Camera.transform.position, targetMap) > 0.1f)
            {
                map_Camera.transform.position = Vector3.SmoothDamp(map_Camera.transform.position, targetMap, ref velocity, 1f);
            }
            yield return null; // 다음 프레임까지 대기
        }

        // 목표 위치에 정확히 맞춤
        Camera.main.transform.position = targetPosition;
        map_Camera.transform.position = targetMap;

        foreach (GameObject minimap in map_List)
        {
            if (cur_Map.gameObject == minimap)
            {
                foreach (Transform child in minimap.transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
            else
            {
                foreach (Transform child in minimap.transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }


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

        if (BattleManager.Instance.dialogue.isTutorial)
        {
            BattleManager.Instance.dialogue.ONOFF(true);
            BattleManager.Instance.dialogue.NextDialogue();
        }


        // 상태 변경
        BattleManager.Instance.ChangePhase(BattleManager.BattlePhase.Start);
        
        yield break;
    }
}
