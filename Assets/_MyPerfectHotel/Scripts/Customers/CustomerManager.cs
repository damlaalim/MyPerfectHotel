using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _MyPerfectHotel.Scripts.Customers
{
    public class CustomerManager : MonoBehaviour
    {
        public Action CustomerMovedAction;
        [SerializeField] private List<Customer> customerList;
        [SerializeField] private List<Transform> targetTransform;
        [SerializeField] private Transform roomTransform, initTransform;

        private Queue<Customer> _createdCustomer = new();

        private void Start()
        {
            CustomerMovedAction += CustomerMoved;
        }

        private void CustomerMoved()
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
        private void DeleteCustomer()
        {
            if (_createdCustomer.Count <= 0)
                return;

            var customer = _createdCustomer.Peek();
            customer.TryGetComponent<NavMeshAgent>(out var agent);
            agent.SetDestination(roomTransform.position);
            
            _createdCustomer.Dequeue();
            CustomerMovedAction?.Invoke();
        }
    }
}