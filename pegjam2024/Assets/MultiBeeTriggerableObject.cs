using System.Collections;
using System.Collections.Generic;
using UnityEngine.Splines;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System;


public class MultiBeeTriggerableObject : MonoBehaviour
{
    [SerializeField]
    SplineContainer _splineContainer;

    public delegate void ReachedThresholdEvent();
    public event ReachedThresholdEvent reachedRequiredNumberOfBees;

    List<WorkerBee> _workerList = new List<WorkerBee>();

    [SerializeField]
    private int NumOfBeesRequiredToCarry = 15;

    private int attatchedBeeCount = 0;


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
            bee.Detatch();
            
        }
        _workerList.Clear();
        attatchedBeeCount = 0;
    }

    public bool full()
    {
        return attatchedBeeCount == NumOfBeesRequiredToCarry;
    }
}
