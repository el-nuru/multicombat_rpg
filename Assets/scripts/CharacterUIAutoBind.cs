using UnityEngine;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    public class CharacterUIAutoBind : MonoBehaviour
    {
        [Header("Character reference (must implement ICharacter)")]
        [SerializeField] private MonoBehaviour characterBehaviour;
        [SerializeField] private CharacterUIBinder binder;

        private void Reset() => binder = GetComponent<CharacterUIBinder>();

        private void Awake()
        {
            if (!binder) binder = GetComponent<CharacterUIBinder>();
            TryBind();
        }

        private void TryBind()
        {
            if (binder == null)
            {
                Debug.LogWarning($"[CharacterUIAutoBind] Can't find CharacterUIBinder on {name}");
                return;
            }
            if (characterBehaviour is ICharacter c)
                binder.Bind(c);
            else if (characterBehaviour != null)
                Debug.LogWarning($"[CharacterUIAutoBind] {characterBehaviour.name} doesn't implement ICharacter");
        }
    }
}
