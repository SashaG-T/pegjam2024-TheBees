using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Navigator))]
public class WorkerBee : MonoBehaviour
{
    [SerializeField]
    GameObject _pollenGameObjects;

    Navigator _navigator;
    Coroutine _queenCoroutine;

    enum State : int
    {
        Idle        = 0, //not doing anything
        Queen       = 1, //following queen
        Harvesting  = 2, //heading towards a flower
        Pollen      = 3, //returning pollen to hive
        Jelly       = 4 //carry jelly back to hive
    }
    State _state = State.Idle;

    Action<State>[] _enterState;
    Action<State>[] _exitState;

    WaitForSeconds _sleep = new WaitForSeconds(1);

    void Start()
    {
        _navigator = GetComponent<Navigator>();
        _pollenGameObjects.SetActive(false);
        _enterState = new Action<State>[]
        {
            null,
            _onQueenEnter,
            _onHarvestingEnter,
            _onPollenEnter,
            _onJellyEnter
        };
        _exitState = new Action<State>[]
        {
            null,
            _onQueenExit,
            null,
            null,
            null
        };

        SetState(State.Queen);
    }

    void _onQueenEnter(State prevState)
    {
        _queenCoroutine = StartCoroutine(_queenUpdate());
    }

    IEnumerator _queenUpdate()
    {
        for (; ; )
        {
            _navigator.SetTarget(Player.instance.musterPoint);
            yield return _sleep;
        }
    }

    void _onQueenExit(State nextState)
    {
        StopCoroutine(_queenCoroutine);
        _queenCoroutine = null;
    }

    void _onHarvestingEnter(State prevState)
    {

    }

    void _onPollenEnter(State prevState)
    {
        _pollenGameObjects.SetActive(true);
        _pollenGameObjects.transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);

        _navigator.SetTarget(Hive.instance.transform.position);
    }

    void _onPollenExit(State nextState)
    {
        _pollenGameObjects.SetActive(false);
    }

    void _onJellyEnter(State prevState)
    {

    }

    void SetState(State state)
    {
        var prevState = _state;
        _state = state;
        _exitState[(int)prevState]?.Invoke(state);
        _enterState[(int)state]?.Invoke(prevState);
    }
}
