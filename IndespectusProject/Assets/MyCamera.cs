using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    [SerializeField] private Camera myCam;
    [SerializeField] private PhotonView myPView;
    // Start is called before the first frame update
    void Start() {
        if(myPView == null) {myPView = this.gameObject.GetComponent<PhotonView>();}
        if(!myPView.IsMine) {
            //myCam.enabled = false;
            Destroy(myCam);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
