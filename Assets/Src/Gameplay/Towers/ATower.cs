

namespace Gameplay.Towers
{
    public abstract class ATower
    {
        protected int _damageType;
        protected int _range;  //Numero de tiles de alcance

        public abstract bool CheckRhythm();
        public abstract void Disable();
        public abstract void Shoot();
        public void SelectTarget() { }  //Implementar
    }

}
