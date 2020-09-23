using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RootMotion.FinalIK;
using UnityEngine;

public class PlayerDeath : MonoBehaviour {
    
    [SerializeField] VRIK vrik;
    [SerializeField] GameObject[] Weapons;
    [SerializeField] PlayerVelocity playerVelocity;
    [SerializeField] Material MainMat;

    [SerializeField] Material dissolveMaterial;
    [SerializeField] GameObject mesh;
    [SerializeField] GameObject dissolveParticle;
    [SerializeField] float dissolveTimer;

    [SerializeField] GameObject deathOverlay;
    [SerializeField] GameObject PlayerAvatar;
    [SerializeField] PostProcessingModifier PostProcessing;

    [SerializeField] bool dead;
    PhotonView pv;

    // Start is called before the first frame update
    void Start() {
        deathOverlay = GameObject.Find("DeathOverlay");
        deathOverlay.SetActive(false);
        dissolveTimer = 0;
        PostProcessing = GameObject.Find("PostProcessingVolume").GetComponent<PostProcessingModifier>();
        pv = this.gameObject.GetComponent<PhotonView>();
        dead = false;
    }

    // Update is called once per frame
    void Update() {
        if(dead) {
            // deathDissolve();
            pv.RPC("deathDissolve", RpcTarget.All);

        }
    }

    [PunRPC]
    private void deathDissolve() {
        print("dead = " + dead);
        dissolveMaterial.SetFloat("_DissolveValue", dissolveTimer);
        dissolveTimer += 0.01f;
        if(dissolveMaterial.GetFloat("_DissolveValue") >= 1) {
            if(deathOverlay != null)
                deathOverlay.SetActive(true);
            PostProcessing.EnableGreyscale();
            PhotonNetwork.Destroy(PlayerAvatar);
        }
    }

    public void DeathAnimation() {
        if(!dead) {
            vrik.enabled = false;
            foreach(GameObject weapon in Weapons) {
                weapon.SetActive(false);
            }
            playerVelocity.OverrideVelocity(1.5f);
            playerVelocity.overrideV = true;
            MainMat = playerVelocity.myMats[1];
            MainMat.SetColor("_GlowColor", new Vector4(0.0006f, 0.0173f, 0.0186f, 1f));
                    
            mesh.GetComponent<Renderer>().material = dissolveMaterial;
            dissolveMaterial.SetFloat("_DissolveValue", 0);
            
            dead = true;
            dissolveParticle.SetActive(true);
        }
    }
}
