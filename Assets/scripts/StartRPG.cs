using UnityEngine;
using Naninovel;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    public class StartRPG : MonoBehaviour
    {
        public void StartRPG_()
        {
            HideNaninovelUI();
            DisableNaninovelCameras();
            InitBattle();
            EnableBattleCanvas();
        }

        private void HideNaninovelUI()
        {
            var uiManager = Engine.GetService<IUIManager>();
            if (uiManager == null) return;
            uiManager.GetUI<Naninovel.UI.ITitleUI>()?.Hide();
        }

        private void DisableNaninovelCameras()
        {
            var cameraManager = Engine.GetService<ICameraManager>();
            if (cameraManager == null) return;
            if (cameraManager.Camera != null)   cameraManager.Camera.enabled   = false;
            if (cameraManager.UICamera != null) cameraManager.UICamera.enabled = false;
        }

        private void InitBattle()
        {
            FindAnyObjectByType<RoomNavigator>()?.NavigateTo(0);
        }

        private void EnableBattleCanvas()
        {
            var canvas = GameObject.Find("Canvas");
            if (canvas == null) return;
            var cg = canvas.GetComponent<CanvasGroup>();
            if (cg != null) cg.blocksRaycasts = true;
        }
    }
}
