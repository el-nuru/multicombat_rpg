using UnityEngine;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class RoomsManager : MonoBehaviour
{
	[Header("UI Panels (one per room)")]
	public GameObject[] roomPanels;
	[Header("Managers")]
	public BattleFlowManager battleFlowManager;
	public RoomCameraManager roomCameraManager;
	[SerializeField] private int currentIndex = 0;

	private void Start()
	{
		SetActiveRoom(0);
	}

	public void SetActiveRoom(int index)
	{
		if (roomPanels == null || roomPanels.Length == 0)
		{
			Debug.LogWarning("RoomsManager: roomPanels not assigned");
			return;
		}
		currentIndex = Mathf.Clamp(index, 0, roomPanels.Length - 1);
		for (int i = 0; i < roomPanels.Length; i++)
		{
			if (roomPanels[i] != null)
			{
				roomPanels[i].SetActive(i == currentIndex);
			}
		}
		if (battleFlowManager != null)
		{
			battleFlowManager.SetActiveRoomIndex(currentIndex);
		}
		if (roomCameraManager != null)
		{
			roomCameraManager.ShowRoomCamera(currentIndex);
		}
	}

	private void Update()
	{
		// Atajos para cambiar de sala con teclas 1-9
#if ENABLE_INPUT_SYSTEM
		var kb = UnityEngine.InputSystem.Keyboard.current;
		if (kb == null) return;
		if (kb.digit1Key.wasPressedThisFrame || kb.numpad1Key.wasPressedThisFrame) SetActiveRoom(0);
		if (kb.digit2Key.wasPressedThisFrame || kb.numpad2Key.wasPressedThisFrame) SetActiveRoom(1);
		if (kb.digit3Key.wasPressedThisFrame || kb.numpad3Key.wasPressedThisFrame) SetActiveRoom(2);
		if (kb.digit4Key.wasPressedThisFrame || kb.numpad4Key.wasPressedThisFrame) SetActiveRoom(3);
		if (kb.digit5Key.wasPressedThisFrame || kb.numpad5Key.wasPressedThisFrame) SetActiveRoom(4);
		if (kb.digit6Key.wasPressedThisFrame || kb.numpad6Key.wasPressedThisFrame) SetActiveRoom(5);
		if (kb.digit7Key.wasPressedThisFrame || kb.numpad7Key.wasPressedThisFrame) SetActiveRoom(6);
		if (kb.digit8Key.wasPressedThisFrame || kb.numpad8Key.wasPressedThisFrame) SetActiveRoom(7);
		if (kb.digit9Key.wasPressedThisFrame || kb.numpad9Key.wasPressedThisFrame) SetActiveRoom(8);
#else
		for (int k = 0; k < 9; k++)
		{
			if (Input.GetKeyDown(KeyCode.Alpha1 + k))
			{
				SetActiveRoom(k);
			}
		}
#endif
	}
}

