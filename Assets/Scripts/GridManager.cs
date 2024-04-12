using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private float genSpeed;
    [SerializeField] private float blinkSpeed;
    private GameObject[,] tileArray;
    private int rows;
    private int cols;


    [SerializeField] private Color tileColor1;
    [SerializeField] private Color tileColor2;
    private bool toggleColor = false;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.grid = this;
    }

    public void GenerateGrid(int[,] layout, float interval)
    {
        StartCoroutine(IE_GenerateTiles(layout, interval, genSpeed));
    }

    public void StartBlinking(float interval)
    {
        StartCoroutine(IE_BlinkGrid(interval, blinkSpeed));
    }

    IEnumerator IE_GenerateTiles(int[,] layout, float interval, float factor)
    {
        rows = layout.GetLength(0);
        cols = layout.GetLength(1);

        tileArray = new GameObject[rows, cols];


        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (layout[i, j] != 0)
                {
                    GameObject tile = Instantiate(tilePrefab, new Vector3(j, 0, -i), Quaternion.identity, transform);
                    tileArray[i, j] = tile;

                    if ((i + j) % 2 == 0)
                    {
                        tileArray[i, j].GetComponentInChildren<Renderer>().material.color = tileColor1;
                    }
                    else
                    {
                        tileArray[i, j].GetComponentInChildren<Renderer>().material.color = tileColor2;
                    }

                    yield return new WaitForSeconds(interval / factor);
                }
            }
        }

        Debug.Log("Grid: Tiles spawned, informing GM");

        GameManager.instance.SpawnEntities();
    }

    IEnumerator IE_BlinkGrid(float interval, float factor)
    {
        while (GameManager.instance.isPlaying)
        {
            yield return new WaitForSeconds(interval / factor);
            SwitchGridColors();
        }
    }


    private void SwitchGridColors()
    {
        // Loop door de array heen
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Color targetColor;

                if ((i + j) % 2 == 0) // Even posities
                {
                    if (toggleColor)
                    {
                        targetColor = tileColor1;
                    }
                    else
                    {
                        targetColor = tileColor2;
                    }
                }
                else
                { // Oneven positions
                    if (toggleColor)
                    {
                        targetColor = tileColor2;
                    }
                    else
                    {
                        targetColor = tileColor1;
                    }
                }

                // Kleur toepassen
                if (tileArray[i, j] != null)
                {
                    Renderer renderer = tileArray[i, j].GetComponentInChildren<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = targetColor;
                    }
                }
            }
        }

        // Switch de kleur voor de volgende tick
        toggleColor = !toggleColor;
    }

    public void ClearGrid(float interval)
    {
        StartCoroutine(IE_ClearGrid(interval, genSpeed));
    }

    IEnumerator IE_ClearGrid(float interval, float factor)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (tileArray[i, j] != null)
                {
                    Destroy(tileArray[i, j]);
                    yield return new WaitForSeconds(interval / factor);
                }
            }
        }
    }
}
