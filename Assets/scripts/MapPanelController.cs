using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    public class MapPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private RoomNavigator roomNavigator;
        [SerializeField] private RoomMapUI mapUI;
        [SerializeField] private InputActionAsset inputActions;

        private InputAction _toggleAction;
        private Action<InputAction.CallbackContext> _toggleCallback;

        private void OnEnable()
        {
            var map = inputActions?.FindActionMap("Camera");
            if (map == null) { Debug.LogError("[MapPanelController] 'Camera' action map not found."); return; }

            _toggleAction = map.FindAction("ToggleMap");
            if (_toggleAction != null)
            {
                _toggleCallback = _ => SetPanelVisible(!panel.activeSelf);
                _toggleAction.performed += _toggleCallback;
                _toggleAction.Enable();
            }

            if (roomNavigator != null)
                roomNavigator.OnRoomChanged += HandleRoomChanged;
        }

        private void OnDisable()
        {
            if (_toggleAction != null && _toggleCallback != null)
                _toggleAction.performed -= _toggleCallback;
            _toggleAction?.Disable();

            if (roomNavigator != null)
                roomNavigator.OnRoomChanged -= HandleRoomChanged;
        }

        private void Start()
        {
            if (panel != null) panel.SetActive(false);
        }

        private void SetPanelVisible(bool visible)
        {
            if (panel != null) panel.SetActive(visible);
        }

        private void HandleRoomChanged(int index)
        {
            mapUI?.SetActiveRoom(index);
        }
    }
}
