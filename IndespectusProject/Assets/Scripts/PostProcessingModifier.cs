using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Rendering.PostProcessing;

public class PostProcessingModifier : MonoBehaviour
{

    // [SerializeField] private PostProcessVolume volume;
    // private Vignette _Vignette;
    // private bool hurt = false;
    // private float intensity;

    // private ColorGrading _ColorGrading;

    // // Start is called before the first frame update
    // void Start()
    // {
    //     volume.profile.TryGetSettings(out _Vignette);
    //     volume.profile.TryGetSettings(out _ColorGrading);

    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (hurt)
    //     {
    //         if(_Vignette.intensity.value < 1f)
    //         {
    //             print("increasing effect");
    //             intensity += 3f * Time.deltaTime;
    //             _Vignette.intensity.value = intensity;
    //         }
    //         else
    //         {
    //             hurt = false;
    //         }
    //     }

    //     if (!hurt && intensity > 0)
    //     {
    //         print("drcreasing effect");
    //         intensity -= 2f * Time.deltaTime;
    //         _Vignette.intensity.value = intensity;
    //     }
    // }

    // public void EnableHurtEffect()
    // {
    //     if(volume.profile.TryGetSettings(out _Vignette) && _Vignette.intensity.value != 0)
    //     {
    //         _Vignette.intensity.value = 0;
    //     }
    //     hurt = true;
    // }

    // public void EnableGreyscale() {
    //     _ColorGrading.saturation.value = -100;
    // }
}
