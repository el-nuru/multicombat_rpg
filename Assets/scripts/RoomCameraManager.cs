using UnityEngine;

public class RoomCameraManager : MonoBehaviour
{
    [Header("Room Cameras")]
    public Camera[] roomCameras;
    private int currentRoomIndex = 0;
    [SerializeField] private bool forceDisplay1 = true;
    [SerializeField] private bool debugLogs = false;

    public void ShowRoomCamera(int index)
    {
        currentRoomIndex = Mathf.Clamp(index, 0, roomCameras.Length - 1);
        for (int i = 0; i < roomCameras.Length; i++)
        {
            var cam = roomCameras[i];
            if (cam == null) continue;
            bool active = (i == currentRoomIndex);
            if (cam.gameObject.activeSelf != active)
                cam.gameObject.SetActive(active);
            if (active && forceDisplay1)
            {
                cam.targetDisplay = 0;
            }
            if (debugLogs)
            {
                Debug.Log($"RoomCameraManager: {(active ? "ENABLED" : "disabled")} {cam.name} | targetDisplay={cam.targetDisplay}");
            }
        }
    }

    public int GetCurrentRoomIndex()
    {
        return currentRoomIndex;
    }
}
