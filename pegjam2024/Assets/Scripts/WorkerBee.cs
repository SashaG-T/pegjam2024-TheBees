using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Serialization;

[RequireComponent(typeof(Navigator))]
public class WorkerBee : MonoBehaviour
{
    [SerializeField]
    GameObject _pollenGameObjects;

    private GameObject reachedTarget;

    Navigator _navigator;
    static uint beeCount = 0;

    enum State : int
    {
        Idle        = 0, //not doing anything
        Queen       = 1, //following queen
        Harvesting  = 2, //heading towards a flower or jelly
        Pollen      = 3, //returning pollen to hive
        Jelly       = 4 //carry jelly back to hive
    }
    State _state = State.Idle;

    Action<State>[] _enterState;
    Action<State>[] _exitState;

    void Start()
    {
        _navigator = GetComponent<Navigator>();
        _pollenGameObjects.SetActive(false);
        _enterState = new Action<State>[]
        {
            null,
            _onQueenEnter,
            null,
            _onPollenEnter,
            _onJellyEnter
        };
        _exitState = new Action<State>[]
        {
            null,
            _onQueenExit,
            null,
            _onPollenExit,
            null
        };

        beeCount++;
        if(beeCount >= Player.MaxBeeCount)
        {
            Destroy(gameObject);
        } else
        {
            SetState(State.Queen);
        }
    }

    void _onQueenEnter(State prevState)
    {
        Player.instance.QueueWorker(this);
    }

    void _onQueenExit(State nextState)
    {

    }

    void _onPollenEnter(State prevState)
    {
        _pollenGameObjects.SetActive(true);
        _pollenGameObjects.transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);

        _navigator.SetTarget(Hive.instance.transform.position);
    }

    void reproduce()
    {
        if (beeCount < Player.MaxBeeCount)
        {
            Instantiate(gameObject, transform.parent);
        }
    }

    void _onPollenExit(State nextState)
    {
        _pollenGameObjects.SetActive(false);
        reproduce();
    }

    void _onJellyEnter(State prevState)
    {
        if(reachedTarget.TryGetComponent<MultiBeeTriggerableObject>( out MultiBeeTriggerableObject lootComponent))
        {
            lootComponent.QueueWorker(this);
        }
        else
        {
            Debug.Log("Jelly does not have loot component");
        }
        Debug.Log("Entered jelly");
    }

    void SetState(State state)
    {
        var prevState = _state;
        _state = state;
        _exitState[(int)prevState]?.Invoke(state);
        _enterState[(int)state]?.Invoke(prevState);
    }

    public void SetTarget(GameObject target)
    {
        _navigator.SetTarget(target.transform.position);
        SetState(State.Harvesting);
    }

    public void SetRank(Vector3 rank)
    {
        _navigator.SetTarget(rank);
    }

    public void ResetBee()
    {
        SetState(State.Queen);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<BeeTarget>() is BeeTarget beeTarget)
        {
            reachedTarget = other.gameObject;
            switch(beeTarget.type)
            {
                case BeeTarget.Type.Flower:
                {
                    SetState(State.Pollen);
                    break;
                }
                case BeeTarget.Type.Jelly:
                {
                    SetState(State.Jelly);
                    break;
                }
                default:
                {
                    Debug.LogError("Type doesn't exist?");
                    break;
                }
            }
        } else if(other.GetComponent<Hive>() is Hive hive)
        {
            SetState(State.Queen);
        }
    }
}
