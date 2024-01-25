using System.Collections;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [HideInInspector]
    public ResourceSO resourceData;//SO � ��� ����������� �����������
    [HideInInspector]
    public bool readyToSelect;
    [HideInInspector]
    public Transform target;
    
    private void Start()
    {
        readyToSelect = false;
        Instantiate(resourceData.visualDisplayResource, transform.position, transform.rotation, transform);//������� 3D ����� �������
        StartCoroutine(RecoveryMine());
    }

    private IEnumerator RecoveryMine()
    {
        yield return new WaitForSeconds(resourceData.timeNotToSelect);
        readyToSelect = true;//������ ��� ����� �������
    }
}
