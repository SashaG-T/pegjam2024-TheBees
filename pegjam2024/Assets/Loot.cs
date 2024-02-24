using System.Collections;
using System.Collections.Generic;
using UnityEngine.Splines;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


[RequireComponent(typeof(Navigator))]
public class Loot : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;
    Navigator _navigator;
    [SerializeField]
    SplineContainer _splineContainer;

    public UnityEvent _reachedRequiredNumberOfBees;

    List<WorkerBee> _workerList = new List<WorkerBee>();

    public const uint MaxBeeCount = 100;

    [SerializeField]
    private int NumOfBeesRequiredToCarry = 15;

    Coroutine _workerCoroutine;
    WaitForSeconds _sleep = new WaitForSeconds(1);

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navigator = GetComponent<Navigator>();
    }

    private void Start()
    {

    }

    public void QueueWorker(WorkerBee workerBee)
    {
        if (_workerCoroutine == null)
        {
            _workerCoroutine = StartCoroutine(_workerUpdateCoroutine());
        }
        _workerList.Add(workerBee);
        Debug.Log("Workers carrying loot, " + _workerList.Count);
        if (_workerList.Count >= NumOfBeesRequiredToCarry)
        {
            _reachedRequiredNumberOfBees.Invoke();
            _navigator.SetTarget(Hive.instance.transform.position);
        }
    }

    IEnumerator _workerUpdateCoroutine()
    {
        for (; ; )
        {
            Spline spline = _splineContainer.Splines[0];
            float step = 1.0f / MaxBeeCount;
            float t = 0.0f;
            foreach (WorkerBee bee in _workerList)
            {
                bee.SetRank(_splineContainer.transform.TransformPoint((Vector3)spline.EvaluatePosition(t)));
                t += step;
            }
            yield return _sleep;
        }
    }
}
