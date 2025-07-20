using UnityEngine;
using UnityEngine.UI;

public class BattleFlowManager : MonoBehaviour
{
    public Slider[] playerTimerBars;
    public Slider enemyTimerBar;
    public float[] playerFillTimes;
    public float enemyFillTime = 5f;
    public Button fightButton;
    public Button runButton;
    public string[] fightMessages;
    public string[] runMessages;

    private float[] playerElapsed;
    private float enemyElapsed = 0f;
    private bool playerCanAct = false;
    private bool timersActive = true;
    private int activeCameraIndex = 0;

    void Start()
    {
        playerElapsed = new float[playerTimerBars.Length];
        ResetPlayerTimer();
        ResetEnemyTimer();
        UpdatePlayerSliderVisibility();
    }

    void Update()
    {
        if (!timersActive) return;

        // Update elapsed time for all players/cameras
        for (int i = 0; i < playerElapsed.Length; i++)
        {
            playerElapsed[i] += Time.deltaTime;
            if (playerTimerBars != null && playerTimerBars.Length > i)
            {
                if (playerFillTimes[i] > 0)
                {
                    playerTimerBars[i].value = Mathf.Clamp01(playerElapsed[i] / playerFillTimes[i]);
                }
                else
                {
                    playerTimerBars[i].value = 0; // Default to 0 if fill time is invalid
                }
            }
        }
        enemyElapsed += Time.deltaTime;
        if (enemyTimerBar != null)
            enemyTimerBar.value = Mathf.Clamp01(enemyElapsed / enemyFillTime);

        if (activeCameraIndex >= 0 && activeCameraIndex < playerElapsed.Length &&
            activeCameraIndex < playerFillTimes.Length &&
            !playerCanAct && playerElapsed[activeCameraIndex] >= playerFillTimes[activeCameraIndex])
        {
            playerCanAct = true;
            fightButton.interactable = true;
            runButton.interactable = true;
        }

        if (enemyElapsed >= enemyFillTime)
        {
            EnemyActs();
        }
    }

    public void OnFightButtonPressed()
    {
        if (playerCanAct)
        {
            string msg;
            if (fightMessages != null && fightMessages.Length > activeCameraIndex)
                msg = fightMessages[activeCameraIndex];
            else
                msg = $"FIGHT{activeCameraIndex + 1}";
            Debug.Log(msg);
            AfterPlayerAction();
        }
    }

    public void OnRunButtonPressed()
    {
        if (playerCanAct)
        {
            string msg;
            if (runMessages != null && runMessages.Length > activeCameraIndex)
                msg = runMessages[activeCameraIndex];
            else
                msg = $"RUN{activeCameraIndex + 1}";
            Debug.Log(msg);
            AfterPlayerAction();
        }
    }

    private void EnemyActs()
    {
        Debug.Log("ENEMY ATTACK");
        AfterEnemyAction();
    }

    private void AfterPlayerAction()
    {
        timersActive = false;
        fightButton.interactable = false;
        runButton.interactable = false;
        Invoke(nameof(ResetPlayerTimer), 0.5f); // Small delay before reset
    }

    private void AfterEnemyAction()
    {
        timersActive = false;
        fightButton.interactable = false;
        runButton.interactable = false;
        Invoke(nameof(ResetEnemyTimer), 0.5f); // Small delay before reset
    }

    private void ResetPlayerTimer()
    {
        if (playerElapsed != null && activeCameraIndex >= 0 && activeCameraIndex < playerElapsed.Length)
        {
            playerElapsed[activeCameraIndex] = 0f;
            if (playerTimerBars != null && playerTimerBars.Length > activeCameraIndex)
                playerTimerBars[activeCameraIndex].value = 0f;
        }
        else
        {
            Debug.LogError("Invalid activeCameraIndex or uninitialized playerElapsed array.");
        }
        playerCanAct = false;
        timersActive = true;
        UpdatePlayerSliderVisibility();
    }

    private void ResetEnemyTimer()
    {
        enemyElapsed = 0f;
        if (enemyTimerBar != null)
            enemyTimerBar.value = 0f;
        timersActive = true;
    }

    // Call this when switching cameras
    public void SetActiveCameraIndex(int index)
    {
        activeCameraIndex = Mathf.Clamp(index, 0, playerTimerBars.Length - 1);
        UpdatePlayerSliderVisibility();
    }

    private void UpdatePlayerSliderVisibility()
    {
        for (int i = 0; i < playerTimerBars.Length; i++)
        {
            playerTimerBars[i].gameObject.SetActive(i == activeCameraIndex);
        }
    }
}
