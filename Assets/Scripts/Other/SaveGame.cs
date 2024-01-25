using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class SaveGame : MonoBehaviour//��������� � ��������� ���� ResourceSO, � � ��� GameObgect ������� ������ ������������� � JSON. �� �������� ���������������
{
    private string savePath;
    private void Awake()
    {
        savePath = Path.Combine(Application.dataPath, "Resources/inventory.json");
    }
    public void SaveInventory(List<InventoryItem> inventory)
    {
        string json = JsonUtility.ToJson(new SerializableInventory(inventory));//����������� ��������� � JSON-������

        File.WriteAllText(savePath, json);//��������� JSON-������ � ����

        Debug.Log("��������� ������� ����������.");
    }

    public List<InventoryItem> LoadInventory()
    {
        if (File.Exists(savePath))
        {
            
            string json = File.ReadAllText(savePath);//��������� JSON-������ �� �����

            
            var inventoryJSON = JsonUtility.FromJson<SerializableInventory>(json).ToInventory();//������������� JSON � ���������
            var SOInFolder = GetResourceNamesInFolder("Assets/Settings/Resources");
            List<InventoryItem> inventoryWithSO = new List<InventoryItem>();
            InventoryItem loadedItemSO = new InventoryItem();
            for (int i = 0; i < inventoryJSON.Count; i++)//�������� �� ������� �������� � ���������� � ������ ResourceSO � ����� �������
            {
                for (int j = 0; j < SOInFolder.Count; j++)
                {
                    if (inventoryJSON[i].Name == SOInFolder[j])//���� ��������� �� ����������� ResourceSO �������� �����
                    {
                        loadedItemSO.Name = inventoryJSON[i].Name;
                        loadedItemSO.Resource = Resources.Load<ResourceSO>(SOInFolder[j]);
                        loadedItemSO.Amount = inventoryJSON[i].Amount;
                        inventoryWithSO.Add(loadedItemSO);
                        loadedItemSO = new InventoryItem();
                    }
                }
            }
            Debug.Log("��������� ������� ����������.");
            return inventoryWithSO;
        }
        else
        {
            Debug.Log("��� ����������.");
            return new List<InventoryItem>();
        }
    }

    [System.Serializable]
    private class SerializableInventory//��������� ������� �������� ��� JSON
    {
        public List<SerializableInventoryItem> Items;

        public SerializableInventory(List<InventoryItem> items)
        {
            Items = new List<SerializableInventoryItem>();
            foreach (var item in items)
            {
                Items.Add(new SerializableInventoryItem(item));
            }
        }

        public List<InventoryItem> ToInventory()
        {
            List<InventoryItem> loadedInventory = new List<InventoryItem>();
            foreach (var item in Items)
            {
                loadedInventory.Add(item.ToInventoryItem());
            }
            return loadedInventory;
        }
    }

    [System.Serializable]
    private class SerializableInventoryItem//������ ��������� ������ �������� ��� JSON
    {
        public string Name;
        public string Resource;
        public int Amount;

        public SerializableInventoryItem(InventoryItem item)
        {
            Name = item.Name;
            Resource = null;
            Amount = item.Amount;
        }

        public InventoryItem ToInventoryItem()
        {
            InventoryItem loadedItem = new InventoryItem();
            loadedItem.Name = Name;
            loadedItem.Resource = null;
            loadedItem.Amount = Amount;
            return loadedItem;
        }
    }

    private List<string> GetResourceNamesInFolder(string folderPath)//������� ������ ���� ResourceSO � �����
    {
        List<string> resourceNames = new List<string>();

        if (Directory.Exists(folderPath))
        {
            string[] files = Directory.GetFiles(folderPath);

            foreach (var file in files)
            {
                if (file.EndsWith(".asset"))
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    resourceNames.Add(fileName);
                }
            }
        }
        else
        {
            Debug.LogError("���� �����: " + folderPath);
        }

        return resourceNames;
    }
}
