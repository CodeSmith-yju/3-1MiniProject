using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SacrificeList : MonoBehaviour
{
    public Inspection[] inspections = new Inspection[3];

    public void ChangeInspectionsVlue(Item _item)
    {
        for (int i = 0; i < inspections.Length; i++)
        {
            int countItem = 0;
            switch (inspections[i].cnt)
            {
                case 0:
                    Debug.Log("0");
                    break;
                case 1:
                    if (_item == null)
                    {
                        inspections[i].SetAlphaToPartial();
                        inspections[i].SetTextColor(Color.red);
                        inspections[i].count.text = "0/1";
                    }
                    else if (_item.itemCode == inspections[i].GetItem().itemCode - 4)
                    {
                        inspections[i].SetAlphaToFull();
                        inspections[i].SetTextColor(Color.green);
                        inspections[i].count.text = _item.itemStack.ToString() + "/1";
                    }
                    break;
                case 3:
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
                    }
                    else
                    {
                        inspections[i].SetAlphaToPartial();
                        inspections[i].SetTextColor(Color.red);
                    }
                    inspections[i].count.text = countItem.ToString() + "/3";
                    break;
                case 300:
                    if (GameMgr.playerData[0].player_Gold >= 300)
                    {
                        inspections[i].SetAlphaToFull();
                        inspections[i].SetTextColor(Color.green);
                    }
                    else
                    {
                        inspections[i].SetAlphaToPartial();
                        inspections[i].SetTextColor(Color.red);
                    }
                    inspections[i].count.text = GameMgr.playerData[0].player_Gold.ToString() + "/300";
                    break;
                default:
                    Debug.LogWarning("Erorr");
                    break;
            }
        }
    }
    public void FirstItemMinus()
    {
        inspections[0].SetAlphaToPartial();
        inspections[0].SetTextColor(Color.red);
        inspections[0].count.text = "0/1";
    }
}