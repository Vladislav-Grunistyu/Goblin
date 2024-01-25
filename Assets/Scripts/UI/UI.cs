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
    private int sumOldObjects = 0;//Сумма ресурсов до обновления UI

    private void Start()
    {
        resourceListContainer = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("root");
    }

    private void UpdateUI()
    {
        resourceListContainer.Clear();// Очистка текущего интерфейса

        int sumObjects = 0;
        foreach (var item in playerObjects)
            sumObjects += item.Amount;
        // Перебор всех ресурсов у игрока
        foreach (var entry in playerObjects)
        {
            ResourceSO resource = entry.Resource;
            int count = entry.Amount;

            VisualElement resourceItem = resourceItemTemplate.CloneTree();

            Image iconImage = resourceItem.Q<Image>("icon");// Получение элементов UI
            Label countLabel = resourceItem.Q<Label>("count");

            iconImage.sprite = resource.icon;// Установка значений иконки и текста
            countLabel.text = count.ToString();

            resourceListContainer.Add(resourceItem);// Добавление элемента в контейнер

            // Если новое значение больше предыдущего, воспроизводим анимацию
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
        playerObjects = player.GetComponent<ResourceInventory>().GetPlayerObjects();//Зарашиваем новый инвентарь игрока
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
