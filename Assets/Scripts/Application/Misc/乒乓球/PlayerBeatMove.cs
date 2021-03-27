using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeatMove : BeatMove
{
    public KeyCode up;
    public KeyCode down;
    [SerializeField, Range(0, 10)] private float speed = 8;

    private void Awake()
    {
        Init();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PPGame.Instance.gameOver||PPGame.Instance.Win()||PPGame.Instance.Lose())
        {
            Pause();
        }
        else Move();
    }

    void Move()
    {
        if (rectTransform.localRotation != initRotation)
        {
            rectTransform.localRotation = initRotation;
        }
        Vector2 v = body.velocity;
        if (Input.GetKey(up))
        {            
            v.y = speed;
        }
        else if (Input.GetKey(down))
        {
            v.y = -speed;
        }
        else
        {
            v.y = 0;
        }
        body.velocity = v;
    }




}
