using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory_Mgr : MonoBehaviour
{
    //�κ��丮 �Ŵ���, �������� �������� �������ִ�
    public List<item> poolItem;//list��� stack�� ����ص��Ȥ�

    private List<Equiment> Listequiment = new List<Equiment>();

    public string[] aryArmorNames;
    public string[] aryBootsNames;
    public string[] aryCapsNames;
    public string[] aryGlovesNames;


    private item CreateItem()
    {
        foreach (item item in poolItem)
        {
            if (!item.gameObject.activeSelf)
                return item;
        }

        //��� ������Ʈ�� ��� ���� ��� ���ο� ������Ʈ ����
        GameObject go = Instantiate(poolItem[0].gameObject, poolItem[0].transform.parent);
        Transform tr = go.transform;

        tr.localScale = Vector3.one;
        tr.localPosition = Vector3.zero;

        item newItem = go.GetComponent<item>();
        poolItem.Add(newItem);

        go.SetActive(true);
        return newItem;

    }
        

    private void Start()
    {
        ClickResetBtn();
    }

    public void ClickResetBtn()
    {
        foreach (var item in poolItem)
        {
            item.gameObject.SetActive(false);
        }

        Listequiment.Clear();

        for (int i = 0; i < 2; i++)
        {
            Listequiment.Add(new Equi_Armor(aryArmorNames[Random.Range(0, aryArmorNames.Length)]));

            Listequiment.Add(new Equi_Boots(aryBootsNames[Random.Range(0, aryBootsNames.Length)]));

            Listequiment.Add(new Equi_Cap(aryCapsNames[Random.Range(0, aryCapsNames.Length)]));

            Listequiment.Add(new Equi_Gloves(aryGlovesNames[Random.Range(0, aryGlovesNames.Length)]));
        }

        for (int i = 0; i < Listequiment.Count; i++)
        {
            CreateItem().Init(Listequiment[i]);
        }
    }

}