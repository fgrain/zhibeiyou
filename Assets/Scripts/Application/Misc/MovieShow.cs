using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MovieShow : MonoBehaviour
{
    private VideoPlayer vPlayer;
    private float time;

    private void Start()
    {
        vPlayer = gameObject.GetComponent<VideoPlayer>();
        time = vPlayer.frameCount / vPlayer.frameRate;
    }

    private void Update()
    {
        if (Mathf.Abs((int)(vPlayer.time - time)) == 0)

        {
            vPlayer.frame = (long)vPlayer.frameCount;

            vPlayer.Stop();

            Game.Instance.LoadLevel(1);
        }
    }
}