using System;
using UnityEngine;

public enum Player
{
        O = 0,
        X = 1,
        Invalid = -1
}

class GameManager : Singleton<GameManager>
{
    [Header("Game Manager")]
    [SerializeField]
    private Player _startingPlayer = Player.X;

    public Action<int, int, Player> OnCellPlayed;
    public Action OnGameStarted;
    public Action OnGameReset;
    public Action OnGameEnded;
    public Action<Player> OnPlayerChanged;
    public Action<Player> OnPlayerWon;
    public Color PlayerXColor { get; private set; } = Color.red;

    public Color PlayerOColor { get; private set; } = Color.blue;

    public uint BoardSize = 3;
    public Player CurrentPlayer { get; private set; } = Player.Invalid;

    private Board _board;

    private bool _hasGameStarted = false;

    private float _elapsedTIme = 0f;
    private DateTime _gameStartTime;
    public DateTime GameStartTime => _gameStartTime;
    private DateTime _gameEndTime;

    public TimeSpan ElapsedTime => DateTime.Now - _gameStartTime;
    void Start()
    {
        InitializeBoard(BoardSize);
        CurrentPlayer = _startingPlayer;
    }

    private void InitializeBoard(uint size)
    {
        _board = new Board(size);
    }

    public void Play(uint x, uint y)
    {
        if (CurrentPlayer == Player.Invalid)
        {
            Log.Error("Invalid player or game state");
            return;
        }

        if (IsMoveValid((int)x, (int)y))
        {
            _board.SetCell((int)x, (int)y, (int)CurrentPlayer);

            OnCellPlayed?.Invoke((int)x, (int)y, CurrentPlayer);

            if (IsGameOver())
            {
                _gameEndTime = DateTime.Now;
                // _gameDuration = _gameEndTime - _gameStartTime;
                OnPlayerWon?.Invoke(CurrentPlayer);
                OnGameEnded?.Invoke();
            }
            else
            {
                if(!_hasGameStarted)
                {
                    _hasGameStarted = true;

                    _gameStartTime = DateTime.Now;
                    OnGameStarted?.Invoke();
                }

                CurrentPlayer = CurrentPlayer == Player.X ? Player.O : Player.X;
                OnPlayerChanged?.Invoke(CurrentPlayer);
            }
        }
    }

    public bool IsMoveValid(int x, int y)
    {
        if (x < 0 || x >= _board.GetLength(0) || y < 0 || y >= _board.GetLength(1))
        {
            return false;
        }

        return _board.GetCell(x, y) == (int)Player.Invalid;
    }

    public bool IsGameOver()
    {
        return CheckWin(Player.X) || CheckWin(Player.O) || IsBoardFull();
    }

    public void Reset()
    {
        InitializeBoard(BoardSize);
        CurrentPlayer = _startingPlayer;

        _hasGameStarted = false;
        _gameStartTime = DateTime.MinValue;

        OnGameReset?.Invoke();
        OnPlayerChanged?.Invoke(CurrentPlayer);
    }

    private bool CheckWin(Player player)
    {
        int size = _board.GetLength(0);

        // Check rows and columns
        for (int i = 0; i < size; i++)
        {
            if (CheckRow(i, player) || CheckColumn(i, player))
            {
                return true;
            }
        }

        // Check diagonals
        return CheckDiagonal(player) || CheckAntiDiagonal(player);
    }

    private bool CheckRow(int row, Player player)
    {
        for (int col = 0; col < _board.GetLength(1); col++)
        {
            if (_board.GetCell(row, col) != (int)player)
            {
                return false;
            }
        }
        return true;
    }

    private bool CheckColumn(int col, Player player)
    {
        for (int row = 0; row < _board.GetLength(0); row++)
        {
            if (_board.GetCell(row, col) != (int)player)
            {
                return false;
            }
        }
        return true;
    }

    private bool CheckDiagonal(Player player)
    {
        for (int i = 0; i < _board.GetLength(0); i++)
        {
            if (_board.GetCell(i, i) != (int)player)
            {
                return false;
            }
        }
        return true;
    }

    private bool CheckAntiDiagonal(Player player)
    {
        int size = _board.GetLength(0);
        for (int i = 0; i < size; i++)
        {
            if (_board.GetCell(i, size - i - 1) != (int)player)
            {
                return false;
            }
        }
        return true;
    }

    private bool IsBoardFull()
    {
        for (int x = 0; x < _board.GetLength(0); x++)
        {
            for (int y = 0; y < _board.GetLength(1); y++)
            {
                if (_board.GetCell(x, y) == (int)Player.Invalid)
                {
                    return false;
                }
            }
        }
        return true;
    }
}

internal class Board
{
    private readonly int[,] _board;

    public int GetLength(int dimension)
    {
        return _board.GetLength(dimension);
    }
    public Board(uint size)
    {
        _board = new int[size, size];

        for (int x = 0; x < _board.GetLength(0); x++)
        {
            for (int y = 0; y < _board.GetLength(1); y++)
            {
                _board[x, y] = -1;
            }
        }
    }

    public void SetCell(int x, int y, int value)
    {
        if (x < 0 || x >= _board.GetLength(0) || y < 0 || y >= _board.GetLength(1))
        {
            throw new System.ArgumentOutOfRangeException("Invalid cell coordinates");
        }

        if (_board[x, y] != -1)
        {
            throw new System.InvalidOperationException("Cell already occupied");
        }

        _board[x, y] = value;
    }

    public int GetCell(int x, int y)
    {
        if (x < 0 || x >= _board.GetLength(0) || y < 0 || y >= _board.GetLength(1))
        {
            throw new System.ArgumentOutOfRangeException("Invalid cell coordinates");
        }

        return _board[x, y];
    }
}