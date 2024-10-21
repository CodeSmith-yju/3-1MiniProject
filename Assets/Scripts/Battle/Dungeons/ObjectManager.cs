using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{

    [Header("Player_Projectile")]
    public GameObject[] player_Prefabs;

    [Header("Enemy_Projectile")]
    public GameObject[] enemy_Prefabs;

    public List<GameObject>[] player_Pools;
    public List<GameObject>[] enemy_Pools;

    public Transform obj_Parent;

    private void Awake()
    {
        player_Pools = new List<GameObject>[player_Prefabs.Length];
        enemy_Pools = new List<GameObject>[enemy_Prefabs.Length];

        for (int i = 0; i < player_Pools.Length; i++) 
        {
            player_Pools[i] = new List<GameObject>();
        }

        for (int i = 0; i < enemy_Pools.Length; i++)
        {
            enemy_Pools[i] = new List<GameObject>();
        }
    }

    public GameObject GetObject(int index, bool isPlayer)
    {
        GameObject select = null;

        // ������ Ǯ�� ��Ȱ��ȭ ��(��� �ִ�)���� ������Ʈ ����
            // �߰��ϸ� select ������ �Ҵ�
        if (isPlayer) 
        {
            foreach (GameObject item in player_Pools[index])
            {
                if (!item.activeSelf)
                {
                    select = item;
                    select.SetActive(true);
                    break;
                }
            }
        }
        else
        {
            foreach (GameObject item in enemy_Pools[index])
            {
                if (!item.activeSelf)
                {
                    select = item;
                    select.SetActive(true);
                    break;
                }
            }
        }
        

        // �� ã����
        if (!select)
        {
            // ���� �����ϰ� select�� �Ҵ�
            if (isPlayer)
            {
                select = Instantiate(player_Prefabs[index], obj_Parent);
                player_Pools[index].Add(select);
            }  
            else
            {
                select = Instantiate(enemy_Prefabs[index], obj_Parent);
                enemy_Pools[index].Add(select);
            }
               
        }

        return select;
    }
    
    public void Poolclear()
    {
        foreach (List<GameObject> item in player_Pools)
        {
            item.Clear();
        }

        foreach (List<GameObject> item in enemy_Pools)
        {
            item.Clear();
        }
    }

}
