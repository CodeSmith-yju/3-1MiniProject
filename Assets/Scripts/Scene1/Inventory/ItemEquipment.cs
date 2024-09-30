using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Equipment_Helmet/Leather Helmet")]
public class ItemEquipment : ItemEffect
{
    public int arrmorPoint = 0;
    public override bool ExcuteRole()
    {
        //curPlayerArrmorPoint += arrmorPoint;

        Debug.Log("PlayerArrmorPoint " + arrmorPoint);
        return true;
    }

}
