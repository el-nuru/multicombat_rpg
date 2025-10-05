using UnityEngine;
using Naninovel;

public class StartRPG : MonoBehaviour
{
    public void Startrpg()
    {
        Debug.Log("Startrpg() method called!");

        // Hide the title menu properly using NaniNovel API
        var uiManager = Engine.GetService<Naninovel.IUIManager>();
        if (uiManager != null)
        {
            var titleUI = uiManager.GetUI<Naninovel.UI.ITitleUI>();
            if (titleUI != null)
            {
                titleUI.Hide();
                Debug.Log("TitleUI hidden via NaniNovel API");
            }
        }

        // Disable NaniNovel camera
        var naniCamera = Engine.GetService<ICameraManager>().Camera;
        if (naniCamera != null)
        {
            naniCamera.enabled = false;
            Debug.Log("NaniNovel camera disabled");
        }

        var roomsManager = FindFirstObjectByType<RoomsManager>();
        if (roomsManager != null)
        {
            roomsManager.SetActiveRoom(0);
            Debug.Log("RPG system initialized - Room 0 activated!");
        }
        else
        {
            Debug.LogError("RoomsManager not found!");
        }

        var battleManager = FindFirstObjectByType<BattleFlowManager>();
        if (battleManager != null)
        {
            battleManager.SetActiveRoomIndex(0);
            battleManager.timersActive = true;
            battleManager.BeginBattle();
            Debug.Log("Battle started!");
        }
        else
        {
            Debug.LogError("BattleFlowManager not found!");
        }

        // Re-enable Graphic Raycaster on battle canvas
        var battleCanvas = GameObject.Find("Canvas");
        if (battleCanvas != null)
        {
            var raycaster = battleCanvas.GetComponent<UnityEngine.UI.GraphicRaycaster>();
            if (raycaster != null)
            {
                raycaster.enabled = true;
                Debug.Log("Battle canvas Graphic Raycaster re-enabled");
            }
        }
    }
}
