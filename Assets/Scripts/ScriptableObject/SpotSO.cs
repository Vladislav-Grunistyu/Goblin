using UnityEngine;

[CreateAssetMenu(fileName = "New SpotSO", menuName = "Game designer tools/SpotSO", order = 3)]
public class SpotSO : ScriptableObject
{
    public GameObject visualDisplaySpot;

    [System.Serializable]
    public struct RequiredResource//Ресурсы требуемые для крафта
    {
        public ResourceSO resourceType;
        public int amount;
    }

    [System.Serializable]
    public struct SentResource//Ресурсы которые отдаем после крафта
    {
        public ResourceSO resourceType;
        public int amount;
    }

    public float materialProcessingTime;
    public float resourceScatter; // Разброс ресурсов при перелете в спот
    public float delayBetweenResources; // Задержка между летящими ресурсами в спот
    public float ejectionForceMultiplication;//Умножение силы выталкивания ресурса
    public Vector3 playerEjectionBefore;//Направление выкидывания ресурсов в спот. Выберяется рандомное напровление между playerEjectionBefore и playerEjectionAfter
    public Vector3 playerEjectionAfter;
    public RequiredResource[] requiredResources; // Нужное количество ресурсов для спота
    public SentResource[] sentResources; // Количество отдаваемых спотом ресурсов
}
