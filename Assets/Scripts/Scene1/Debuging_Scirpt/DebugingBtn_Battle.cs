using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugingBtn_Battle : MonoBehaviour
{
    public void ActionDebuingBtn()
    {
        Debug.Log("Now_cur - exp" + GameMgr.playerData[0].player_cur_Exp);
        Debug.Log("Now_max - exp" + GameMgr.playerData[0].player_max_Exp);

        GameMgr.playerData[0].GetPlayerExp(40);

        Debug.Log("PlayerData - QID: " + GameUiMgr.single.questMgr.questId);
        Debug.Log("PlayerData - AID: " + GameUiMgr.single.questMgr.questActionIndex);
        Debug.Log("NowGold: " + GameMgr.playerData[0].player_Gold);
        Debug.Log("Now_SN" + GameMgr.playerData[0].cur_Player_Sn);
        Debug.Log("Now_HP" + GameMgr.playerData[0].cur_Player_Hp);

        Debug.Log("Now_Lv" + GameMgr.playerData[0].player_level);
        Debug.Log("Now_cur - exp" + GameMgr.playerData[0].player_cur_Exp);
        Debug.Log("Now_max - exp" + GameMgr.playerData[0].player_max_Exp);

        Debug.Log("Load Type: " + GameMgr.single.LoadChecker() );

        GameMgr.playerData[0].cur_Player_Sn -= 15;
        GameMgr.playerData[0].cur_Player_Hp -= 15;

        GameUiMgr.single.GameSave();
        GameMgr.single.IsGameLoad(true);
        Debug.Log("Load Type: " + GameMgr.single.LoadChecker());
        
        SceneManager.LoadScene("Town");

        Debug.Log("이거 동작함?");
    }
}
