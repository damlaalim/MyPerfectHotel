using UnityEngine;
using UnityEngine.AI;

namespace _MyPerfectHotel.Scripts.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float crossFadeIdleTime, crossFadeWalkTime;
        [SerializeField] private DynamicJoystick joystick; 
        
        private Animator _animator;
        private NavMeshAgent _agent;
        private bool _isWalk;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
        }

        private void FixedUpdate()
        {
            var dest = transform.position + new Vector3(joystick.Horizontal, _agent.destination.y, joystick.Vertical);

            if (joystick.Horizontal != 0 || joystick.Vertical != 0)
                _agent.SetDestination(dest);
            
            if ((joystick.Horizontal != 0 || joystick.Vertical != 0) && !_isWalk)
            {
                _animator.CrossFade("Walking", crossFadeWalkTime);
                _isWalk = true;
            }
            else if (joystick.Horizontal == 0 && joystick.Vertical == 0 && _isWalk)
            {
                _animator.CrossFade("Idle", crossFadeIdleTime);
                _isWalk = false;
            }

        }
    }
}
