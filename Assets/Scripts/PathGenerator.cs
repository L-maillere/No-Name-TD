using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
  public GridManager gridManager; // Référence publique au script GridManager
  public GameObject pathPrefab; // Préfabriqué pour le marqueur de chemin

  private Vector2Int startPoint = new Vector2Int(1, 14); // 2ème colonne, 1ère ligne
  private Vector2Int endPoint = new Vector2Int(13, 1); // 13ème colonne, 2ème ligne
  private List<Vector2Int> pathPoints = new List<Vector2Int>(); // Liste pour stocker les points du chemin
  // Start is called before the first frame update
  void Start()
  {
    pathPoints.Clear(); // Vide la liste des points du chemin
    pathPoints.Add(startPoint); // Ajoute le point de départ à la liste

    Vector2Int currentPoint = startPoint; // Initialise le point courant au point de départ

    int maxIterations = 500000; // Un nombre suffisamment grand pour permettre la génération d'un chemin
    int iterations = 0;

    while (currentPoint != endPoint && iterations < maxIterations) // Tant que le point courant n'est pas le point d'arrivée
    {
      List<Vector2Int> possibleMoves = GetPossibleMoves(currentPoint); // Récupère les mouvements possibles pour le point courant
      if (possibleMoves.Count > 0)
      {
        currentPoint = possibleMoves[UnityEngine.Random.Range(0, possibleMoves.Count)]; // Choix aléatoire d'un mouvement possible
        pathPoints.Add(currentPoint); // Ajoute le point courant à la liste
        iterations++;
      }
      else
      {
        // Si aucun mouvement n'est possible, revenez en arrière
        if (pathPoints.Count > 1)
        {
          int pointsToRemove = Math.Min(15, pathPoints.Count - 1); // Calcule le nombre de points à supprimer
          pathPoints.RemoveRange(pathPoints.Count - pointsToRemove, pointsToRemove); // Supprime les points
          currentPoint = pathPoints[pathPoints.Count - 1]; // Met à jour le point courant au nouveau dernier point
        }
        else
        {
          // Si vous êtes revenu au point de départ et qu'il n'y a toujours pas de mouvements possibles,
          // vous pouvez choisir de réinitialiser complètement ou de gérer différemment.
          break; // Sortie simple de la boucle pour cet exemple
        }
      }
    }
    if (iterations >= maxIterations)
    {
      Debug.LogError("Maximum iterations reached, path generation failed.");
    }

    VisualizePath(pathPoints); // Visualise le chemin
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
}