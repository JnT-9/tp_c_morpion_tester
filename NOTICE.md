# Projet de Tests du Morpion

## Description
Ce projet contient les tests unitaires pour le jeu de Morpion. Il utilise xUnit comme framework de test et FluentAssertions pour des assertions plus lisibles.

## Structure des Tests

### Classes de Test

1. **TestGame** (`TestGame.cs`)
   - Teste la logique du jeu
   - Vérifie la sélection du mode de jeu
   - Teste les conditions de victoire et match nul

2. **TestPlayer** (`TestPlayer.cs`)
   - Teste le comportement des joueurs
   - Vérifie les mouvements de l'IA
   - Teste les stratégies de jeu

3. **TestDisplay** (`TestDisplay.cs`)
   - Implémentation de test pour IGameDisplay
   - Enregistre les messages et actions d'affichage
   - Permet de vérifier les interactions avec l'interface utilisateur

### Types de Tests

1. **Tests de Mode de Jeu**
   - Vérification du choix Humain vs Humain
   - Vérification du choix Humain vs IA
   - Validation des entrées invalides

2. **Tests de Partie**
   - Affichage initial du plateau
   - Déroulement d'une partie gagnante
   - Vérification des matchs nuls
   - Test des conditions de sortie

3. **Tests de l'IA**
   - Vérification des mouvements gagnants
   - Test du blocage des mouvements adverses
   - Validation des stratégies de jeu

## Comment Exécuter les Tests

1. Ouvrez le projet dans Visual Studio ou VS Code
2. Utilisez l'Explorateur de Tests ou
3. Exécutez `dotnet test` dans le terminal

## Architecture des Tests

- Tests isolés et indépendants
- Utilisation de TestDisplay pour simuler l'interface
- Vérification des états et comportements
- Tests lisibles et maintenables 