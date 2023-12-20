using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyField : MonoBehaviour
{
    public Transform lastSeenPlayer;
    public bool isPlayerSeen;
    
    public enum Enemy_Sensitivity
    {
        LOOSE,
        STRICT
    }

    public Enemy_Sensitivity currentSensitivity = Enemy_Sensitivity.STRICT;

    [SerializeField] private float angleView = 60f;
    [SerializeField] private Transform eyePoint;
    
    
    
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
             lastSeenPlayer = other.transform;
            Vector3 eyePosition = eyePoint.position;
            switch (currentSensitivity)
            {
                case Enemy_Sensitivity.LOOSE:
                    
                    isPlayerSeen = FOV(lastSeenPlayer.position, eyePosition) || PlayerInSight(lastSeenPlayer.position,eyePosition);
                    break;
                case Enemy_Sensitivity.STRICT:
                    isPlayerSeen = FOV(lastSeenPlayer.position, eyePosition) && PlayerInSight(lastSeenPlayer.position,eyePosition);
                    break;
            }
        }
    }

    bool FOV(Vector3 target, Vector3 eyePoint)
    {
        Vector3 dirr = target - eyePoint;

        float targetAngl = Vector3.Angle(eyePoint,dirr);
    
        if(targetAngl <= angleView)
            return true;

        return false;
    }

    bool PlayerInSight(Vector3 target, Vector3 eyePoint)
    {
        RaycastHit hit;
        if (Physics.Raycast(eyePoint, target, out hit))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isPlayerSeen = false;
            lastSeenPlayer = other.transform;
        }
    }
}
