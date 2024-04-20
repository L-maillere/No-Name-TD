using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class PathGenerator : MonoBehaviour
{
  // Références aux objets nécessaires dans l'éditeur Unity
  [SerializeField] private GridManager gridManager; // Gestionnaire de la grille
  [SerializeField] private GameObject straightPathPrefab, leftTurnPathPrefab, rightTurnPathPrefab, grassPrefab; // Préfabriqués pour les différents types de chemins
  [SerializeField] private NavMeshSurface surface;

  // Points de départ et d'arrivée du chemin dans la grille
  private Vector2Int startPoint = new Vector2Int(1, 14); // 2ème colonne, 1ère ligne
  private Vector2Int endPoint = new Vector2Int(13, 1); // 13ème colonne, 2ème ligne
  private List<Vector2Int> pathPoints = new List<Vector2Int>(); // Liste pour stocker les points du chemin

  private void Start()
  {
    GeneratePath(); // Génère le chemin dès le démarrage
    surface.BuildNavMesh();
  }

  private void GeneratePath()
  {
    // Calcul du nombre total de cases dans la grille (en excluant les bords)
    int totalCases = (gridManager.gridSizeX - 2) * (gridManager.gridSizeY - 2);
    // Longueur minimale du chemin (40% des cases)
    int minPathLength = Mathf.CeilToInt(totalCases * 0.4f);
    bool pathMeetsRequirement = false; // Indique si le chemin respecte les exigences

    // Continue de générer des chemins jusqu'à ce qu'un chemin valide soit trouvé
    while (!pathMeetsRequirement)
    {
      pathPoints.Clear(); // Efface le chemin précédent
      pathPoints.Add(startPoint); // Ajoute le point de départ

      // Le point juste en dessous du point de départ est le deuxième point du chemin
      Vector2Int currentPoint = new Vector2Int(startPoint.x, startPoint.y - 1);
      pathPoints.Add(currentPoint);

      // Limite le nombre d'itérations pour éviter les boucles infinies
      int maxIterations = 100000, iterations = 0;

      // Construit le chemin point par point
      while (currentPoint != endPoint && iterations < maxIterations)
      {
        var possibleMoves = GetPossibleMoves(currentPoint); // Obtient les mouvements possibles depuis la position actuelle
        if (possibleMoves.Count > 0)
        {
          currentPoint = possibleMoves[UnityEngine.Random.Range(0, possibleMoves.Count)]; // Choisis un mouvement aléatoire parmi les mouvements possibles
          pathPoints.Add(currentPoint); // Ajoute le nouveau point au chemin
        }
        else // Si aucun mouvement n'est possible, revient en arrière
        {
          if (pathPoints.Count > 15) // Ne revient en arrière que si la longueur du chemin le permet
          {
            int pointsToRemove = Mathf.Min(15, pathPoints.Count - 1);
            pathPoints.RemoveRange(pathPoints.Count - pointsToRemove, pointsToRemove); // Supprime les derniers points
            currentPoint = pathPoints[pathPoints.Count - 1]; // Met à jour le point courant
          }
          else break; // Sort de la boucle si le chemin est trop court pour revenir en arrière
        }
        iterations++;
      }

      // Vérifie si le chemin généré respecte la longueur minimale requise
      pathMeetsRequirement = pathPoints.Count >= minPathLength;
    }

    // Si un chemin valide a été trouvé, le visualise et remplit les cases restantes
    if (pathMeetsRequirement)
    {
      VisualizePath(pathPoints); // Visualise le chemin
      FillNonPath(); // Remplit les cases qui ne font pas partie du chemin
    }
    else
    {
      Debug.LogError("Échec de la génération d'un chemin respectant la longueur minimale requise.");
    }
  }
  List<Vector2Int> GetPossibleMoves(Vector2Int currentPoint)
  {
    List<Vector2Int> moves = new List<Vector2Int>(); // Liste pour stocker les mouvements possibles

    if (currentPoint.x > 1) moves.Add(new Vector2Int(currentPoint.x - 1, currentPoint.y)); // Ajoute le mouvement à gauche
    if (currentPoint.x < gridManager.gridSizeX - 2) moves.Add(new Vector2Int(currentPoint.x + 1, currentPoint.y)); // Ajoute le mouvement à droite
    if (currentPoint.y > 1) moves.Add(new Vector2Int(currentPoint.x, currentPoint.y - 1)); // Ajoute le mouvement en bas
    if (currentPoint.y < gridManager.gridSizeY - 2) moves.Add(new Vector2Int(currentPoint.x, currentPoint.y + 1)); // Ajoute le mouvement en haut

    moves.RemoveAll(move => pathPoints.Contains(move)); // Supprime les mouvements déjà effectués
    moves.RemoveAll(move => IsAdjacentToPath(move)); // Supprime les mouvements adjacents au chemin
    return moves;
  }


  // Vérifie si une case est adjacente à un point du chemin
  private bool IsAdjacentToPath(Vector2Int move)
  {
    foreach (Vector2Int point in pathPoints)
    {
      if ((Mathf.Abs(point.x - move.x) == 1 && point.y == move.y) || (Mathf.Abs(point.y - move.y) == 1 && point.x == move.x))
      {
        if (point != pathPoints[pathPoints.Count - 1]) return true; // Exclut le dernier point du chemin
      }
    }
    return false; // Si aucune case adjacente n'est trouvée
  }

  Quaternion DetermineTurnRotation(Vector2 directionToPrev, Vector2 directionToNext)
  {
    // Détermine l'orientation du préfabriqué basée sur les directions relatives
    // Ajoutez ici votre logique pour déterminer la rotation en fonction des directions
    // (+Y)
    if (directionToPrev == new Vector2Int(0,1))
    {
          // (+Y,+X)
          if (directionToNext == new Vector2Int(1,0))
          {
            return Quaternion.Euler(0,270,0);
          }
          // (+Y,-X)
          if (directionToNext == new Vector2Int(-1,0))
          {
            return Quaternion.Euler(0,0,0);
          }
    }
    // (-Y)
    if (directionToPrev == new Vector2Int(0,-1))
    {
          // (-Y,+X)
          if (directionToNext == new Vector2Int(1,0))
          {
            return Quaternion.Euler(0,180,0);
          }
          // (-Y,-X)
          if (directionToNext == new Vector2Int(-1,0))
          {
            return Quaternion.Euler(0,90,0);
          }
    }
    // (+X)
    if (directionToPrev == new Vector2Int(1,0))
    {
          // (+X,+Y)
          if (directionToNext == new Vector2Int(0,1))
          {
            return Quaternion.Euler(0,90,0);
          }
          // (+X,-Y)
          if (directionToNext == new Vector2Int(0,-1))
          {
            return Quaternion.Euler(0,0,0);
          }
    }
  // (-X)
    if (directionToPrev == new Vector2Int(-1,0))
    {
          // (-X,+Y)
          if (directionToNext == new Vector2Int(0,1))
          {
            return Quaternion.Euler(0,180,0);
          }
          // (-X,-Y)
          if (directionToNext == new Vector2Int(0,-1))
          {
            return Quaternion.Euler(0,270,0);
          }
    }
    return Quaternion.identity; // Retourne une rotation par défaut pour l'instant
  }

  void VisualizePath(List<Vector2Int> pathPoints)
  {
    for (int i = 0; i < pathPoints.Count; i++)
    {
      Vector3 worldPosition = gridManager.GetWorldPosition(pathPoints[i]) + new Vector3(gridManager.cellSize / 2, 0.01f, gridManager.cellSize / 2);
      Quaternion rotation = Quaternion.identity;
      GameObject pathPrefabToUse = straightPathPrefab; // Utilisez le chemin tout droit par défaut

      if (i > 0 && i < pathPoints.Count - 1) // Ignore le premier et le dernier point
      {
        Vector2 directionToPrev = ((Vector2)(pathPoints[i] - pathPoints[i - 1])).normalized;
        Vector2 directionToNext = ((Vector2)(pathPoints[i + 1] - pathPoints[i])).normalized;

        // Chemin tout droit
        if (directionToPrev == directionToNext)
        {
          pathPrefabToUse = straightPathPrefab;
          rotation = directionToPrev.x != 0 ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 90, 0);
        }
        else // Tournant
        {
          // Calcule le produit vectoriel pour déterminer la direction du tournant
          float crossProduct = directionToPrev.x * directionToNext.y - directionToPrev.y * directionToNext.x;
          pathPrefabToUse = crossProduct > 0 ? leftTurnPathPrefab : rightTurnPathPrefab;

          // Détermine l'orientation du préfabriqué basée sur les directions relatives
          Debug.Log((i, pathPoints[i], directionToPrev, directionToNext));
          rotation = DetermineTurnRotation(directionToPrev, directionToNext);
        }
      }
      else if (i == 0) // Pour le premier point, utilisez le chemin tout droit
      {
        pathPrefabToUse = straightPathPrefab;
        Vector2 directionToNext = ((Vector2)(pathPoints[i + 1] - pathPoints[i])).normalized;
        rotation = directionToNext.x != 0 ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 90, 0);
      }
      Instantiate(pathPrefabToUse, worldPosition, rotation, transform);
    }
  }

  // Remplit les cases qui ne font pas partie du chemin
  private void FillNonPath()
  {
    for (int x = 0; x < gridManager.gridSizeX; x++)
    {
      for (int y = 0; y < gridManager.gridSizeY; y++)
      {
        Vector2Int gridPoint = new Vector2Int(x, y);
        if (!pathPoints.Contains(gridPoint)) // Vérifie si la case n'est pas dans le chemin
        {
          Vector3 worldPosition = gridManager.GetWorldPosition(gridPoint) + new Vector3(gridManager.cellSize / 2, 0f, gridManager.cellSize / 2);
          Quaternion rotation = Quaternion.Euler(-89.98f, UnityEngine.Random.Range(0, 4) * 90f, 0f); // Rotation aléatoire pour varier l'apparence
          Instantiate(grassPrefab, worldPosition, rotation, transform); // Instancie le préfabriqué d'herbe
        }
      }
    }
  }
}