using UnityEngine;

class Hive : MonoBehaviour
{
    static public Hive instance { get; private set; }

    [SerializeField]
    private Transform targetPosition;
    public Transform TargetPosition { get { return targetPosition; } }

    private void Awake()
    {
        instance = this;
    }
}