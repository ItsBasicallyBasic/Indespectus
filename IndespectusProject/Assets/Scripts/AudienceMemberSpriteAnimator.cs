using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceMemberSpriteAnimator : MonoBehaviour
{
    [SerializeField]
    private GameObject frame1;
    [SerializeField]
    private GameObject frame2;

    private float mSecondsInBetweenFrames = 1f;
    private float timer;

    private int currFrame = 0;

    // Start is called before the first frame update
    void Start()
    {
        currFrame = Random.Range(0,2);

        frame1.SetActive(false);
        frame2.SetActive(false);

        if (currFrame == 0)
        {
            frame1.SetActive(true);
            transform.Translate(0, -0.05f, 0);
        }
        if (currFrame == 1)
        {
            frame2.SetActive(true);
            transform.Translate(0, 0.05f, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > timer)
        {
            timer = Time.time + mSecondsInBetweenFrames;

            if (frame1.activeSelf)
            {
                transform.Translate(0, 0.05f, 0);
                frame1.SetActive(false);
                frame2.SetActive(true);
                return;
            }

            if (frame2.activeSelf)
            {
                transform.Translate(0, -0.05f, 0);
                frame1.SetActive(true);
                frame2.SetActive(false);
                return;
            }
        }
    }
}
