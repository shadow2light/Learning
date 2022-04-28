using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map
{
    public class TextureCache
    {
        private readonly List<TextureCacheItem> _cacheItems = new List<TextureCacheItem>();

        public bool TryGet(int tileMatrix, int row, int column, out TextureCacheItem textureCacheItem)
        {
            textureCacheItem =
                _cacheItems.FirstOrDefault(c => c.TileMatrix == tileMatrix && c.Column == column && c.Row == row);

            return textureCacheItem != null;
        }

        public void Add(TextureCacheItem textureCacheItem)
        {
            if (!Exists(textureCacheItem.TileMatrix, textureCacheItem.Row, textureCacheItem.Column))
                _cacheItems.Add(textureCacheItem);
        }

        private bool Exists(int tileMatrix, int row, int column)
        {
            return
                _cacheItems.Any(c => c.TileMatrix == tileMatrix && c.Column == column && c.Row == row);
        }
    }

    public class TextureCacheItem
    {
        public int TileMatrix;
        public int Column;
        public int Row;
        public Texture Texture;
        public bool IsDefaultTexture;
    }
}