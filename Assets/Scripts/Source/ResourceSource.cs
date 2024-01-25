using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSource : MonoBehaviour
{
    public ResourceSourceSO data;//SO � ������� ��������� ��� ����������� ����������
    [HideInInspector]
    public bool readyToMine;//����� �� �������� ��������
    private GameObject lastStageVisual;//3D ������ ��������� ������� ��� �� �����
    private int lightStage;
    private int currentStage;
    private new Collider collider;
    private DropResurces dropResurces;
    private void Start()
    {
        if (GetComponent<DropResurces>())
        {
            dropResurces = GetComponent<DropResurces>();
        }
        collider = GetComponent<Collider>();
        RespawnSourse();
    }

    private void RespawnSourse()
    {
        collider.enabled = true;
        lightStage = data.stages.Length;
        currentStage = 1;
        readyToMine = true;
        lastStageVisual = Instantiate(data.stages[0].visualDisplayStage, transform.GetChild(0).position, Quaternion.identity, transform);
    }

    private void NextStage(GameObject target, GameObject whoMine)//���� ������ ����� � ������� ����� �������
    {
        if (target == gameObject)
        {
            
            collider.enabled = false;//��������� ����� ������ �� �������
            dropResurces.CreateResource(data.resourceType, transform.GetChild(0).position, whoMine.transform, data.resourcePerHit, 0.5f);//���������� ������
            transform.GetChild(1).DOPunchScale(transform.localScale * (1 - data.animationForce), 1f,1)
             .OnComplete(() =>
             {

                 Destroy(lastStageVisual);//���������� ������ ��������
                 if (currentStage >= lightStage)//���� ���������� ������ ���������� ��� �������������
                 {
                    StartCoroutine(RecoveryMine());
                 }
                 else//���� ��� �� �������� ������ ������
                 {
                     lastStageVisual = Instantiate(data.stages[currentStage].visualDisplayStage, transform.GetChild(0).position, Quaternion.identity, transform);
                     currentStage++;
                     collider.enabled = true;//����� ���������
                 }

             });

        }
    }

    private IEnumerator RecoveryMine()
    {
        yield return new WaitForSeconds(data.fullRecoveryTime);
        RespawnSourse();
    }

    private void OnEnable()
    {
        PlayerAnimation.onHit += NextStage;
    }
    private void OnDisable()
    {
        PlayerAnimation.onHit -= NextStage;
    }
}
