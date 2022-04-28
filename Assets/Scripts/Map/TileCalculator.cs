using System;
using Common;
using Tinject;
using Tinject.LifeCircle;
using UnityEngine;

namespace Map
{
    public class TileCalculator:IStart, IOnDestroy
    {
        public event Action OnTileMatrixChanged; 
        
        private const int MinTileMatrix = 1;
        private const int MaxTileMatrix = 18;
        private const float DeltaTileMatrix = 0.1f;

        private float tileMatrixF;
        public int TileMatrix
        {
            get { return Mathf.FloorToInt(tileMatrixF);}
        }
        public int ColumnCount { get; private set; }
        public int RowCount { get; private set; }

        [Inject] private InputScale inputScale;

        public void Start()
        {
            tileMatrixF = MinTileMatrix;
            inputScale.OnScale += OnScale;
        }

        public void OnDestroy()
        {
            inputScale.OnScale -= OnScale;
        }

        private void OnScale(float dir)
        {
            if(dir.IsApproximateZero())
                return;
            var temp = tileMatrixF;
            if (dir > 0)
            {
                tileMatrixF+=DeltaTileMatrix;
                if (tileMatrixF > MaxTileMatrix)
                    tileMatrixF = MaxTileMatrix;
            }
            else
            {
                tileMatrixF-=DeltaTileMatrix;
                if (tileMatrixF < MinTileMatrix)
                    tileMatrixF = MinTileMatrix;
            }
            RowCount = 2 ^ (TileMatrix - 1);
            ColumnCount = 2 * RowCount;

            var tempInt = Mathf.FloorToInt(temp);
            if(tempInt!=TileMatrix)
                OnTileMatrixChanged?.Invoke();
        }

        public bool IsOverBoundary(int row, int column)
        {
            return row >= RowCount || column >= ColumnCount;
        }
    }
}