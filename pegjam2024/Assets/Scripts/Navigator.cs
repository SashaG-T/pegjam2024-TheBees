using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Navigator : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;

    bool _navigating = false;
    public float _pathEndThreshold = 0.1f;

    public delegate void NavigationEvent(Navigator navigator);
    public event NavigationEvent onArrived;

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void SetTarget(Vector3 position)
    {
        if (_navMeshAgent.enabled)
        {
            try
            {
                _navMeshAgent.destination = position;
            } catch(System.Exception e)
            {
                transform.position = position;
            }
            _navigating = true;
        }
    }

    private void Update()
    {
        if(_navigating)
        {
            CheckIfComplete();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Hive.instance)
        {

        }
    }

    void CheckIfComplete()
    {
        if(_navMeshAgent.hasPath && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + _pathEndThreshold)
        {
            Debug.Log(Vector3.Distance(_navMeshAgent.destination, this.transform.position));
            if(Vector3.Distance(_navMeshAgent.destination, this.transform.position) <= 0.5)
            {
                onArrived?.Invoke(this);
                _navigating = false;
            }

        }
    }

}
