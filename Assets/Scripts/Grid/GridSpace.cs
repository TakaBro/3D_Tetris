using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpace : MonoBehaviour
{
    public static GridSpace instance;

    [SerializeField] private GameEvent layerCleared;
    [SerializeField] private int gridSizeX, gridSizeY, gridSizeZ;

    [Header("Pieces")]
    [SerializeField] private string[] pieceListTag;
    [SerializeField] private string[] ghostListTag;
    [SerializeField] private GameObject[] pieceList;
    [SerializeField] private GameObject[] ghostList;

    [Header("Grid Component")]
    private Transform[,,] theGrid;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        theGrid = new Transform[gridSizeX, gridSizeY, gridSizeZ];
        StartCoroutine(WaitAndSpawnTetrisPiece(2.0f));
    }

    private IEnumerator WaitAndSpawnTetrisPiece(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SpanwTetrisPiece();
    }

    public bool CheckInsideGrid(Vector3 position)
    {
        return ((int)position.x >= 0 && (int)position.x < gridSizeX &&
                (int)position.z >= 0 && (int)position.z < gridSizeZ &&
                (int)position.y >= 0);
    }

    public void UpdateGrid(TetrisPiece piece)
    {
        // Delete parent piece obj piece
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    if (theGrid[x,y,z] != null)
                    {
                        if (theGrid[x, y, z].parent == piece.transform)
                        {
                            theGrid[x, y, z] = null;
                        }
                    }
                }
            }
        }

        // Register all child piece objs on grid
        foreach (Transform child in piece.transform)
        {
            Vector3 position = GameManager.instance.Round(child.position);
            if (position.y < gridSizeY)
            {
                theGrid[(int)position.x, (int)position.y, (int)position.z] = child;
            }
        }
    }

    public Transform GetTransformOnGridPosition(Vector3 position)
    {
        if (position.y > gridSizeY - 1)
        {
            return null;
        }
        else
        {
            return theGrid[(int)position.x, (int)position.y, (int)position.z];
        }
    }

    public void SpanwTetrisPiece()
    {
        Vector3 spawnPoint = new Vector3((int)((transform.position.x + (float)gridSizeX / 2)),
                                        (int)transform.position.y + gridSizeY,
                                        (int)((transform.position.z + (float)gridSizeZ / 2)));

        int randomPieceIndex = Random.Range(0, pieceListTag.Length);

        // Get tetris piece from Obj Pool
        GameObject newPiece = ObjectPool.instance.Get(pieceListTag[randomPieceIndex]);
        if (newPiece != null)
        {
            newPiece.transform.position = spawnPoint;
            newPiece.SetActive(true);
        }

        // Get ghost projection from Obj Pool
        GameObject newGhost = ObjectPool.instance.Get(ghostListTag[randomPieceIndex]);
        if (newGhost != null)
        {
            newGhost.GetComponent<GhostPiece>().SetParent(newPiece);
            newGhost.SetActive(true);
        }
        
    }

    public void DeleteFullLayer()
    {
        int layersCleared = 0;
        for (int y = gridSizeY-1; y >= 0; y--)
        {
            if (CheckFullLayer(y))
            {
                DeleteLayerAt(y);
                MoveAllLayerDown(y);
                layerCleared.Raise(); // Layer Cleared Audio
                layersCleared++;
            }
        }
        if (layersCleared > 0) GameManager.instance.LayersCleared(layersCleared);
    }

    private bool CheckFullLayer(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                if (theGrid[x,y,z] == null)
                {
                    return false; // Layer not full
                }
            }
        }
        return true;
    }

    private void DeleteLayerAt(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                theGrid[x, y, z].gameObject.SetActive(false);
                theGrid[x, y, z] = null;
            }
        }
    }

    private void MoveAllLayerDown(int y)
    {
        for (int i = y; i < gridSizeY; i++)
        {
            MoveOneLayerDown(i);
        }
    }

    private void MoveOneLayerDown(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                if (theGrid[x,y,z] != null)
                {
                    theGrid[x, y - 1, z] = theGrid[x, y, z];
                    theGrid[x, y, z] = null;
                    theGrid[x, y - 1, z].position += Vector3.down;
                }
            }
        }
    }
}
