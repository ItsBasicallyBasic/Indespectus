using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyShader : MonoBehaviour {
    
    public Renderer meshRenderer;
    public Material material;

    public float TransparancyTransition;

    // Start is called before the first frame update
    void Start() {
        meshRenderer = gameObject.GetComponent<Renderer>();
        material = meshRenderer.material;
    }

    // Update is called once per frame
    void Update() {
        material.SetFloat("_Transition", TransparancyTransition);
    }
}
