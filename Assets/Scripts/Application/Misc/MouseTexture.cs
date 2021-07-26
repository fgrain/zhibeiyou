using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseTexture : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private Texture2D mouse1;
    private Texture2D mouse2;
    public int level = -1;

    private void Start()
    {
        mouse1 = Resources.Load<Texture2D>("UI/鼠标1");
        mouse2 = Resources.Load<Texture2D>("UI/鼠标2");
    }

    public void OnMouseEnter()
    {
        Cursor.SetCursor(mouse2, Vector2.zero, CursorMode.Auto);
    }

    public void OnMouseExit()
    {
        Cursor.SetCursor(mouse1, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(mouse2, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(mouse1, Vector2.zero, CursorMode.Auto);
    }

    private void OnEnable()
    {
        Cursor.SetCursor(mouse1, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Game.Instance.sound.PlayEffect("Select");
        if (level != -1)
        {
            if (Game.state == 10)
            {
                Game.Instance.LoadLevel(16);
            }
            else
            {
                gameObject.transform.parent.gameObject.SetActive(false);
                Game.Instance.LoadLevel(level);
            }
        }
    }
}