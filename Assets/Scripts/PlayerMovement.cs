using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _forwardAcceleration;
    [SerializeField] private float _backwardAcceleration;
    [SerializeField] private float _strafeAcceleration;
    [SerializeField] private float _gravityAcceleration;
    [SerializeField] private float _jumpAcceleration;
    [SerializeField] private float _maxForwardVelocity;
    [SerializeField] private float _maxBackwardVelocity;
    [SerializeField] private float _maxStrafeVelocity;
    [SerializeField] private float _maxFallVelocity;
    [SerializeField] private float _RotationVelocityFactor;
    [SerializeField] private float _maxHeadUpAngle;
    [SerializeField] private float _minHeadDownAngle;
    [SerializeField] private float _charNormalHeight;
    [SerializeField] private float _charCrouchMultiplier;
    [SerializeField] private float _headWhileCrouch;
    


    private CharacterController _characterController;
    private Transform           _head;
    private Vector3             _acceleration;
    private Vector3             _velocity;
    private bool                _startJump;
    private Vector3             _headPosition;
    private bool                _startCrouch;
    private float               _sinPI4;
    private float               _headBeforeCrouch;


    void Start()
    {
        _characterController    = GetComponent<CharacterController>();
        _head                   = GetComponentInChildren<Camera>().transform;
        _velocity               = Vector3.zero;
        _acceleration           = Vector3.zero;
        _startJump              = false;
        _headPosition           = Vector3.zero;
        _startCrouch            = false;
        _sinPI4                 = Mathf.Sin(Mathf.PI / 4);
        _headBeforeCrouch       = _characterController.transform.localPosition.y + (_head.localPosition.y-0.1f);

        HideCursor();
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        UpdateRotation();
        UpdateHead();
        CheckForJump();
        CheckForCrouch();
    }

    private void UpdateRotation()
    {
        float rotation = Input.GetAxis("Mouse X") * _RotationVelocityFactor;

        transform.Rotate(0f, rotation, 0f);
    }

    private void UpdateHead()
    {
        Vector3 headRotation = _head.localEulerAngles;

        headRotation.x -= Input.GetAxis("Mouse Y") * _RotationVelocityFactor;

        if (headRotation.x < 180f)
            headRotation.x = Mathf.Min(headRotation.x, _maxHeadUpAngle);
        else
            headRotation.x = Mathf.Max(headRotation.x, _minHeadDownAngle);

        _head.localEulerAngles = headRotation;
    }

    private void CheckForJump()
    {
        if(Input.GetButtonDown("Jump") && _characterController.isGrounded)
            _startJump = true;
    }

    private void CheckForCrouch()
    {
        if (Input.GetButtonDown("Crouch") && _characterController.isGrounded)
        {
            _startCrouch = true;
            UpdateCrouch();
        }
        else if (Input.GetButtonUp("Crouch") && _characterController.isGrounded)
        {
            _startCrouch = false;
            UpdateCrouch();
        }
            
    }

    private void FixedUpdate()
    {
        UpdateAcceleration();
        UpdateVelocity();
        UpdatePosition();
    }


    private void UpdateAcceleration()
    {
        UpdateForwardAcceleration();
        UpdateStrafeAcceleration();
        UpdateVerticalAcceleration();
    }


    private void UpdateForwardAcceleration()
    {
        float forwardAxis = Input.GetAxis("Forward");

        if (forwardAxis > 0)
            _acceleration.z = _forwardAcceleration;
        else if (forwardAxis < 0)
            _acceleration.z = _backwardAcceleration;
        else
            _acceleration.z = 0;
    }


    private void UpdateStrafeAcceleration()
    {
        float strafeAxis = Input.GetAxis("Strafe");

        if (strafeAxis > 0)
            _acceleration.x = _strafeAcceleration;
        else if (strafeAxis < 0)
            _acceleration.x = -_strafeAcceleration;
        else
            _acceleration.x = 0;
    }


    private void UpdateVerticalAcceleration()
    {
        _acceleration.y = _gravityAcceleration;
        if (_startJump)
            _acceleration.y = _jumpAcceleration;
        else
            _acceleration.y = _gravityAcceleration;
    }

    private void UpdateCrouch()
    {
        if (_startCrouch)
        {
            Vector3 headPosition = _head.position;
            headPosition.y = headPosition.y - _headWhileCrouch;
            _characterController.height /= _charCrouchMultiplier;
            _head.position = headPosition;
        }
        else
        {
            //Return Camera and Character to the start position
            Vector3 headPosition = _head.position;
            headPosition.y = _headBeforeCrouch;
            _characterController.height = _charNormalHeight;

            _head.position = headPosition;
        }
    }

    private void UpdateVelocity()
    {
        _velocity += _acceleration * Time.fixedDeltaTime;

        if (_acceleration.z == 0f || (_acceleration.z * _velocity.z < 0f))
            _velocity.z = 0f;
        else if (_acceleration.x == 0f)
            _velocity.z = Mathf.Clamp(_velocity.z, _maxBackwardVelocity, _maxForwardVelocity);
        else
            _velocity.z = Mathf.Clamp(_velocity.z, _maxBackwardVelocity * _sinPI4, _maxForwardVelocity * _sinPI4);

        if (_acceleration.x == 0f || (_acceleration.x * _velocity.x < 0f))
            _velocity.x = 0f;
        else if (_acceleration.z == 0f)
            _velocity.x = Mathf.Clamp(_velocity.x, -_maxStrafeVelocity, _maxStrafeVelocity);
        else
            _velocity.x = Mathf.Clamp(_velocity.x, -_maxStrafeVelocity * _sinPI4, _maxStrafeVelocity * _sinPI4);

        if (_characterController.isGrounded && !_startJump)
            _velocity.y = -0.1f;
        else
            _velocity.y = Mathf.Max(_velocity.y, _maxFallVelocity);

        _startJump = false;
    }


    private void UpdatePosition()
    {
        Vector3 motion = _velocity * Time.fixedDeltaTime;
        motion = transform.TransformVector(motion);
        _characterController.Move(motion);
    }
}
