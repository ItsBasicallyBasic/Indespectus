using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeightRescaler : MonoBehaviour
{

    [SerializeField]
    private float defaultHeight = 1.8f;
    [SerializeField]
    private Camera camera;

    private void RescalePlayer()
    {
        float headHeight = camera.transform.localPosition.y;
        float scale = defaultHeight / headHeight;
        transform.localScale = Vector3.one * scale;
        transform.Translate(0, scale, 0);
    }

    private void OnEnable()
    {
        RescalePlayer();
    }
}
