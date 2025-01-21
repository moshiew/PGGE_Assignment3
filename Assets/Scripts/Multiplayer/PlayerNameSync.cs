using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerNameSync : MonoBehaviourPun, IPunObservable
{
    private TextMesh playerNameText;

    // Start is called before the first frame update
    void Start()
    {
        playerNameText = GetComponent<TextMesh>();
        if(playerNameText != null )
        {
            playerNameText.text = PhotonNetwork.NickName;
        }
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(playerNameText.text);
        }
        else
        {
            playerNameText.text = (string)stream.ReceiveNext();
        }
    }
}
