using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class HitDie : MonoBehaviour
{
    public float hitdietime;
    
    // Start is called before the first frame update
    void Start()
    {
        hitdietime = Time.time + 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if(hitdietime < Time.time) {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
