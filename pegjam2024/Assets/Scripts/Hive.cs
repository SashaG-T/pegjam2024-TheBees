using UnityEngine;

class Hive : MonoBehaviour
{
    static public Hive instance { get; private set; }
    static public GameObject WorkerBeeInstance { get; private set; }
    [SerializeField]
    private GameObject workerBeeInstance;

    [SerializeField]
    private Transform targetPosition;
    public Transform TargetPosition { get { return targetPosition; } }

    private void Awake()
    {
        instance = this;
        WorkerBeeInstance = workerBeeInstance;
    }
}