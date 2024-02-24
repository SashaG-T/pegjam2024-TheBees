using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using UnityEngine.UIElements;

[RequireComponent(typeof(Navigator))]
public class Player : MonoBehaviour
{
    static public Player instance { get; private set; }
    public const uint MaxBeeCount = 100;

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
    int _layerMask;

    [SerializeField]
    SplineContainer _splineContainer;
    Coroutine _workerCoroutine;
    WaitForSeconds _sleep = new WaitForSeconds(1);

    Queue<WorkerBee> _workerQueue = new Queue<WorkerBee>();
    
    Vector2 pointerPosition => _positionAction.ReadValue<Vector2>();
    WorkerBee nextBee => _workerQueue.Count > 0 ? _workerQueue.Dequeue() : null;

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
        _holdAction.performed += onHoldStarted;
        _holdAction.canceled += onHoldCanceled;
        _inputActionMap.Enable();

        _navigator = GetComponent<Navigator>();
        _layerMask = LayerMask.GetMask(new string[] { "BeeTarget" });
        _navigator.onArrived += _navigator_onArrived;
    }

    private void _navigator_onArrived(Navigator navigator)
    {
        _pointer.SetActive(false);
    }

    public void QueueWorker(WorkerBee workerBee)
    {
        if(_workerCoroutine == null)
        {
            _workerCoroutine = StartCoroutine(_workerUpdateCoroutine());
        }
        _workerQueue.Enqueue(workerBee);
    }

    private void onTapActionPerformed(InputAction.CallbackContext ctx)
    {
        //deploy a bee!
        Ray ray = _camera.ScreenPointToRay(pointerPosition);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, 100, _layerMask))
        {
            nextBee?.SetTarget(hitInfo.collider.gameObject);
        }
        if(_workerQueue.Count == 0)
        {
            if (_workerCoroutine != null)
            {
                StopCoroutine(_workerCoroutine);
                _workerCoroutine = null;
            }
        }
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
        if (_holdCoroutine != null)
        {
            StopCoroutine(_holdCoroutine);
            _holdCoroutine = null;
        }
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

    IEnumerator _workerUpdateCoroutine()
    {
        for(; ;)
        {
            Spline spline = _splineContainer.Splines[0];
            float step = 1.0f / MaxBeeCount;
            float t = 0.0f;
            foreach(WorkerBee bee in _workerQueue)
            {
                bee.SetRank(_splineContainer.transform.TransformPoint((Vector3)spline.EvaluatePosition(t)));
                t += step;
            }
            yield return _sleep;
        }
    }

}
