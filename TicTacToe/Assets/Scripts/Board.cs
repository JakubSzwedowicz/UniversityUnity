using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Board : MonoBehaviour
    {
        public Tile[,] Tiles { get; private set; }
        public int TilesInRow { get; private set; }
        public int TilesInColumn { get; private set; }

        public bool IsEnded { get; private set; }
        public bool IsPlayerAWinner { get; private set; }
        public Tile LastPressedTile { get; private set; }

        [SerializeField] private GameObject _backgroundPrefab;
        [SerializeField] private Tile _tilePrefab;
        private GameManager _gameManager;
        private GameObject _background;
        public GameResult Winner{ get; private set; }
        private int _unpressedTiles;
        private readonly float _spacingBetweenTiles;

        public Board()
        {
            Tiles = new Tile[0, 0];
            _spacingBetweenTiles = 0.1f;
            TilesInRow = 0;
            TilesInColumn = 0;
            _unpressedTiles = 0;
            Winner = GameResult.None;
            IsEnded = false;
        }

        public void Init(int tilesInRow, int tilesInColumn)
        {
            bool newBoard = (TilesInRow != tilesInRow) && (TilesInColumn != tilesInColumn);
            TilesInRow = tilesInRow;
            TilesInColumn = tilesInColumn;
            _unpressedTiles = tilesInColumn * tilesInRow;
            Winner = GameResult.None;
            IsEnded = false;
            InitFields(newBoard);
            InitBackground(newBoard);
        }

        public void TileUpdated(Tile updatedTile)
        {
            if (updatedTile.IsPressed())
            {
                _unpressedTiles--;
                LastPressedTile = updatedTile;
                CheckIfGameEnded();
                _gameManager.MoveMade();
            }
            else
            {
                Debug.LogError("Called TileUpdated when button is not pressed!");
            }
        }

        private void CheckIfGameEnded()
        {
            int x = LastPressedTile.XY.x;
            int y = LastPressedTile.XY.y;
            bool wasPressedByPlayer = LastPressedTile.WasPressedByPlayer;
            var hasWon = CheckIfWonInColumn(x, y, wasPressedByPlayer) || CheckIfWonInRow(x, y, wasPressedByPlayer) || CheckIfWonInDiagonal(x, y, wasPressedByPlayer);
            if (hasWon)
            {
                Winner = wasPressedByPlayer ? GameResult.Player : GameResult.Bot;
                IsEnded = true;
            }
            else if (_unpressedTiles == 0)
            {
                Winner = GameResult.Draw;
                IsEnded = true;
            }
        }

        private void Awake()
        {
            _gameManager = GetComponentInParent<GameManager>();
        }

        private void InitFields(bool instantiate)
        {
            if (instantiate)
            {
                foreach (var tile in Tiles)
                {
                    Destroy(tile);
                }

                Tiles = new Tile[TilesInRow, TilesInColumn];
            }

            for (int x = 0; x < TilesInRow; x++)
            {
                for (int y = 0; y < TilesInColumn; y++)
                {
                    if (instantiate)
                    {
                        Tiles[x, y] = Instantiate(_tilePrefab,
                            new Vector3((x + 1) * _spacingBetweenTiles + x, (y + 1) * _spacingBetweenTiles + y),
                            Quaternion.identity, this.transform);
                        Tiles[x, y].name = $"Tile {x} {y}";
                        Tiles[x, y].XY = new Vector2Int(x, y);
                    }

                    Tiles[x, y].Init();
                }
            }
        }

        private void InitBackground(bool instantiate)
        {
            if (instantiate)
            {
                if (_background)
                {
                    Destroy(_background);
                }

                var tilesCenterX = (_tilePrefab.transform.localScale.x / 2) * (TilesInRow - 1);
                var tilesCenterY = (_tilePrefab.transform.localScale.y / 2) * (TilesInColumn - 1);
                var spacingCenterX = (_spacingBetweenTiles) * ((float)(TilesInRow + 1) / 2);
                var spacingCenterY = (_spacingBetweenTiles) * ((float)(TilesInColumn + 1) / 2);

                var center = new Vector2(tilesCenterX + spacingCenterX, tilesCenterY + spacingCenterY);
                _background = Instantiate(_backgroundPrefab, center, Quaternion.identity, this.transform);
                //background.transform.localPosition = new Vector3(center.x, center.y, -10);
                _background.transform.localScale = new Vector3(TilesInRow + (TilesInRow + 1) * _spacingBetweenTiles,
                    TilesInColumn + (TilesInColumn + 1) * _spacingBetweenTiles, 1);
            }
        }

        private bool CheckIfWonInRow(int x, int y, bool wasPressedByPlayer)
        {
            int lowerBound = Math.Max(x - 4, 0);
            int upperBound = Math.Min(x + 4, TilesInRow - 1);
            int count = 0;
            for (int i = lowerBound; i <= upperBound; i++)
            {
                IncreaseCountIfMatches(ref count, i, y, wasPressedByPlayer);
                if (count == 5)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckIfWonInColumn(int x, int y, bool wasPressedByPlayer)
        {
            int lowerBound = Math.Max(y - 4, 0);
            int upperBound = Math.Min(y + 4, TilesInColumn - 1);
            int count = 0;
            for (int i = lowerBound; i <= upperBound; i++)
            {
                IncreaseCountIfMatches(ref count, x, i, wasPressedByPlayer);
                if (count == 5)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckIfWonInDiagonal(int x, int y, bool wasPressedByPlayer)
        {
            int leftDownDistance = Math.Min(Math.Min(x, y), 4);
            int distanceTillRight = TilesInRow - x - 1;
            int rightDownDistance = Math.Min(Math.Min(distanceTillRight, y), TilesInRow - 1);
            Vector2Int leftDownXY = new Vector2Int(x - leftDownDistance, y - leftDownDistance);
            Vector2Int rightDownXY = new Vector2Int(x + rightDownDistance, y - rightDownDistance);
            int count = 0;

            const int maxIter = 10;
            for (int i = 0; leftDownXY.x < TilesInRow && leftDownXY.y < TilesInColumn && i < maxIter; leftDownXY.x++, leftDownXY.y++, i++)
            {
                Debug.Log(String.Format("Checking leftDown ({0},{1})", leftDownXY.x, leftDownXY.y));
                IncreaseCountIfMatches(ref count, leftDownXY.x, leftDownXY.y, wasPressedByPlayer);
                if (count == 5)
                {
                    return true;
                }
            }
            for (int i =0; rightDownXY.x >= 0 && rightDownXY.y < TilesInColumn && i < maxIter; rightDownXY.x--, rightDownXY.y++, i++)
            {
                Debug.Log(String.Format("Checking rightDown ({0},{1})", rightDownXY.x, rightDownXY.y));
                IncreaseCountIfMatches(ref count, rightDownXY.x, rightDownXY.y, wasPressedByPlayer);
                if (count == 5)
                {
                    return true;
                }
            }
            return false;
        }

        private void IncreaseCountIfMatches(ref int count, int x, int y, bool wasPressedByPlayer)
        {
            if (Tiles[x, y].IsPressed() && Tiles[x, y].WasPressedByPlayer == wasPressedByPlayer)
            {
                count++;
            }
            else
            {
                count = 0;
            }
        }
        public enum GameResult
        {
            None,
            Player,
            Bot,
            Draw,
        }
    }
}