using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public static NetworkManager Instance { get; private set; }
    public NetworkRunner SessionRunner { get; private set; }
    
    [SerializeField] private GameObject _runnerPrefab;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    
    async void Start()
    {
        CreateRunner();

        await Connect();
    }

    private async Task Connect()
    {
        var args = new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = "TestSession",
            SceneManager = GetComponent<NetworkSceneManagerDefault>()
        };

        var result = await SessionRunner.StartGame(args);

        if (result.Ok)
        {
            print("startGame Successfull");
        }
        else
        {
            print(result.ErrorMessage);
        }

    }

    private void CreateRunner()
    {
        SessionRunner = Instantiate(_runnerPrefab, transform).GetComponent<NetworkRunner>();
        
        SessionRunner.AddCallbacks(this);
    }


    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        print("new player joined the session");
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        print("Runner shutdown");
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
}
