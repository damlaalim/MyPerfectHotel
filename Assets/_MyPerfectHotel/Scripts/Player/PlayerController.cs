using _MyPerfectHotel.Scripts.Controller;
using _MyPerfectHotel.Scripts.UI;
using UnityEngine;

namespace _MyPerfectHotel.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent<ControlPoint>(out var controlPoint))
                controlPoint.EnterThePoint();
            else if (other.transform.TryGetComponent<MoneyStackController>(out var stackController))
                stackController.MoveAllMoneyToPlayer(this, stackController.type);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.TryGetComponent<ControlPoint>(out var controlPoint))
                controlPoint.ExitThePoint();            
        }
    }
}
