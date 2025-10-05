using UnityEngine;

namespace c1a_proy.rpg.rpg.Assets.scripts
{
    
    public class CharacterUIAutoBind : MonoBehaviour
    {
        [Header("Character reference (must implement ICharacter)")]
        [SerializeField] private MonoBehaviour characterBehaviour; 
        
        [SerializeField] private CharacterUIBinder binder;
        
        public MonoBehaviour CharacterBehaviour => characterBehaviour;

        private void Reset()
        {
            if (!binder) binder = GetComponent<CharacterUIBinder>();
        }

        private void Awake()
        {
            if (!binder) binder = GetComponent<CharacterUIBinder>();
            TryBind();
        }

        public void SetCharacter(MonoBehaviour behaviour)
        {
            characterBehaviour = behaviour;
            TryBind();
        }

        private void TryBind()
        {
            if (binder == null)
            {
                Debug.LogWarning($"[CharacterUIAutoBind] cant find CharacterUIBinder in {name}");
                return;
            }
            if (characterBehaviour is ICharacter c)
            {
                binder.Bind(c);
            }
            else if (characterBehaviour != null)
            {
                Debug.LogWarning($"[CharacterUIAutoBind] the object doesnt implement ICharacter: {characterBehaviour.name}");
            }
        }
    }
}
