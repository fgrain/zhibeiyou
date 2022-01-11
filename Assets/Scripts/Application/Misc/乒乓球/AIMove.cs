using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : BeatMove
{
    private Vector3 AIspeed;

    // Start is called before the first frame update
    private void Awake()
    {
        Init();
    }

    private void FixedUpdate()
    {
        if (PPGame.Instance.gameOver || PPGame.Instance.Win() || PPGame.Instance.Lose())
        {
            Pause();
        }
        else PositionUpdate();
    }

    private void PositionUpdate()
    {
        if (rectTransform.localRotation != initRotation)
        {
            rectTransform.localRotation = initRotation;
        }
        float transSpeed = PPGame.Instance.SpeedVertical;
        AIspeed.y = transSpeed;
        body.velocity = AIspeed;
    }
}