using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private int gridSize = 5;
    [SerializeField] private int[,] levelArray;
    private GameObject[,] tileArray;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private bool isPlaying;

    private Color tileColor1 = Color.cyan;
    private Color tileColor2 = Color.magenta;
    private bool toggleColor = false;
    [SerializeField] private float interval = 0.5f;

    public HLP.KeyPress lastInput = HLP.KeyPress.None;
    public Transform player;

    public TextMeshProUGUI lastInputTxt;
    

    void Awake()
    {
        if(instance == null) { 
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        GenerateLevel();
        isPlaying = true;
        StartCoroutine(Tick(interval));
    }

    IEnumerator Tick(float interval)
    {
        while (isPlaying)
        {
            Debug.Log("Tick");
            SwitchGridColors();
            MovePlayer();
            yield return new WaitForSeconds(interval);
        }
        

    }

    private void GenerateLevel()
    {
        levelArray = new int[gridSize, gridSize];
        tileArray = new GameObject[gridSize, gridSize];

        int rows = levelArray.GetLength(0);
        int cols = levelArray.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, 0, j), Quaternion.identity);
                tileArray[i,j] = tile;
                if ((i + j) % 2 == 0)
                {
                    tileArray[i, j].GetComponentInChildren<Renderer>().material.color = tileColor1;
                } else
                {
                    tileArray[i, j].GetComponentInChildren<Renderer>().material.color = tileColor2;
                }
            }
        }
    }

    private void SwitchGridColors()
    {
        // Get the dimensions of your 2D array
        int rows = tileArray.GetLength(0);
        int cols = tileArray.GetLength(1);

            // Loop through each element in the 2D array
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Color targetColor;
                    if ((i + j) % 2 == 0) // Even positions
                    {
                        targetColor = toggleColor ? tileColor1 : tileColor2;
                    } else { // Odd positions
                        targetColor = toggleColor ? tileColor2 : tileColor1;
                    }

                // Apply the color
                Renderer renderer = tileArray[i, j].GetComponentInChildren<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = targetColor;
                }
                }
            }

            // Toggle the color for the next iteration
            toggleColor = !toggleColor;
    }

    private void MovePlayer()
    {
        // TODO: check if can move in specified direction
        // Use array for coordinates?
        switch (lastInput)
        {
            case HLP.KeyPress.Up:
            player.transform.Translate(0, 0, 1);
            break;

            case HLP.KeyPress.Down:
            player.transform.Translate(0, 0, -1);
            break;

            case HLP.KeyPress.Left:
            player.transform.Translate(-1, 0, 0);
            break;

            case HLP.KeyPress.Right:
            player.transform.Translate(1, 0, 0);
            break;

            default:
            break;
        }

        lastInput = HLP.KeyPress.None;
        lastInputTxt.text = "Last Input: None";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) {
            lastInputTxt.text = "Last Input: W";
            lastInput = HLP.KeyPress.Up;
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            lastInputTxt.text = "Last Input: A";
            lastInput = HLP.KeyPress.Left;
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            lastInputTxt.text = "Last Input: S";
            lastInput = HLP.KeyPress.Down;
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            lastInputTxt.text = "Last Input: D";
            lastInput = HLP.KeyPress.Right;
        }
    }

}
