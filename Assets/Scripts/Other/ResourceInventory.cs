using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceInventory : MonoBehaviour
{
    private List<InventoryItem> inventory = new List<InventoryItem>();
    public static Action<GameObject> onPlayerChangeInventory;
    private DropResurces drop;

    private void Awake()
    {
        if (gameObject.GetComponent<PlayerMovement>())
        {
            inventory = gameObject.AddComponent<SaveGame>().LoadInventory();//Если это игрок то загружаем сохранение
            RemoveDuplicates();
        }
    }

    private void Start()
    {
        drop = GetComponent<DropResurces>();
        onPlayerChangeInventory?.Invoke(gameObject);//Обновляем UI
    }
    public void AddResource(ResourceSO resource, int count)
    {
        var existingItem = inventory.Find(item => item.Resource == resource);

        if (existingItem != null)//Если такой ресурс уже есть в списке, то просто прибовляем количество
        {
            existingItem.Amount += count;
            OnPlayerChangeInventory(gameObject);
        }
        else//Если такого ресурса еще нет в списке, то содаем
        {
            inventory.Add(new InventoryItem { Name = resource.ResourceName, Resource = resource, Amount = count });
            OnPlayerChangeInventory(gameObject);
        }

        inventory.RemoveAll(item => item.Amount <= 0);//Убираем типы ресурсов с нулевым количеством
    }

    public void SubtractResource(ResourceSO resource, Transform target, int count, float time)
    {
        var existingItem = inventory.Find(item => item.Resource.ResourceName == resource.ResourceName);

        if (existingItem != null && existingItem.Amount >= count)
        {
            existingItem.Amount -= count;

            drop.CreateResource(resource, transform.position, target, count, time);

            inventory.RemoveAll(item => item.Amount <= 0);//Убираем типы ресурсов с нулевым количеством
            OnPlayerChangeInventory(gameObject);
        }
    }

    private void OnPlayerChangeInventory(GameObject player)//Если все это происходило с игроком, то сохраняемся и говорим об изменений инвентаря
    {
        if (player.GetComponent<PlayerMovement>())
        {
            player.AddComponent<SaveGame>().SaveInventory(inventory);
            onPlayerChangeInventory?.Invoke(gameObject);
        }
    }

    public void RemoveDuplicates()
    {
        List<InventoryItem> uniqueItems = new List<InventoryItem>();//Создаем новый список, в который будем добавлять уникальные элементы

        foreach (var item in inventory)//Идем по каждому элементу в исходном списке
        {
            var existingItem = uniqueItems.Find(uItem => uItem.Name == item.Name);//Проверяем, есть ли уже элемент с таким ресурсом в новом списке
            
            if (existingItem == null)//Если элемент не найден, то добавляем его в новый список
            {
                uniqueItems.Add(item);
            }
            else
            {
                existingItem.Amount += item.Amount;//Если элемент уже есть, то обновляем количество
            }
        }

        inventory = uniqueItems;//Присваиваем новый список в исходный
    }

    public int GetResourceCount(ResourceSO resource)//Получения количества определенного ресурса
    {
        var item = inventory.Find(i => i.Resource == resource);
        return item != null ? item.Amount : 0;
    }

    public List<InventoryItem> GetPlayerObjects()
    {
        return new List<InventoryItem>(inventory);
    }

}

public class InventoryItem
{
    public string Name { get; set; }
    public ResourceSO Resource { get; set; }
    public int Amount { get; set; }
}
