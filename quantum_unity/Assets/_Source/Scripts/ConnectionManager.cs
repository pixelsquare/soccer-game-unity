using Photon.Deterministic;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;
using Quantum;
using System;

namespace SoccerGame
{
    public class ConnectionManager : SingletonMonobehavior<ConnectionManager>
    {
        [SerializeField] private bool _isDebugMode = false;
        [SerializeField] private RuntimeConfig _runtimeConfig = null;

        private EnterRoomParams _enterRoomParams = null;

        public static QuantumLoadBalancingClient Client { get; private set; }

        private const int MaxPlayerCount = 4;

        public void Update()
        {
            Client?.Service();
        }

        public void OnGUI()
        {
            if (!_isDebugMode)
            {
                return;
            }

            if (GUILayout.Button("Connect"))
            {
                ConnectToServer();
            }

            if (GUILayout.Button("Disconnect"))
            {
                DisconnectToServer();
            }
        }

        public void ConnectToServer()
        {
            var appSettings = PhotonServerSettings.CloneAppSettings(PhotonServerSettings.Instance.AppSettings);
            Client = new QuantumLoadBalancingClient(appSettings.Protocol);

            Debug.Log($"Using region `{appSettings.FixedRegion}`");
            Debug.Log($"Using app version `{appSettings.AppVersion}`");

            if (!Client.ConnectUsingSettings(appSettings))
            {
                Debug.LogError($"Failed to connect with app settings: `{appSettings}`");
                return;
            }

            Debug.Log("Connection to server successful!");
            _ = new ConnectionCallbackHandler(Client);
        }

        public void DisconnectToServer()
        {
            Client?.Disconnect();
        }

        public void JoinRandomRoom()
        {
            var joinRandomRoomParams = new OpJoinRandomRoomParams();

            var roomOps = new RoomOptions()
            {
                IsVisible = true,
                MaxPlayers = MaxPlayerCount,
                Plugins = new string[] { "QuantumPlugin" },
                PlayerTtl = PhotonServerSettings.Instance.PlayerTtlInSeconds * 1000,
                EmptyRoomTtl = PhotonServerSettings.Instance.EmptyRoomTtlInSeconds * 1000
            };

            _enterRoomParams = new EnterRoomParams()
            {
                RoomOptions = roomOps
            };

            if (!Client.OpJoinRandomOrCreateRoom(joinRandomRoomParams, _enterRoomParams))
            {
                Debug.LogError("Failed to send join random room operation.");
                return;
            }

            Debug.Log("Join random room operation successful!");
        }

        public void CreateRoom()
        {
            if (!Client.OpCreateRoom(_enterRoomParams))
            {
                Debug.LogError("Failed to create room operation.");
                return;
            }
        }

        public void StartGame()
        {
            if (Client != null 
                && Client.InRoom 
                && Client.LocalPlayer.IsMasterClient 
                && Client.CurrentRoom.IsOpen)
            {
                if (!Client.OpRaiseEvent(CustomPhotonEventCode.StartGame, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable))
                {
                    Debug.LogError("Failed to broadcast start game event.");
                }
            }
        }

        public void StartQuantumGame()
        {
            if (QuantumRunner.Default != null)
            {
                Debug.LogWarning($"Another QuantumRunner `{QuantumRunner.Default.name}` has prevented starting this game.");
                return;
            }

            var runtimeConfig = _runtimeConfig != null
                              ? RuntimeConfig.FromByteArray(RuntimeConfig.ToByteArray(_runtimeConfig))
                              : new RuntimeConfig();

            var startParams = new QuantumRunner.StartParameters
            {
                RuntimeConfig               = runtimeConfig,
                DeterministicConfig         = DeterministicSessionConfigAsset.Instance.Config,
                ReplayProvider              = null,
                GameMode                    = DeterministicGameMode.Multiplayer,
                FrameData                   = null,
                InitialFrame                = 0,
                PlayerCount                 = MaxPlayerCount,
                LocalPlayerCount            = 1,
                RecordingFlags              = RecordingFlags.None,
                NetworkClient               = Client,
                StartGameTimeoutInSeconds   = 10f
            };

            Debug.Log($"Starting QuantumRunner with map guid `{runtimeConfig.Map.Id} and requesting `{startParams.LocalPlayerCount}` player(s).");

            var idProvider = Guid.NewGuid().ToString();
            QuantumRunner.StartGame(idProvider, startParams);
        }

        public void StopQuantumGame()
        {
            QuantumRunner.ShutdownAll(true);
        }
    }
}
