using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public bool PlayerTurn { get; private set; }

        [SerializeField] private Board _boardPrefab;
        /*[SerializeField] private Transform _camera;*/
        [SerializeField] private Camera _camera;
        [SerializeField] private TextMeshProUGUI _winnerName;
        [SerializeField] private Canvas _gameFinishedCanvas;
        private Board _board;
        private GraphicRaycaster _graphicRaycaster;
        private Canvas _gameCanva;
        private StateMachine _stateMachine;
        private Bot _bot;
        private int _tilesInRow;
        private int _tilesInColumn;

        public GameManager()
        {
            _stateMachine = new StateMachine(this);
        }

        private void AnnounceWinner()
        {
            switch (_board.Winner)
            {
                case Board.GameResult.Player:
                    Debug.Log("Game finished: Player won!");
                    break;
                case Board.GameResult.Bot:
                    Debug.Log("Game finished: Bot won!");
                    break;
                case Board.GameResult.Draw:
                    Debug.Log("Game finished: Draw!");
                    break;
                default:
                    Debug.Log("Invalid GameResult!");
                    break;
            }
            _winnerName.text = _board.Winner.ToString();
            Debug.Log("DUPA"); 
            _gameFinishedCanvas.enabled = true;
        }

        public void MoveMade()
        {
            ProgressStateMachine();
        }

        private void ProgressStateMachine()
        {
            switch (_stateMachine.CurrentState)
            {
                case State.Init:
                    _stateMachine.MoveNext(Command.Start);
                    break;
                case State.PlayerTurnBegin:
                    _stateMachine.MoveNext(Command.Move);
                    ProgressStateMachine();
                    break;
                case State.PlayerTurnEnd:
                    if (_board.IsEnded) _stateMachine.MoveNext(Command.Finished);
                    else
                    {
                        _stateMachine.MoveNext(Command.NextTurn);
                        _bot.MakeMove();
                    }
                    break;
                case State.BotTurnBegin:
                    _stateMachine.MoveNext(Command.Move);
                    ProgressStateMachine();
                    break;
                case State.BotTurnEnd:
                    if (_board.IsEnded) _stateMachine.MoveNext(Command.Finished);
                    else _stateMachine.MoveNext(Command.NextTurn);
                    break;
                case State.GameOver:
                    _stateMachine.MoveNext(Command.Start);
                    break;

            }
        }
        private void ChangePlayerMoveRight(bool isAllowed)
        {
            _graphicRaycaster.enabled = isAllowed;

        }

        private void Awake()
        {
            _graphicRaycaster = GetComponent<GraphicRaycaster>();
            _gameCanva = GetComponent<Canvas>();
        }

        private void Start()
        {
            Play();
        }

        public void Play()
        {
            InitGame();
            _stateMachine.MoveNext(Command.Start);
        }

        public void Exit()
        {
            SceneManager.LoadScene("Menu");
        }
        private void InitGame()
        {
            Init();
            InitBoard();
            InitBot();
            InitCamera();
        }

        private void Init()
        {
            _tilesInRow = OptionsMenu.BoardSize;
            _tilesInColumn = OptionsMenu.BoardSize;
            _stateMachine.Reset();
            _gameFinishedCanvas.enabled = false;
            PlayerTurn = true;
        }

        private void InitCamera()
        {
            _camera.transform.position =
                new Vector3((float)_tilesInRow / 2 - 0.5f, (float)_tilesInColumn / 2 + 0.1f, -1* (_tilesInColumn));
        }

        private void InitBoard()
        {
            if (_board == null)
            {
                _board = Instantiate(_boardPrefab, new Vector3(0, 0), Quaternion.identity, this.transform);
            }

            _board.Init(_tilesInRow, _tilesInColumn);
        }

        private void InitBot()
        {
            if (_bot == null)
            {
                _bot = new Bot();
            }
            _bot.Init(_board);
        }


        private enum State
        {
            Init,
            PlayerTurnBegin,
            PlayerTurnEnd,
            BotTurnBegin,
            BotTurnEnd,
            GameOver
        }

        private enum Command
        {
            Start,
            Move,
            NextTurn,
            Finished
        }

        private class StateMachine
        {
            private GameManager _gameManager;
            Dictionary<StateTransition, Func<State>> _transitions;
            public State CurrentState { get; private set; }

            public StateMachine(GameManager gameManager)
            {
                _gameManager = gameManager;
                CurrentState = State.Init;
                _transitions = new Dictionary<StateTransition, Func<State>>
                {
                    {
                        new StateTransition(State.Init, Command.Start), () =>
                        {
                            _gameManager.InitGame();
                            _gameManager.ChangePlayerMoveRight(true);
                            return State.PlayerTurnBegin;
                        }
                    },
                    {
                        new StateTransition(State.PlayerTurnBegin, Command.Move), () =>
                        {
                            _gameManager.ChangePlayerMoveRight(false);
                            return State.PlayerTurnEnd;
                        }
                    },
                    {
                        new StateTransition(State.PlayerTurnEnd, Command.Finished), () =>
                        {
                            _gameManager.AnnounceWinner();
                            return State.GameOver;
                        }
                    },
                    {
                        new StateTransition(State.PlayerTurnEnd, Command.NextTurn), () =>
                        {
                            return State.BotTurnBegin;
                        }
                    },
                    {
                        new StateTransition(State.BotTurnBegin, Command.Move), () =>
                        {
                            return State.BotTurnEnd;
                        }
                    },
                    {
                        new StateTransition(State.BotTurnEnd, Command.Finished), () =>
                        {
                            _gameManager.AnnounceWinner();
                            return State.GameOver;
                        }
                    },
                    {
                        new StateTransition(State.BotTurnEnd, Command.NextTurn), () =>
                        {
                            _gameManager.ChangePlayerMoveRight(true);
                            return State.PlayerTurnBegin;
                        }
                    },
                    {
                        new StateTransition(State.GameOver, Command.Start), () =>
                        {
                            _gameManager.InitGame();
                            _gameManager.ChangePlayerMoveRight(true);
                            return State.PlayerTurnBegin;
                        }
                    },
                };
            }

            private State GetNext(Command command)
            {
                StateTransition transition = new StateTransition(CurrentState, command);
                Func<State> onTransition;
                if (!_transitions.TryGetValue(transition, out onTransition))
                {
                    throw new Exception($"Invalid transition from '{CurrentState}' with command '{command}'");
                }

                State nextState = onTransition();
                Debug.Log(
                    $"Transitioning game state from '{CurrentState}' to '{nextState}'");

                return nextState;
            }

            public State MoveNext(Command command)
            {
                CurrentState = GetNext(command);
                return CurrentState;
            }

            public void Reset()
            {
                CurrentState = State.Init;
            }

            private class StateTransition
            {
                private readonly State _currentState;
                private readonly Command _command;

                public StateTransition(State currentState, Command command)
                {
                    _currentState = currentState;
                    _command = command;
                }

                public override int GetHashCode()
                {
                    return 17 + 31 * _currentState.GetHashCode() + 31 * _command.GetHashCode();
                }

                public override bool Equals(object obj)
                {
                    StateTransition other = obj as StateTransition;
                    return other != null && this._currentState == other._currentState && _command == other._command;
                }
            }
        }

    }
}