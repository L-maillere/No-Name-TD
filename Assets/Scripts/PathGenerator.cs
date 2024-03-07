using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
  public GridManager gridManager; // Référence publique au script GridManager
  public GameObject pathPrefab; // Préfabriqué pour le marqueur de chemin
  public GameObject straightPathPrefab; // Préfabriqué pour le chemin tout droit
  public GameObject leftTurnPathPrefab; // Préfabriqué pour le tournant gauche
  public GameObject rightTurnPathPrefab; // Préfabriqué pour le tournant droite

  public GameObject grassPrefab; // Préfabriqué pour les cases non chemin

  private Vector2Int startPoint = new Vector2Int(1, 14); // 2ème colonne, 1ère ligne
  private Vector2Int endPoint = new Vector2Int(13, 1); // 13ème colonne, 2ème ligne
  private List<Vector2Int> pathPoints = new List<Vector2Int>(); // Liste pour stocker les points du chemin
  // Start is called before the first frame update
  void Start()
  {
    int totalCases = (gridManager.gridSizeX - 2) * (gridManager.gridSizeY - 2); // Calcul en évitant les bords
    int minPathLength = (int)(totalCases * 0.4); // 30% des cases de la grille
    bool pathMeetsRequirement;

    do
    {
      pathPoints.Clear(); // Commencez par vider la liste des points du chemin
      pathPoints.Add(startPoint); // Ajoute le point de départ à la liste

      Vector2Int secondPoint = new Vector2Int(startPoint.x, startPoint.y - 1); // Fixe la deuxième case du chemin directement en dessous de la première
      pathPoints.Add(secondPoint);

      Vector2Int currentPoint = secondPoint; // Initialise le point courant à la deuxième case


      int maxIterations = 500000; // Un nombre suffisamment grand pour permettre la génération d'un chemin
      int iterations = 0;

      while (currentPoint != endPoint && iterations < maxIterations)
      {
        List<Vector2Int> possibleMoves = GetPossibleMoves(currentPoint);
        if (possibleMoves.Count > 0)
        {
          currentPoint = possibleMoves[UnityEngine.Random.Range(0, possibleMoves.Count)];
          pathPoints.Add(currentPoint);
        }
        else
        {
          if (pathPoints.Count > 15)
          {
            int pointsToRemove = Math.Min(15, pathPoints.Count - 1);
            pathPoints.RemoveRange(pathPoints.Count - pointsToRemove, pointsToRemove);
            currentPoint = pathPoints[pathPoints.Count - 1];
          }
          else
          {
            break; // Sortie de la boucle si le chemin est trop court pour revenir en arrière
          }
        }
        iterations++;
      }

      pathMeetsRequirement = pathPoints.Count >= minPathLength;
    }
    while (!pathMeetsRequirement);

    if (!pathMeetsRequirement)
    {
      Debug.LogError("Failed to generate a path that meets the minimum length requirement.");
    }
    else
    {
      VisualizePath(pathPoints); // Visualise le chemin
      FillNonPath(); // Remplis les cases non chemin
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

  bool IsAdjacentToPath(Vector2Int move)
  {
    foreach (Vector2Int point in pathPoints)
    {
      // Vérifie si 'move' est adjacent à un point du chemin
      if ((Mathf.Abs(point.x - move.x) == 1 && point.y == move.y) ||
          (Mathf.Abs(point.y - move.y) == 1 && point.x == move.x))
      {
        // Si 'move' est adjacent à un point du chemin autre que la fin du chemin,
        // alors ce mouvement n'est pas autorisé.
        if (point != pathPoints[pathPoints.Count - 1])
        {
          return true; // 'move' est adjacent à une partie du chemin
        }
      }
    }
    return false; // 'move' n'est pas adjacent à une partie du chemin
  }

  void VisualizePath(List<Vector2Int> pathPoints)
  {
    foreach (Vector2Int point in pathPoints)
    {
      Vector3 worldPosition = gridManager.GetWorldPosition(point) + new Vector3(gridManager.cellSize / 2, 0.01f, gridManager.cellSize / 2); // Centre de la cellule
      Instantiate(pathPrefab, worldPosition, Quaternion.identity, transform);
    }
  }

  void FillNonPath()
  {
    for (int x = 0; x < gridManager.gridSizeX; x++) // Inclut les bords
    {
      for (int y = 0; y < gridManager.gridSizeY; y++) // Inclut les bords
      {
        Vector2Int gridPoint = new Vector2Int(x, y);

        // Vérifie si le point actuel n'est pas dans pathPoints
        if (!pathPoints.Contains(gridPoint))
        {
          // Ce point n'est pas sur le chemin, instanciez le préfabriqué grassPrefab ici
          Vector3 worldPosition = gridManager.GetWorldPosition(gridPoint) + new Vector3(gridManager.cellSize / 2, 0.01f, gridManager.cellSize / 2); // Centre de la cellule

          // Sélectionne aléatoirement un angle de rotation sur l'axe Y parmi 0, 90, 180, 270
          float[] angles = { 0f, 90f, 180f, 270f };
          float randomYRotation = angles[UnityEngine.Random.Range(0, angles.Length)];

          // Crée une rotation sur l'axe X de -89.98 degrés
          Quaternion rotation = Quaternion.Euler(-89.98f, randomYRotation, 0f);
          Instantiate(grassPrefab, worldPosition, rotation, transform);
        }
      }
    }
  }
}