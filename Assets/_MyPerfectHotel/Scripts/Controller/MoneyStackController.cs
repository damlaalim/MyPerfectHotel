using System.Collections.Generic;
using _MyPerfectHotel.Scripts.Data;
using _MyPerfectHotel.Scripts.Managers;
using _MyPerfectHotel.Scripts.Player;
using UnityEngine;
using Zenject;

namespace _MyPerfectHotel.Scripts.Controller
{
    public class MoneyStackController : MonoBehaviour
    {
        public MoneyStackType type;
        public float GetMoneyAmount => _moneyAmount;

        [SerializeField] private Vector3Int stackCount;
        [SerializeField] private Vector3 stackDistance;
        [SerializeField] private float moneyUnitPrice;

        [Inject] private MoneyManager _moneyManager;
        
        private Vector3Int _moneyPos = Vector3Int.zero;
        private float _moneyAmount = 0;
        private List<GameObject> _moneyList = new();

        public Vector3 GetNextMoneyPos(GameObject moneyObject)
        {
            _moneyList.Add(moneyObject);
            _moneyAmount += moneyUnitPrice;
            
            var resizedStackCount = new Vector3Int(stackCount.x, stackCount.y - 1, stackCount.z - 1);

            if (_moneyPos == resizedStackCount)
                return _moneyPos + transform.position;
            
            if (_moneyPos.x == resizedStackCount.x)
            {
                _moneyPos.x = 1;

                if (_moneyPos.z == resizedStackCount.z)
                {
                    _moneyPos.z = 0;

                    if (_moneyPos.y == resizedStackCount.y)
                        return _moneyPos + transform.position;
                    else
                        _moneyPos.y++;
                }
                else
                    _moneyPos.z++;
            }
            else
                _moneyPos.x++;

            var moneyEndPos = new Vector3(_moneyPos.x * stackDistance.x, _moneyPos.y * stackDistance.y, _moneyPos.z * stackDistance.z) + transform.position;

            return moneyEndPos;
        }

        public void MoveAllMoneyToPlayer(PlayerController playerController, MoneyStackType stackType)
        {
            foreach (var money in _moneyList)
                _moneyManager.MoveToMoney(money, playerController, stackType);

            _moneyAmount = 0;
            _moneyList.Clear();
            _moneyPos = Vector3Int.zero;
        }
    }
}