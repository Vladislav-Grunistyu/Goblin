using UnityEngine;

[CreateAssetMenu(fileName = "New SpotSO", menuName = "Game designer tools/SpotSO", order = 3)]
public class SpotSO : ScriptableObject
{
    public GameObject visualDisplaySpot;

    [System.Serializable]
    public struct RequiredResource//������� ��������� ��� ������
    {
        public ResourceSO resourceType;
        public int amount;
    }

    [System.Serializable]
    public struct SentResource//������� ������� ������ ����� ������
    {
        public ResourceSO resourceType;
        public int amount;
    }

    public float materialProcessingTime;
    public float resourceScatter; // ������� �������� ��� �������� � ����
    public float delayBetweenResources; // �������� ����� �������� ��������� � ����
    public float ejectionForceMultiplication;//��������� ���� ������������ �������
    public Vector3 playerEjectionBefore;//����������� ����������� �������� � ����. ���������� ��������� ����������� ����� playerEjectionBefore � playerEjectionAfter
    public Vector3 playerEjectionAfter;
    public RequiredResource[] requiredResources; // ������ ���������� �������� ��� �����
    public SentResource[] sentResources; // ���������� ���������� ������ ��������
}
