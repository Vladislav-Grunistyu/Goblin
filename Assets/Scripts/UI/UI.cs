using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    [SerializeField] 
    private VisualTreeAsset resourceItemTemplate;
    private VisualElement resourceListContainer;

    private List<InventoryItem> playerObjects = new List<InventoryItem>();
    private int sumOldObjects = 0;//����� �������� �� ���������� UI

    private void Start()
    {
        resourceListContainer = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("root");
    }

    private void UpdateUI()
    {
        resourceListContainer.Clear();// ������� �������� ����������

        int sumObjects = 0;
        foreach (var item in playerObjects)
            sumObjects += item.Amount;
        // ������� ���� �������� � ������
        foreach (var entry in playerObjects)
        {
            ResourceSO resource = entry.Resource;
            int count = entry.Amount;

            VisualElement resourceItem = resourceItemTemplate.CloneTree();

            Image iconImage = resourceItem.Q<Image>("icon");// ��������� ��������� UI
            Label countLabel = resourceItem.Q<Label>("count");

            iconImage.sprite = resource.icon;// ��������� �������� ������ � ������
            countLabel.text = count.ToString();

            resourceListContainer.Add(resourceItem);// ���������� �������� � ���������

            // ���� ����� �������� ������ �����������, ������������� ��������
            if (sumOldObjects < sumObjects)
            {
                StartCoroutine(ShakeAnimation(resourceItem));
            }
            
        }
        sumOldObjects = sumObjects;
    }

    private IEnumerator ShakeAnimation(VisualElement resourceItem)
    {
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Mathf.Sin(elapsed * 20f) * 5f;
            resourceItem.style.left = x;
            elapsed += Time.deltaTime;
            yield return null;
        }

        resourceItem.style.left = 0f;
    }

    private void UpdateUIDictionary(GameObject player)
    {
        playerObjects = player.GetComponent<ResourceInventory>().GetPlayerObjects();//���������� ����� ��������� ������
        UpdateUI();
    }

    private void OnEnable()
    {
        ResourceInventory.onPlayerChangeInventory += UpdateUIDictionary;
    }

    private void OnDisable()
    {
        ResourceInventory.onPlayerChangeInventory -= UpdateUIDictionary;
    }
}
