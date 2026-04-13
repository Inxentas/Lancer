using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Character))]

public class CharacterJump : Entity
{
    public int maxJumpFrames = 5;
    public float _jumpF = 0.6f;

    private Character _character;
    private bool isGrounded { get { return this._character.isGrounded; } }

    [Header("Jump Data")]
    [SerializeField] private InputAction jumpAction;
    [SerializeField] private bool _canJump = true;
    [SerializeField] private bool _isJumping = false;
    [SerializeField] private Vector3 _headingMove;
    [SerializeField] private int _jumpFrames = 0;

    // getters
    public bool isJumping { get { return _isJumping; } }

    protected override void OnAwake()
    {
        base.OnAwake();
        this._character = GetComponent<Character>();
        this.jumpAction = InputSystem.actions.FindAction("Jump");
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        SetJumpValues();
        SetJumpFrames();
    }
    // Set all values related to jumping.
    private void SetJumpValues()
    {
        if (_isJumping)
        {
            if (this._jumpFrames > maxJumpFrames || this.isGrounded)
            {
                _isJumping = false;
                _jumpFrames = 0;
                _canJump = true;
            }
        }

        // If the button to jump is pressed this frame...
        if (jumpAction.triggered && _jumpFrames == 0)
        {
            // If we can jump...
            if (_canJump && !_isJumping)
            {
                _isJumping = true;
                _canJump = false;
            }
        }

        if (_isJumping)
        {
            _headingMove.y = _jumpF;
        } else
        {
            _headingMove.y = -0.2f;
        }

        this._character.cc.Move(_headingMove);
    }
    // Keep track of jump frames.
    private void SetJumpFrames()
    {
        if (_isJumping)
        {
            _jumpFrames++;
        }
        else
        {
            _jumpFrames = 0;
        }
    }
}
