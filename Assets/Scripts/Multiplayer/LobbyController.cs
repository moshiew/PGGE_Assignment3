using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class LobbyController : MonoBehaviourPunCallbacks
{
    public InputField playerNameInput;
    public Button joinRandomButton;
    public Button refreshButton;
    public Transform roomListContent;
    public GameObject roomButtonPrefab;
    public TextMeshProUGUI statusText;

    public static LobbyController Instance;

    private List<RoomInfo> roomList = new List<RoomInfo>();
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        joinRandomButton.onClick.AddListener(JoinRandomRoom);
        refreshButton.onClick.AddListener(RefreshRoomList);

        statusText.gameObject.SetActive(false);
    }

    public void UpdateRoomListUI(List<RoomInfo> updatedRoomList)
    {
        roomList = updatedRoomList;
        UpdateRoomList();
    }

    private void UpdateRoomList()
    {
        foreach (Transform child in roomListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (RoomInfo room in roomList)
        {
            GameObject roomButton = Instantiate(roomButtonPrefab, roomListContent);
            Button button = roomButton.GetComponent<Button>();
            Text buttonText = roomButton.GetComponentInChildren<Text>();

            buttonText.text = room.Name;
            if (room.PlayerCount == room.MaxPlayers)
            {
                button.interactable = false;
                buttonText.color = Color.gray;
            }
            else
            {
                button.onClick.AddListener(() => { JoinRoom(room.Name); });
            }
        }
    }
    private void JoinRoom(string roomName)
    {
        statusText.gameObject.SetActive(true);
        statusText.text = "Joining Room: " + roomName;

        PhotonNetwork.JoinRoom(roomName);
    }

    private void JoinRandomRoom()
    {
        statusText.gameObject.SetActive(true);
        statusText.text = "Joining a random room...";

        PhotonNetwork.JoinRandomRoom();
    }

    private void RefreshRoomList()
    {
        statusText.gameObject.SetActive(true);
        statusText.text = "Fetching Rooms...";
    }

    public override void OnJoinedRoom()
    {
        statusText.text = "Successfully Joined Room!";
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        statusText.text = "Failed to join room: " + message;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        this.roomList = roomList;
        UpdateRoomList();
    }
}
