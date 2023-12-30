using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFieldOfView : MonoBehaviour
{
    public bool isplayerSeen = false;
    public Transform lastSeenPlayerPosition;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isplayerSeen = true;
            lastSeenPlayerPosition = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isplayerSeen = false;
            lastSeenPlayerPosition = other.transform;
        }
    }
}
