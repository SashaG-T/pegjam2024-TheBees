using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    InputActionAsset _inputActionAsset;
    InputActionMap _inputActionMap;
    InputAction _tapAction;
    InputAction _holdAction;

    private void Start()
    {
        _inputActionMap = _inputActionAsset.FindActionMap("Map", true);
        _tapAction = _inputActionMap.FindAction("Tap", true);
        _holdAction = _inputActionMap.FindAction("Hold", true);
        _tapAction.performed += onTapActionPerformed;
        _holdAction.performed += onHoldPerformed;
        _inputActionMap.Enable();
    }

    private void onTapActionPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Tap");
    }

    private void onHoldPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Hold");
    }
}
