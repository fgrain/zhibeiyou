using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoccerMove : MonoBehaviour, IPointerDownHandler
{
    private Rigidbody2D body;
    private Vector2 velocity;
    public float UpHight = 2;
    public float speed = 6;
    public Text text;
    private bool gameOver = false;

    private void Awake()
    {
    }

    private void Start()
    {
        body = transform.GetComponentInChildren<Rigidbody2D>();
        velocity = body.velocity;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RaycastResult rr = eventData.pointerCurrentRaycast;
        Debug.Log(rr.gameObject.tag);
        if (rr.gameObject.CompareTag(Tag.Soccer) && !gameOver)
        {
            Game.Instance.sound.PlayEffect("Soccer");
            Vector3 pointPos = rr.worldPosition;
            Move(pointPos);
        }
    }

    private void Move(Vector3 pointPos)
    {
        Vector2 forward = transform.position - pointPos;
        velocity.y = Mathf.Sqrt(-2f * Physics.gravity.y * UpHight);
        velocity.x = forward.x * speed;
        body.velocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Tag.Goal)
        {
            gameOver = true;
            body.Sleep();
            if (Manager.Instance.index == Manager.Instance.gameIndex.Length - 1)
            {
                text.text = "通关！";
            }
            text.gameObject.SetActive(true);
        }
    }

    public void NextGame()
    {
        if (Manager.Instance.index < Manager.Instance.gameIndex.Length - 1)
        {
            Manager.Instance.gameIndex[Manager.Instance.index].SetActive(false);
            Manager.Instance.index++;
            Manager.Instance.gameIndex[Manager.Instance.index].SetActive(true);
            text.gameObject.SetActive(false);
        }
        else
        {
            Game.Instance.LoadLevel(1);
        }
    }
}