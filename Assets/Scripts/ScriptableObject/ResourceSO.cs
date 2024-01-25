using UnityEngine;
[CreateAssetMenu(fileName = "New ResourceSO", menuName = "Game designer tools/ResourceSO", order = 1)]
public class ResourceSO : ScriptableObject
{
    public GameObject visualDisplayResource; //3D ������ �������
    public Sprite icon; //������ �������� ����������� � UI
    public string ResourceName;
    public float timeNotToSelect;//����� �� ������� ����� �� ����� ������� ������ ����� ������ 
    public float upwardForce;//���� � ��������� �������������� ������ �� ����� ������
    public float sidewaysForce;
    public float flySpeed;//�������� ������ � ������ ��� �����
}
