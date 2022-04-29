using Common;
using Tinject;
using Tinject.LifeCircle;
using UnityEngine;

namespace Map
{
    public class MapInstaller : MonoInstaller
    {
        [SerializeField] private MainMap mainMap;
        [SerializeField] private UIInfo uiInfo;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private RectTransform uiRoot;
        [SerializeField] private Transform sceneRoot;
        [SerializeField] private InputDrag inputDrag;
        [SerializeField] private InputBeginEndDrag inputBeginEndDrag;
        [SerializeField] private InputScale inputScale;
        [SerializeField] private Texture defaultTexture;
        [SerializeField] private GameObject tilePrefab;
        protected override void OnInstalling(Container container)
        {
            container.Bind(typeof(MainMap)).To<MainMap>().FromInstance(mainMap);
            container.Bind(typeof(UIInfo)).To<UIInfo>().FromInstance(uiInfo);
            container.Bind(typeof(Transform)).WithId(BindingID.CameraTransform).To<Transform>().FromInstance(cameraTransform);
            container.Bind(typeof(RectTransform)).WithId(BindingID.UIRoot).To<RectTransform>().FromInstance(uiRoot);
            container.Bind(typeof(Transform)).WithId(BindingID.SceneRoot).To<Transform>().FromInstance(sceneRoot);
            container.Bind(typeof(InputDrag)).To<InputDrag>().FromInstance(inputDrag);
            container.Bind(typeof(InputBeginEndDrag)).To<InputBeginEndDrag>().FromInstance(inputBeginEndDrag);
            container.Bind(typeof(InputScale)).To<InputScale>().FromInstance(inputScale);
            container.Bind(typeof(Texture)).To<Texture>().FromInstance(defaultTexture);
            container.Bind(typeof(GameObject)).To<GameObject>().FromInstance(tilePrefab);

            container.Bind(typeof(IStart), typeof(IOnDestroy), typeof(UIManager)).To<UIManager>().FromNew();
            container.Bind(typeof(IStart), typeof(IOnDestroy), typeof(CameraManipulation)).To<CameraManipulation>().FromNew();
            container.Bind(typeof(TextureCache)).To<TextureCache>().FromNew();
            container.Bind(typeof(TextureDownloader)).To<TextureDownloader>().FromNew();
            container.Bind(typeof(IStart), typeof(IOnDestroy), typeof(TileCalculator)).To<TileCalculator>().FromNew();
            container.Bind(typeof(IStart), typeof(IOnDestroy), typeof(TileManager)).To<TileManager>().FromNew();
            
        }
    }
}

