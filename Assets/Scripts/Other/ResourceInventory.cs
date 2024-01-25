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
            inventory = gameObject.AddComponent<SaveGame>().LoadInventory();//���� ��� ����� �� ��������� ����������
            RemoveDuplicates();
        }
    }

    private void Start()
    {
        drop = GetComponent<DropResurces>();
        onPlayerChangeInventory?.Invoke(gameObject);//��������� UI
    }
    public void AddResource(ResourceSO resource, int count)
    {
        var existingItem = inventory.Find(item => item.Resource == resource);

        if (existingItem != null)//���� ����� ������ ��� ���� � ������, �� ������ ���������� ����������
        {
            existingItem.Amount += count;
            OnPlayerChangeInventory(gameObject);
        }
        else//���� ������ ������� ��� ��� � ������, �� ������
        {
            inventory.Add(new InventoryItem { Name = resource.ResourceName, Resource = resource, Amount = count });
            OnPlayerChangeInventory(gameObject);
        }

        inventory.RemoveAll(item => item.Amount <= 0);//������� ���� �������� � ������� �����������
    }

    public void SubtractResource(ResourceSO resource, Transform target, int count, float time)
    {
        var existingItem = inventory.Find(item => item.Resource.ResourceName == resource.ResourceName);

        if (existingItem != null && existingItem.Amount >= count)
        {
            existingItem.Amount -= count;

            drop.CreateResource(resource, transform.position, target, count, time);

            inventory.RemoveAll(item => item.Amount <= 0);//������� ���� �������� � ������� �����������
            OnPlayerChangeInventory(gameObject);
        }
    }

    private void OnPlayerChangeInventory(GameObject player)//���� ��� ��� ����������� � �������, �� ����������� � ������� �� ��������� ���������
    {
        if (player.GetComponent<PlayerMovement>())
        {
            player.AddComponent<SaveGame>().SaveInventory(inventory);
            onPlayerChangeInventory?.Invoke(gameObject);
        }
    }

    public void RemoveDuplicates()
    {
        List<InventoryItem> uniqueItems = new List<InventoryItem>();//������� ����� ������, � ������� ����� ��������� ���������� ��������

        foreach (var item in inventory)//���� �� ������� �������� � �������� ������
        {
            var existingItem = uniqueItems.Find(uItem => uItem.Name == item.Name);//���������, ���� �� ��� ������� � ����� �������� � ����� ������
            
            if (existingItem == null)//���� ������� �� ������, �� ��������� ��� � ����� ������
            {
                uniqueItems.Add(item);
            }
            else
            {
                existingItem.Amount += item.Amount;//���� ������� ��� ����, �� ��������� ����������
            }
        }

        inventory = uniqueItems;//����������� ����� ������ � ��������
    }

    public int GetResourceCount(ResourceSO resource)//��������� ���������� ������������� �������
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
