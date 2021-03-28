using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoSingleton<FollowPlayer>
{
    Transform m_player;
    Vector3 m_offset;
    Vector3 moveTarget;
    float speed = 5;
    float leftBorder = 0;
    List<Transform> rightBorder = new List<Transform>();
    float distance;
    float cramaPosx;
    Transform[] borders;

    protected override void Awake()
    {
        base.Awake();
        borders = GameObject.Find("Border").GetComponentsInChildren<Transform>();
        initBorderPos();
    }
    void Start()
    {
        m_player = GameObject.FindWithTag(Tag.Player).transform;
        m_offset = transform.position - m_player.position;
    }

    private void FixedUpdate()
    {
        //ChangeState();
        if (m_player.position.x >= leftBorder && m_player.position.x <= cramaPosx) 
        {
            CramaMove();
        }
        else if(m_player.position.x < leftBorder)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0, -1), speed * Time.deltaTime);
        }
        else if (m_player.position.x > cramaPosx)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(cramaPosx, 0, -1), speed * Time.deltaTime);
        }
    }

    void CramaMove()
    {
        moveTarget = new Vector3(m_offset.x + m_player.position.x, 0, -1);
        transform.position = Vector3.Lerp(transform.position, moveTarget, speed * Time.deltaTime);
    }

    void initBorderPos()
    {
        foreach (var go in borders)
        {
            rightBorder.Add(go);
        }
        rightBorder.RemoveAt(0);
        distance = Mathf.Abs(transform.position.x - rightBorder[0].position.x);
        rightBorder.RemoveAt(0);
        cramaPosx = rightBorder[0].position.x - distance;
    }

    public void ChangeState()
    {
        if (rightBorder.Count > 1)
        {
            Destroy(rightBorder[0].gameObject);
            rightBorder.RemoveAt(0);
            cramaPosx = rightBorder[0].position.x - distance;
        }        
    }
}
