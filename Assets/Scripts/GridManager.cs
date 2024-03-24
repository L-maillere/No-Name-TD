using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{
  public int gridSizeX = 15; // Largeur de la grille
  public int gridSizeY = 15; // Hauteur de la grille
  public float cellSize = 1.0f; // Taille d'une cellule de la grille
  private Vector2Int selectedCell = new Vector2Int(-1, -1); // Initialise la cellule sélectionnée à une valeur hors grille
  public GameObject cellHightlightPrefab; // Préfabriqué pour le marqueur de cellule sélectionnée
  void OnDrawGizmos()
  {
    Gizmos.color = Color.grey; // Couleur des lignes de la grille
    for (int x = 0; x < gridSizeX; x++)
    {
      for (int y = 0; y < gridSizeY; y++)
      {
        // Dessine les lignes verticales
        Vector3 startPos = new Vector3(x * cellSize, 0, 0);
        Vector3 endPos = new Vector3(x * cellSize, 0, gridSizeY * cellSize);
        Gizmos.DrawLine(startPos, endPos);

        // Dessine les lignes horizontales
        startPos = new Vector3(0, 0, y * cellSize);
        endPos = new Vector3(gridSizeX * cellSize, 0, y * cellSize);
        Gizmos.DrawLine(startPos, endPos);
      }
    }
  }

  void Update()
  {
    // Détecte un clic gauche de la souris
    if (Input.GetMouseButtonDown(0))
    {
      // Crée un rayon depuis la caméra vers la position de la souris
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
      {
        // Convertit la position du rayon en position de la grille
        int x = Mathf.FloorToInt(hit.point.x / cellSize);
        int y = Mathf.FloorToInt(hit.point.z / cellSize);
        Debug.Log($"Cellule cliquée : X={x}, Y={y}");
        selectedCell = new Vector2Int(x, y); // Met à jour la cellule sélectionnée

        HighlightCell(selectedCell); // Met en surbrillance la cellule sélectionnée
      }
    }
  }
  void HighlightCell(Vector2Int cell)
  {
    // Supprime le marqueur de cellule sélectionnée précédent
    foreach (Transform child in transform)
    {
      if (child.CompareTag("Highlight"))
      {
        Destroy(child.gameObject);
      }
    }

    // Vérifie que la cellule sélectionnée est dans la grille
    if (cell.x >= 0 && cell.x < gridSizeX && cell.y >= 0 && cell.y < gridSizeY)
    {
      // Ajuste la position pour centrer le marqueur sur la cellule
      Vector3 cellCenter = GetWorldPosition(cell) + new Vector3(cellSize / 2, 1f, cellSize / 2);

      // Crée et place le nouveau marqueur
      GameObject highlight = Instantiate(cellHightlightPrefab, cellCenter, Quaternion.identity);
      highlight.transform.parent = this.transform; // Définit la grille comme parent du marqueur
    }
  }

  public Vector3 GetWorldPosition(Vector2Int gridPosition)
  {
    return new Vector3(gridPosition.x * cellSize, 0, gridPosition.y * cellSize);
  }
}