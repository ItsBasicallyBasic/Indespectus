using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSDisp : MonoBehaviour
{
    public int FPS;
    public TextMeshPro fpsdisp;
    

    // Update is called once per frame
    void Update()
    {
        float curremt = (int) (1f/ Time.deltaTime);
        if(Time.frameCount % 50 == 0) {
            fpsdisp.text = (curremt.ToString() + "fps");
        }
    }
}
