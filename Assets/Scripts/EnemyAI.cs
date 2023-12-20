using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent _enemyAgent;
    private FieldView _enemyFieldView;
    private MeshRenderer _enemyRenderer;
    private bool _isTargetFound = false;
    private Vector3 _currentTarget = Vector3.zero;
    private EnemyStates _currentState = EnemyStates.Idle;
    
    
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
                    StartCoroutine( IdleState());
                    break;
                case EnemyStates.Chase:
                    StartCoroutine( Chase());
                    break;
                case EnemyStates.Attack:
                    StartCoroutine( Attack());
                    break;
            }
        }
    }

     void Awake()
    {
        _enemyAgent = GetComponent<NavMeshAgent>();
        _enemyFieldView = GetComponent<FieldView>();
        _enemyRenderer = GetComponent<MeshRenderer>();
    }

     void Start()
    {
        currentState = EnemyStates.Idle;
    }

    

    private IEnumerator Attack()
    {
        while (_currentState == EnemyStates.Attack)
        {
            _enemyAgent.SetDestination(_enemyFieldView.lastSeenPlayer.position);
            _enemyAgent.isStopped = false;
            
            while (_enemyAgent.pathPending)
                yield return null;
            
            if (_enemyAgent.remainingDistance <= _enemyAgent.stoppingDistance)
            {
                _enemyRenderer.material.color = Color.blue;
            }
            
            if (_enemyAgent.remainingDistance > _enemyAgent.stoppingDistance)
            {
                currentState = EnemyStates.Chase;
                _enemyRenderer.material.color = Color.red;
                _enemyAgent.isStopped = true;
                yield break;
            }
            
            yield return null;
        }
    }

    private IEnumerator Chase()
    {
        while (_currentState == EnemyStates.Chase)
        {
            _enemyFieldView.enemyAwarnees = FieldView.Eneny_Visual_Sinsitivity.LOOSE;
            
            _enemyAgent.SetDestination(_enemyFieldView.lastSeenPlayer.position);
            _enemyAgent.isStopped = false;
            
            while (_enemyAgent.pathPending)
                yield return null;
            
            if (_enemyAgent.remainingDistance <= _enemyAgent.stoppingDistance)
            {
                currentState = EnemyStates.Attack;
                _enemyAgent.isStopped = true;
                yield break;
            }
            
            if (!_enemyFieldView.isPlayerSeen)
            {
                currentState = EnemyStates.Idle;
                _enemyAgent.isStopped = true;
                yield break;
            }
            
            yield return null;
        }
    }

   private IEnumerator IdleState()
    {
        while (_currentState == EnemyStates.Idle)
        {
            _enemyFieldView.enemyAwarnees = FieldView.Eneny_Visual_Sinsitivity.STRICT;   
            Vector3 randomTarget = !_isTargetFound ? new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)) : _currentTarget ;
            
            _enemyAgent.SetDestination(randomTarget);
            
            _enemyAgent.isStopped = false;
    
            while (_enemyAgent.pathPending)
                yield return null;

            _isTargetFound = true;
            _currentTarget = randomTarget;
            
            if (_enemyFieldView.isPlayerSeen)
            {
                currentState = EnemyStates.Chase;
                _enemyAgent.isStopped = true;
                yield break;
            }

            if (_enemyAgent.remainingDistance <= _enemyAgent.stoppingDistance)
            {
                _isTargetFound = false;
            }

            yield return null;
        }
    }

}
