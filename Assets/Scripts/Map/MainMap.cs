using Tinject;
using UnityEngine;

namespace Map
{
    public class MainMap : MonoBehaviour
    {
        [Inject] private TileCalculator tileCalculator;

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