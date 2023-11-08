using System.Threading.Tasks;
using _MyPerfectHotel.Scripts.Customers;
using _MyPerfectHotel.Scripts.Room;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _MyPerfectHotel.Scripts.UI
{
    public class ControlPoint : MonoBehaviour
    {
        [SerializeField] private float pointFillTime, customerWaitDelay;
        [SerializeField] private Image pointFillImage;

        [Inject] private CustomerManager _customerManager;
        [Inject] private RoomManager _roomManager;

        private bool _isEnterThePoint;

        private void Start()
        {
            _isEnterThePoint = false;
            pointFillImage.fillAmount = 0f;
        }

        public void EnterThePoint()
        {
            _isEnterThePoint = true;
            FillThePointTask();
        }

        public void ExitThePoint()
        {
            _isEnterThePoint = false;
        }

        private async void FillThePointTask()
        {
            while (_isEnterThePoint)
            {
                await Task.Delay((int)(customerWaitDelay * 1000));

                if (_customerManager.CreatedCustomerCount <= 0 || !_roomManager.GetRoom())
                    continue;
                
                var elapsedTime = pointFillImage.fillAmount * pointFillTime;
                var sendCustomer = false;
                
                while (elapsedTime < pointFillTime && pointFillImage)
                {
                    pointFillImage.fillAmount = Mathf.Lerp(0, 1, elapsedTime / pointFillTime);

                    elapsedTime += Time.deltaTime;
                    await Task.Yield();
                    
                    if (!_isEnterThePoint) return;
                    sendCustomer = true;
                }

                if (!sendCustomer)
                    return;
                
                Debug.Log("fill point task");
                pointFillImage.fillAmount = 0f;
                _customerManager.SendCustomerToTheRoom();
            }
        }
    }
}