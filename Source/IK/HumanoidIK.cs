using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HumanoidIK : MonoBehaviour
{
    public LayerMask layerMask;
    [Range(0f, 1f)] public float distanceToGround = 0.1f;
    public float footLerp = 0.1f;

    private Animator _animator;

    [Header("Head")]
    [SerializeField] private Vector3 _headLookPositionIK;
    [SerializeField] private float _headLookWeightIK = 0;

    [Header("Left Foot")]
    [SerializeField] private Vector3 _leftFootPositionIK;
    [SerializeField] private bool _leftFootIKPosition = false;
    [SerializeField] private bool _leftFootIKRotation = false;
    [SerializeField] [Range(0f, 1f)] private float _leftFootStrengthIk = 0.0f;
    [SerializeField] private Vector3 _leftFootPositionIKIdle;

    [Header("Right Foot")]
    [SerializeField] private Vector3 _rightFootPositionIK;
    
    [SerializeField] private bool _rightFootIKPosition = false;
    [SerializeField] private bool _rightFootIKRotation = false;
    [SerializeField] [Range(0f, 1f)] private float _rightFootStrengthIk = 0.0f;
    [SerializeField] private Vector3 _rightFootPositionIKIdle;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (_animator)
        {
            _animator.SetLookAtWeight(_headLookWeightIK);
            _animator.SetLookAtPosition(_headLookPositionIK);

            // left foot position & rotation
            _leftFootPositionIK = _animator.GetIKPosition(AvatarIKGoal.LeftFoot);
            if (_leftFootIKPosition)
            {
                _leftFootStrengthIk = Mathf.Lerp(_leftFootStrengthIk, 1, footLerp);

            }
            else
            {
                _leftFootStrengthIk = Mathf.Lerp(_leftFootStrengthIk, 0, footLerp);
            }
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, _leftFootStrengthIk);
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, _leftFootStrengthIk);

            // right foot position & rotation
            _rightFootPositionIK = _animator.GetIKPosition(AvatarIKGoal.RightFoot);
            if (_rightFootIKPosition)
            {
                _rightFootStrengthIk = Mathf.Lerp(_rightFootStrengthIk, 1, footLerp);

            }
            else
            {
                _rightFootStrengthIk = Mathf.Lerp(_rightFootStrengthIk, 0, footLerp);
            }
            _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, _rightFootStrengthIk);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, _rightFootStrengthIk);

            if (_leftFootStrengthIk > 0)
            {
                FootIKPass(AvatarIKGoal.LeftFoot);
            }
            if (_rightFootStrengthIk > 0)
            {
                FootIKPass(AvatarIKGoal.RightFoot);
            }
        }
    }

    private void FootIKPass(AvatarIKGoal goal)
    {
        RaycastHit hit;
        Ray ray = new Ray(_animator.GetIKPosition(goal) + Vector3.up, Vector3.down * 10);
        RaycastHit[] hits = Physics.RaycastAll(ray, 2.0f, this.layerMask);
        if (hits.Length > 0)
        {
            Vector3 point = hits[0].point;
            point.y += distanceToGround;
            _animator.SetIKPosition(goal, point);
            _animator.SetIKRotation(goal, Quaternion.LookRotation(transform.forward, hits[0].normal));
        }

        /*
        if (Physics.Raycast(ray, out hit, distanceToGround + 1f))
        {
            if (hit.collider.gameObject.layer.ToString() == "9")
            {
                Vector3 point = hit.point;
                point.y += distanceToGround;

                Debug.DrawRay(hit.point, hit.normal, Color.white);
                Debug.DrawRay(hit.point, -hit.normal, Color.red);

                //TODO: the ray is actually taken from a MOVING object, and rounding doesnt help against the flickering...
                //maybe remember the first time during an idle stance we do this, then use those values for as long
                //as we're idle...?

                //point.x = (float)Math.Round(point.x * 100f) / 100f;
                //point.y = (float)Math.Round(point.y * 100f) / 100f;
                //point.z = (float)Math.Round(point.z * 100f) / 100f;
                _animator.SetIKPosition(goal, point);
                _animator.SetIKRotation(goal, Quaternion.LookRotation(transform.forward, hit.normal));
            }
        }
        */
    }

    #region + Event handlers

    /*
    private void OnSelectEvent(GameObject obj)
    {
        _headLookWeightIK = 0.6f;
        _headLookPositionIK = obj.transform.position;
    }

    private void OnDeselectEvent(GameObject obj)
    {
        _headLookWeightIK = 0.0f;
        _headLookPositionIK = obj.transform.position;
    }

    private void OnDialogueSelectEvent(DialogueKeeper keeper, InputController input)
    {
        //_headLookWeightIK = 0.2f;
        //_headLookPositionIK = keeper.transform.position;
    }

    private void OnDialogueDeselectEvent(DialogueKeeper keeper, InputController input)
    {
        //_headLookWeightIK = 0.0f;
        //_headLookPositionIK = keeper.transform.position;
    }

    private void OnFreeLookOnEvent(Vector3 position)
    {
        if (_animator.GetBool("grounded"))
        {
            _headLookWeightIK = Mathf.Lerp(_headLookWeightIK, 1.0f, 0.05f);
            _headLookPositionIK = position;
        }
        else
        {
            _headLookWeightIK = Mathf.Lerp(_headLookWeightIK, 0.0f, 0.05f);
        }
    }

    private void OnFreeLookOffEvent()
    {
        _headLookWeightIK = Mathf.Lerp(_headLookWeightIK, 0.0f, 0.05f);
    }
    */

    #endregion

    #region + Animation Event Callbacks

    public void OnFootLandLeft()
    {
        _leftFootIKPosition = true;
        _leftFootIKRotation = true;
    }
    public void OnFootRaiseLeft()
    {
        _leftFootIKPosition = false;
        _leftFootIKRotation = false;
    }
    public void OnFootLandRight()
    {
        _rightFootIKPosition = true;
        _rightFootIKRotation = true;
    }
    public void OnFootRaiseRight()
    {
        _rightFootIKPosition = false;
        _rightFootIKRotation = false;
    }
    public void EnableFeetIK()
    {
        _leftFootIKPosition = true;
        _leftFootIKRotation = true;
        _rightFootIKPosition = true;
        _rightFootIKRotation = true;
    }
    public void DisableFeetIK()
    {
        _leftFootIKPosition = false;
        _leftFootIKRotation = false;
        _rightFootIKPosition = false;
        _rightFootIKRotation = false;
    }
    public void OnJump()
    {
        _leftFootIKPosition = false;
        _leftFootIKRotation = false;
        _rightFootIKPosition = false;
        _rightFootIKRotation = false;
    }
    public void OnSit()
    {
        _leftFootIKPosition = true;
        _leftFootIKRotation = false;
        _rightFootIKPosition = true;
        _rightFootIKRotation = false;
    }

    public void SetFootLerp(float lerp)
    {
        this.footLerp = lerp;
    }

    public void DisableIK()
    {
        this.enabled = false;
    }
    public void EnableIK()
    {
        this.enabled = true;
    }

    #endregion
}
