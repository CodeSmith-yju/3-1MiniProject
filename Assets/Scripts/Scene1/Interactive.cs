/*using UnityEngine;

public class ChangeSortingOrder : MonoBehaviour
{
    public SpriteRenderer mySR;

    private bool enterPlayer = false;

    *//*private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTrigger Potal");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player In Potal");
            mySR.sortingOrder = 6;
        }
    }*//*

    private void Update()
    {
        if (enterPlayer)
        {
            mySR.sortingOrder = 6;
            //mySR.order
        }
        else
        {
            mySR.sortingOrder = 2;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTrigger Potal - Detected something");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player In Potal");
        }
        else
        {
            Debug.Log("Other object: " + other.gameObject.name);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ���⿡ �浹 �� ���������� ȣ���� �ڵ带 �߰��� �� �ֽ��ϴ�.
            // ���� ���, �ִϸ��̼� ���¸� Ȯ���ϰų� �߰����� ������ ó���� �� �ֽ��ϴ�.
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Exit");
            mySR.sortingOrder = 2;
        }
    }

}
*/