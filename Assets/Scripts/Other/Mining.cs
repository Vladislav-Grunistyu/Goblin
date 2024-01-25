using System;
using UnityEngine;

public class Mining : MonoBehaviour
{
    public static Action <GameObject, GameObject>onAttack;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<ResourceSource>())
        {
            if (GetComponent<PlayerMovement>())
            {
                if (other.gameObject.GetComponent<ResourceSource>().readyToMine == true)
                {
                    onAttack?.Invoke(other.gameObject, gameObject);//Говорим что гг может добывать
                }
            }
            else
            {
                //логика для не гг
            }
        }
    }

}
