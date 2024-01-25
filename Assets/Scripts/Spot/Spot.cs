using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Spot : MonoBehaviour
{
    public SpotSO data;//������ �� SO � ���������� ��� �����
    private new Collider collider;
    private int neededResources;//������� ��� �������� ������ �� �� ������� �������
    private int resources;//���������� �������� � �����
    private GameObject target;
    private void Start()
    {
        Instantiate(data.visualDisplaySpot, transform.GetChild(0).position, Quaternion.identity, transform);//������� 3D ������ �����
        collider = GetComponent<Collider>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerMovement>()) //����� ��
        {
            if (other.GetComponent<NavMeshAgent>().destination == other.transform.position)
            {
                GiveResourcesToSpot(other.gameObject);
            }
        }
        else
        {
            //���� ����� �� ��
        }
    }

    private void GiveResourcesToSpot(GameObject player)
    {
        int neededCell = 0;
        foreach (var requiredResource in data.requiredResources)//��������� ���� �� � �� ����� �������
        {
            neededResources += requiredResource.amount;
            if (player.GetComponent<ResourceInventory>().GetResourceCount(requiredResource.resourceType) >= requiredResource.amount)//���� ���������� ������� ������� ����, �� ���������� �������
            {
                neededCell++;
            }        
        }
        if (neededCell == data.requiredResources.Length)//���� ���� ��� �������, �� �� ������
        {
            foreach (var requiredResource in data.requiredResources)
            {
                player.GetComponent<ResourceInventory>().SubtractResource(requiredResource.resourceType, transform, requiredResource.amount, data.delayBetweenResources);
            }
            collider.enabled = false;//��������� ����� �� ����������� � �� �������
            target = player;
        }
        else
        neededResources = 0;
    }

    public void PickUp()
    {
        resources++;
        if (neededResources == resources)//���� ��� ����������� ������� ������, �� �������� �����
        {
            StartCoroutine(WaitToDrop(data.materialProcessingTime));
            resources = 0;
        }
    }
    
    private void DropResult()
    {
        foreach (var sentResources in data.sentResources)
        {
            gameObject.GetComponent<DropResurces>().CreateResource(sentResources.resourceType, transform.position, target.transform, sentResources.amount, 0.5f);
        }
        neededResources = 0;
        collider.enabled = true;
    }

    private IEnumerator WaitToDrop(float time)//���� ��� ��������
    {
        transform.GetChild(1).DOShakeScale(time, 0.2f, 2);
        yield return new WaitForSeconds(time);
        DropResult();
    }
}
