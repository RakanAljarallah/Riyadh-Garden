using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyStateMachine : MonoBehaviour
{
    private NavMeshAgent _enemyAgent;
    private EnemyFieldOfView _enemyField;
    private Enemny_States _currentState;
    private MeshRenderer _enemyMaterial;
    private bool _isTargetFound = false;
    private Vector3 _targetPostion = Vector3.zero;

    public Enemny_States currentState
    {
        get { return _currentState; }
        set
        {
            _currentState = value;
            
            StopAllCoroutines();
            
            switch (_currentState)
            {
                case Enemny_States.Idle:
                    StartCoroutine(Idle());
                    break;
                case Enemny_States.Chase:
                    StartCoroutine(Chase());
                    break;
                case Enemny_States.Attack:
                    StartCoroutine(Attack());
                    break;
            }
        }
    }
    
    
    public enum Enemny_States
    {
        Idle,
        Chase,
        Attack
    }

    private void Awake()
    {
        _enemyField = GetComponent<EnemyFieldOfView>();
        _enemyAgent = GetComponent<NavMeshAgent>();
        _enemyMaterial = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        currentState = Enemny_States.Idle;
    }


    IEnumerator Idle()
    {
        while (_currentState == Enemny_States.Idle)
        {
            float randomX = Random.Range(-5, 5);
            float randomZ = Random.Range(-5, 5);

            Vector3 randomTarget = _isTargetFound ? _targetPostion :
                                    new Vector3(randomX, 0f, randomZ);

            _enemyAgent.SetDestination(randomTarget);

            while (_enemyAgent.pathPending)
            {
                yield return null;
            }
            
            print("here");
            _isTargetFound = true;
            _targetPostion = randomTarget;
            
            if (_enemyField.isplayerSeen)
            {
                currentState = Enemny_States.Chase;
                //_enemyAgent.isStopped = true;
                yield break;
            }
            
            if (_enemyAgent.remainingDistance <= _enemyAgent.stoppingDistance)
            {
                _isTargetFound = false;
            }

            yield return null;
        }
    }
    
    IEnumerator Chase()
    {
        while (_currentState == Enemny_States.Chase)
        {
            _enemyAgent.SetDestination(_enemyField.lastSeenPlayerPosition.position);
            while (_enemyAgent.pathPending)
            {
                yield return null;
            }
            
            if (_enemyAgent.remainingDistance <= _enemyAgent.stoppingDistance)
            {
                currentState = Enemny_States.Attack;
                yield break;
            }
            
            if (!_enemyField.isplayerSeen)
            {
                currentState = Enemny_States.Idle;
                yield break;
            }
            yield return null;
        }
    }
    
    IEnumerator Attack()
    {
        while (_currentState == Enemny_States.Attack)
        {
            
            _enemyAgent.SetDestination(_enemyField.lastSeenPlayerPosition.position);
            
            while (_enemyAgent.pathPending)
            {
                yield return null;
            }
            
            if (_enemyAgent.remainingDistance <= _enemyAgent.stoppingDistance)
            {
                _enemyMaterial.material.color = Color.black;
                yield return null;
            }
            if (_enemyAgent.remainingDistance > _enemyAgent.stoppingDistance)
            {
                currentState = Enemny_States.Chase;
                _enemyMaterial.material.color = Color.red;
                yield break;
            }
            
            yield return null;
        }
    }
}
