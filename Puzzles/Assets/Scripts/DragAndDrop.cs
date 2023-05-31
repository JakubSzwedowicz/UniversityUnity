using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class DragAndDrop : MonoBehaviour
{
    public GameManager GameManager;

    private Piece _selectedPiece;
    private int _placedPieces = 0;

    void Update()
    { 
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.transform != null && hit.transform.CompareTag("Puzzle"))
            {
                if (hit.transform.GetComponent<Piece>().IsSelectable())
                {
                    _selectedPiece = hit.transform.GetComponent<Piece>();
                    _selectedPiece.Select();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_selectedPiece != null)
            {
                if (_selectedPiece.Deselect())
                {
                    GameManager.SoundManager.PlaySoundPutRight();
                    _placedPieces++;
                }
                else
                {
                    GameManager.SoundManager.PlaySoundPutWrong();
                }
                _selectedPiece = null;
            }
        }
        if (_selectedPiece != null)
        {
            Vector3 MousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _selectedPiece.transform.position = new Vector3(MousePoint.x,MousePoint.y,0);
        }             
        if (_placedPieces == 16)
        {
            GameManager.StopGame();
        }
    }
}