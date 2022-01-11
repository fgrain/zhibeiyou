using UnityEngine;

//状态类
public abstract class MoveState
{
    protected Rigidbody2D _body;//角色的刚体
    protected Animator _ator;//角色的动画器
    protected GameObject _player;//角色的GameObject
    protected SetMoveState _state = null;//状态的上下文
    protected float speed = 3;
    protected Vector2 velocity;

    public MoveState(SetMoveState state, GameObject player)
    {
        _state = state;
        _player = player;
        _body = player.GetComponent<Rigidbody2D>();
        _ator = player.GetComponent<Animator>();
        velocity = _body.velocity;
        PlayAnima();
        PlayEffect();
    }

    public abstract void PlayEffect();

    public abstract void PlayAnima();

    public abstract void HandleInput();

    public void Move()
    {
        velocity = _body.velocity;
        velocity.x = Input.GetAxis("Horizontal") * speed;
        if (Input.GetKey(KeyCode.D))
        {
            if (!Input.GetKey(KeyCode.A))
                _player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (!Input.GetKey(KeyCode.D))
                _player.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        _body.velocity = velocity;
    }

    public void Update()
    {
        Move();
    }
}

public class SetMoveState
{
    protected MoveState moveState;//持有状态类的引用

    public SetMoveState(GameObject player)
    {
        moveState = new CharacterStay(this, player);
    }

    //切换状态
    public void SetNewState(MoveState newState)
    {
        moveState = newState;
    }

    public void Update()
    {
        moveState.HandleInput();
        moveState.Update();
    }
}

//移动状态
public class CharacterMove : MoveState
{
    public CharacterMove(SetMoveState state, GameObject player) : base(state, player)
    {
    }

    public override void HandleInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _state.SetNewState(new CharacterJump(_state, _player));
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            _state.SetNewState(new CharacterStay(_state, _player));
        }
    }

    public override void PlayAnima()
    {
        _ator.Play("PlayerWalk");
    }

    public override void PlayEffect()
    {
        Game.Instance.sound.StopPlayEffect();
        Game.Instance.sound.PlayEffect("Walk");
    }
}

//跳跃状态
public class CharacterJump : MoveState
{
    private float jumpPhase = 0;
    private float maxAirJumps = 3;
    private float jumpHeight = 2;

    public CharacterJump(SetMoveState state, GameObject player) : base(state, player)
    {
        Jump();
    }

    public override void HandleInput()
    {
        if (jumpPhase < maxAirJumps)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        PlayEffect();
        jumpPhase++;
        velocity.y = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
        _body.velocity = velocity;
    }

    public override void PlayAnima()
    {
        _ator.Play("PlayerJump");
    }

    public override void PlayEffect()
    {
        Game.Instance.sound.StopPlayEffect();
        Game.Instance.sound.PlayEffect("Jump");
    }
}

//待机状态
public class CharacterStay : MoveState
{
    public CharacterStay(SetMoveState state, GameObject player) : base(state, player)
    {
    }

    public override void HandleInput()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            _state.SetNewState(new CharacterMove(_state, _player));
        }
        if (Input.GetButtonDown("Jump"))
        {
            _state.SetNewState(new CharacterJump(_state, _player));
        }
    }

    public override void PlayAnima()
    {
        _ator.Play("PlayerStay");
    }

    public override void PlayEffect()
    {
        Game.Instance.sound.StopPlayEffect();
    }
}