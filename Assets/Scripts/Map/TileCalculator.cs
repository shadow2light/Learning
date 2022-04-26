using Common.LifeCircle;
using Plugins.Tinject;
using Plugins.Tinject.LifeCircle;
using UnityEngine;

namespace Map
{
    public class TileCalculator:IAwake, IStart, IOnDestroy
    {
        public int TileMatrix { get; private set; }

        public TileCalculator(int tileMatrix)
        {
            TileMatrix = tileMatrix;
        }

        public void Awake()
        {
            Debug.Log($"{nameof(TileCalculator)}.Awake()");
        }
        
        public void Start()
        {
            // TileMatrix = 1;
            Debug.Log($"{nameof(TileCalculator)}.Start()");
        }

        public void OnDestroy()
        {
            
        }

    }
}