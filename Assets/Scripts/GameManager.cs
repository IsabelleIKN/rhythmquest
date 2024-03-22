using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int[,] levelArray;
    private GameObject[,] tileArray;
    private int rows;
    private int cols;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject treasurePrefab;
    [SerializeField] private GameObject[] enemyPrefab;
    private bool isPlaying = false;

    [SerializeField] int CameraYOffset = 5;
    [SerializeField] int CameraZOffset = -3;

    private Color tileColor1 = Color.cyan;
    private Color tileColor2 = Color.magenta;
    private bool toggleColor = false;
    [SerializeField] private float interval = 2f;

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

        StartCoroutine(GenerateLevel(interval));

    }

    IEnumerator Tick(float interval)
    {
        while (isPlaying)
        {
            yield return new WaitForSeconds(interval / 2);
            Debug.Log("Half Tick");
            SwitchGridColors();
            
            yield return new WaitForSeconds(interval / 2);
            Debug.Log("Tick");
            SwitchGridColors();
            MovePlayer();
        }
        

    }

    IEnumerator GenerateLevel(float interval)
    {
        levelArray = HLP.level1;
        rows = levelArray.GetLength(0);
        cols = levelArray.GetLength(1);

        tileArray = new GameObject[rows, cols];

        SetCameraPosition(Vector3.zero, new Vector3(rows - 1, 0, cols - 1));

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
                yield return new WaitForSeconds(interval/16);
            }
        }
    
        SpawnEntities();
    }

    private void SetCameraPosition(Vector3 a, Vector3 b)
    {
        float centerX = (a.x + b.x) * 0.5f;
        Vector3 cameraPos = new Vector3(centerX, CameraYOffset, CameraZOffset);
        Camera.main.transform.position = cameraPos;
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
                        targetColor = toggleColor ? tileColor1 : tileColor2;
                    } else { // Oneven positions
                        targetColor = toggleColor ? tileColor2 : tileColor1;
                    }

                // Kleur toepassen
                Renderer renderer = tileArray[i, j].GetComponentInChildren<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = targetColor;
                }
                }
            }

            // Switch de kleur voor de volgende tick
            toggleColor = !toggleColor;
    }

    private void SpawnEntities()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if(levelArray[i, j] == 1)
                {
                    Instantiate(playerPrefab, new Vector3(i, 0, j), Quaternion.identity);
                } else if (levelArray[i, j] == 2)
                {
                    // Spawn treasures
                    Instantiate(treasurePrefab, new Vector3(i, 0, j), Quaternion.identity);
                } else if (levelArray[i, j] == 3)
                {
                    // Spawn enemies
                    Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], new Vector3(i, 0, j), Quaternion.identity);
                }
            }
        }

        isPlaying = true;
        StartCoroutine(Tick(interval));
    }

    private void MovePlayer()
    {
        int pRow = (int)player.transform.position.x;
        int pCol = (int)player.transform.position.z;

        switch (lastInput)
        {
            case HLP.KeyPress.Left:
            if (pRow > 0)
            {
                player.transform.Translate(-1, 0, 0);
            }
            break;

            case HLP.KeyPress.Right:
            if (pRow < rows - 1)
            {
                player.transform.Translate(1, 0, 0);
            }
            break;

            case HLP.KeyPress.Up:
            if (pCol < cols - 1)
            {
                player.transform.Translate(0, 0, 1);
            }
            break;

            case HLP.KeyPress.Down:
            if (pCol > 0)
            {
                player.transform.Translate(0, 0, -1);
            }
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
