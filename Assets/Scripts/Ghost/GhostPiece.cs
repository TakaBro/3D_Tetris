using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPiece : MonoBehaviour
{
    GameObject parent;
    TetrisPiece parentPiece;

    private bool hasJustInstantiated = true;

    private void OnEnable()
    {
        StartCoroutine(RepositionPiece());
    }

    public void SetParent(GameObject _parent)
    {
        parent = _parent;
        parentPiece = parent.GetComponent<TetrisPiece>();
    }

    private void UpdateGhostPosition()
    {
        transform.position = parent.transform.position;
        transform.rotation = parent.transform.rotation;
    }

    IEnumerator RepositionPiece()
    {
        if (!hasJustInstantiated)
        {
            while (parentPiece.enabled)
            {
                UpdateGhostPosition();
                MoveDown();

                yield return new WaitForSeconds(.1f);
            }
            // Return ghost to obj pool if parentPiece is disabled
            gameObject.SetActive(false);
            yield return null;
        }
        if (hasJustInstantiated) hasJustInstantiated = false;
    }

    private void MoveDown()
    {
        while (CheckValidMove())
        {
            transform.position += Vector3.down;
        }
        if (!CheckValidMove())
        {
            transform.position += Vector3.up;
        }
    }

    bool CheckValidMove()
    {
        foreach (Transform child in transform)
        {
            Vector3 position = GameManager.instance.Round(child.position);

            // Not valid if not inside grid
            if (!GridSpace.instance.CheckInsideGrid(position))
            {
                return false;
            }

            Transform t = GridSpace.instance.GetTransformOnGridPosition(position);
            // Valid if stumble over active piece
            if (t != null && t.parent == parent.transform)
            {
                return true;
            }

            // Not valid if position is not null and not the piece itself
            if (t != null && t.parent != transform)
            {
                return false;
            }
        }
        return true;
    }
}
