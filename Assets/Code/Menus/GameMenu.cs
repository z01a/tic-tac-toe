using System;
using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : Menu
{
    [Header("Game Menu")]
    [SerializeField]
    [Tooltip("Reference to the game board panel")]
    protected GameObject _gameBoardPanel;

    [Tooltip("Reference to the game board cell prefab")]
    [SerializeField]
    protected GameObject _gameBoardCellPrefab;

    protected Button[,] _buttonGrid;

    protected uint _playerMoveCount = 0;

    [SerializeField]
    [Header("Game Menu Buttons")]
    [Tooltip("Button to go back")]
    public Button backButton;

    [SerializeField]
    [Header("Game Menu Text")]
    [Tooltip("Info text for the game")]
    public GameObject infoText;

    [SerializeField]
    [Tooltip("Text for timer")]
    public GameObject timerText;

    void Start()
    {
        GameManager.Instance.OnGameStarted += OnGameStarted;
        GameManager.Instance.OnGameEnded += OnGameEnded;
        GameManager.Instance.OnPlayerChanged += OnPlayerChanged;
        GameManager.Instance.OnCellPlayed += OnCellPlayed;
        GameManager.Instance.OnPlayerWon += OnPlayerWon;
        GameManager.Instance.OnGameReset += OnGameReset;

        AddButtonOnClickListener(backButton, OnBackButtonClicked);

        InitializeBoard();

    }

    private void OnGameReset()
    {
        _buttonGrid = null;
        _playerMoveCount = 0;

        UnInitializeBoard();
        InitializeBoard();
    }

    private void UnInitializeBoard()
    {
        UnInstantiateCells();
    }

    private void UnInstantiateCells()
    {
        foreach (Transform child in _gameBoardPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void Update()
    {
        UpdateInfo();
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        if (timerText != null && GameManager.Instance.GameStartTime != DateTime.MinValue)
        {
            if (timerText.TryGetComponent<TextMeshProUGUI>(out var text))
            {
                text.text = $"{GameManager.Instance.ElapsedTime:ss\\.ff}";
            }
            else
            {
                Log.Warn("Timer text is not assigned in the inspector.");
            }
        }
        else
        {
            Log.Warn("Timer text is not assigned in the inspector.");
        }
    }

    private void UpdateInfo()
    {
        if (infoText != null)
        {
            if (infoText.TryGetComponent<TextMeshProUGUI>(out var text))
            {
                text.text = $"{_playerMoveCount} moves and player {GameManager.Instance.CurrentPlayer} turn";
            }
            else
            {
                Log.Warn("Info text is not text mesh pro.");
            }
        }
        else
        {
            Log.Warn("Info text is not assigned in the inspector.");
        }
    }

    private void OnPlayerWon(Player player)
    {
        // TODO: Show popup with the winner
        Log.Info($"Player {player} won the game");
    }

    private void InitializeBoard()
    {
        InitializeGrid();
        InstantiateCells();
    }

    private void InitializeGrid()
    {
        if (_gameBoardPanel != null)
        {
            GridLayoutGroup gridLayoutGroup = _gameBoardPanel.GetComponent<GridLayoutGroup>();
            if (gridLayoutGroup != null)
            {
                gridLayoutGroup.constraintCount = (int)GameManager.Instance.BoardSize;

                float panelWidth = _gameBoardPanel.GetComponent<RectTransform>().rect.width;
                float panelHeight = _gameBoardPanel.GetComponent<RectTransform>().rect.height;
                
                float cellWidth = (panelWidth - (gridLayoutGroup.spacing.x * (GameManager.Instance.BoardSize - 1))) / GameManager.Instance.BoardSize;
                float cellHeight = (panelHeight - (gridLayoutGroup.spacing.y * (GameManager.Instance.BoardSize - 1))) / GameManager.Instance.BoardSize;
                
                gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);

                // TODO: Set spacing based on the number of cells
                gridLayoutGroup.spacing = new Vector2(5, 5);
            }
        }
        else
        {
            Log.Warn("Game board panel is not assigned in the inspector.");
        }
    }

    private void InstantiateCells()
    {
        _buttonGrid = new Button[GameManager.Instance.BoardSize, GameManager.Instance.BoardSize];

        for (uint i = 0; i < GameManager.Instance.BoardSize * GameManager.Instance.BoardSize; i++)
        {
            GameObject cell = Instantiate(_gameBoardCellPrefab, _gameBoardPanel.transform);
            cell.name = $"Cell {i}";
                    
            if (cell.TryGetComponent<Button>(out var cellButton))
            {
                uint x = i / GameManager.Instance.BoardSize;
                uint y = i % GameManager.Instance.BoardSize;

                _buttonGrid[x, y] = cellButton;

                cellButton.onClick.AddListener(() =>
                {
                    GameManager.Instance.Play(x, y);
                });
            }
        }
    }

    private void AddButtonOnClickListener(Button button, Action callback)
    {
        if (button != null)
        {
            button.onClick.AddListener(() => callback());
        }
        else
        {
            Log.Warn($"Button {button.name} is not assigned in the inspector.");
        }
    }

    private void OnCellPlayed(int x, int y, Player player)
    {
        Log.Info($"Cell played at ({x}, {y})");
        _playerMoveCount++;
        
        if (_buttonGrid[x, y] != null)
        {
            _buttonGrid[x, y].interactable = false;

            TextMeshProUGUI text = _buttonGrid[x, y].GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = player == Player.X ? "X" : "O";
                text.color = player == Player.X ? GameManager.Instance.PlayerXColor : GameManager.Instance.PlayerOColor;
            }
        }
        else
        {
            Log.Warn($"Button at ({x}, {y}) is not assigned in the grid.");
        }
    }

    private void OnPlayerChanged(Player player)
    {
        // TODO: Maybe show some icon who is playing?
        Log.Info($"Player changed to {player}");
        // throw new NotImplementedException();
    }

    private void OnGameEnded()
    {
        Log.Info("Game ended");
        GameManager.Instance.Reset();
        // throw new NotImplementedException();
    }

    private void OnGameStarted()
    {
        // TODO: We need to start timer here!
        Log.Info("Game started");
        // throw new NotImplementedException();
    }

    private void OnBackButtonClicked()
    {
        MenuManager.Instance.NavigateBack();
    }
}
