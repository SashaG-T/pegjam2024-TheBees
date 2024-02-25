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
    }
    private void ReturnToHive()
    {
        _navigator.SetTarget(Hive.instance.transform.position);
    }

    private void ReachedHive(Navigator navigator)
    {
        _triggerableObject.ReleaseBees();
        this.gameObject.SetActive(false);
    }

}
