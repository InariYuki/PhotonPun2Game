using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    CanvasGroup mainCanvasGroup;
    TMP_InputField inputPlayerName;
    TMP_InputField inputCreateRoomName;
    TMP_InputField inputJoinRoomName;
    Button createRoom;
    Button joinRoom;
    Button joinRandomRoom;
    string playerName;
    string createRoomName;
    string joinRoomName;

    TextMeshProUGUI textRoomName;
    TextMeshProUGUI textPlayerCount;
    CanvasGroup roomCanvasGroup;
    Button leaveRoom;
    Button startGame;
    private void Awake()
    {
        InitLobby();
        InitRoom();
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        mainCanvasGroup.interactable = true;
        mainCanvasGroup.blocksRaycasts = true;
        print("connected !!");
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("room created");
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("room joined");
        roomCanvasGroup.alpha = 1;
        roomCanvasGroup.interactable = true;
        roomCanvasGroup.blocksRaycasts = true;
        textRoomName.text = "Room name:" + PhotonNetwork.CurrentRoom.Name;
        textPlayerCount.text = $"players:{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
    }
    void InitLobby()
    {
        inputPlayerName = GameObject.Find("��J���a�W��").GetComponent<TMP_InputField>();
        inputCreateRoomName = GameObject.Find("��J�Ыةж��W��").GetComponent<TMP_InputField>();
        inputJoinRoomName = GameObject.Find("��J�[�J�ж��W��").GetComponent<TMP_InputField>();
        createRoom = GameObject.Find("�Ыةж����s").GetComponent<Button>();
        joinRoom = GameObject.Find("�[�J���w�ж����s").GetComponent<Button>();
        joinRandomRoom = GameObject.Find("�[�J�H���ж����s").GetComponent<Button>();
        mainCanvasGroup = GameObject.Find("MainCanvas").GetComponent<CanvasGroup>();
        inputPlayerName.onEndEdit.AddListener((input) => { 
            playerName = input;
            PhotonNetwork.NickName = playerName;
        });
        inputCreateRoomName.onEndEdit.AddListener((input) => createRoomName = input);
        inputJoinRoomName.onEndEdit.AddListener((input) => joinRoomName = input);
        createRoom.onClick.AddListener(CreateRoom);
        joinRoom.onClick.AddListener(JoinRoom);
        joinRandomRoom.onClick.AddListener(JoinRandomRoom);
    }
    void InitRoom()
    {
        textRoomName = GameObject.Find("RoomName").GetComponent<TextMeshProUGUI>();
        textPlayerCount = GameObject.Find("PlayerCount").GetComponent<TextMeshProUGUI>();
        roomCanvasGroup = GameObject.Find("RoomCanvas").GetComponent<CanvasGroup>();
        leaveRoom = GameObject.Find("BackButton").GetComponent<Button>();
        startGame = GameObject.Find("StartButton").GetComponent<Button>();
        leaveRoom.onClick.AddListener(LeaveRoom);
        startGame.onClick.AddListener(() => photonView.RPC("RPCStartGame", RpcTarget.All));
    }
    void CreateRoom()
    {
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20;
        ro.IsVisible = true;
        PhotonNetwork.CreateRoom(createRoomName, ro);
    }
    void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinRoomName);
        
    }
    void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        roomCanvasGroup.alpha = 0;
        roomCanvasGroup.interactable = false;
        roomCanvasGroup.blocksRaycasts = false;
    }
    [PunRPC]
    void RPCStartGame()
    {
        PhotonNetwork.LoadLevel("�C��");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        textPlayerCount.text = $"players:{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        textPlayerCount.text = $"players:{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
    }
}
