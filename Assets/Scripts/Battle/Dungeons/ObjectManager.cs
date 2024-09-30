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

        // ������ Ǯ�� ��Ȱ��ȭ ��(��� �ִ�)���� ������Ʈ ����
            // �߰��ϸ� select ������ �Ҵ�
        foreach(GameObject item in pools[index])
        {
            if(!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // �� ã����
        if (!select)
        {
            // ���� �����ϰ� select�� �Ҵ�
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
