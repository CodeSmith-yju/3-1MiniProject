using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum TypeInspection
{
    Equip,
    Ect,
    Gold
}

public class Inspection : MonoBehaviour
{
    [SerializeField] Item myItem;
    [SerializeField] TypeInspection type;
    public Image icon;
    public TextMeshProUGUI count;
    public int cnt;
    public bool renovateOk;

    [Header("Item Key")]
    public string ItemPK;
    public void Init(Item _item)
    {
        DBConnector.LoadItemByCodeFromDB(_item.itemCode, ref _item.itemImage, ref _item.typeIcon);
        ItemPK = "";
        myItem = _item;
        cnt = 0;
        Debug.Log("Init Pls");
        Setting();
        Debug.Log("Now cnt: "+cnt);
        renovateOk = false;
    }

    void Setting()
    {
        if (gameObject.transform.parent.GetComponent<SacrificeList>().ParentCk(gameObject.transform.parent.gameObject).Equals("Renovate"))
        {
            switch (type)
            {
                case TypeInspection.Equip:
                    icon.sprite = ItemResources.instance.itemRS[myItem.itemCode - 4].itemImage;
                    count.text = "0/1";
                    cnt = 1;
                    break;
                case TypeInspection.Ect:
                    icon.sprite = ItemResources.instance.itemRS[myItem.itemCode + 4].itemImage;
                    count.text = "0/3";//for()
                    cnt = 3;
                    break;
                case TypeInspection.Gold:
                    //Default icon == Coin
                    count.text = "0/300";//GmaeMgr.playerData[0].golde /300 
                    cnt = 300;
                    break;
                default:
                    break;
            }
        }
        else if (gameObject.transform.parent.GetComponent<SacrificeList>().ParentCk(gameObject.transform.parent.gameObject).Equals("Upgrade"))
        {
            switch (type)
            {
                case TypeInspection.Equip:
                    icon.sprite = ItemResources.instance.itemRS[myItem.itemCode].itemImage;
                    count.text = "0/1";
                    cnt = 1;
                    break;
                case TypeInspection.Ect:
                    icon.sprite = ItemResources.instance.itemRS[myItem.itemCode + 4].itemImage;
                    cnt = SetEctItemCnt(myItem.modifyStack);
                    count.text = "0/"+cnt.ToString();
                    break;
                case TypeInspection.Gold:
                    //Default icon == Coin
                    cnt = SetGoldCnt(myItem.modifyStack);
                    count.text = "0/"+cnt.ToString();
                    break;
                default:
                    break;
            }
        }
        
        SetAlphaToPartial();
    }

    public void SetTextColor(Color color)
    {
        if (count != null)
        {
            count.color = color;
        }
        else
        {
            Debug.LogWarning("Target TextMeshProUGUI is not assigned!");
        }
    }
    public void SetAlphaToFull()
    {
        SetImageAlpha(1f);
    }
    public void SetAlphaToPartial()
    {
        SetImageAlpha(65f / 255f);
    }
    private void SetImageAlpha(float alphaValue)
    {
        if (icon != null)
        {
            Color currentColor = icon.color;
            currentColor.a = alphaValue;
            icon.color = currentColor;
        }
        else
        {
            Debug.LogWarning("Target Image is not assigned!");
        }
    }
    public Item GetItem()
    {
        return myItem;
    }
    public void SetItemPK(string _str)
    {
        ItemPK = _str;
    }
    int SetEctItemCnt(int _modifyStack)
    {
        int _cnt = 0;

        if (_modifyStack < 1)
        {
            _cnt = 1;
        }
        else
        {
            _cnt = ++_modifyStack + 2;
        }

        return _cnt;
    }
    int SetGoldCnt(int _modifyStack)
    {
        int _cnt = 300;

        if (_modifyStack < 1)
        {
            _cnt = 300;
        }
        else
        {
            _cnt *= (_modifyStack + 1);
            _cnt -= 100;
        }

        return _cnt;
    }
}

