using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [HideInInspector]
    private ThirdPersonCamera mThirdPersonCamera;
 

    private void Start()
    {
        ShowCharacterSelectionUI(); // Call to function
    }
    void ShowCharacterSelectionUI()
    {
        characterSelectionPanel.SetActive(true); // Display selection UI panel

        // Add listener for each button to select the corresponding character
        for (int i = 0; i < characterButtons.Length; i++)
        {
            int index = i; // Local copy of the index for the button listener
            characterButtons[i].onClick.AddListener(() => OnCharacterSelected(index)); // To identify which button is clicked
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

        string mPlayerPrefabName = mPlayerPrefabs[selectedCharacterIndex].name; // Retrieves the name of prefab
        Transform randomSpawnTransform = mSpawnPoints.GetSpawnPoint();
        // Instantiate through file path Resources/Prefabs/Soldier_Networked or SciFiPlayer_Networked
        mPlayerGameObject = PhotonNetwork.Instantiate("Prefabs/" + mPlayerPrefabName,
            randomSpawnTransform.position,
            randomSpawnTransform.rotation,
            0);

        mThirdPersonCamera = Camera.main.gameObject.AddComponent<ThirdPersonCamera>();
        mThirdPersonCamera.mPlayer = mPlayerGameObject.transform;
        mThirdPersonCamera.mDamping = 20.0f;
        mThirdPersonCamera.mCameraType = CameraType.Follow_Track_Pos_Rot;
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
