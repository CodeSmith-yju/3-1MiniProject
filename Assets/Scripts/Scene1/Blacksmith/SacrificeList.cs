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
            int countItem = 0;
            for (int i = 0; i < inspections.Length; i++)
            {
                inspections[i].renovateOk = false;
                if (inspections[i].GetMyInspectionType() == TypeInspection.Equip)
                {
                    Debug.Log("Type is Equip");
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
                    else if (_item.itemCode == inspections[i].GetItem().itemCode)
                    {
                        inspections[i].SetAlphaToFull();
                        inspections[i].SetTextColor(Color.green);
                        inspections[i].count.text = _item.itemStack.ToString() + "/1";
                        inspections[i].renovateOk = true;

                        Debug.Log("=============== 제 발 요 ===============");
                        inspections[i].SetItemPK(_item.GetItemUniqueCode());
                    }
                }
                else if (inspections[i].GetMyInspectionType() == TypeInspection.Ect)
                {
                    Debug.Log("Type is Ect");
                    for (int j = 0; j < Inventory.Single.items.Count; j++)
                    {
                        if (Inventory.Single.items[j].itemCode == inspections[i].GetItem().itemCode + 4)
                        {
                            countItem += Inventory.Single.items[j].itemStack;
                            continue;
                        }
                    }
                    if (countItem >= inspections[i].cnt)
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
                    inspections[i].count.text = countItem.ToString() + "/" + inspections[i].cnt.ToString();
                }
                else if (inspections[i].GetMyInspectionType() == TypeInspection.Gold)
                {
                    Debug.Log("Type is Gold");
                    if (GameMgr.playerData[0].player_Gold >= inspections[i].cnt)
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
                    inspections[i].count.text = GameMgr.playerData[0].player_Gold.ToString() + "/" + inspections[i].cnt.ToString();
                }
            }

            if (_item == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    inspections[i].Refresh();
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
