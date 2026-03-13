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
            try
            {
                var scripts = Engine.GetService<IScriptManager>();
                if (scripts == null) { Debug.LogError("[NaninovelBridge] IScriptManager not found."); return; }
                await scripts.ScriptLoader.LoadOrErr("Main", this);
                _scriptLoaded = true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[NaninovelBridge] Failed to load script 'Main': {e.Message}");
            }
        }

        public void Toggle()
        {
            if (!_scriptLoaded) return;
            if (_inVN) ExitVN();
            else EnterVN();
        }

        private void EnterVN()
        {
            if (battleCanvas == null) { Debug.LogError("[NaninovelBridge] battleCanvas not assigned."); return; }
            var cam = Engine.GetService<ICameraManager>();
            if (cam == null) { Debug.LogError("[NaninovelBridge] ICameraManager not found."); return; }
            var player = Engine.GetService<IScriptPlayer>();
            if (player == null) { Debug.LogError("[NaninovelBridge] IScriptPlayer not found."); return; }

            _inVN = true;
            cam.Camera.enabled = true;
            if (cam.UICamera != null) cam.UICamera.enabled = true;
            battleCanvas.alpha = 0f;
            battleCanvas.blocksRaycasts = false;

            player.Play("Main");
        }

        private void ExitVN()
        {
            if (battleCanvas == null) { Debug.LogError("[NaninovelBridge] battleCanvas not assigned."); return; }
            var cam = Engine.GetService<ICameraManager>();
            if (cam == null) { Debug.LogError("[NaninovelBridge] ICameraManager not found."); return; }

            _inVN = false;
            cam.Camera.enabled = false;
            if (cam.UICamera != null) cam.UICamera.enabled = false;
            battleCanvas.alpha = 1f;
            battleCanvas.blocksRaycasts = true;

            if (roomNavigator != null) roomNavigator.NavigateTo(roomNavigator.CurrentRoomIndex);
        }
    }
}
