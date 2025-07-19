using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    // Main controller for turn actions and state transitions
    public class TurnAction : MonoBehaviour
    {
        public Slider timeBar; // Assign in inspector
        public int fillTimeSeconds = 5; // Editable in inspector
        public string fightMessage = "FIGHT"; // Set to FIGHT1, FIGHT2, FIGHT3 per camera
        public string runMessage = "RUN"; // Set to RUN1, RUN2, RUN3 per camera
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

        public void StartTimeBar(int seconds)
        {
            StartCoroutine(FillTimeBar(seconds));
        }

        private IEnumerator FillTimeBar(int seconds)
        {
            float elapsed = 0f;
            timeBar.value = 0f;
            while (elapsed < seconds)
            {
                elapsed += Time.deltaTime;
                timeBar.value = Mathf.Clamp01(elapsed / seconds);
                yield return null;
            }
            timeBar.value = 1f;
            ChangeState(new ReadyState());
        }

        // Called by the Fight button; delegates to current state

        // Show or hide the timer UI for this TurnAction
        public void SetTimerVisibility(bool visible)
        {
            if (timeBar != null)
                timeBar.gameObject.SetActive(visible);
        }
        public void OnFightButtonPressed()
        {
            if (currentState != null)
                currentState.OnFight(this);
        }

        // Called by the Run button; delegates to current state
        public void OnRunButtonPressed()
        {
            if (currentState != null)
                currentState.OnRun(this);
        }

        // Print the fight message
        public void PrintFightMessage()
        {
            Debug.Log(fightMessage);
        }

        // Print the run message
        public void PrintRunMessage()
        {
            Debug.Log(runMessage);
        }
    }
}