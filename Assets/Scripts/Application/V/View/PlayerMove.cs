using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMove : View
{
    #region 常量

    private const float maxAirJumps = 3;
    private const float jumpHeight = 2;
    [SerializeField, Range(3, 10)] private float speed = 3;

    #endregion 常量

    #region 字段

    private KeyCode left;
    private KeyCode right;
    private Animator ator;
    private bool onGround;
    private float jumpPhase = 0;
    private bool desiredJump = false;
    private Vector3 velocity;
    private Rigidbody2D body;
    public GameObject[] Border;
    private Coroutine PicedCor;
    private GameModel gm;
    private State state = State.State_Stay;
    private bool ispicked = false;
    private SetMoveState moveState;

    #endregion 字段

    #region 属性

    public override string Name => Consts.V_PlayerMove;

    #endregion 属性

    #region 枚举

    private enum State
    {
        State_Stay,
        State_Walk,
        State_Jump,
        State_Run
    }

    #endregion 枚举

    #region 方法

    #region 移动

    private void HandleInput()
    {
        #region switch状态机

        //velocity.x = Input.GetAxis("Horizontal") * speed;
        //if (Input.GetKey(right))
        //{
        //    if (!Input.GetKey(left))
        //        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        //}
        //else if (Input.GetKey(left))
        //{
        //    if (!Input.GetKey(right))
        //        transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        //}
        //switch (state)
        //{
        //    case State.State_Stay:
        //        if (Input.GetKey(right) || Input.GetKey(left))
        //        {
        //            Game.Instance.sound.PlayEffect("Walk");
        //            state = State.State_Walk;
        //            ator.Play("PlayerWalk");
        //        }
        //        if (desiredJump)
        //        {
        //            desiredJump = false;
        //            state = State.State_Jump;
        //            Jump();
        //            ator.Play("PlayerJump");
        //        }
        //        break;

        //    case State.State_Walk:
        //        ator.Play("PlayerWalk");
        //        if (!Input.GetKey(right) && !Input.GetKey(left))
        //        {
        //            Game.Instance.sound.StopPlayEffect();
        //            state = State.State_Stay;
        //            ator.Play("PlayerStay");
        //        }
        //        else if (desiredJump)
        //        {
        //            desiredJump = false;
        //            state = State.State_Jump;
        //            Jump();
        //            ator.Play("PlayerJump");
        //        }
        //        break;

        //    case State.State_Jump:
        //        //ator.Play("PlayerJump");
        //        if (desiredJump)
        //        {
        //            desiredJump = false;
        //            Jump();
        //        }
        //        if (onGround)
        //        {
        //            state = State.State_Stay;
        //        }
        //        break;
        //}

        #endregion switch状态机
    }

    private void Move()
    {
        UpdateState();
        HandleInput();
        body.velocity = velocity;
        onGround = false;
    }

    private void Jump()
    {
        if (onGround || jumpPhase < maxAirJumps)
        {
            ator.Play("PlayerJump");
            Game.Instance.sound.StopPlayEffect();
            Game.Instance.sound.PlayEffect("Jump");
            jumpPhase++;
            velocity.y = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
        }
    }

    #endregion 移动

    #region 碰撞

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.Picked) && !ispicked)
        {
            //Debug.Log("picked");
            ispicked = true;
            PicedCor = StartCoroutine(PickedObj(collision));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.Picked))
        {
            //Debug.Log("outpicked");
            ispicked = false;
            StopCoroutine(PicedCor);
        }
    }

    #endregion 碰撞

    #endregion 方法

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
        moveState = new SetMoveState(gameObject);
    }

    private void Update()
    {
        //desiredJump |= Input.GetButtonDown("Jump");
        if (!UIControl.Instance.isPause)
        {
            //desiredJump |= Input.GetButtonDown("Jump");
            moveState.Update();
        }
    }

    private void FixedUpdate()
    {
        if (!UIControl.Instance.isPause)
        {
            //Move();
        }
        else
        {
            state = State.State_Stay;
            ator.Play("PlayerStay");
            body.velocity = new Vector2(0, body.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == Tag.Ground)
        {
            //onGround = true;
            moveState.SetNewState(new CharacterStay(moveState, gameObject));
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded: " + scene.name);
        SceneAgrs e = new SceneAgrs
        {
            //获取当前场景索引
            sceneIndex = scene.buildIndex
        };
        //发送进入场景事件
        SendEvent(Consts.E_EnterScenes, e);
    }

    // called when the game is terminated
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #endregion Unity回调

    #region 事件回调

    public override void HandleEvent(string enentName, object data)
    {
        throw new System.NotImplementedException();
    }

    #endregion 事件回调

    #region 帮助方法

    private void UpdateState()
    {
        velocity = body.velocity;
        if (onGround)
        {
            jumpPhase = 0;
            if (curAnima("PlayerJump"))
                ator.Play("PlayerStay");
        }
    }

    private bool curAnima(string state)
    {
        return ator.GetCurrentAnimatorStateInfo(0).IsName(state);
    }

    private IEnumerator PickedObj(Collider2D collision)
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                gm.PickedStateChange(collision.name);
                //移除相机障碍
                collision.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Picked";
                bool remove = false;
                //if (gm.PickedObj.Count % 2 == 0)
                //{
                //    //Debug.Log(gm.PickedObj.Count);
                //    remove = true;
                //}
                UIControl.Instance.PickedAnima(collision.gameObject, remove);
                ispicked = false;
                break;
            }
            yield return null;
        }
    }

    #endregion 帮助方法
}