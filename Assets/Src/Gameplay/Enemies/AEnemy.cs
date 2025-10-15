


namespace Gameplay.Enemies
{
    public abstract class AEnemy
    {
        //Entiendo que no es [SerializeField] porque estos datos se asignaran en cuanto se generen y no en el inspector
        protected int _health;
        protected float _speed;
        protected int _damage;
        protected int _damageType;
        //private Path path;

        public void Move() { }  //Implementar desplazamiento cuando el path este en la rama principal
        public void Atack() { }
        public void TakeDamage() { }

        //Enum de DamageTypeEnum
    }
}

