using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviourPunCallbacks
{

    [SerializeField] private Button startButton;
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject playerPrefab;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        GameData.playerName = playerNameInputField.text;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.IsOpen = true;
        options.IsVisible = true;
        options.MaxPlayers = 4;

        PhotonNetwork.JoinOrCreateRoom("Room", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(sceneName);
        }
    }
}
