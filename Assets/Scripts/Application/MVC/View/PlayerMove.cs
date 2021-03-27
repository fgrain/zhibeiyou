using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMove : View
{
    #region 常量
    const float maxAirJumps = 3;
    const float jumpHeight = 2;
    [SerializeField, Range(3, 10)] float speed = 3;
    #endregion

    #region 字段
    KeyCode left;
    KeyCode right;
    Animator ator;  
    bool onGround;
    float jumpPhase = 0;
    bool desiredJump = false;
    private Vector3 velocity;
    Rigidbody2D body;
    public GameObject[] Border;

    GameModel gm;

    #endregion

    #region 事件
    #endregion

    #region 属性
    public override string Name => Consts.V_PlayerMove;
    #endregion

    #region 方法

    #region 移动
    void Move()
    {
        UpdateState();
        if (desiredJump)
        {
            desiredJump = false;
            //ator.SetBool("Jump", true);
            //ator.SetBool("Walk", false);
            //ator.SetBool("Stay", false);
            if(!curAnima("PlayerJump"))
                ator.Play("PlayerJump");
            Jump();
        }
        velocity.x = Input.GetAxis("Horizontal") * speed;
        if (Input.GetKey(right))
        {
            //ator.SetBool("Walk", true);
            if (!curAnima("PlayerJump") && !curAnima("PlayerWalk"))
            {
                ator.Play("PlayerWalk");             
            }
            if(!Input.GetKey(left))
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (Input.GetKey(left))
        {
            //ator.SetBool("Walk", true);
            if (!curAnima("PlayerJump") && !curAnima("PlayerWalk"))
            {
                ator.Play("PlayerWalk");                
            }
            if (!Input.GetKey(right))
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else
        {
            //ator.SetBool("Walk", false);
            if (!curAnima("PlayerJump"))
                ator.Play("PlayerStay");
        }
        body.velocity = velocity;
        onGround = false;
    }

    private void Jump()
    {
        if (onGround || jumpPhase < maxAirJumps)
        {
            jumpPhase++;
            velocity.y = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
        }
    }
    #endregion

    #region 碰撞
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.Picked))
        {
            FollowPlayer.Instance.test();
            Debug.Log(collision);
            if (Input.GetKeyDown(KeyCode.F))
            {
                Destroy(collision.gameObject);
                gm.PickedStateChange(collision.name);
                if (gm.PickedObj.Count % 2 == 0)
                {
                    FollowPlayer.Instance.ChangeState();
                    FollowPlayer.Instance.test();
                }
            }
        }
    }
    #endregion

    #endregion

    #region Unity回调
    private void Awake()
    {
        left = KeyCode.A;
        right = KeyCode.D;
        body = GetComponent<Rigidbody2D>();
        ator = GetComponent<Animator>();
        gm = GetModel<GameModel>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        //desiredJump |= Input.GetButtonDown("Jump");
        if (!UIControl.Instance.isPause)
        {
            desiredJump |= Input.GetButtonDown("Jump");
        }
    }

    private void FixedUpdate()
    {
        //Move();
        if (!UIControl.Instance.isPause)
        {
            Move();
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == Tag.Ground)
        {
            onGround = true;            
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        SceneAgrs e = new SceneAgrs
        {
            //获取当前场景索引
            sceneIndex = scene.buildIndex
        };
        //发送进入场景事件
        SendEvent(Consts.E_EnterScenes, e);
    }

    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion

    #region 事件回调
    public override void HandleEvent(string enentName, object data)
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region 帮助方法
    private void UpdateState()
    {
        velocity = body.velocity;
        if (onGround)
        {
            jumpPhase = 0;
            if(curAnima("PlayerJump"))
                ator.Play("PlayerStay");
            //ator.SetBool("Jump", false);
            //ator.SetBool("Stay", true);
        }
    }
    public void IsPause()
    {
        gm.IsPause = !gm.IsPause;
        if (gm.IsPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private bool curAnima(string state)
    {
        return ator.GetCurrentAnimatorStateInfo(0).IsName(state);
    }
    #endregion

}
