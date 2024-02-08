using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class item : MonoBehaviour
{
    [SerializeField] private Image imgIcon;
    [SerializeField] private Image imgFrame;
    [SerializeField] private TextMeshProUGUI textName; //�ν�����â���� ������ ���Ŀ� ������ ���� ���� ������ private
    [SerializeField] private TextMeshProUGUI textUpgrade;

    [SerializeField] private Image info_imgIcon;
    [SerializeField] private TextMeshProUGUI info_textName;
    [SerializeField] private TextMeshProUGUI info_textUpgrade;

    private int ran;

    private void Start()
    {
        //imgIcon.sprite = RescourceMgr.GetBoots("greyknight");
        //imgIcon.sprite = RescourceMgr.GetArmor("RoyalTunic");
        imgFrame.color = Color.red;     //imgFrame.color = new Color32();
        //textName.text = "hellp me";
        //textUpgrade.text = "+000";
    }

    public void Click_item()
    {
        info_imgIcon.sprite = imgIcon.sprite;
        info_textName.text = textName.text;
        info_textUpgrade.text = "UPGRADE"+textUpgrade.text;
    }

    public void Init(Equiment equipment)
    {
        ran = Random.Range(0, 10000);

        gameObject.SetActive(true);
        imgIcon.sprite = equipment.GetIcon();

        if (imgIcon.sprite == null)
            Debug.LogError("Sprite is Null");
        textName.text = imgIcon.sprite.name;
        textUpgrade.text = "+"+ran.ToString();
    }

}
