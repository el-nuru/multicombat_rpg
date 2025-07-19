using UnityEngine;
using c1a_proy.rpg.rpg.Assets.scripts;

public class GameManager : MonoBehaviour
{
    public TurnAction[] turnActions;
    private int currentIndex = 0;

    // Called by CameraSwitcher when camera changes
    public void SetActiveIndex(int index)
    {
        currentIndex = index;
    }

    // Called by Fight button
    public void OnFightButtonPressed()
    {
        if (currentIndex >= 0 && currentIndex < turnActions.Length)
            turnActions[currentIndex].OnFightButtonPressed();
    }

    // Called by Run button
    public void OnRunButtonPressed()
    {
        if (currentIndex >= 0 && currentIndex < turnActions.Length)
            turnActions[currentIndex].OnRunButtonPressed();
    }
}
