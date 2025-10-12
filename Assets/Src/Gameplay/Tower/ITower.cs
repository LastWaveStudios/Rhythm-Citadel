using UnityEngine;

public abstract class ITower
{
    private int damageType;
    private int range;  //Nº de tiles de alcance

    public abstract bool CheckRhythm();
    public abstract void Disable();
    public abstract void Shoot();
    public void SelectTarget() { }  //Implementar
}
