using _MyPerfectHotel.Scripts.Customers;
using _MyPerfectHotel.Scripts.Managers;
using _MyPerfectHotel.Scripts.Room;
using Zenject;

namespace _MyPerfectHotel.Scripts.Installers
{
    public class ZenjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CustomerManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<MoneyManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<RoomManager>().FromComponentInHierarchy().AsSingle();
        }
    }
}