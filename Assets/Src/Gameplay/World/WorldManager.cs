using Gameplay.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Gameplay.World
{
    public class WorldManager : Utilities.ServiceLocator.AService
    {

        [SerializeField] private List<GameObject> _pathObjects;
        [SerializeField] private Tilemap _pathTilemap;
        [SerializeField] private Tilemap _highlightTilemap;

        private List<Path> _paths;

        public override void Init()
        {
            _paths = new List<Path>();
            InitPaths();
            if (_pathTilemap == null) Debug.LogWarning("There is no path tilemap added in editor");
            if (_highlightTilemap == null) Debug.LogWarning("There is no hghlight tilemap added in editor");
            
        }

        public Vector3Int GetNextTile(int pathID, int currentIndex)
        {
            return _paths[pathID].GetTile(currentIndex + 1);
        }

        public Vector3Int GetRandomTile(Vector3 position)
        {
            Vector3Int currentPos = GetCellFromWorldPos(position);
            int direction = UnityEngine.Random.Range(0, 4);
            switch (direction)
            {
                case 0:
                    return (currentPos + new Vector3Int(0, 1, 0));
                case 1:
                    return (currentPos + new Vector3Int(0, -1, 0));
                case 2:
                    return (currentPos + new Vector3Int(1, 0, 0));
                case 3:
                    return (currentPos + new Vector3Int(-1, 0, 0));
            }
            return currentPos;
        }

        public Vector3Int GetTile(int pathID, int index)
        {
            return _paths[pathID].GetTile(index);
        }

        public Vector3 GetCellCenterWorld(Vector3Int CellCoordinates)
        {
            return _pathTilemap.GetCellCenterWorld(CellCoordinates);
        }

        public Vector3Int GetCellFromWorldPos(Vector3 Pos)
        {
            return _pathTilemap.WorldToCell(Pos);
        }

        void InitPaths()
        {
            foreach (GameObject pathObject in _pathObjects)
            {
                _paths.Add(new Path(pathObject));
            }

        }

        public List<Vector3Int> GetSpawnPoints()
        {
            List<Vector3Int> spawnPointsList = new List<Vector3Int>();
            foreach (Path pathObject in _paths)
            {
                spawnPointsList.Add(pathObject.GetSpawnPoint());
            }
            return spawnPointsList;
        }

        public int GetTileCount(int pathID)
        {
            return _paths[pathID].GetTileCount();
        }


        public Vector3Int GetLastTile(int pathID)
        {
            return _paths[pathID].GetTile(_paths[pathID].TilesCount - 1);
        }

        public Vector3 GetTileSize()
        {
            return _pathTilemap.layoutGrid.cellSize;
        }

        public List<Vector3Int> GetTilesInRange(Vector3Int center, int range)
        {
            List<Vector3Int> tilesInRange = new List<Vector3Int>();

            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y <= range; y++)
                {
                    if (Mathf.Abs(x) + Mathf.Abs(y) <= range)
                    {
                        Vector3Int selectedTile = new Vector3Int(center.x + x, center.y + y, center.z);
                        tilesInRange.Add(selectedTile);
                    }

                }
            }
            return tilesInRange;
        }

        public bool IsPositionOnPathTileMap(Vector3Int position)
        {
            return _pathTilemap.HasTile(position);
        }

        #region -------------------- HIGHLIGHT --------------------
        /// <summary>
        /// We have a dictionary so we can reach every tile courutine
        /// If the couritine is in the dictionary, we dont need to add it
        /// When its called the clear highlight we stop the courutine and remove the highlight
        /// 
        /// </summary>
        [Header("Highlight options")]
        private Dictionary<Vector3Int, Coroutine> _activeHighlights = new Dictionary<Vector3Int, Coroutine>();
        [SerializeField] private float TIMETOMAXALPHA = 0.5f;
        [SerializeField] private float TIMETOWAIT = 0.5f;
        [SerializeField] private float MINALPHA = 1f;
        [SerializeField] private float MAXALPHA = 55f;

        public void Highlight(Vector3Int tile, Color? color = null)
        {
            if (_activeHighlights.ContainsKey(tile)) return;

            _highlightTilemap.SetTileFlags(tile, TileFlags.None);
            if (color == null) color = _highlightTilemap.GetColor(tile);

            Coroutine c = StartCoroutine(HighlightRoutine(tile, color.Value));
            _activeHighlights.Add(tile, c);
        }


        public void ClearHighlight(Vector3Int tile)
        {
            if (_activeHighlights.ContainsKey(tile))
            {
                StopCoroutine(_activeHighlights[tile]);
                _activeHighlights.Remove(tile);
            }

            _highlightTilemap.SetColor(tile, Color.white);
        }

        /// <summary>
        /// 
        /// First we fade in the color
        /// Then we wait for a second
        /// We fade out the color
        /// 
        /// We can use the same courutine from 0 to 55 as 55 to 0 so just just call lerp Alpha
        /// </summary>
        private IEnumerator HighlightRoutine(Vector3Int tile, Color color)
        {

            while (true)
            {
                yield return LerpAlpha(tile, MINALPHA, MAXALPHA, TIMETOMAXALPHA, color);

                yield return new WaitForSeconds(TIMETOWAIT);

                yield return LerpAlpha(tile, MAXALPHA, MINALPHA, TIMETOMAXALPHA, color);
            }
        }

        private IEnumerator LerpAlpha(Vector3Int tile, float from, float to, float duration, Color color)
        {
            float t = 0f;

            //Color c = _highlightTilemap.GetColor(tile);
            Color c = color;
            while (t < duration)
            {
                t += Time.deltaTime;
                float a = Mathf.Lerp(from, to, t / duration);

                c.a = a;
                _highlightTilemap.SetColor(tile, c);

                yield return null;
            }

            c.a = to;
            _highlightTilemap.SetColor(tile, c);
        }


        #endregion
    }
}


