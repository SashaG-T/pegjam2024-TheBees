using System.Collections;
using System.Collections.Generic;
using UnityEngine.Splines;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System;
using Unity.VisualScripting;


public class MultiBeeTriggerableObject : MonoBehaviour
{
    [SerializeField]
    SplineContainer _splineContainer;

    public delegate void TriggerEvent();
    public delegate void TriggerEvent2(List<WorkerBee> beeList);
    public event TriggerEvent reachedRequiredNumberOfBees;
    public event TriggerEvent2 beesReleased;

    Counter _counter;


    List<WorkerBee> _workerList = new List<WorkerBee>();

    [SerializeField]
    private int NumOfBeesRequiredToCarry = 15;

    private int attatchedBeeCount = 0;

    static List<MultiBeeTriggerableObject> _allTriggers = new List<MultiBeeTriggerableObject>();

    public static void ReleaseAllBees()
    {
        foreach(var t in _allTriggers)
        {
            t.ReleaseBees();
        }
    }

    private void Start()
    {
        _allTriggers.Add(this);
        _counter = GetComponentInChildren<Counter>();
        _counter.count = 0;
        _counter.required = NumOfBeesRequiredToCarry;
    }
    private void OnDestroy()
    {
        _allTriggers.Remove(this);
    }

    public void QueueWorker(WorkerBee workerBee)
    {
        Spline spline = _splineContainer.Splines[0];
        workerBee.SetRank(_splineContainer.transform.TransformPoint((Vector3)spline.EvaluatePosition((float)_workerList.Count / NumOfBeesRequiredToCarry)));
        if (workerBee.TryGetComponent<Navigator>(out Navigator navigator))
        {
            navigator.onArrived += BeeArrivedAtPosition;
        }
        _workerList.Add(workerBee);
        Debug.Log("Workers carrying loot, " + _workerList.Count);

    }

    private void BeeArrivedAtPosition(Navigator navigator)
    {
        attatchedBeeCount++;
        _counter.count = attatchedBeeCount;
        navigator.onArrived -= BeeArrivedAtPosition;
        if (navigator.TryGetComponent<WorkerBee>(out WorkerBee workerBee)) 
        {
            workerBee.AttachTo(transform);
        }
        if (attatchedBeeCount >= NumOfBeesRequiredToCarry)
        {
            Debug.Log("Required number of bees reached");
            reachedRequiredNumberOfBees?.Invoke();
        }
    }


    public void ReleaseBees()
    {
        foreach (WorkerBee bee in _workerList)
        {
            bee.ResetBee();
            
        }
        beesReleased?.Invoke(_workerList);
        _workerList.Clear();
        attatchedBeeCount = 0;
        _counter.count = 0;
    }

    public bool full()
    {
        return attatchedBeeCount == NumOfBeesRequiredToCarry;
    }
}
