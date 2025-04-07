using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : Menu
{
    [Header("Game Menu")]
    [Tooltip("Reference to the game board panel")]
    [SerializeField]
    private GameObject _gameBoardPanel;

    [Tooltip("Reference to the game board cell prefab")]
    [SerializeField]
    protected GameObject _gameBoardCellPrefab;

    protected Button[,] _buttonGrid;

    [Header("Game Menu Buttons")]
    [Tooltip("Button to go back")]
    [SerializeField]
    public Button backButton;

    [Header("Game Menu Text")]
    [Tooltip("Info text for the game")]
    [SerializeField]
    public TextMeshProUGUI infoText;

    [Tooltip("Text for timer")]
    [SerializeField]
    public TextMeshProUGUI timerText;

    void Start()
    {
        GameManager.Instance.OnGameEnded += OnGameEnded;
        GameManager.Instance.OnCellPlayed += OnCellPlayed;
        GameManager.Instance.OnGameReset += OnGameReset;

        AddButtonOnClickListener(backButton, OnBackButtonClicked);

        InitializeBoard();
    }

    private void OnGameEnded(Player player)
    {
        string message = player == Player.Invalid ? "Draw, wanna play more?" : $"{player} WON!, wanna play more?";
        
        DialogBox.Instance.Show(message,
        () =>
        {
            GameManager.Instance.Reset();
        }, () =>
        {
            GameManager.Instance.Reset();
            MenuManager.Instance.NavigateTo<MainMenu>();
        });
    }

    private void OnGameReset()
    {
        _buttonGrid = null;

        UnInitializeBoard();
        InitializeBoard();
    }

    private void UnInitializeBoard()
    {
        // TODO: Not needed could be smarter :D
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
        if (timerText != null && GameManager.Instance.HasGameStarted)
        {
            timerText.text = $"{GameManager.Instance.ElapsedTime:ss\\.ff}";
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
            infoText.text = $"{GameManager.Instance.PlayerMoveCount} moves and player {GameManager.Instance.CurrentPlayer} turn";
        }
        else
        {
            Log.Warn("Info text is not assigned in the inspector.");
        }
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

    private void OnCellPlayed(int x, int y, Player player)
    {
        Log.Info($"Cell played at ({x}, {y})");
        
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

    private void OnBackButtonClicked()
    {
        MenuManager.Instance.NavigateBack();
    }
}
