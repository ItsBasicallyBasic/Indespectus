using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCaps : MonoBehaviour
{
    public GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = cam.transform.position;
        pos.y -= 0.27f;
        pos.z += 4;
        transform.position = pos;
        transform.rotation = cam.transform.rotation;
    }
}
