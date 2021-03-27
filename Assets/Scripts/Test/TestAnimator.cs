using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimator : MonoBehaviour
{
    Animator ator;
    bool desJump = false;
    // Start is called before the first frame update
    void Start()
    {
        ator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            desJump = true;
            
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            ator.Play("PlayerWalk");
        }
        
    }

    private void FixedUpdate()
    {
        if (desJump)
        {
            desJump = false;
            ator.Play("PlayerJump");
        }
    }
}
