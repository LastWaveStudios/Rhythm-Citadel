using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public abstract class IEnemy
{
    //Entiendo que no es [SerializeField] porque estos datos se asignaran en cuanto se generen y no en el inspector
    private int health;
    private float speed;
    private int damage;
    private int damageType;
    //private Path path;

    public void move() { }  //Implementar desplazamiento cuando el path este en la rama principal
    public void atack() { }
    public void TakeDamage() { }

    //Enum de DamageTypeEnum
}
