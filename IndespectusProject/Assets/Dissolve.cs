using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour {
    
    [SerializeField] Material dissolveMaterial;
    [SerializeField] GameObject mesh;
    [SerializeField] GameObject dissolveParticle;
    [SerializeField] float dissolveTime;
    [SerializeField] float dissolveTimer;

    
    // Start is called before the first frame update
    void Start() {
        mesh.GetComponent<Renderer>().material = dissolveMaterial;
        dissolveMaterial.SetFloat("_DissolveValue", 0);
        dissolveParticle.SetActive(true);
        dissolveTimer = 0;
        
    }

    // Update is called once per frame
    void Update() {
        // while(dissolveTimer <= 1000) {
        //     dissolveMaterial.SetFloat("_DissolveValue", Mathf.Lerp(0, 1, (dissolveTimer / dissolveTime)));
        //     dissolveTimer -= Time.deltaTime;
        // }
        
        dissolveMaterial.SetFloat("_DissolveValue", dissolveTimer);
        dissolveTimer += 0.01f;  
        if(dissolveMaterial.GetFloat("_DissolveValue") > 1) {
            Destroy(this);
            //do other death things
        }

        
    }
}
