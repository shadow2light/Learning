﻿using Tinject;
using UnityEngine;

namespace Map
{
    public class MainMap : MonoBehaviour
    {
        [Inject(ID = 1)] private TileCalculator tileCalculator;

        private void Awake()
        {
            Debug.Log(tileCalculator.TileMatrix);
        }

        private void Start()
        {
            Debug.Log(tileCalculator.TileMatrix);
        }
    }
}