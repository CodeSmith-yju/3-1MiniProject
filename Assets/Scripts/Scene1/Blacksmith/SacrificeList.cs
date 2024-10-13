using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SacrificeList : MonoBehaviour
{
    public Inspection[] inspections = new Inspection[3];
    public bool inpectionRedy = false;

    public void ChangeInspectionsVlue(Item _item)
    {
        Debug.Log("Run ChangeValue");
        if (ParentCk(gameObject).Equals("Renovate"))
        {
            Debug.Log("Run Renovate Change Value ");
            for (int i = 0; i < inspections.Length; i++)
            {
                int countItem = 0;
                switch (inspections[i].cnt)
                {
                    case 0:
                        Debug.Log("0");
                        inspections[i].renovateOk = false;
                        break;
                    case 1:
                        //DBConnector.LoadItemByCodeFromDB(_item.itemCode, ref _item.itemImage, ref _item.typeIcon);
                        if (_item == null)
                        {
                            inspections[i].SetItemPK(string.Empty);
                            inspections[i].SetAlphaToPartial();
                            inspections[i].SetTextColor(Color.red);
                            inspections[i].count.text = "0/1";
                            inspections[i].renovateOk = false;

                            Debug.Log("아이템이없어요");
                            inspections[i].SetItemPK("");
                        }
                        else if (_item.itemCode == inspections[i].GetItem().itemCode - 4)
                        {
                            inspections[i].SetAlphaToFull();
                            inspections[i].SetTextColor(Color.green);
                            inspections[i].count.text = _item.itemStack.ToString() + "/1";
                            inspections[i].renovateOk = true;

                            Debug.Log("=============== 제 발 요 ===============");
                            inspections[i].SetItemPK(_item.GetItemUniqueCode());
                        }
                        break;
                    case 3:
                        //DBConnector.LoadItemByCodeFromDB(_item.itemCode, ref _item.itemImage, ref _item.typeIcon);
                        for (int j = 0; j < Inventory.Single.items.Count; j++)
                        {
                            if (Inventory.Single.items[j].itemCode == inspections[i].GetItem().itemCode + 4)
                            {
                                countItem += Inventory.Single.items[j].itemStack;
                                continue;
                            }
                        }
                        if (countItem >= 3)
                        {
                            inspections[i].SetAlphaToFull();
                            inspections[i].SetTextColor(Color.green);
                            inspections[i].renovateOk = true;
                        }
                        else
                        {
                            inspections[i].SetAlphaToPartial();
                            inspections[i].SetTextColor(Color.red);
                            inspections[i].renovateOk = false;
                        }
                        inspections[i].count.text = countItem.ToString() + "/3";
                        break;
                    case 300:
                        if (GameMgr.playerData[0].player_Gold >= 300)
                        {
                            inspections[i].SetAlphaToFull();
                            inspections[i].SetTextColor(Color.green);
                            inspections[i].renovateOk = true;
                        }
                        else
                        {
                            inspections[i].SetAlphaToPartial();
                            inspections[i].SetTextColor(Color.red);
                            inspections[i].renovateOk = false;
                        }
                        inspections[i].count.text = GameMgr.playerData[0].player_Gold.ToString() + "/300";
                        break;
                    default:
                        Debug.LogWarning("Erorr");
                        break;
                }
            }
        }
        else if (ParentCk(gameObject).Equals("Upgrade"))
        {
            for (int i = 0; i < inspections.Length; i++)
            {
                int countItem = 0;
                switch (inspections[i].cnt)
                {
                    case 0:
                        Debug.Log("0");
                        inspections[i].renovateOk = false;
                        break;
                    case 1:
                        //DBConnector.LoadItemByCodeFromDB(_item.itemCode, ref _item.itemImage, ref _item.typeIcon);
                        if (_item == null)
                        {
                            Debug.Log("Upgrade할 아이템이 없어요");

                            inspections[i].SetItemPK(string.Empty);
                            inspections[i].SetAlphaToPartial();
                            inspections[i].SetTextColor(Color.red);
                            inspections[i].count.text = "0/1";
                            inspections[i].renovateOk = false;
                            inspections[i].SetItemPK("");
                        }
                        else if(_item.itemType == Item.ItemType.Ect)
                        {
                            //여기에이제 뭐세팅하고 _item null로들어오는거 다른쪽로직검사좀해봐야겠네
                        }
                        else
                        {
                            Debug.Log("Upgrade할 장비가 있어요");
                            if (_item.itemCode == inspections[i].GetItem().itemCode)
                            {
                                Debug.Log("=============== 제 발 요 ===============");
                                inspections[i].SetAlphaToFull();
                                inspections[i].SetTextColor(Color.green);
                                inspections[i].count.text = _item.itemStack.ToString() + "/1";
                                inspections[i].renovateOk = true;

                                inspections[i].SetItemPK(_item.GetItemUniqueCode());
                            }
                        }
                        
                        break;
                    case 3:
                        //DBConnector.LoadItemByCodeFromDB(_item.itemCode, ref _item.itemImage, ref _item.typeIcon);
                        for (int j = 0; j < Inventory.Single.items.Count; j++)
                        {
                            if (Inventory.Single.items[j].itemCode == inspections[i].GetItem().itemCode + 4)
                            {
                                countItem += Inventory.Single.items[j].itemStack;
                                continue;
                            }
                        }
                        if (countItem >= 3)
                        {
                            inspections[i].SetAlphaToFull();
                            inspections[i].SetTextColor(Color.green);
                            inspections[i].renovateOk = true;
                        }
                        else
                        {
                            inspections[i].SetAlphaToPartial();
                            inspections[i].SetTextColor(Color.red);
                            inspections[i].renovateOk = false;
                        }
                        inspections[i].count.text = countItem.ToString() + "/3";
                        break;
                    case 300:
                        if (GameMgr.playerData[0].player_Gold >= 300)
                        {
                            inspections[i].SetAlphaToFull();
                            inspections[i].SetTextColor(Color.green);
                            inspections[i].renovateOk = true;
                        }
                        else
                        {
                            inspections[i].SetAlphaToPartial();
                            inspections[i].SetTextColor(Color.red);
                            inspections[i].renovateOk = false;
                        }
                        inspections[i].count.text = GameMgr.playerData[0].player_Gold.ToString() + "/300";
                        break;
                    default:
                        Debug.LogWarning("Erorr");
                        break;
                }
            }
        }

        AllCk();
    }
    public void FirstItemMinus()
    {
        inspections[0].SetAlphaToPartial();
        inspections[0].SetTextColor(Color.red);
        inspections[0].count.text = "0/1";
    }
    public bool AllCk()
    {
        inpectionRedy = false;
        for (int i = 0; i< inspections.Length; i++)
        {
            inpectionRedy = inspections[i].renovateOk;
            if (inspections[i].renovateOk == false)
            {
                inpectionRedy = false; 
                break;
            }
        }
        return inpectionRedy;
    }
    public string ParentCk(GameObject go)
    {
        Debug.Log("Run ParentCk: "+go.name);
        string str;
        if (go.transform.parent.name.Equals("Renovate"))//Debug.Log("Renovate True");
            str = "Renovate";
        else if (go.transform.parent.name.Equals("Upgrade"))//Debug.Log("Upgrade True");
            str = "Upgrade";
        else
            str = "";
        Debug.Log("str: "+ str);
        return str;
    }
}
