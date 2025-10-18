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
        [SerializeField] Tilemap tilemap;   //No se pueden obtener las coordenadas directamente de un TileBase, por lo que se necesita identificar el Tilemap

        private void Awake()
        {
            if (_position == null)
            {
                _position = transform;
            }
            tilemap = WorldManager.FindAnyObjectByType<Tilemap>();
        }
        private void Start()
        {
            {
                StartCoroutine(Move());
            }
        }
       IEnumerator Move()
        {
            bool moving = true;
            while (moving)
            {
                _nextTile = WorldManager.Instance.GetNextTile(_path, _index);
                _index++;
                Vector3Int _null = new Vector3Int(0, 0, 1);
                if (_nextTile == _null)
                {
                    moving = false;
                    Destroy(gameObject);
                }
                else
                {
                    Vector3 _newPos = tilemap.GetCellCenterWorld(_nextTile);
                    _position.position = _newPos;
                }
                yield return new WaitForSeconds(1f);
            }
        }
        public void Atack() { }
        public void TakeDamage() { }

    }
}

