using Common;
using Tinject;
using Tinject.LifeCircle;
using UnityEngine;

namespace Map
{
    //正交摄像机处理
    public class CameraManipulation : IStart, IOnDestroy
    {
        private Camera _camera;
        private Vector3 _offset;

        [Inject(BindingID.CameraTransform)] private Transform cameraTransform;
        [Inject] private InputDrag inputDrag;
        [Inject] private InputBeginEndDrag inputBeginEndDrag;
        [Inject] private TileCalculator tileCalculator;
        [Inject] private TextureDownloader textureDownloader;

        public void Start()
        {
            _camera = cameraTransform.GetComponent<Camera>();
            inputDrag.OnDragging += OnDragging;
            inputBeginEndDrag.OnDragged += OnDragged;
            tileCalculator.OnTileMatrixChanged += OnTileMatrixChanged;
        }

        public void OnDestroy()
        {
            tileCalculator.OnTileMatrixChanged -= OnTileMatrixChanged;
            inputDrag.OnDragging -= OnDragging;
            inputBeginEndDrag.OnDragged -= OnDragged;
        }

        private void OnDragging(Vector2 dir)
        {
            dir *= 0.01f;
            _offset -= new Vector3(dir.x, 0, dir.y);
            cameraTransform.position = _offset;
        }

        private void OnDragged(Vector2 dir)
        {
            TryDownload();
        }

        private void OnTileMatrixChanged()
        {
            TryDownload();
        }

        private void TryDownload()
        {
            if (GetTilesLocation(out var rf, out var cf, out var rt, out var ct))
            {
                textureDownloader.Download(rf, cf, rt, ct);
            }
        }

        /// <summary>
        /// 获取瓦片左下角坐标和瓦片个数
        /// </summary>
        /// <returns></returns>
        private bool GetTilesLocation(out int rowFrom, out int columnFrom, out int rowTo, out int columnTo)
        {
            var point = GetMapPoint();
            // if (GetIntersection(out var point))
            // {
            var rowCount = tileCalculator.RowCount;
            var columnCount = tileCalculator.ColumnCount;
            var mapWidth = TileManager.SideLength * columnCount;
            var mapHeight = TileManager.SideLength * rowCount;
            var row = (int) (rowCount * (point.z+ mapHeight / 2) / mapHeight);
            var column = (int) (columnCount * (point.x + mapWidth / 2) / mapWidth);
            var screenRowCount = Mathf.CeilToInt(Screen.width / TileManager.SideLength);
            var screenColumnCount = Mathf.CeilToInt(Screen.height / TileManager.SideLength);
            rowFrom = row - screenRowCount / 2;
            rowTo = row + screenRowCount / 2;
            columnFrom = column - screenColumnCount / 2;
            columnTo = column + screenColumnCount / 2;
            return true;
            // }

            // rowFrom = 0;
            // rowTo = 0;
            // columnFrom = 0;
            // columnTo = 0;
            // return false;
        }

        private Vector3 GetMapPoint()
        {
            var position = cameraTransform.position;
            return new Vector3(position.x, 0, position.z);
        }

        private bool GetIntersection(out Vector3 intersection)
        {
            intersection = Vector3.zero;
            var screenX = Screen.width / 2f;
            var screenY = Screen.height / 2f;
            var ray = _camera.ScreenPointToRay(new Vector3(screenX, screenY, 0));
            var t = (Vector3.Dot(Vector3.up, Vector3.forward) - Vector3.Dot(Vector3.up, ray.origin)) /
                    Vector3.Dot(Vector3.up, ray.direction);
            var hasIntersection = t >= 0;
            if (hasIntersection)
            {
                intersection = ray.origin + t * ray.direction;
            }

            return hasIntersection;
        }
    }
}