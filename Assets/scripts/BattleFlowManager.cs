using UnityEngine;
using UnityEngine.UI;
using c1a_proy.rpg.rpg.Assets.scripts;
using System.Collections.Generic;

public class BattleFlowManager : MonoBehaviour
{
    public enum PlayerAction { Fight, Run }
    public int activeRoomIndex = 0;
    public float resetDelay = 0f;
    public bool timersActive = true;

    private Dictionary<MonoBehaviour, CharacterUIBinder> binderCache = new Dictionary<MonoBehaviour, CharacterUIBinder>();

    [System.Serializable]
    public struct CombatantSlot
    {
        public int roomIndex;
        public bool isEnemy;
        public MonoBehaviour combatant;
        public ICharacter Character => combatant as ICharacter;
    }

    [Header("Combatant Slots")]
    public CombatantSlot[] combatantSlots;

    public void BeginBattle()
    {
        activeRoomIndex = 0;
        ResetActiveCombatantTimer();
    }

    void Update()
    {
        if (!timersActive) return;
        bool anyPlayerReady = false;
        for (int i = 0; i < combatantSlots.Length; i++)
        {
            var slot = combatantSlots[i];
            var character = slot.Character;
            if (character != null)
            {
                if (!slot.isEnemy && slot.roomIndex == activeRoomIndex && character.ElapsedTime >= character.FillTime)
                {
                    anyPlayerReady = true;
                }
                if (slot.isEnemy && character.ElapsedTime >= character.FillTime)
                {
                    // Print enemy attack message
                    Debug.Log($"[ENEMY] {character.FightMessage} (room {slot.roomIndex}, slot {i})");
                    character.ElapsedTime = 0f;
                    // Update enemy slider if CharacterUIBinder exists
                    var characterMB = character as MonoBehaviour;
                    if (characterMB != null)
                    {
                        if (!binderCache.TryGetValue(characterMB, out var binder))
                        {
                            binder = characterMB.GetComponent<CharacterUIBinder>();
                            binderCache[characterMB] = binder;
                        }
                        if (binder != null)
                        {
                            binder.RefreshAll();
                        }
                    }
                }
            }
        }
    }

    public void OnFightButtonPressed()
    {
    TryExecutePlayerActionInRoom(activeRoomIndex, PlayerAction.Fight);
    }

    public void OnRunButtonPressed()
    {
    TryExecutePlayerActionInRoom(activeRoomIndex, PlayerAction.Run);
    }

    private void AfterCombatantAction()
    {
        timersActive = false;
        Invoke(nameof(ResetActiveCombatantTimer), resetDelay);
    }

    private void ResetActiveCombatantTimer()
    {
        // Only reset timer for player in current room
        for (int i = 0; i < combatantSlots.Length; i++)
        {
            var slot = combatantSlots[i];
            var character = slot.Character;
            if (character != null && !slot.isEnemy && slot.roomIndex == activeRoomIndex)
            {
                character.ElapsedTime = 0f;
                // Force slider update if CharacterUIBinder exists
                var characterMB = character as MonoBehaviour;
                if (characterMB != null)
                {
                    if (!binderCache.TryGetValue(characterMB, out var binder))
                    {
                        binder = characterMB.GetComponent<CharacterUIBinder>();
                        binderCache[characterMB] = binder;
                    }
                    if (binder != null)
                    {
                        binder.RefreshAll();
                    }
                }
            }
        }
        timersActive = true;
    }

    public void SetActiveRoomIndex(int roomIndex)
    {
        activeRoomIndex = roomIndex;
    }


    public ICharacter GetRandomPlayer()
    {
        var players = new System.Collections.Generic.List<ICharacter>();
        for (int i = 0; i < combatantSlots.Length; i++)
        {
            var c = combatantSlots[i].Character;
            if (c != null && !combatantSlots[i].isEnemy)
                players.Add(c);
        }
        if (players.Count == 0) return null;
        return players[Random.Range(0, players.Count)];
    }

    public bool IsAnyPlayerReadyInRoom(int roomIndex)
    {
        for (int i = 0; i < combatantSlots.Length; i++)
        {
            var slot = combatantSlots[i];
            var character = slot.Character;
            if (character != null && !slot.isEnemy && slot.roomIndex == roomIndex && character.ElapsedTime >= character.FillTime)
                return true;
        }
        return false;
    }

    public bool TryExecutePlayerActionInRoom(int roomIndex, PlayerAction action)
    {
        for (int i = 0; i < combatantSlots.Length; i++)
        {
            var slot = combatantSlots[i];
            var character = slot.Character;
            if (character == null || slot.isEnemy || slot.roomIndex != roomIndex) continue;
            if (character.ElapsedTime < character.FillTime) continue;

            string actorName = string.IsNullOrEmpty(character.characterName) ? $"Player{i}" : character.characterName;
            string verb = action == PlayerAction.Fight
                ? (string.IsNullOrEmpty(character.FightMessage) ? "FIGHT" : character.FightMessage)
                : (string.IsNullOrEmpty(character.RunMessage) ? "RUN" : character.RunMessage);
            Debug.Log($"[{actorName}] {verb} (room {roomIndex}, slot {i})");
            character.ElapsedTime = 0f;
            AfterCombatantAction();
            return true;
        }
        return false;
    }
}
