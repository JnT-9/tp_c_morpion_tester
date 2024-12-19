using TicTacToe;
using FluentAssertions;

namespace tp_c_morpion_tester;

/// <summary>
/// Tests for the game initialization, display, and flow
/// </summary>
public class TestGame
{
    // Test implementation of IGameDisplay
    private class TestDisplay : IGameDisplay
    {
        public List<string> Messages { get; } = new();
        public int ClearCount { get; private set; }
        public int DisplayCount { get; private set; }
        public int WinnerShownCount { get; private set; }
        public int DrawShownCount { get; private set; }
        public Player? LastWinnerShown { get; private set; }

        public void ShowMessage(string message) => Messages.Add(message);
        public void Display() => DisplayCount++;
        public void Clear() => ClearCount++;
        public void ShowTurn(IPlayer player) => Messages.Add($"Turn: {player.PlayerType}");
        public void ShowInvalidMove() => Messages.Add("Invalid move");
        public void ShowWinner(Player player)
        {
            WinnerShownCount++;
            LastWinnerShown = player;
            Messages.Add($"Winner: {player}");
        }
        public void ShowDraw() => DrawShownCount++;
        public void ShowInvalidInput(string message) => Messages.Add($"Invalid input: {message}");
    }

    // Test implementation of IPlayer for simulating moves
    private class TestPlayer : IPlayer
    {
        private readonly Queue<(int row, int column)?> _moves;
        public Player PlayerType { get; }

        public TestPlayer(Player playerType, params (int row, int column)[] moves)
        {
            PlayerType = playerType;
            _moves = new Queue<(int row, int column)?>(moves.Select(m => ((int, int)?)m));
        }

        public (int row, int column)? GetNextMove(GameBoard board)
        {
            return _moves.Count > 0 ? _moves.Dequeue() : null;
        }
    }

    private readonly TestDisplay _display;
    private readonly GameBoard _board;
    private readonly GameRules _rules;
    private Game _game;

    public TestGame()
    {
        _display = new TestDisplay();
        _board = new GameBoard();
        _rules = new GameRules();
        _game = new Game(_board, _display, _rules);
    }

    /// <summary>
    /// Test #1: Vérification du mode Humain vs Humain
    /// - Vérifie que le mode "1" est accepté
    /// - Vérifie que le message de confirmation est affiché
    /// - Simule le choix du mode 2 joueurs humains
    /// </summary>
    [Fact]
    public void SelectGameMode_ShouldCreateHumanVsHuman_WhenChoice1()
    {
        // Act: Sélectionne le mode "1" (Humain vs Humain)
        bool result = _game.SelectGameMode("1");

        // Assert: Vérifie que le mode est accepté et le message correct est affiché
        result.Should().BeTrue();
        _display.Messages.Should().Contain("Human vs Human mode selected!");
    }

    /// <summary>
    /// Test #2: Vérification du mode Humain vs IA
    /// - Vérifie que le mode "2" est accepté
    /// - Vérifie que les messages de confirmation sont affichés
    /// - Simule le choix du mode contre l'IA
    /// </summary>
    [Fact]
    public void SelectGameMode_ShouldCreateHumanVsAI_WhenChoice2()
    {
        // Act: Sélectionne le mode "2" (Humain vs IA)
        bool result = _game.SelectGameMode("2");

        // Assert: Vérifie que le mode est accepté et les messages corrects sont affichés
        result.Should().BeTrue();
        _display.Messages.Should().Contain("Human vs AI mode selected!");
        _display.Messages.Should().Contain("You will play as O (Player One)");
    }

    /// <summary>
    /// Test #3: Vérification du rejet des choix invalides
    /// - Vérifie qu'un choix invalide ("3") est rejeté
    /// - Simule une entrée incorrecte de l'utilisateur
    /// </summary>
    [Fact]
    public void SelectGameMode_ShouldReturnFalse_WhenInvalidChoice()
    {
        // Act: Tente de sélectionner un mode invalide "3"
        bool result = _game.SelectGameMode("3");

        // Assert: Vérifie que le choix invalide est rejeté
        result.Should().BeFalse();
    }

    /// <summary>
    /// Test #4: Vérification de l'affichage initial
    /// - Vérifie que le plateau est affiché au début
    /// - Vérifie que l'écran est d'abord effacé
    /// - Simule le début d'une nouvelle partie
    /// </summary>
    [Fact]
    public void PlayGame_ShouldDisplayInitialBoard()
    {
        // Arrange: Configure une nouvelle partie contre l'IA
        _game.SelectGameMode("2");

        // Configure un plateau qui mène à une victoire immédiate
        _board.TryPlay(1, 1, Player.Two);  // X en haut à gauche
        _board.TryPlay(1, 2, Player.Two);  // X en haut au milieu
        _board.TryPlay(1, 3, Player.Two);  // X en haut à droite - victoire

        // Act: Démarre la partie (se termine immédiatement car victoire)
        var result = _game.PlayGame();

        // Assert: Vérifie que l'affichage est correctement initialisé
        _display.ClearCount.Should().BeGreaterThan(0);
        _display.DisplayCount.Should().BeGreaterThan(0);
        result.Should().Be(GameResult.Player2Wins);
    }

    /// <summary>
    /// Test #5: Vérification d'une victoire
    /// - Simule une partie où le Joueur 1 gagne
    /// - Vérifie que la victoire est détectée
    /// - Vérifie que le bon message de victoire est affiché
    /// Configuration du plateau:
    /// O O O
    /// X X -
    /// - - -
    /// </summary>
    [Fact]
    public void PlayGame_ShouldShowWinner_WhenPlayer1Wins()
    {
        // Configure un plateau qui mène à une victoire immédiate de O (Player One)
        // O O -
        // X X -
        // - - -
        _board.TryPlay(1, 1, Player.One);  // O en haut à gauche
        _board.TryPlay(2, 1, Player.Two);  // X au milieu à gauche
        _board.TryPlay(1, 2, Player.One);  // O en haut au milieu
        _board.TryPlay(2, 2, Player.Two);  // X au milieu au milieu

        // Create test players with simulated moves
        var player1 = new TestPlayer(Player.One, (1, 3));  // O en haut à droite - victoire
        var player2 = new TestPlayer(Player.Two);  // No moves needed
        _game = new Game(_board, _display, _rules, player1, player2);

        // Act: Play the game with the simulated moves
        var result = _game.PlayGame();

        // Assert: Vérifie que la victoire est correctement détectée et affichée
        result.Should().Be(GameResult.Player1Wins);
        _display.WinnerShownCount.Should().Be(1);
        _display.LastWinnerShown.Should().Be(Player.One);
        _display.Messages.Should().Contain("Winner: One");
    }

    /// <summary>
    /// Test #6: Vérification d'un match nul
    /// - Simule une partie se terminant par un match nul
    /// - Vérifie que le match nul est détecté
    /// - Vérifie que le bon message est affiché
    /// Configuration du plateau:
    /// O X O
    /// X O X
    /// X O X
    /// </summary>
    [Fact]
    public void PlayGame_ShouldShowDraw_WhenBoardIsFull()
    {
        // Arrange: Configure une partie entre humains
        _game.SelectGameMode("1");
        
        // Configure un plateau complètement rempli menant à un match nul
        _board.TryPlay(1, 1, Player.One);  // O en haut à gauche
        _board.TryPlay(1, 2, Player.Two);  // X en haut au milieu
        _board.TryPlay(1, 3, Player.One);  // O en haut à droite
        _board.TryPlay(2, 1, Player.Two);  // X au milieu à gauche
        _board.TryPlay(2, 2, Player.One);  // O au centre
        _board.TryPlay(2, 3, Player.Two);  // X au milieu à droite
        _board.TryPlay(3, 1, Player.Two);  // X en bas à gauche
        _board.TryPlay(3, 2, Player.One);  // O en bas au milieu
        _board.TryPlay(3, 3, Player.Two);  // X en bas à droite

        // Act: Termine la partie (se termine immédiatement car plateau plein)
        var result = _game.PlayGame();

        // Assert: Vérifie que le match nul est correctement détecté et affiché
        result.Should().Be(GameResult.Draw);
        _display.DrawShownCount.Should().Be(1);
    }
} 