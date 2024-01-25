using UnityEngine;

[CreateAssetMenu(fileName = "New ResourceSourceSO", menuName = "Game designer tools/ResourceSourceSO", order = 2)]
public class ResourceSourceSO : ScriptableObject
{
    public ResourceStage[] stages;//������ ����� ������� �����
    public ResourceSO resourceType;//��� ������� ������� ����������� ��������
    public float miningSpeed;//�������� � ������� ���������� ������
    public float animationForce;//�� ������� ������ ����� ��������� �������� ��� ���������
    public int resourcePerHit;//������� �������� �������� �� ���� ����
    public float fullRecoveryTime;//����� ������������� ��� ������ ������� ���������
}

[System.Serializable]
public class ResourceStage//����� � ��������� ��� ��� �� ������ ������ ����� ����� ������������� ���������
{
    public GameObject visualDisplayStage;//3D �������� ������
}
