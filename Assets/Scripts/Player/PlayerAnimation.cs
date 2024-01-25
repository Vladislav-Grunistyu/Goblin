using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimation : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    public static Action <GameObject, GameObject>onHit;
    private GameObject target;
    private GameObject whoMine;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float speed = agent.velocity.magnitude;
        if (speed > 0)
        {
            animator.speed = speed/2;
            animator.SetBool("RUN", speed > 0.1f);
            animator.SetBool("Idle", false);
            animator.SetBool("Attack", false);
            animator.SetFloat("AttackSpeed", 1);
        }
        else
        {
            animator.speed = 1;
            animator.SetBool("RUN", false);
            animator.SetBool("Idle", true);
        }

    }

    private void StarAttackAnimation(GameObject minedTarget, GameObject who�alled)
    {
        if (who�alled.GetComponent<PlayerMovement>() == true) //���� ����� ��
        {
            target = minedTarget;
            whoMine = who�alled;
            animator.SetFloat("AttackSpeed", minedTarget.GetComponent<ResourceSource>().data.miningSpeed);
            animator.SetBool("Attack", true);
        }
    }
    public void GetDamage()
    {
        onHit?.Invoke(target, whoMine);//������� ��� �������� ����� �����������
        animator.SetBool("Attack", false);
        target = null;
        whoMine = null;
    }
    private void OnEnable()
    {

        Mining.onAttack += StarAttackAnimation;


    }
    private void OnDisable()
    {
        Mining.onAttack -= StarAttackAnimation;
    }
}