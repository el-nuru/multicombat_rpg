using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    // Main controller for turn actions and state transitions
    public class TurnAction : MonoBehaviour
    {
        [Header("UI")] 
        public Slider timeBar; // Assign in inspector
        [Header("Assigned Camera")]
        public Camera assignedCamera; // Assign the camera for this TurnAction in inspector

        private ITurnState currentState;

        void Start()
        {
            ChangeState(new WaitingState());
        }

        public void ChangeState(ITurnState newState)
        {
            if (currentState != null)
                currentState.Exit(this);
            currentState = newState;
            if (currentState != null)
                currentState.Enter(this);
        }

        // Show or hide the timer UI for this TurnAction
        public void SetTimerVisibility(bool visible)
        {
            if (timeBar != null)
                timeBar.gameObject.SetActive(visible);
        }

        // Add more logic here to handle specific turn actions
    }
}