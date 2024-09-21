using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ItemEft/Consumable/Health")]
public class ItemHealing : ItemEffect
{
    public int healPoint = 0;
    public override bool ExcuteRole()
    {
        if ( GameUiMgr.single.player_Cur_HP <= GameUiMgr.single.player_Cur_HP - healPoint )
        {
            GameUiMgr.single.player_Cur_HP += GameUiMgr.single.player_Cur_HP*0.3f;
            GameUiMgr.single.SliderChange();
        }
        else
        {
            GameUiMgr.single.player_Cur_HP = GameUiMgr.single.player_Max_HP;
            GameUiMgr.single.SliderChange();
        }

        return true;
    }
}
