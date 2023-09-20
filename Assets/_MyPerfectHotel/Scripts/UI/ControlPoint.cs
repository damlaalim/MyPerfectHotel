using System.Threading.Tasks;
using _MyPerfectHotel.Scripts.Customers;
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
            // TODO: only room is empty
            while (_isEnterThePoint)
            {
                await Task.Delay((int)(customerWaitDelay * 1000));

                if (_customerManager.CreatedCustomerCount <= 0)
                    continue;
                
                var elapsedTime = pointFillImage.fillAmount * pointFillTime;
                while (elapsedTime < pointFillTime && pointFillImage)
                {
                    pointFillImage.fillAmount = Mathf.Lerp(0, 1, elapsedTime / pointFillTime);

                    elapsedTime += Time.deltaTime;
                    await Task.Yield();
                    
                    if (!_isEnterThePoint) return;
                }

                pointFillImage.fillAmount = 0f;
                _customerManager.SendCustomerToTheRoom();
            }
        }
    }
}