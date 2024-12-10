using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class NetworkRunnerHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkRunner _runnerPrefab;
    NetworkRunner _currentRunner;

    //Se va a llamar cuando la conexion al Lobby sea exitosa
    //(Se va a registrar el Main Menu para cambiar del
    //panel de "Conectando..." al panel con el buscador de sesiones)
    public event Action OnJoinedLobby;

    //Se va a registrar aquel que quiera recibir la lista de sesiones actualizada
    public event Action<List<SessionInfo>> OnSessionListUpdate;


    #region Lobby

    public void JoinLobby()
    {
        if (_currentRunner) Destroy(_currentRunner.gameObject);

        _currentRunner = Instantiate(_runnerPrefab);

        _currentRunner.AddCallbacks(this);

        var clientTask = JoinLobbyTask();
    }

    async Task JoinLobbyTask()
    {
        var result = await _currentRunner.JoinSessionLobby(SessionLobby.Custom, "Normal Lobby");

        if (!result.Ok)
        {
            Debug.LogError("[Custom Error] Unable to Join Lobby");
        }
        else
        {
            Debug.Log("[Custom Msg] Lobby joined");

            OnJoinedLobby?.Invoke();
        }
    }

    #endregion

    #region Host/Join Session

    public void CreateSession(string sessionName, string sceneName)
    {
        var clientTask = InitializeSession(_currentRunner, GameMode.Host, sessionName, 
                                            SceneUtility.GetBuildIndexByScenePath($"Scenes/{sceneName}"));
    }

    public void JoinSession(SessionInfo sessionInfo)
    {
        var clientTask = InitializeSession(_currentRunner, GameMode.Client, sessionInfo.Name, 
                                            SceneManager.GetActiveScene().buildIndex);
    }

    async Task InitializeSession(NetworkRunner runner, GameMode gameMode, string sessionName, SceneRef scene)
    {
        var sceneManager = runner.GetComponent<NetworkSceneManagerDefault>();

        var result = await runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Scene = scene,
            SessionName = sessionName,
            CustomLobbyName = "Normal Lobby",
            SceneManager = sceneManager
        });

        if (!result.Ok)
        {
            Debug.LogError("[Custom Error] Unable to Start Game");
        }
        else
        {
            Debug.Log("[Custom Msg] Game Started");
        }
    }

    #endregion

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        OnSessionListUpdate?.Invoke(sessionList);

        // //Si hay alguna sesion ya creada
        // if (sessionList.Count > 0)
        // {
        //     //Recorremos cada una
        //     foreach (var session in sessionList)
        //     {
        //         //Preguntamos si la cantidad de jugadores conectados a esa sala
        //         //es menor a la cantidad maxima que pueden conectarse
        //         if (session.PlayerCount < session.MaxPlayers)
        //         {
        //             //Nos conectamos
        //             JoinSession(session);
        //
        //             return;
        //         }
        //     }
        // }
        //
        // //Sino creamos una sala
        // CreateSession("Sarasa", "Game");
    }

    #region Unused Callbacks
    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnDisconnectedFromServer(NetworkRunner runner) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnInput(NetworkRunner runner, NetworkInput input) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    #endregion
}
