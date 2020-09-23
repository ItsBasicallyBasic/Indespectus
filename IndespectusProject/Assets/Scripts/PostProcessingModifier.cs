using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingModifier : MonoBehaviour
{

    [SerializeField] private PostProcessVolume volume;
    private Vignette vignette;
    private bool hurt = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hurt)
        {
            if(vignette.intensity.value < 0.3f)
            {
                vignette.intensity.value = Mathf.Lerp(0, 0.3f, 0.01f * Time.deltaTime);
            }
            else
            {
                hurt = false;
            }
        }
    }

    void EnableHurtEffect()
    {
        if(volume.profile.TryGetSettings(out vignette) && vignette.intensity.value != 0)
        {
            vignette.intensity.value = 0;
        }
        hurt = true;
    }
}
