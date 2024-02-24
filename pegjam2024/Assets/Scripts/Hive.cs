using UnityEngine;

class Hive : MonoBehaviour
{
    static public Hive instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }
}