using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerInput : MonoBehaviour, INetworkRunnerCallbacks
{
     [SerializeField] private InputActionReference _moveInput;

    private void OnEnable()
    {
        _moveInput.action?.Enable();
    }

    private void OnDisable()
    {
        _moveInput.action?.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Instance.SessionRunner.AddCallbacks(this);
    }
    
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        Vector2 dir = _moveInput.action.ReadValue<Vector2>();
        Vector3 direction = new Vector3(dir.x, 0, dir.y);
        PlayerInputData inputData = new PlayerInputData();
        
        
        inputData.Direction =direction;

        input.Set(inputData);
    }
    
    #region unusedCallBacks
    
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        
    }
    
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }
    #endregion
    
}

public struct PlayerInputData : INetworkInput
{
    public Vector3 Direction;
}
