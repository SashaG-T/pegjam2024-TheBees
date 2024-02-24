using System;
using System.Collections;
using System.Collections.Generic;
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
        _navMeshAgent.destination = position;
        _navigating = true;
    }

    private void Update()
    {
        if(_navigating)
        {
            CheckIfComplete();
        }
    }

    void CheckIfComplete()
    {
        if(_navMeshAgent.hasPath && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + _pathEndThreshold)
        {
            onArrived?.Invoke(this);
            _navigating = false;
        }
    }

}
