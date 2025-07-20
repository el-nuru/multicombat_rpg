using UnityEngine;

using UnityEngine.InputSystem;
using c1a_proy.rpg.rpg.Assets.scripts;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] public Camera[] cameras;
    [Header("Turn Actions")]
    [SerializeField] public TurnAction[] turnActions;
    [Header("Input Actions")]
    [SerializeField] public InputActionAsset inputActions;

    // ...existing code...

    [Header("Battle Flow Manager")]
    public BattleFlowManager battleFlowManager;

    private InputAction[] camActions;
    private int currentIndex = 0;


    private void Start()
    {
        // Detect which camera is enabled at start and show the correct slider
        int activeIndex = -1;
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i].enabled)
            {
                activeIndex = i;
                break;
            }
        }
        for (int i = 0; i < turnActions.Length; i++)
        {
            turnActions[i].SetTimerVisibility(i == activeIndex);
        }
    // ...existing code...
    }

    private void OnEnable()
    {
        var cameraMap = inputActions.FindActionMap("Camera");
        camActions = new InputAction[cameras.Length];
        for (int i = 0; i < cameras.Length; i++)
        {
            string actionName = $"ActivateCam{i + 1}";
            camActions[i] = cameraMap.FindAction(actionName);
            int idx = i; // local copy for closure
            camActions[i].performed += ctx => ShowCamera(idx);
            camActions[i].Enable();
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < camActions.Length; i++)
        {
            camActions[i].performed -= ctx => ShowCamera(i);
            camActions[i].Disable();
        }
    }

    public void ShowCamera(int index)
    {
        currentIndex = index;
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].enabled = (i == index);
            turnActions[i].SetTimerVisibility(i == index);
        }
        if (battleFlowManager != null)
            battleFlowManager.SetActiveCameraIndex(index);
    }
}