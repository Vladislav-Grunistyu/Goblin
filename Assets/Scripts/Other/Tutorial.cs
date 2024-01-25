using DG.Tweening;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private Transform[] tutorialPoints;//������ ����� ���� ������ �������� �������
    [SerializeField]
    private ResourceSO[] needResource;//������ ����������� ��� ����� �����
    [SerializeField]
    private ResourceInventory inventory;//��������� ������
    [SerializeField]
    private GameObject tutorialArrowPrefab;//�������� ���������
    private GameObject tutorialArrow;
    private int currentTutorialPointIndex = 0;//����� � ������ ������ ������

    private void Start()
    {
        if (tutorialPoints.Length > 0 && inventory.GetPlayerObjects().Count == 0)//���� ����� �� ����� � � ������ ��� ��������(������ �� ��������� ���� � ������ ���). ������� ������� � ������� � ������ �����
        {
            tutorialArrow = Instantiate(tutorialArrowPrefab, inventory.gameObject.transform.position, Quaternion.identity, transform);
            MoveToNextPoint();
        }
    }

    private void MoveToNextPoint()
    {
        if (currentTutorialPointIndex < tutorialPoints.Length)//���� ������� ���������� ����, �� �� �� ����������
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
        if (currentTutorialPointIndex < tutorialPoints.Length)//���������, ����� �� ����� ������, ���� �� �� � ��������� �����
        {
            if (inventory.GetResourceCount(needResource[currentTutorialPointIndex]) > 0)
            {
                currentTutorialPointIndex++;
                MoveToNextPoint();
            }
        }
    }
    private void StartArrowAnimation()//��� �������
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
