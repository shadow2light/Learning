using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Tinject;
using Tinject.LifeCircle;
using UnityEngine;

namespace Map
{
    public class TileManager:IStart, IOnDestroy
    {
        public const float SideLength = 25.6f;

        private readonly List<TileGameObject> _tileGameObjects = new List<TileGameObject>();

        [Inject] private TextureDownloader textureDownloader;
        [Inject] private GameObject tilePrefab;
        [Inject] private TileCalculator tileCalculator;
        [Inject(ID = BindingID.SceneRoot)] private Transform sceneRoot;
        
        public void Start()
        {
            PoolManager.WarmPool(tilePrefab, 50);
            textureDownloader.OnDownloadFinished += OnDownloadFinished;
        }

        public void OnDestroy()
        {
            textureDownloader.OnDownloadFinished -= OnDownloadFinished;
        }

        private void OnDownloadFinished(List<SendingTexture> sendingTextures)
        {
            ReleaseTiles();

            if(!sendingTextures.Any())
                return;

            foreach (var sendingTexture in sendingTextures)
            {
                CalculateWorldPosition(sendingTexture.Row, sendingTexture.Column, out var x, out var y);
                var go = PoolManager.SpawnObject(tilePrefab);
                var tileGameObject = new TileGameObject(go);
                tileGameObject.Transform.SetParent(sceneRoot);
                tileGameObject.Transform.position = new Vector3(x, 0, y);
                tileGameObject.Material.mainTexture = sendingTexture.Texture;
                _tileGameObjects.Add(tileGameObject);
            }
        }

        private void CalculateWorldPosition(int row, int col, out float x, out float y)
        {
            var mapWidth = tileCalculator.ColumnCount * SideLength;
            var mapHeight = tileCalculator.RowCount * SideLength;
            x = col * mapWidth / tileCalculator.ColumnCount + mapWidth / 2;
            y = row * mapHeight / tileCalculator.RowCount + mapHeight / 2;
        }

        private void ReleaseTiles()
        {
            foreach (var tileGameObject in _tileGameObjects)
            {
                tileGameObject.GameObject.SetActive(false);
                PoolManager.ReleaseObject(tileGameObject.GameObject);
            }
            _tileGameObjects.Clear();
        }
        
        private class TileGameObject
        {
            public GameObject GameObject;
            public Transform Transform;
            public Material Material;

            public TileGameObject(GameObject go)
            {
                GameObject = go;
                Transform = go.transform;
                Material = go.GetComponent<Renderer>().material;
            }
        }
    }
}