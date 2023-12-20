using System;
using UnityEngine;


public class FieldView : MonoBehaviour
{
    private SphereCollider _enmySphereCollider;
    
    public bool isPlayerSeen = false;
    public Transform lastSeenPlayer;

    public Transform eyePoint;

    [SerializeField] private float _enemyMaxAngleView = 60f;


    private void Awake()
    {
        _enmySphereCollider = GetComponent<SphereCollider>();
    }

    public enum  Eneny_Visual_Sinsitivity
    {
        LOOSE,
        STRICT
    }

    public Eneny_Visual_Sinsitivity enemyAwarnees = Eneny_Visual_Sinsitivity.STRICT;


    bool EnemyFieldView(Vector3 eyePostion, Vector3 targetPostion)
    {
        Vector3 dir = targetPostion - eyePostion;

        float dirAngle = Vector3.Angle(eyePostion, dir);

        if (dirAngle <= _enemyMaxAngleView)
        {
            print("came here EnemyFieldView");
            return true;
        }

        return false;
    }

    bool EnemySight(Vector3 eyePostion, Vector3 targetPostion)
    {
        Vector3 dir = targetPostion - eyePostion;
        RaycastHit hit;
        if (Physics.Raycast(eyePostion, dir, out hit, _enmySphereCollider.radius))
        {
            if (hit.transform.CompareTag("Player"))
            {
                print("came here EnemySight");
                return true;

            }
        }
        return false;

    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Vector3 eyePointPosition = eyePoint.position;
            Vector3 targetPosiotion = other.transform.position;
            switch (enemyAwarnees)
            {
                case Eneny_Visual_Sinsitivity.LOOSE:
                    isPlayerSeen = EnemyFieldView(eyePointPosition, targetPosiotion) || EnemySight(eyePointPosition, targetPosiotion);
                    lastSeenPlayer = other.transform;
                    break;
                case Eneny_Visual_Sinsitivity.STRICT:
                    isPlayerSeen = EnemyFieldView(eyePointPosition, targetPosiotion) && EnemySight(eyePointPosition, targetPosiotion);
                    lastSeenPlayer = other.transform;
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            print("player run away");
            isPlayerSeen = false;
            lastSeenPlayer = other.transform;
        }
    }
}
