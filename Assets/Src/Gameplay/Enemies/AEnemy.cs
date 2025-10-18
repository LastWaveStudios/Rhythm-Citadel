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
        protected TileBase _nextTile;
        protected Transform _position;
        protected int indicePrueba=0;
        [SerializeField] Tilemap tilemap;

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
        /*
        public void UpdateFixd()
        {
            Debug.Log("Entrando a Update");
            Move();
        }*/
       IEnumerator Move()
        {
            Debug.Log("Valor del index" + _index);
            Debug.Log("Indice de la corutina " + indicePrueba);
            _nextTile = WorldManager.Instance.GetNextTile(_path, _index);
            _index++;
            if (_nextTile==null)
            {
                Debug.Log("Objecto destruido");
                Destroy(gameObject);
            } else
            {
                //Tilemap tilemap = WorldManager.FindAnyObjectByType<Tilemap>();  //No se pueden obtener las coordenadas directamente de un TileBase, por lo que se necesita identificar el Tilemap
                //Debug.Log("HASTA AQUI LLEGA");
                Vector3Int _newCell;
                foreach (var cell in tilemap.cellBounds.allPositionsWithin)
                {
                    Debug.Log("HASTA AQUI LLEGA");
                    if (tilemap.GetTile(cell) == _nextTile)  //Se busca la celda que coincida con el tile indicado
                    {
                        _newCell = cell;
                        Debug.Log("Asignando valor a _newCell" + _newCell);
                        Vector3 _newPos = tilemap.GetCellCenterWorld(_newCell);
                        _position.position = _newPos;
                        yield return new WaitForSeconds(2f);
                    }
                }
            }
            indicePrueba++;
            Debug.Log("Indice de la corutina aumentado " + indicePrueba);
                
        }
        public void Atack() { }
        public void TakeDamage() { }

    }
}

