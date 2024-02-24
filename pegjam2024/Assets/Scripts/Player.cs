using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Navigator))]
public class Player : MonoBehaviour
{
    static public Player instance { get; private set; }

    [SerializeField]
    InputActionAsset _inputActionAsset;
    InputActionMap _inputActionMap;
    InputAction _tapAction;
    InputAction _holdAction;
    InputAction _positionAction;

    [SerializeField]
    Camera _camera;
    [SerializeField]
    GameObject _pointer;

    [SerializeField]
    Coroutine _holdCoroutine;

    Navigator _navigator;

    [SerializeField, Range(0.0f, 5.0f)]
    float _musterDistance = 2.0f;
    
    Vector2 pointerPosition => _positionAction.ReadValue<Vector2>();
    public Vector3 musterPoint {
        get
        {
            return transform.position - transform.forward * _musterDistance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _inputActionMap = _inputActionAsset.FindActionMap("Map", true);
        _tapAction = _inputActionMap.FindAction("Tap", true);
        _holdAction = _inputActionMap.FindAction("Hold", true);
        _positionAction = _inputActionMap.FindAction("Position", true);
        _tapAction.performed += onTapActionPerformed;
        _holdAction.started += onHoldStarted;
        _holdAction.canceled += onHoldCanceled;
        _inputActionMap.Enable();

        _navigator = GetComponent<Navigator>();
    }

    private void onTapActionPerformed(InputAction.CallbackContext ctx)
    {
        //deploy a bee!
    }

    private void onHoldStarted(InputAction.CallbackContext ctx)
    {
        if(_holdCoroutine == null)
        {
            _holdCoroutine = StartCoroutine(_updatePointerCoroutine());
        }
    }

    private void onHoldCanceled(InputAction.CallbackContext ctx)
    {
        StopCoroutine(_holdCoroutine);
        _holdCoroutine = null;
    }

    private void setPointer(Vector3 position)
    {
        _pointer.SetActive(true);
        _pointer.transform.position = position;
        _navigator.SetTarget(position);
    }

    private void doRaycast()
    {
        Ray ray = _camera.ScreenPointToRay(pointerPosition);
        if(Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            setPointer(hitInfo.point);
        }
    }

    IEnumerator _updatePointerCoroutine()
    {
        for (; ; )
        {
            doRaycast();
            yield return null;
        }
    }
}
