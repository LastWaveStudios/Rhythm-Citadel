using Gameplay.World;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Gameplay.Enemies
{
    public class AEnemy: MonoBehaviour  //Para poder probarlo he quitado que sea una clase abstracta
    {
        protected int _health;
        protected float _speed;
        protected int _damage;
        protected int _damageType;
        protected int _path = 0;    //Valor del path al que accede
        protected int _index = 0;   //Numero del tile actual
        protected Vector3Int _nextTile;
        protected Transform _position;
        protected int indicePrueba=0;
        [SerializeField] Tilemap tilemap;   //No se pueden obtener las coordenadas directamente de un TileBase, por lo que se necesita identificar el Tilemap

        private void Awake()
        {
            if (_position == null)
            {
                _position = transform;
                Debug.Log("Transform asignado");
            }
        }
        private void Start()
        {
            {
                Debug.Log("Entrando a corrutina "+Time.time);
                StartCoroutine(Move());
            }
        }
       IEnumerator Move()
        {
            _nextTile = WorldManager.Instance.GetNextTile(_path, _index);
            _index++;
            Debug.Log("El valor de _nextTile es de " + _nextTile);
            if (_nextTile==null)
            {
                Debug.Log("Objecto destruido");
                Destroy(gameObject);
            } else
            {
                Vector3 _newPos = tilemap.GetCellCenterWorld(_nextTile);
                Debug.Log("La nueva posicion es "+ _newPos);
                _position.position = _newPos;
                yield return new WaitForSeconds(2f);
            }
            indicePrueba++;
            Debug.Log("Indice de la corutina aumentado " + indicePrueba);
                
        }
        public void Atack() { }
        public void TakeDamage() { }

    }
}

