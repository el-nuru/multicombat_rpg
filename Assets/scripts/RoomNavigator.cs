using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    /// <summary>
    /// Single point of control for room navigation.
    /// Replaces CameraSwitcher, RoomsManager, and RoomCameraManager.
    /// Press 1-9 to switch rooms.
    /// </summary>
    public class RoomNavigator : MonoBehaviour
    {
        [System.Serializable]
        public struct RoomEntry
        {
            public Camera    camera;
            public GameObject uiPanel;
        }

        [Header("Rooms (index = room number)")]
        [SerializeField] private RoomEntry[] rooms;

        [Header("Input")]
        [SerializeField] private InputActionAsset inputActions;

        [Header("Dependencies")]
        [SerializeField] private BattleFlowManager battleFlowManager;

        public event Action<int> OnRoomChanged;

        private int _currentIndex;
        private InputAction[] _camActions;

        private void OnEnable()
        {
            var map = inputActions.FindActionMap("Camera");
            _camActions = new InputAction[rooms.Length];
            for (int i = 0; i < rooms.Length; i++)
            {
                int captured = i;
                string actionName = $"ActivateCam{i + 1}";
                _camActions[i] = map.FindAction(actionName);
                if (_camActions[i] != null)
                {
                    _camActions[i].performed += _ => NavigateTo(captured);
                    _camActions[i].Enable();
                }
            }
        }

        private void OnDisable()
        {
            if (_camActions == null) return;
            foreach (var a in _camActions)
                a?.Disable();
        }

        private void Start() => NavigateTo(0);

        public void NavigateTo(int index)
        {
            _currentIndex = Mathf.Clamp(index, 0, rooms.Length - 1);

            for (int i = 0; i < rooms.Length; i++)
            {
                bool active = i == _currentIndex;
                if (rooms[i].camera  != null) rooms[i].camera.gameObject.SetActive(active);
                if (rooms[i].uiPanel != null) rooms[i].uiPanel.SetActive(active);
            }

            battleFlowManager?.SetActiveRoomIndex(_currentIndex);
            OnRoomChanged?.Invoke(_currentIndex);
        }

        public int CurrentRoomIndex => _currentIndex;
    }
}
