using Tinject;
using Tinject.LifeCircle;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class UIManager: IStart, IOnDestroy
    {
        private Slider _sldTileMatrix;
        private InputField _iptToken;
        private Dropdown _ddServer;
        
        [Inject(ID = BindingID.UIRoot)] private RectTransform root;
        [Inject] private TileCalculator tileCalculator;
        [Inject] private TextureDownloader textureDownloader;
        [Inject] private UIInfo uiInfo;
        
        public void Start()
        {
            _sldTileMatrix = root.Find("bg/sldTileMatrix").GetComponent<Slider>();
            _iptToken = root.Find("bg/iptToken").GetComponent<InputField>();
            _ddServer = root.Find("bg/ddServer").GetComponent<Dropdown>();
            _sldTileMatrix.onValueChanged.AddListener(OnTileMatrix);
            _iptToken.onValueChanged.AddListener(OnToken);
            _ddServer.onValueChanged.AddListener(OnServer);

            _sldTileMatrix.value = uiInfo.TileMatrix;
            _iptToken.text = uiInfo.Token;
            _ddServer.value = uiInfo.Server;
        }

        public void OnDestroy()
        {
            _ddServer.onValueChanged.RemoveAllListeners();
            _sldTileMatrix.onValueChanged.RemoveAllListeners();
            _iptToken.onValueChanged.RemoveAllListeners();
        }

        private void OnTileMatrix(float v)
        {
            tileCalculator.SetTileMatrix(v);
            uiInfo.TileMatrix = tileCalculator.TileMatrix;
        }

        private void OnToken(string v)
        {
            textureDownloader.Token = v;
            uiInfo.Token = v;
        }

        private void OnServer(int v)
        {
            textureDownloader.Server = v;
            uiInfo.Server = v;
        }
    }
}