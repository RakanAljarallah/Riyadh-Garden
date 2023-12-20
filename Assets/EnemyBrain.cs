using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyBrain : MonoBehaviour
{
    private NavMeshAgent _enemyAgent;
    private EnemyField _enemyFieldView;
    private EnemyStates _currentState;
    private Vector3 _enemyDistination;
    private bool _isDistnationFound = false;
    private MeshRenderer _enemyRender;

    public enum EnemyStates
    {
        Idle,
        Chase,
        Attack
    }

    public EnemyStates currentState
    {
        get { return _currentState;}
        set
        {
            _currentState = value;
            
            StopAllCoroutines();
            
            switch (_currentState)
            {
                case EnemyStates.Idle:
                    StartCoroutine(Idle());
                    break;
                case EnemyStates.Chase:
                    StartCoroutine(Chase());
                    break;
                case EnemyStates.Attack:
                    StartCoroutine(Attack());
                    break;
            }
        }
    }

    private void Awake()
    {
        _enemyAgent = GetComponent<NavMeshAgent>();
        _enemyFieldView = GetComponent<EnemyField>();
        _enemyRender = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        currentState = EnemyStates.Idle;
    }

    IEnumerator Idle()
    {
        while (_currentState == EnemyStates.Idle)
        {
            _enemyFieldView.currentSensitivity = EnemyField.Enemy_Sensitivity.STRICT;
            
            float randomX = Random.Range(30f,45f);
            float randomZ = Random.Range(30f,45f);
            Vector3 randTarget = _isDistnationFound ? _enemyDistination : new Vector3(randomX, 0f, randomZ);
            
            _enemyAgent.SetDestination(randTarget);

            _enemyAgent.isStopped = false;

            while (_enemyAgent.pathPending)
            {
                yield return null;
            }
            _isDistnationFound = true;
            _enemyDistination = randTarget;

            if (_enemyAgent.remainingDistance <= _enemyAgent.stoppingDistance)
            {
                _isDistnationFound = false;
            }

            if (_enemyFieldView.isPlayerSeen)
            {
                currentState = EnemyStates.Chase;
                _enemyAgent.isStopped = true;
                yield break;
            }
            
            yield return null;
        }
    }
    
    IEnumerator Chase()
    {
        while (_currentState == EnemyStates.Chase)
        {
            _enemyFieldView.currentSensitivity = EnemyField.Enemy_Sensitivity.LOOSE;
            
            _enemyAgent.SetDestination(_enemyFieldView.lastSeenPlayer.position);
            
            _enemyAgent.isStopped = false;

            while (_enemyAgent.pathPending)
            {
                yield return null;
            }
            
            if (_enemyAgent.remainingDistance <= _enemyAgent.stoppingDistance)
            {
                currentState = EnemyStates.Attack;
                _enemyAgent.isStopped = true;
                yield break;
            }
            
            if (!_enemyFieldView.isPlayerSeen)
            {
                currentState = EnemyStates.Idle;
                _enemyRender.material.color = Color.red;
                _enemyAgent.isStopped = true;
                yield break;
            }
            yield return null;
        }
    }
    
    IEnumerator Attack()
    {
        while (_currentState == EnemyStates.Attack)
        {
            _enemyAgent.SetDestination(_enemyFieldView.lastSeenPlayer.position);
            
            _enemyAgent.isStopped = false;

            while (_enemyAgent.pathPending)
            {
                yield return null;
            }

            if(_enemyAgent.remainingDistance <= _enemyAgent.stoppingDistance)
            {
                _enemyRender.material.color = Color.blue;
                yield return null;
            }
            
            if (_enemyAgent.remainingDistance > _enemyAgent.stoppingDistance)
            {
                currentState = EnemyStates.Chase;
                _enemyRender.material.color = Color.yellow;
                yield break;
            }
            
            yield return null;
        }
    }


}
