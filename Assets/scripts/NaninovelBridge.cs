using UnityEngine;
using Naninovel;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    public class NaninovelBridge : MonoBehaviour
    {
        [SerializeField] private CanvasGroup battleCanvas;
        [SerializeField] private RoomNavigator roomNavigator;

        private bool _inVN = false;
        private bool _scriptLoaded = false;

        private void OnEnable()
        {
            if (Engine.Initialized) LoadScript();
            else Engine.OnInitializationFinished += LoadScript;
        }

        private void OnDisable()
        {
            Engine.OnInitializationFinished -= LoadScript;
        }

        private async void LoadScript()
        {
            Engine.OnInitializationFinished -= LoadScript;
            var scripts = Engine.GetService<IScriptManager>();
            await scripts.ScriptLoader.LoadOrErr("Main", this);
            _scriptLoaded = true;
        }

        public void Toggle()
        {
            if (!_scriptLoaded) return;
            if (_inVN) ExitVN();
            else EnterVN();
        }

        private void EnterVN()
        {
            var cam = Engine.GetService<ICameraManager>();
            if (cam == null) { Debug.LogError("[NaninovelBridge] ICameraManager not found."); return; }
            _inVN = true;
            cam.Camera.enabled = true;
            if (cam.UICamera != null) cam.UICamera.enabled = true;
            battleCanvas.alpha = 0f;
            battleCanvas.blocksRaycasts = false;

            Engine.GetService<IScriptPlayer>().Play("Main");
        }

        private void ExitVN()
        {
            var cam = Engine.GetService<ICameraManager>();
            if (cam == null) { Debug.LogError("[NaninovelBridge] ICameraManager not found."); return; }
            _inVN = false;
            cam.Camera.enabled = false;
            if (cam.UICamera != null) cam.UICamera.enabled = false;
            battleCanvas.alpha = 1f;
            battleCanvas.blocksRaycasts = true;

            roomNavigator?.NavigateTo(roomNavigator.CurrentRoomIndex);
        }
    }
}
