using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerMovment : NetworkBehaviour
{
    public float moveSpeed;
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        
        if (GetInput<PlayerInputData>(out var inputData))
        {
            transform.Translate(inputData.Direction * Runner.DeltaTime * moveSpeed);
        }
    }
}
