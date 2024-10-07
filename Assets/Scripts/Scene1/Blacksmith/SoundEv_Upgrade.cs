using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEv_Upgrade : MonoBehaviour
{
    public void UpgradeSfx(int _index)
    {
        Debug.Log("Upgrade Sfx Sound Run" + _index);
        //Sound ev
        AudioManager.single.PlaySfxClipChange(_index);
    }
}
