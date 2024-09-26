using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLogReset : MonoBehaviour
{
    private void OnEnable()
    {
        LogReset();
    }


    private void LogReset()
    {
        if (BattleManager.Instance.ui.log_Pos.childCount != 0)
        {
            foreach (Transform child in BattleManager.Instance.ui.log_Pos)
            {
                Destroy(child.gameObject);
            }
        }
    }

}
