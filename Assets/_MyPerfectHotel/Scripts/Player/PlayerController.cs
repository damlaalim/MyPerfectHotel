using _MyPerfectHotel.Scripts.UI;
using UnityEngine;

namespace _MyPerfectHotel.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            other.transform.TryGetComponent<ControlPoint>(out var controlPoint);
            
            Debug.Log(controlPoint);
        }
    }
}
