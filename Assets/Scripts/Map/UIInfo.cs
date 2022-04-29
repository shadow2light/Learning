using UnityEngine;

namespace Map
{
    [CreateAssetMenu(fileName = "Scriptable", menuName = "UIInfo", order = 0)]
    public class UIInfo : ScriptableObject
    {
        public int TileMatrix;
        public string Token;
        public int Server;
    }
}