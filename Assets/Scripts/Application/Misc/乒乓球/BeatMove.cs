using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatMove : MonoBehaviour
{
    protected Rigidbody2D body;
    protected RectTransform rectTransform;
    protected Vector3 initPosition;
    protected Quaternion initRotation;

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Border")
        {
            body.velocity = Vector2.zero;
        }
    }

    protected void Pause()
    {
        body.velocity = Vector2.zero;
        rectTransform.anchoredPosition = initPosition;
        rectTransform.localRotation = initRotation;
    }

    protected void Init()
    {
        rectTransform = GetComponent<RectTransform>();
        initPosition = rectTransform.anchoredPosition;
        initRotation = rectTransform.localRotation;
        //initRotation = rectTransform.localRotation;
        if (gameObject.GetComponent<Rigidbody2D>() == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
        }
        body = gameObject.GetComponent<Rigidbody2D>();
        body.gravityScale = 0;
    }
}
