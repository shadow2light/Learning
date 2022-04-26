using Common.LifeCircle;
using Plugins.Tinject;
using UnityEngine;

namespace Map
{
    public class MainMap : Main
    {
        [Inject(ID = 2)] private TileCalculator tileCalculator;

        protected override void Start()
        {
            base.Start();
            Debug.Log(tileCalculator.TileMatrix);
        }
    }
}