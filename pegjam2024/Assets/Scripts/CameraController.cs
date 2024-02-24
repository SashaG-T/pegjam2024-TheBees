using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform _target;
    Vector3 _offset;

    void Start()
    {
        _target = transform.parent;
        transform.SetParent(_target.parent);
        _offset = transform.position - _target.position;
    }

    void Update()
    {
        transform.position = _target.position + _offset;
    }
}
