using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject[] prefabs;

    public List<GameObject>[] pools;

    public Transform obj_Parent;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++) 
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject GetObject(int index)
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
            select = Instantiate(prefabs[index], obj_Parent);
            pools[index].Add(select);
        }

        return select;
    }
    
}
