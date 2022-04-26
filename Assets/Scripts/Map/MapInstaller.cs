using Common.LifeCircle;
using Plugins.Tinject;
using Plugins.Tinject.LifeCircle;
using UnityEngine;

namespace Map
{
    public class MapInstaller : MonoInstaller
    {
        [SerializeField] private MainMap mainMap;
        protected override void OnInstalling(Container container)
        {
            container.Bind(typeof(Main), typeof(MainMap)).To<MainMap>()
                .FromInstance(mainMap);
            container.Bind(typeof(IAwake), typeof(IStart), typeof(IOnDestroy), typeof(TileCalculator)).WithId(1)
                .To<TileCalculator>().FromNew(10);
            container.Bind(typeof(IAwake), typeof(IStart), typeof(IOnDestroy), typeof(TileCalculator)).WithId(2)
                .To<TileCalculator>().FromNew(18);
        }
    }
}