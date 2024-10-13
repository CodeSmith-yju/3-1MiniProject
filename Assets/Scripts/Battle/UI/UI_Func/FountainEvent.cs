using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainEvent : MonoBehaviour
{
    public bool event_Check = false;

    private void OnEnable()
    {
        event_Check = true;    
    }

    private void OnMouseDown()
    {
        if (BattleManager.Instance.dialogue != null && BattleManager.Instance.dialogue.isTutorial)
        {
            if (BattleManager.Instance.ui.dialogue_Box.activeSelf)
                return;
        }

        if (!BattleManager.Instance.ui.option_UI.activeSelf)
        {
            if (event_Check)
            {
                event_Check = false;

                if (BattleManager.Instance.dialogue != null)
                {
                    if (BattleManager.Instance.dialogue.isTutorial)
                    {
                        BattleManager.Instance.tutorial.EndTutorial(20);
                    }
                }

                if (!BattleManager.Instance.ui.event_Popup.activeSelf)
                {
                    BattleManager.Instance.ui.OpenPopup(BattleManager.Instance.ui.event_Popup);

                }
                else
                {
                    event_Check = true;
                    return;
                }
            }
        }
    }
}
