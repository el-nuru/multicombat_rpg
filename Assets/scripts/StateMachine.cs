namespace c1a_proy.rpg.rpg.Assets.scripts
{
    public interface IState
    {
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }

    public class StateMachine
    {
        private IState _current;

        public IState Current => _current;

        public void ChangeState(IState next)
        {
            _current?.OnExit();
            _current = next;
            _current?.OnEnter();
        }

        public void Update() => _current?.OnUpdate();
    }
}
