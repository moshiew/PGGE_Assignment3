using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public GameObject[] mPlayerPrefabs;
    public PlayerSpawnPoints mSpawnPoints;

    public GameObject characterSelectionPanel;
    public Button[] characterButtons;

    private int selectedCharacterIndex = -1;

    [HideInInspector]
    public GameObject mPlayerGameObject;
    public GameObject playerNamePrefab;
    [HideInInspector]
    private ThirdPersonCamera mThirdPersonCamera;

    private void Start()
    {
        ShowCharacterSelectionUI();
        
        /*int randomIndex = Random.Range(0, mPlayerPrefabs.Length);
        string mPlayerPrefabName = mPlayerPrefabs[randomIndex].name;

        Transform randomSpawnTransform = mSpawnPoints.GetSpawnPoint();
        mPlayerGameObject = PhotonNetwork.Instantiate("Prefabs/" + mPlayerPrefabName,
            randomSpawnTransform.position,
            randomSpawnTransform.rotation,
            0);

        mThirdPersonCamera = Camera.main.gameObject.AddComponent<ThirdPersonCamera>();

        //mPlayerGameObject.GetComponent<PlayerMovement>().mFollowCameraForward = false;
        mThirdPersonCamera.mPlayer = mPlayerGameObject.transform;
        mThirdPersonCamera.mDamping = 20.0f;
        mThirdPersonCamera.mCameraType = CameraType.Follow_Track_Pos_Rot;*/
    }
    void ShowCharacterSelectionUI()
    {
        characterSelectionPanel.SetActive(true);

        // Add listener for each button to select the corresponding character
        for (int i = 0; i < characterButtons.Length; i++)
        {
            int index = i; // Local copy of the index for the button listener
            characterButtons[i].onClick.AddListener(() => OnCharacterSelected(index));
        }
    }

    void OnCharacterSelected(int index)
    {
        selectedCharacterIndex = index;

        // Hide the character selection UI once a character is selected
        characterSelectionPanel.SetActive(false);

        // Start the player instantiation after selection
        InstantiatePlayer();
    }

    // Instantiate the selected player prefab
    void InstantiatePlayer()
    {
        if (selectedCharacterIndex < 0 || selectedCharacterIndex >= mPlayerPrefabs.Length)
        {
            Debug.LogError("No character selected!");
            return;
        }

        string mPlayerPrefabName = mPlayerPrefabs[selectedCharacterIndex].name;
        Transform randomSpawnTransform = mSpawnPoints.GetSpawnPoint();
        mPlayerGameObject = PhotonNetwork.Instantiate("Prefabs/" + mPlayerPrefabName,
            randomSpawnTransform.position,
            randomSpawnTransform.rotation,
            0);

        mThirdPersonCamera = Camera.main.gameObject.AddComponent<ThirdPersonCamera>();
        mThirdPersonCamera.mPlayer = mPlayerGameObject.transform;
        mThirdPersonCamera.mDamping = 20.0f;
        mThirdPersonCamera.mCameraType = CameraType.Follow_Track_Pos_Rot;

        if(playerNamePrefab != null)
        {
            GameObject playerNameObject = PhotonNetwork.Instantiate("Prefabs/" + playerNamePrefab.name, mPlayerGameObject.transform.position + Vector3.up * 2.75f, Quaternion.identity, 0);

            TextMesh nameTextMesh = playerNameObject.GetComponent<TextMesh>();
            if(nameTextMesh != null)
            {
                nameTextMesh.text = PhotonNetwork.NickName;
            }
            playerNameObject.transform.SetParent(mPlayerGameObject.transform);
        }
        else
        {
            Debug.Log("Player Name prefab not assigned");
        }
    }

    public void LeaveRoom()
    {
        Debug.LogFormat("LeaveRoom");
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        //Debug.LogFormat("OnLeftRoom()");
        SceneManager.LoadScene("Menu");
    }

}
