using System.Linq;
using _MyPerfectHotel.Scripts.Helper;
using UnityEngine;

namespace _MyPerfectHotel.Scripts.Room
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<RoomController, bool> roomAccessibilityList;

        public void OpenNewRoom(RoomController room)
        {
            if (!roomAccessibilityList.Keys.Contains(room) || roomAccessibilityList[room]) return;
            
            roomAccessibilityList[room] = true;
            room.OpenTheRoom();
            
            Debug.Log($"Opened new room: {room}", room.gameObject);
        }

        public RoomController GetRoom()
        {
            foreach (var (room, access) in roomAccessibilityList)
            {
                if (access && room.RoomIsAvailable())
                    return room;
            }

            return null;
        }
    }
}