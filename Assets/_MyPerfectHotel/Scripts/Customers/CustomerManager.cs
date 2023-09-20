using System;
using System.Collections.Generic;
using _MyPerfectHotel.Scripts.Data;
using _MyPerfectHotel.Scripts.Managers;
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
        [SerializeField] private Transform roomTransform, initTransform, moneyStackPos;

        [Inject] private MoneyManager _moneyManager;
        
        private Queue<Customer> _createdCustomer = new();

        private void Start()
        {
            CustomerLeftQueue += CustomerLeftTheQueue;

            for (var i = 0; i < 3; i++)
            {
                CreateCustomer();
            }
        }

        private void CustomerLeftTheQueue()
        {
            var i = 0;
            foreach (var customer in _createdCustomer)
            {
                customer.TryGetComponent<NavMeshAgent>(out var agent);
                agent.SetDestination(targetTransform[i].position);
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
            newCustomer.transform.position = initTransform.position;
            newCustomer.TryGetComponent<NavMeshAgent>(out var agent);
            _createdCustomer.Enqueue(newCustomer);

            agent.SetDestination(targetTransform[_createdCustomer.Count - 1].position);
        }

        [Button]
        public void SendCustomerToTheRoom()
        {
            if (_createdCustomer.Count <= 0)
                return;

            var customer = _createdCustomer.Peek();
            customer.TryGetComponent<NavMeshAgent>(out var agent);
            agent.SetDestination(roomTransform.position);
            
            _createdCustomer.Dequeue();
            CustomerLeftQueue?.Invoke();

            _moneyManager.MoveToMoney(customer.transform.position, MoneyStackType.Reception);
        }
    }
}