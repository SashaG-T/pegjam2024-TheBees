using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class OpenGate : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private NavMeshObstacle navMeshObstacle;

    private MultiBeeTriggerableObject triggerableObject;

    private bool isOpen = false;

    private void Awake()
    {
        triggerableObject = GetComponent<MultiBeeTriggerableObject>();
        triggerableObject.reachedRequiredNumberOfBees += ToggleDoor;
    }

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (navMeshObstacle == null)
        {
            navMeshObstacle = GetComponent<NavMeshObstacle>();
        }
    }

    private void SetDoorOpen(bool openState)
    {
        isOpen = openState;
        navMeshObstacle.enabled = !isOpen;
        animator.SetBool("Open", isOpen);
    }

    private void ToggleDoor()
    {
        Debug.Log("Changing door state");
        isOpen = !isOpen;
        navMeshObstacle.enabled = isOpen;
        animator.SetBool("Open", isOpen);
    }
}
