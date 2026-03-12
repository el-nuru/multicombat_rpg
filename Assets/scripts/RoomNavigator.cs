using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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
        [FormerlySerializedAs("_bridge")]
        [SerializeField] private NaninovelBridge bridge;

        public event Action<int> OnRoomChanged;

        private int _currentIndex;
        private InputAction[] _camActions;
        private Action<InputAction.CallbackContext>[] _camCallbacks;
        private InputAction _cam0Action;
        private Action<InputAction.CallbackContext> _cam0Callback;

        private void OnEnable()
        {
            var map = inputActions?.FindActionMap("Camera");
            if (map == null) { Debug.LogError("[RoomNavigator] 'Camera' action map not found."); return; }

            _camActions   = new InputAction[rooms.Length];
            _camCallbacks = new Action<InputAction.CallbackContext>[rooms.Length];
            for (int i = 0; i < rooms.Length; i++)
            {
                int captured = i;
                _camActions[i] = map.FindAction($"ActivateCam{i + 1}");
                if (_camActions[i] != null)
                {
                    _camCallbacks[i] = _ => NavigateTo(captured);
                    _camActions[i].performed += _camCallbacks[i];
                    _camActions[i].Enable();
                }
            }

            _cam0Action = map.FindAction("ActivateCam0");
            if (_cam0Action != null)
            {
                _cam0Callback = _ => bridge?.Toggle();
                _cam0Action.performed += _cam0Callback;
                _cam0Action.Enable();
            }
        }

        private void OnDisable()
        {
            if (_camActions != null)
            {
                for (int i = 0; i < _camActions.Length; i++)
                {
                    if (_camActions[i] != null && _camCallbacks[i] != null)
                        _camActions[i].performed -= _camCallbacks[i];
                    _camActions[i]?.Disable();
                }
            }

            if (_cam0Action != null && _cam0Callback != null)
                _cam0Action.performed -= _cam0Callback;
            _cam0Action?.Disable();
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
