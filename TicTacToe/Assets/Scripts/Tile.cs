using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _botImg;
        [SerializeField] private Sprite _playerImg;
        private GameManager _gameManager;
        private Board _board;
        private Image _image;
        private Button _button;
        public Vector2Int XY { get; set; }
        public bool WasPressedByPlayer { get; private set; }

        public bool IsPressed()
        {
            return !_button.interactable;
        }

        public void Init()
        {
            _image.sprite = null;
            _button.interactable = true;
            WasPressedByPlayer = false;
        }

        public bool MakeMove(bool playerMove)
        {
            if (_button.interactable)
            {
                _button.interactable = false;
                WasPressedByPlayer = playerMove;
                UpdateImage();
                _board.TileUpdated(this);
                return true;
            }

            return false;
        }

        public void OnPressed()
        {
            MakeMove(true);
        }

        private void Awake()
        {
            _image = GetComponent<Image>();
            _button = GetComponent<Button>();
            _board = GetComponentInParent<Board>();
            _gameManager = GetComponentInParent<GameManager>();
        }

        private void UpdateImage()
        {
            _image.sprite = WasPressedByPlayer ? _playerImg : _botImg;
        }
    }
}