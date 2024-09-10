using DarkPixelRPGUI.Scripts.UI.Equipment;
using TMPro;
using UnityEngine;

public class InSlot : MonoBehaviour
{
    [SerializeField] Item myItem;
    public TextMeshProUGUI textName;
    //public Image HightLight;
    public void Init(Item _item)
    {
        myItem = _item;
        textName.text = myItem.itemName;

        gameObject.SetActive(true);
    }

    public Item GetItem()
    {
        if (myItem == null)
        {
            Debug.Log("myItem is Null");
            return null;
        }
        return myItem;
    }

    public void Highlight()
    {
        // 하이라이트 켜기/끄기
    }

}
