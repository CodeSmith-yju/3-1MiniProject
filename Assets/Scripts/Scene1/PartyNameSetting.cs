using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyNameSetting : MonoBehaviour
{
    // ���, �ü�, �������� �̸� ����Ʈ
    public List<string> knightNames;
    public List<string> archerNames;
    public List<string> mageNames;
    public List<string> priestNames;
    public List<string> demonNames;
    

    /* //�ѱ����̸��� �ϴ� ��������
        knightNames = new List<string> { "��������", "����յ�", "�ط���", "������", "����", "����", "���̸յ�", "������", "���̺�", "��" };
        archerNames = new List<string> {"���ϸ�", "���", "�̼Һ�", "�������", "�ݸ�", "���̳��", "�Ƶ���", "����", "����ī", "�ٷ�"};
        mageNames = new List<string> { "����Ŭ", "��ó��", "�������", "���긮��", "��ӽ�", "�˷����", "������", "ũ��������", "�ʸ�", "���̴�", };
    */
    public string GetRandomName(List<string> nameList)// �̸� ����Ʈ���� �����ϰ� �ϳ� �����ϴ� �Լ�
    {
        if (nameList.Count == 0)
        {
            Debug.LogWarning("�̸��� �� �̻� ������ �� �����ϴ�.");
            return "Unknown"; // ����Ʈ�� �̸��� ���� ��� �⺻�� ��ȯ
        }

        // ������ �ε��� ����
        int randomIndex = Random.Range(0, nameList.Count);

        // �������� ���õ� �̸��� ����
        string selectedName = nameList[randomIndex];

        // ���õ� �̸��� ����Ʈ���� ����
        nameList.RemoveAt(randomIndex);
        //Debug.Log("��ȯ�Ǿ� ����Ʈ���� ���ŵ�:" + selectedName);
        // ���õ� �̸� ��ȯ
        return selectedName;
    }
    public void RefreshiNameList()
    {
        /// <summary>
        /// public������ �����ϸ� ����Ƽ �ν�����â�����̴� SerialLize�ý��۶����� �ڵ����� null�� �ƴ϶� ����ִ� �迭�� �ʱ�ȭ�Ǿ �׷��ٰ���
        /// nameList ??= new List<string> { "A", "B", "C" }; �̷��� �����ٴ� ��. 
        /// ����ִ� ����Ʈ�� �ʱ�ȭ�Ǿ��⶧���� null�˻� �ϴ� �����Ҵ� �˻繮(" ??= ")�ȸ�����, Count�� 0�ι迭�̶� Count�˻��ؾߵ�.
        /// </summary>
        if (knightNames.Count < 10)
        {
            knightNames.Clear();
            knightNames = new List<string> { "Alaric", "Gawain", "Roland", "Lancelot", "Arthas", "Tristan", "Bedivere", "Galahad", "Percival", "Kay" };
        }
        if (archerNames.Count < 10)
        {
            archerNames.Clear();
            archerNames = new List<string> { "Robin", "Elduin", "Faolan", "Nimue", "Ayla", "Vara", "Rylai", "Orin", "Shara", "Elandra" };
        }
        if (mageNames.Count < 10)
        {
            mageNames.Clear();
            mageNames = new List<string> { "Merlin", "Gandalf", "Morgana", "Zatanna", "Aegis", "Althas", "Luna", "Arcanis", "Solara", "Faye" };
        }
        if (priestNames.Count < 10)
        {
            priestNames.Clear();
            priestNames = new List<string> { "Aldric", "Benedict", "Elspeth", "Ishara", "Tobias", "Lucius", "Cyril", "Anselm", "Seraphine", "Eldora" };
        }
        if (demonNames.Count < 10)
        {
            demonNames.Clear();
            demonNames = new List<string> { "Garruk", "Thorne", "Drex", "Magnus", "Borin", "Huldan", "Grimnir", "Varg", "Ragnar", "Krogan" };
        }
    }
}
