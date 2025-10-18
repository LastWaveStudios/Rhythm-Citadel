using Gameplay.World;
using System;
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
        protected TileBase _nextTile;
        protected Transform _position;

        private void Awake()
        {
            if (_position == null)
            {
                _position = transform;
                Debug.Log("Transform asignado");
            }
        }
        public void Update()
        {
            Debug.Log("Entrando a Update");
            Move();
        }
        public void Move()
        {
            Debug.Log("Valor del index" + _index);
            _nextTile = WorldManager.Instance.GetNextTile(_path, _index);
            if (_nextTile==null)
            {
                Destroy(this);
            } else
            {
                _index++;
                Tilemap tilemap = WorldManager.FindAnyObjectByType<Tilemap>();  //No se pueden obtener las coordenadas directamente de un TileBase, por lo que se necesita identificar el Tilemap
                Vector3Int _newCell;
                foreach (var cell in tilemap.cellBounds.allPositionsWithin)
                {
                    if (tilemap.GetTile(cell) == _nextTile)  //Se busca la celda que coincida con el tile indicado
                    {
                        Debug.Log("Asignando valor a _newCell");
                        _newCell = cell;
                        Vector3 _newPos = tilemap.GetCellCenterWorld(_newCell);
                        _position.position = _newPos;
                    }
                }
            }
            
        }
        public void Atack() { }
        public void TakeDamage() { }

    }
}

