using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class QuickMover : MonoBehaviour {

    public int ms = 5;
    public SetMaterials sm;
    Vector3 pos;

    [SerializeField] private PhotonView PV;

    // Update is called once per frame
    void Update() {
        if(PV.IsMine) {
            if(sm.myNumber == 0) {
                if(Input.GetKey("w")) {
                    transform.position += Vector3.forward * ms * Time.deltaTime;
                } 
                if(Input.GetKey("a")) {
                    transform.position += Vector3.left * ms * Time.deltaTime;
                }
                if(Input.GetKey("s")) {
                    transform.position -= Vector3.forward * ms * Time.deltaTime;
                }
                if(Input.GetKey("d")) {
                    transform.position += Vector3.right * ms * Time.deltaTime;
                }
            } else if(sm.myNumber ==1) {
                if(Input.GetKey(KeyCode.UpArrow)) {
                    transform.position += Vector3.forward * ms * Time.deltaTime;
                } 
                if(Input.GetKey(KeyCode.LeftArrow)) {
                    transform.position += Vector3.left * ms * Time.deltaTime;
                }
                if(Input.GetKey(KeyCode.DownArrow)) {
                    transform.position -= Vector3.forward * ms * Time.deltaTime;
                }
                if(Input.GetKey(KeyCode.RightArrow)) {
                    transform.position += Vector3.right * ms * Time.deltaTime;
                }
            }
        }
    }
}
