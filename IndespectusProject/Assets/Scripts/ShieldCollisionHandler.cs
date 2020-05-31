using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollisionHandler : MonoBehaviour
{

    private PlayerResources playerResources;

    // Start is called before the first frame update
    void Start()
    {
        playerResources = GetComponentInParent<PlayerResources>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Sword" && collision.gameObject.transform.parent.gameObject.tag != "Player")
        {
            playerResources.LooseEssence(50);
        }
    }
}
