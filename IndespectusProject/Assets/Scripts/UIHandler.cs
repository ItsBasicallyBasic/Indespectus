using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    private Image essenceFill;
    [SerializeField]
    private Image healthFill;

    private PlayerResources playerResources;

    // Start is called before the first frame update
    void Start()
    {
        playerResources = GetComponent<PlayerResources>();
    }

    // Update is called once per frame
    void Update()
    {
        essenceFill.fillAmount = playerResources.GetEssence() / 100;
        healthFill.fillAmount = playerResources.GetHealth() / 100;
    }
}
