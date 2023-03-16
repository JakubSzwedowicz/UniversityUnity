using System;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    internal class Bot
    {
        private Board _board;
        private int _tilesInRow, _tilesInColumn;
        private Random _random;

        public Bot()
        {
            _random = new Random();
        }

        public void Init(Board board)
        {
            _board = board;
            _tilesInRow = board.TilesInRow;
            _tilesInColumn = board.TilesInColumn;
        }

        public void MakeMove()
        {
            Vector2Int move;
            do
            {
                move = FindPressedTile();
            } while (!_board.Tiles[move.x, move.y].MakeMove(false));
        }

        private Vector2Int FindPressedTile()
        {
            Tile last = _board.LastPressedTile;
            const int offset = 3;
            int x = 0;
            int y = 0;
            x = _random.Next(Math.Max(last.XY.x - offset, 0), Math.Min(last.XY.x + offset, _tilesInRow));
            y = _random.Next(Math.Max(last.XY.y - offset, 0), Math.Min(last.XY.y + offset, _tilesInColumn));
            return new Vector2Int(x, y);
        }
    }
}
