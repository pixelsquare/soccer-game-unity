using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;

namespace SoccerGame
{
    public class ConnectionCallbackHandler : IConnectionCallbacks, IMatchmakingCallbacks, IOnEventCallback
    {
        private QuantumLoadBalancingClient _client = null;

        public ConnectionCallbackHandler(QuantumLoadBalancingClient client)
        {
            _client = client;
            _client?.AddCallbackTarget(this);
        }

        ~ConnectionCallbackHandler()
        {
            _client?.RemoveCallbackTarget(this);
            _client = null;
        }

        #region IConnectionCallbacks

        public void OnConnected()
        {
            Debug.Log("OnConnected");
        }

        public void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");

            if (!string.IsNullOrEmpty(_client.CloudRegion))
            {
                Debug.Log($"Connected to master server in region `{_client.CloudRegion}`");
            }
            else
            {
                Debug.Log($"Connected to master server in `{_client.MasterServerAddress}`");
            }

            Debug.Log($"UserId: `{_client.UserId}`");
            ConnectionManager.Instance.JoinRandomRoom();
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
            Debug.Log("OnCustomAuthenticationFailed");
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            Debug.Log("OnCustomAuthenticationResponse");
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"OnDisconnected: {cause}");

            switch (cause)
            {
                case DisconnectCause.DisconnectByClientLogic:
                    break;

                default:
                    break;
            }

            ConnectionManager.Instance.StopQuantumGame();
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
            Debug.Log("OnRegionListReceived");
        }

        #endregion // IConnectionCallbacks

        #region IMatchmakingCallbacks

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            Debug.Log("OnFriendListUpdate");
        }

        public void OnCreatedRoom()
        {
            Debug.Log("OnCreatedRoom");
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("OnCreateRoomFailed");
        }

        public void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");
            Debug.Log($"Entered room `{_client.CurrentRoom.Name}` as actor `{_client.LocalPlayer}`");
            Debug.Log($"PlayerCount: `{_client.CurrentRoom.PlayerCount}/{_client.CurrentRoom.MaxPlayers}`");
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRoomFailed");

            if (returnCode != ErrorCode.NoRandomMatchFound)
            {
                return;
            }

            ConnectionManager.Instance.CreateRoom();
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed");
        }

        public void OnLeftRoom()
        {
            Debug.Log("OnLeftRoom");
        }

        #endregion // IMatchmakingCallbacks

        #region IOnEventCallback

        public void OnEvent(EventData photonEvent)
        {
            // Debug.Log($"Photon Event Code: {photonEvent.Code}");

            switch (photonEvent.Code)
            {
                case EventCode.Join:
                {
                    ConnectionManager.Instance.StartGame();
                    break;
                }
                case CustomPhotonEventCode.StartGame:
                {
                    ConnectionManager.Instance.StartQuantumGame();
                    break;
                }
            }
        }

        #endregion // IOnEventCallback
    }
}
