using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(Navigator))]
public class Loot : MonoBehaviour
{
    [SerializeField]
    private MultiBeeTriggerableObject _triggerableObject;
    NavMeshAgent _navMeshAgent;
    Navigator _navigator;

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navigator = GetComponent<Navigator>();
        _triggerableObject = GetComponent<MultiBeeTriggerableObject>();


        _triggerableObject.reachedRequiredNumberOfBees += ReturnToHive;
        _navigator.onArrived += ReachedHive;
        _triggerableObject.beesReleased += _triggerableObject_beesReleased;
    }

    private void _triggerableObject_beesReleased(List<WorkerBee> bees)
    {
        _navigator.SetTarget(transform.position);
    }

    private void ReturnToHive()
    {
        _navigator.SetTarget(Hive.instance.TargetPosition.transform.position);
    }

    private void ReachedHive(Navigator navigator)
    {
        Debug.Log("Reached hive");
        _triggerableObject.ReleaseBees();
        StartCoroutine(WaitToSetInactive());
    }

    IEnumerator WaitToSetInactive()
    {
        yield return new WaitForEndOfFrame();
        this.gameObject.SetActive(false);
    }

}
