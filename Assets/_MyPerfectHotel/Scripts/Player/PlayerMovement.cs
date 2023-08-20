using UnityEngine;

namespace _MyPerfectHotel.Scripts.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private DynamicJoystick joystick;

        private Rigidbody _rigidbody;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            var dir = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
            _rigidbody.velocity = dir * moveSpeed * Time.fixedDeltaTime;
        }
    }
}
