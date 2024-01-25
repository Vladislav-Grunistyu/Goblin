using DG.Tweening;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private Transform[] tutorialPoints;//Список точек куда должна двгаться стрелка
    [SerializeField]
    private ResourceSO[] needResource;//Ресурс необходимый для смены точки
    [SerializeField]
    private ResourceInventory inventory;//Инвентарь игрока
    [SerializeField]
    private GameObject tutorialArrowPrefab;//Моделька стрелочки
    private GameObject tutorialArrow;
    private int currentTutorialPointIndex = 0;//Какая в данный момент стадия

    private void Start()
    {
        if (tutorialPoints.Length > 0 && inventory.GetPlayerObjects().Count == 0)//Если точки не пусты и у игрока нет ресурсов(значит он запускает игру в первый раз). Создаем стрелку и двигаеи к первой точке
        {
            tutorialArrow = Instantiate(tutorialArrowPrefab, inventory.gameObject.transform.position, Quaternion.identity, transform);
            MoveToNextPoint();
        }
    }

    private void MoveToNextPoint()
    {
        if (currentTutorialPointIndex < tutorialPoints.Length)//Если стрелка отработала свое, то мы ее уничтожаем
        {
            Vector3 nextPoint = tutorialPoints[currentTutorialPointIndex].position;
            MoveToTarget(nextPoint);
        }
        else
        {
            Destroy(tutorialArrow);
        }
    }
    private void Update()
    {
        if (currentTutorialPointIndex < tutorialPoints.Length)//Проверяем, добыл ли игрок ресурс, если да то к следующей точке
        {
            if (inventory.GetResourceCount(needResource[currentTutorialPointIndex]) > 0)
            {
                currentTutorialPointIndex++;
                MoveToNextPoint();
            }
        }
    }
    private void StartArrowAnimation()//Как красиво
    {
        transform.DORotate(new Vector3(0f, 0f, 15f), 1f / 2f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                transform.DORotate(new Vector3(0f, 0f, -15f), 1f / 2f)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        StartArrowAnimation();
                    });
            });
    }
    private void MoveToTarget(Vector3 target)
    {
        transform.DOMove(target + new Vector3(-0.4f,2, 4.4f), 10)
            .OnComplete(() =>
            {
                StartArrowAnimation();
            });
    }
}
