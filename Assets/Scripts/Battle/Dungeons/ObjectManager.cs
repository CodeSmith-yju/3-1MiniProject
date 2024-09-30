using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{

    [Header("Player_Projectile")]
    public GameObject[] player_Prefabs;

    [Header("Enemy_Projectile")]
    public GameObject[] enemy_Prefabs;

    public List<GameObject>[] pools;

    public Transform obj_Parent;

    private void Awake()
    {
        pools = new List<GameObject>[(player_Prefabs.Length + enemy_Prefabs.Length)];

        for (int i = 0; i < pools.Length; i++) 
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject GetObject(int index, bool isPlayer)
    {
        GameObject select = null;

        // 선택한 풀의 비활성화 된(놀고 있는)게임 오브젝트 접근
            // 발견하면 select 변수에 할당
        foreach(GameObject item in pools[index])
        {
            if(!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // 못 찾으면
        if (!select)
        {
            // 새로 생성하고 select에 할당
            if (isPlayer)
            {
                select = Instantiate(player_Prefabs[index], obj_Parent);
            }  
            else
            {
                select = Instantiate(enemy_Prefabs[index], obj_Parent);
            }
                
            pools[index].Add(select);
        }

        return select;
    }
    
    public void Poolclear()
    {
        foreach (List<GameObject> item in pools)
        {
            item.Clear();
        }
    }

}
