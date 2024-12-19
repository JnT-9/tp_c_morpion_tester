using TicTacToe;
using FluentAssertions;

namespace tp_c_morpion_tester;

/// <summary>
/// Tests pour vérifier le comportement des joueurs (IA et Humain)
/// </summary>
public class TestPlayer
{
    /// <summary>
    /// Test #1: Vérification de la prise de position gagnante par l'IA
    /// - Configure un plateau où l'IA peut gagner en un coup
    /// - Vérifie que l'IA choisit le coup gagnant
    /// Configuration du plateau:
    /// X | - | X    Le coup gagnant est (1,2)
    /// - | O | -    pour aligner trois X
    /// - | - | -
    /// </summary>
    [Fact]
    public void AIPlayer_ShouldTakeWinningMove()
    {
        // Arrange: Crée un plateau avec une possibilité de victoire pour l'IA
        var board = new GameBoard();
        IPlayer player = new AIPlayer(Player.Two);
        
        // Configure le plateau avec deux X et un O
        board.TryPlay(1, 1, Player.Two);  // X en haut à gauche
        board.TryPlay(1, 3, Player.Two);  // X en haut à droite
        board.TryPlay(2, 2, Player.One);  // O au centre

        // Act: Demande à l'IA de jouer
        var move = player.GetNextMove(board);

        // Assert: Vérifie que l'IA choisit la position gagnante (1,2)
        move.HasValue.Should().BeTrue();
        move!.Value.Should().Be((1, 2));
    }

    /// <summary>
    /// Test #2: Vérification du blocage d'une victoire adverse
    /// - Configure un plateau où l'adversaire peut gagner au prochain coup
    /// - Vérifie que l'IA bloque ce coup
    /// Configuration du plateau:
    /// X | X | -    Le coup bloquant est (1,3)
    /// - | O | -    pour empêcher trois X
    /// - | - | -
    /// </summary>
    [Fact]
    public void AIPlayer_ShouldBlockOpponentWinningMove()
    {
        // Arrange: Crée un plateau où l'adversaire peut gagner
        var board = new GameBoard();
        IPlayer player = new AIPlayer(Player.One);
        
        // Configure le plateau avec l'adversaire proche de la victoire
        board.TryPlay(1, 1, Player.Two);  // X en haut à gauche
        board.TryPlay(1, 2, Player.Two);  // X en haut au milieu
        board.TryPlay(2, 2, Player.One);  // O au centre

        // Act: Demande à l'IA de jouer
        var move = player.GetNextMove(board);

        // Assert: Vérifie que l'IA bloque la victoire adverse (1,3)
        move.HasValue.Should().BeTrue();
        move!.Value.Should().Be((1, 3));
    }

    /// <summary>
    /// Test #3: Vérification de la prise du centre
    /// - Vérifie que l'IA prend le centre quand il est disponible
    /// - Le centre est une position stratégique importante
    /// Configuration du plateau:
    /// - | - | -    L'IA doit choisir
    /// - | - | -    le centre (2,2)
    /// - | - | -
    /// </summary>
    [Fact]
    public void AIPlayer_ShouldTakeCenterIfAvailable()
    {
        // Arrange: Crée un plateau vide
        var board = new GameBoard();
        IPlayer player = new AIPlayer(Player.Two);

        // Act: Demande à l'IA de jouer sur un plateau vide
        var move = player.GetNextMove(board);

        // Assert: Vérifie que l'IA choisit le centre (2,2)
        move.HasValue.Should().BeTrue();
        move!.Value.Should().Be((2, 2));
    }

    /// <summary>
    /// Test #4: Vérification de la prise d'un coin
    /// - Vérifie que l'IA prend un coin quand le centre est occupé
    /// - Les coins sont les positions stratégiques après le centre
    /// Configuration du plateau:
    /// - | - | -    L'IA doit choisir
    /// - | X | -    un des coins car
    /// - | - | -    le centre est pris
    /// </summary>
    [Fact]
    public void AIPlayer_ShouldTakeCornerIfCenterTaken()
    {
        // Arrange: Crée un plateau avec le centre occupé
        var board = new GameBoard();
        IPlayer player = new AIPlayer(Player.One);
        
        // Place un X au centre
        board.TryPlay(2, 2, Player.Two);  // X au centre

        // Act: Demande à l'IA de jouer
        var move = player.GetNextMove(board);

        // Assert: Vérifie que l'IA choisit un des coins
        move.HasValue.Should().BeTrue();
        var corners = new[] { (1, 1), (1, 3), (3, 1), (3, 3) };
        corners.Should().Contain(move!.Value);
    }
}
