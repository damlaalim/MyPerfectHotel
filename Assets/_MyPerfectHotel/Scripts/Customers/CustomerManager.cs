using System;
using System.Collections.Generic;
using _MyPerfectHotel.Scripts.Data;
using _MyPerfectHotel.Scripts.Managers;
using _MyPerfectHotel.Scripts.Room;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;

namespace _MyPerfectHotel.Scripts.Customers
{
    public class CustomerManager : MonoBehaviour
    {
        public Action CustomerLeftQueue;

        public int CreatedCustomerCount => _createdCustomer.Count;
        
        [SerializeField] private List<Customer> customerList;
        [SerializeField] private List<Transform> targetTransform;
        [SerializeField] private Transform initTransform;

        [Inject] private MoneyManager _moneyManager;
        [Inject] private RoomManager _roomManager;
        
        private Queue<Customer> _createdCustomer = new();

        private void Start()
        {
            CustomerLeftQueue += CustomerLeftTheQueue;

            for (var i = 0; i < 3; i++)
                CreateCustomer();
        }

        private void CustomerLeftTheQueue()
        {
            var i = 0;
            foreach (var customer in _createdCustomer)
            {
                var newOrderPos = targetTransform[i].position;
                customer.MoveThePosition(newOrderPos);
                i++;
            }
            
            CreateCustomer();
        }

        [Button]
        private void CreateCustomer()
        {
            if (_createdCustomer.Count >= 3)
                return;
            
            var randomIndex = Random.Range(0, customerList.Count);
            var newCustomer    = Instantiate(customerList[randomIndex]);
            var initPos = initTransform.position;
            
            newCustomer.Initialize(initPos);
            _createdCustomer.Enqueue(newCustomer);
            
            var orderPos= targetTransform[_createdCustomer.Count - 1].position;
            newCustomer.MoveThePosition(orderPos);
        }

        [Button]
        public void SendCustomerToTheRoom()
        {
            if (_createdCustomer.Count <= 0)
                return;

            var room = _roomManager.GetRoom();
            if (room is null) return;
            
            var customer = _createdCustomer.Peek();
            customer.GoToTheRoom(room);
            room.GetTheCustomerInRoom(customer);
            
            _createdCustomer.Dequeue();
            CustomerLeftQueue?.Invoke();

            _moneyManager.MoveToMoney(customer.transform.position, MoneyStackType.Reception);
        }
    }
}