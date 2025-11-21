namespace Gameplay.Enemies.Behaviours
{
    // A Perception for Death, basically this must be implemented by all the enemies and
    // must change the IsAlive value to false and call the PushDeath method on his Behaviour (just Update 2 times for act immediate)
    public interface IPushDeathPerception
    {
        public void PushDeath();
    }
}