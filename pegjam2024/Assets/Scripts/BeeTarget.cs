using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BeeTarget : MonoBehaviour
{
    [Serializable]
    public enum Type
    {
        Flower, Jelly
    }

    [SerializeField]
    Type _type;

    public Type type { get { return _type; } }
}
