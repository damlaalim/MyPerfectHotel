using _MyPerfectHotel.Scripts.Customers;
using UnityEngine;

namespace _MyPerfectHotel.Scripts.Room
{
    public class RoomController : MonoBehaviour
    {
        [SerializeField] private Transform bedTransform;
        
        private bool _roomIsDirty, _roomIsFull;

        public Transform GetBedTransform => bedTransform;

        public bool RoomIsAvailable()
        {
            return !_roomIsDirty && !_roomIsFull;
        }
        
        public void OpenTheRoom()
        {
            
        }

        public void GetTheCustomerInRoom(Customer customer)
        {
            _roomIsFull = true;
        }

        public void ExitTheCustomerInRoom()
        {
            _roomIsFull = false;
            _roomIsDirty = true;
        }

        public void CleanTheRoom()
        {
            
        }
    }
}