using System;
using System.Collections;
using System.Collections.Generic;
using Tinject;
using UnityEngine;
using UnityEngine.Networking;

namespace Map
{
    public class TextureDownloader
    {
        public event Action<List<SendingTexture>> OnDownloadFinished;

        private const int ResponseSuccess = 200;

        private readonly List<SendingTexture> _sendingTextures = new List<SendingTexture>();

        public int Server { get; set; } = 5;
        public string Token { get; set; } = "1cfd7dad19ff9eb29f333612dd1e333b";

        [Inject] private TileCalculator tileCalculator;
        [Inject] private Texture defaultTexture;
        [Inject] private MainMap mainMap;
        [Inject] private TextureCache textureCache;

        public void Download(int rowFrom, int colFrom, int rowTo, int colTo)
        {
            _sendingTextures.Clear();
            for (var i = rowFrom; i <= rowTo; i++)
            {
                for (var j = colFrom; j <= colTo; j++)
                {
                    mainMap.StartCoroutine(DownloadAsync(i, j));
                }
            }

            OnDownloadFinished?.Invoke(_sendingTextures);
        }

        private IEnumerator DownloadAsync(int row, int col)
        {
            //超界了，直接给默认图
            if (tileCalculator.IsOverBoundary(row, col))
            {
                _sendingTextures.Add(new SendingTexture {Column = col, Row = row, Texture = defaultTexture});
                yield break;
            }

            var tileMatrix = tileCalculator.TileMatrix;

            //优先从缓存获取
            if (textureCache.TryGet(tileMatrix, row, col, out var textureCacheItem))
            {
                if (!textureCacheItem.IsDefaultTexture)
                {
                    _sendingTextures.Add(new SendingTexture
                        {Column = col, Row = row, Texture = textureCacheItem.Texture});
                    yield break;
                }
            }

            var api =
                $"http://t{Server}.tianditu.gov.cn/vec_c/wmts?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0&LAYER=vec&TILEMATRIXSET=c&tk={Token}&TILECOL={col}&TILEROW={row}&TILEMATRIX={tileMatrix}";
            var request = UnityWebRequestTexture.GetTexture(api);
            yield return request.SendWebRequest();

            if (request.responseCode != ResponseSuccess)
            {
                _sendingTextures.Add(new SendingTexture {Column = col, Row = row, Texture = defaultTexture});
                textureCache.Add(new TextureCacheItem
                {
                    Column = col, Row = row, IsDefaultTexture = true, TileMatrix = tileMatrix,
                    Texture = defaultTexture
                });
            }
            else
            {
                var texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
                _sendingTextures.Add(new SendingTexture {Column = col, Row = row, Texture = texture});
                textureCache.Add(new TextureCacheItem
                {
                    Column = col, Row = row, IsDefaultTexture = false, TileMatrix = tileMatrix,
                    Texture = texture
                });
            }
        }
    }

    public class SendingTexture
    {
        public int Column;
        public int Row;
        public Texture Texture;
    }
}