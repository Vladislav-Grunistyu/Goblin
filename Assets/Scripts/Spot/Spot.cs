using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Spot : MonoBehaviour
{
    public SpotSO data;//Ссылка на SO с информацие для спота
    private new Collider collider;
    private int neededResources;//Счетчик при проверке сможет ли гг столько скинуть
    private int resources;//Количество ресурсов в споте
    private GameObject target;
    private void Start()
    {
        Instantiate(data.visualDisplaySpot, transform.GetChild(0).position, Quaternion.identity, transform);//Создаем 3D модель спота
        collider = GetComponent<Collider>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerMovement>()) //Вошел гг
        {
            if (other.GetComponent<NavMeshAgent>().destination == other.transform.position)
            {
                GiveResourcesToSpot(other.gameObject);
            }
        }
        else
        {
            //Если вошел не гг
        }
    }

    private void GiveResourcesToSpot(GameObject player)
    {
        int neededCell = 0;
        foreach (var requiredResource in data.requiredResources)//Проверяем есть ли у гг такие ресурсы
        {
            neededResources += requiredResource.amount;
            if (player.GetComponent<ResourceInventory>().GetResourceCount(requiredResource.resourceType) >= requiredResource.amount)//Если количество данного ресурса есть, то прибавляем счетчик
            {
                neededCell++;
            }        
        }
        if (neededCell == data.requiredResources.Length)//Если есть все ресурсы, то гг отдает
        {
            foreach (var requiredResource in data.requiredResources)
            {
                player.GetComponent<ResourceInventory>().SubtractResource(requiredResource.resourceType, transform, requiredResource.amount, data.delayBetweenResources);
            }
            collider.enabled = false;//Выключаем чтобы не выпрашивать у гг ресурсы
            target = player;
        }
        else
        neededResources = 0;
    }

    public void PickUp()
    {
        resources++;
        if (neededResources == resources)//Если все запрошенные ресурсы пришли, то начинаем крафт
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

    private IEnumerator WaitToDrop(float time)//Ждем под анимацию
    {
        transform.GetChild(1).DOShakeScale(time, 0.2f, 2);
        yield return new WaitForSeconds(time);
        DropResult();
    }
}
