
namespace UI.Menus.States
{
    public interface IMenuState
    {
        public void Enter();

        public void Update(float deltaTime);

        public void Exit();
    }
}


