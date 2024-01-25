using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class SaveGame : MonoBehaviour//Посколько в инвенторе есть ResourceSO, а в нем GameObgect который нельзя преобразовать в JSON. То пришлось импровизировать
{
    private string savePath;
    private void Awake()
    {
        savePath = Path.Combine(Application.dataPath, "Resources/inventory.json");
    }
    public void SaveInventory(List<InventoryItem> inventory)
    {
        string json = JsonUtility.ToJson(new SerializableInventory(inventory));//Преобразуем инвентарь в JSON-строку

        File.WriteAllText(savePath, json);//Сохраняем JSON-строку в файл

        Debug.Log("Инвентарь успешно сохранился.");
    }

    public List<InventoryItem> LoadInventory()
    {
        if (File.Exists(savePath))
        {
            
            string json = File.ReadAllText(savePath);//Загружаем JSON-строку из файла

            
            var inventoryJSON = JsonUtility.FromJson<SerializableInventory>(json).ToInventory();//Десериализуем JSON в инвентарь
            var SOInFolder = GetResourceNamesInFolder("Assets/Settings/Resources");
            List<InventoryItem> inventoryWithSO = new List<InventoryItem>();
            InventoryItem loadedItemSO = new InventoryItem();
            for (int i = 0; i < inventoryJSON.Count; i++)//Проходим по каждому элементу и сравниваем с именем ResourceSO в папке проекта
            {
                for (int j = 0; j < SOInFolder.Count; j++)
                {
                    if (inventoryJSON[i].Name == SOInFolder[j])//Если совпадают то присваиваем ResourceSO согласно имени
                    {
                        loadedItemSO.Name = inventoryJSON[i].Name;
                        loadedItemSO.Resource = Resources.Load<ResourceSO>(SOInFolder[j]);
                        loadedItemSO.Amount = inventoryJSON[i].Amount;
                        inventoryWithSO.Add(loadedItemSO);
                        loadedItemSO = new InventoryItem();
                    }
                }
            }
            Debug.Log("Инвентарь успешно загрузился.");
            return inventoryWithSO;
        }
        else
        {
            Debug.Log("Нет сохранения.");
            return new List<InventoryItem>();
        }
    }

    [System.Serializable]
    private class SerializableInventory//Инвентарь который подходит для JSON
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
    private class SerializableInventoryItem//Ячейка инвенторя которя подходит для JSON
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

    private List<string> GetResourceNamesInFolder(string folderPath)//Находим список всех ResourceSO в папке
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
            Debug.LogError("Нету папки: " + folderPath);
        }

        return resourceNames;
    }
}
