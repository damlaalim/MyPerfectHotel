using UnityEngine;

namespace _MyPerfectHotel.Scripts.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
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
            var dir = new Vector3(joystick.Horizontal * moveSpeed, _rigidbody.velocity.y, joystick.Vertical * moveSpeed);
            _rigidbody.velocity = dir;
            
            if (joystick.Horizontal != 0 || joystick.Vertical != 0)
                transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
            
            if (dir != Vector3.zero && !_isWalk)
            {
                _animator.CrossFade("Walking", .1f);
                _isWalk = true;
            }
            else if (dir == Vector3.zero)
            {
                _animator.CrossFade("Idle", .1f);
                _isWalk = false;
            }
        }
    }
}
