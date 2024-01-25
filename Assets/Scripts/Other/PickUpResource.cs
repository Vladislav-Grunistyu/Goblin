using System.Collections.Generic;
using UnityEngine;

public class PickUpResource : MonoBehaviour
{
    [SerializeField]
    private float attractRadius = 5f; //Радиус притяжения
    private float destroyDistance = 0.1f; //Расстояние, при котором объект исчезает
    private float attractionSpread;//Разброс притягивания

    private List<Transform> attractedObjects = new List<Transform>();
    private void Start()
    {
        if (GetComponent<Spot>())
        {
            attractRadius = 100;
            attractionSpread = gameObject.GetComponent<Spot>().data.resourceScatter;
        }
    }

    void Update()//Находим все объекты в радиусе притяжения с компонентом "Resource"
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attractRadius);
        attractedObjects.Clear();

        foreach (var collider in colliders)
        {   
            if (collider.GetComponent<Resource>() && collider.GetComponent<Resource>().readyToSelect)
            {
               attractedObjects.Add(collider.transform);
            }
        }

        // Притягиваем объекты к себе
        AttractObjects();

        // Проверяем, достаточно ли близко объекты для исчезновения
        CheckDestroyDistance();
    }

    void AttractObjects()
    {
        foreach (var attractedObject in attractedObjects)
        {
            if (attractedObject.GetComponent<Resource>().target == transform)//Если этот ресурс преднозначен нам
            {
                if (gameObject.GetComponent<Spot>())//Если мы спот, то притягиваем с разбросом
                {
                    Vector3 attractionDirection = (transform.position - attractedObject.position).normalized;
                    float distance = Vector3.Distance(transform.position, attractedObject.position);
                    float attractionStrength = Mathf.Clamp01((attractRadius - distance) / attractionSpread) * attractedObject.GetComponent<Resource>().resourceData.flySpeed;

                    attractedObject.position += attractionDirection * (attractionStrength * Time.deltaTime);
                }
                Vector3 directionToPlayer = (transform.position - attractedObject.position).normalized;
                attractedObject.Translate(directionToPlayer * attractedObject.GetComponent<Resource>().resourceData.flySpeed * Time.deltaTime, Space.World);
            }
        }
    }

    void CheckDestroyDistance()
    {
        for (int i = attractedObjects.Count - 1; i >= 0; i--)
        {
            float distanceToTarget = Vector3.Distance(transform.position, attractedObjects[i].position);//Проверяем расстояние ресурса цели

            if (distanceToTarget < destroyDistance && attractedObjects[i].GetComponent<Resource>().target == transform)//Если ресурс достаточно близко, уничтожаем его и удаляем из списка
            {
                if (gameObject.GetComponent<PlayerMovement>())//Если подобрал гг, то добавляем в инвентарь
                {
                    gameObject.GetComponent<ResourceInventory>().AddResource(attractedObjects[i].GetComponent<Resource>().resourceData, 1);

                }
                if (gameObject.GetComponent<Spot>())//Если подобрал спот, то он засчитывает себе этот ресурс
                {
                    gameObject.GetComponent<Spot>().PickUp();
                }
                //+если подбирает не игрок
                Destroy(attractedObjects[i].gameObject);
                attractedObjects.RemoveAt(i);
            }
        }
    }
}
