using UnityEngine;

namespace _MyPerfectHotel.Scripts.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed, crossFadeTime;
        [SerializeField] private DynamicJoystick joystick; 
        
        private Animator _animator;
        private Rigidbody _rigidbody;
        private bool _isWalk;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = new Vector3(joystick.Horizontal * moveSpeed, _rigidbody.velocity.y, joystick.Vertical * moveSpeed);
            
            if (joystick.Horizontal != 0 || joystick.Vertical != 0)
                transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
            
            if ((joystick.Horizontal != 0 || joystick.Vertical != 0) && !_isWalk)
            {
                _animator.CrossFade("Walking", crossFadeTime);
                _isWalk = true;
            }
            else if (joystick.Horizontal == 0 && joystick.Vertical == 0 && _isWalk)
            {
                _animator.CrossFade("Idle", crossFadeTime);
                _isWalk = false;
            }
        }
    }
}
