using System;
using System.Collections;
using _MyPerfectHotel.Scripts.Room;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace _MyPerfectHotel.Scripts.Customers
{
    public class Customer : MonoBehaviour
    {
        [SerializeField] private float stayRoomTime;
        
        private NavMeshAgent _agent;
        private RoomController _currentRoom;
        private Animator _animator;

        public void Initialize(Vector3 initPos)
        {
            transform.position = initPos;
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _agent.autoBraking = true;
            _agent.autoRepath = true;
            NavMesh.avoidancePredictionTime = 0.5f;  
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<RoomController>(out var room) && room == _currentRoom)
                StartCoroutine(StayInTheRoomRoutine());
        }

        public void GoToTheRoom(RoomController room)
        {
            Debug.Log("odaya gidiyor");
            _currentRoom = room;
            
            MoveThePosition(room.GetBedTransform.position);
            _animator.CrossFade("Walking", 0);
        }
        
        private IEnumerator StayInTheRoomRoutine()
        {
            Debug.Log("odada bekle");
            _animator.CrossFade("Idle", 0);
            var elapsedTime = 0f;
            
            while (elapsedTime < stayRoomTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            ExitTheRoom();
        }

        public void ExitTheRoom()
        {
            Debug.Log("Bekleme bitti odadan çık");

            _currentRoom.ExitTheCustomerInRoom();
            
            gameObject.SetActive(false);
        }

        public void MoveThePosition(Vector3 position)
        {
            if (!_agent)
                return;
            
            _agent.SetDestination(position);
        }
    }
}