using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpenGate : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private NavMeshObstacle navMeshObstacle;
    [SerializeField]
    private Transform dropOffPosition;
    [SerializeField]
    private AudioSource openGate;
    [SerializeField]
    private AudioSource closeGate;

    private MultiBeeTriggerableObject triggerableObject;

    private void Awake()
    {
        triggerableObject = GetComponent<MultiBeeTriggerableObject>();
        triggerableObject.reachedRequiredNumberOfBees += SetDoorOpen;
        triggerableObject.beesReleased += SetDoorClose;
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

    private void SetDoorOpen()
    {
        Debug.Log("Opening gate");
        navMeshObstacle.enabled = false;
        animator.SetBool("Open", true);
        openGate.Play();
    }

    private void SetDoorClose(List<WorkerBee> releasedBees)
    {
        navMeshObstacle.enabled = true;
        animator.SetBool("Open", false);
        closeGate.Play();
        foreach(var b in releasedBees)
        {
            b.Move(dropOffPosition);
        }
    }
}
