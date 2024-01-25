using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField]
    private float baseSpeed = 10f;
    private float rotationSpeed = 5000f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.angularSpeed = rotationSpeed;
    }

    private void FixedUpdate()
    {
        Vector3 inputDirection = Vector3.forward * JoystickController.input.y + Vector3.right * JoystickController.input.x;

        float joystickDistance = inputDirection.magnitude;
        float currentSpeed = baseSpeed * joystickDistance/2;
        if (inputDirection.magnitude >= 0.1f)
        {
            Vector3 moveDestination = transform.position + inputDirection * currentSpeed;
            agent.SetDestination(moveDestination);
        }
        else
        {
            agent.destination = transform.position;//Если джойстик не нажат, останавливаем агента
        }
    }
}
