using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ShieldCollisionHandler : MonoBehaviour
{

    private PlayerResources playerResources;
    [SerializeField] CheckNetworked cn;
    [SerializeField] private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        if(PV == null) {PV = gameObject.GetComponent<PhotonView>();}
        if(cn == null) {cn = GameObject.FindGameObjectWithTag("NetworkCheck").GetComponent<CheckNetworked>();}
        if(PV.IsMine || !cn.networked) {
           playerResources = GetComponentInParent<PlayerResources>(); 
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(PV.IsMine || !cn.networked){
            if(collision.gameObject.tag == "Sword" && collision.gameObject.transform.parent.gameObject.tag != "Player")
            {
                playerResources.LooseEssence(50);
            }
        }
    }
}
