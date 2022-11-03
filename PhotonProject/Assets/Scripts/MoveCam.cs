using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MoveCam : MonoBehaviourPunCallbacks, IPunObservable
{
    public Transform cameraPosition;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            //stream.SendNext(IsFiring);
            //stream.SendNext(Health);
        }
        else
        {
            // Network player, receive data
            //this.IsFiring = (bool)stream.ReceiveNext();
            //this.Health = (float)stream.ReceiveNext();
        }
    }
    
    private void Update()
    {
        if (photonView.IsMine){
            transform.position = cameraPosition.position;
        }
    }
}
