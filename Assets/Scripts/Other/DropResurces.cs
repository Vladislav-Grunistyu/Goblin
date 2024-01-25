using System.Collections;
using UnityEngine;

public class DropResurces : MonoBehaviour
{
    [SerializeField]
    private GameObject resourcePrefab;

    public void CreateResource(ResourceSO resourceData, Vector3 dropPoint, Transform target, int amount, float time)
    {
        StartCoroutine(CreateWithDelayEnum(resourceData, dropPoint, target, amount, time));
    }
    private IEnumerator CreateWithDelayEnum(ResourceSO resourceData, Vector3 dropPoint, Transform target, int amount, float time)
    {
        for (int i = 0; i < amount; i++) 
        {
            yield return new WaitForSeconds(time);
            GameObject res = Instantiate(resourcePrefab, dropPoint, Quaternion.identity);
            Resource resScr = res.GetComponent<Resource>();
            resScr.resourceData = resourceData;
            resScr.target = target;
            Rigidbody rb = res.GetComponent<Rigidbody>();
            Vector3 launchDirection;
            if (gameObject.GetComponent<PlayerMovement>() && target.GetComponent<Spot>())//Если мы выкидываем в спот
            {
                var spotSettings = target.GetComponent<Spot>().data;
                launchDirection = new Vector3(Random.Range(spotSettings.playerEjectionBefore.x, spotSettings.playerEjectionAfter.x), Random.Range(spotSettings.playerEjectionBefore.y, spotSettings.playerEjectionAfter.y), Random.Range(spotSettings.playerEjectionBefore.z, spotSettings.playerEjectionAfter.z)).normalized;
                rb.AddForce(launchDirection * resourceData.sidewaysForce * spotSettings.ejectionForceMultiplication, ForceMode.Impulse);
                rb.AddTorque(Vector3.up * resourceData.upwardForce * spotSettings.ejectionForceMultiplication, ForceMode.Impulse);
            }
            else//Если просто выкидываем
            {
                launchDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized;
                rb.AddForce(launchDirection * resourceData.sidewaysForce, ForceMode.Impulse);
                rb.AddTorque(Vector3.up * resourceData.upwardForce, ForceMode.Impulse);
            }
        }
    }
}
