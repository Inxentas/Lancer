using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    [Tooltip("The default movement speed modifier.")]
    public float walkSpeed = 0.1f;

    [Tooltip("The default run speed modifier.")]
    public float runSpeed = 0.2f;

    [Tooltip("Whether movement input should be ignored.")]
    public bool suspendMotion = false;

    [Tooltip("Whether look input should be ignored.")]
    public bool suspendLookDirection = false;

    [Tooltip("The default animator our Character uses, if any.")]
    public Animator animator;

    [Tooltip("Should we lerp the Animator's object's rotation when we move?")]
    public bool rotateAnimator = false;

    [Tooltip("If yes, by what value will we lerp it?")]
    public float rotateAnimatorLerp = 0.1f;

    // TODO: now that we have rotation down, I'd also like to lerp the moveBlend value AND do so optionally.
    // TODO: And now that we have THAT down, we could add a toggle to ALSO apply this to the actual motion vector.

    [Tooltip("The moveBlend value we've calculated.")]
    public float moveBlend = 0.0f;

    [Tooltip("Should we lerp the Animator's moveBlend parameter when we move?")]
    public bool accelerateMoveBlend = false;

    [Tooltip("If yes, by what value will we lerp it?")]
    public float moveBlendLerp = 0.1f;

    [Tooltip("The default object our Character looks at, if any.")]
    public Transform lookObject;

    // components
    private CharacterController _cc;
    private SkinnedMeshRenderer[] _skinnedMeshRenderers;
    // input
    private InputAction moveAction;
    private InputAction sprintAction;
    // movement
    private float _axisH;
    private float _axisV;
    private bool _run;
    private bool _gravity = true;
    private bool _dead = false;
    // vector
    [SerializeField] private Vector3 _motionVector;
    [SerializeField] private Vector3 _motionVectorForward;

    #region Region : Getters
    public CharacterController cc { get { return this._cc; } }
    public bool isGrounded { get { return this._cc.isGrounded; } }
    public float axisH { get { return this._axisH; } }
    public float axisV { get { return this._axisV; } }
    public bool run { get { return this._run; } }
    public bool gravity { get { return this._gravity; } }
    public bool dead { get { return this._dead; } }
    public Vector3 motionVector { get { return this._motionVector; } }
    #endregion

    private void Awake()
    {
        this._cc = GetComponent<CharacterController>();
        this._skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        this.moveAction = InputSystem.actions.FindAction("Move");
        this.sprintAction = InputSystem.actions.FindAction("Sprint");
    }
    private void Update()
    {
        NormalMovement();
    }

    private Vector3 NormalizeInput(float axisH, float axisV)
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        Vector3 result = (forward * axisV) + (right * axisH);
        return result.normalized;
    }
    public void NormalMovement()
    {
        if (!_dead)
        {
            this._gravity = true;
            if (!suspendMotion)
            {
                ReadInput();
                SetMovement();
            }
        }
    }
    private void ReadInput()
    {
        if (!suspendMotion)
        {
            // Determine our movement axis and whether or not we're running.
            Vector2 axis = moveAction.ReadValue<Vector2>();
            this._axisH = axis.x;
            this._axisV = axis.y;
            this._run = ((_axisH != 0 || _axisV != 0) && sprintAction.ReadValue<float>() == 1);
        }
        else
        {
            this._run = false;
        }
    }
    private void SetMovement()
    {
        float moveBlend = 0;
        Vector3 moveVector = NormalizeInput(_axisH, _axisV);
        this._motionVector = moveVector;

        if (moveVector != Vector3.zero)
        {
            _motionVectorForward = moveVector;
            if (_run)
            {
                moveVector *= runSpeed;
                moveBlend = 1.0f;
            }
            else
            {
                moveVector *= walkSpeed;
                moveBlend = 0.5f;
            }
        }
        else
        {
            moveBlend = 0.0f;
        }

        if (this._gravity)
        {
            moveVector.y = Physics.gravity.y;
        }

        if (this.cc && this.cc.enabled)
        {
            this._cc.Move(moveVector * Time.timeScale);
        }

        if (this.animator)
        {   
            if (this.accelerateMoveBlend)
            {
                this.SetMoveBlend();
            } else
            {
                this.animator.SetFloat("moveBlend", moveBlend);
            }
            if (this.rotateAnimator)
            {
                this.RotateAnimatorTowardsMovement();
            }
        }
    }
    private void SetMoveBlend()
    {
        if (this.accelerateMoveBlend)
        {
            float newMoveBlend = 0;
            if (this._motionVector != Vector3.zero)
            {
                if (_run)
                {
                    newMoveBlend = 1.0f;
                }
                else
                {
                    newMoveBlend = 0.5f;
                }
            }
            this.moveBlend = Mathf.Lerp(this.moveBlend, newMoveBlend, this.moveBlendLerp);
            this.moveBlend = Mathf.Clamp01(this.moveBlend);
            this.animator.SetFloat("moveBlend", this.moveBlend);
        }
    }

    // Rotation

    /**
     * Rotates the animator's Gameobject towards our movement vector. This plays nice with most cameras.
     */
    public void RotateAnimatorTowardsMovement()
    {
        if (this.animator && this._motionVectorForward != Vector3.zero)
        {
            Transform t = this.animator.gameObject.transform;
            //direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(this._motionVectorForward);
            t.rotation = Quaternion.Lerp(t.rotation, targetRotation, rotateAnimatorLerp);
        }
        
    }
    /**
     * Rotates the Gameobject towards our movement vector. This doesn't work well with Cinemachine.
     */
    public void RotateCharacterTowardsMovement()
    {
        Quaternion targetRotation = Quaternion.LookRotation(this._motionVectorForward);
        this.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 1.0f);
    }
}
