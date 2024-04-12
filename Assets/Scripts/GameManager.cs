using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GridManager grid;
    private int[,] levelArray;
    private int rows;
    private int cols;
    private int currentLevel = 0;
    
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject treasurePrefab;
    [SerializeField] private GameObject enemyPrefab;

    private List<Enemy> enemies = new();
    private List<GameObject> treasures = new();
    private List<GameObject> walls = new();
    
    public bool isPlaying = false;
    [SerializeField] private float interval = 2f;

    public Player player;
    public UIManager ui;
    private CameraControl camCtrl;

    

    void Awake()
    {
        if(instance == null) { 
            instance = this;
            Debug.Log("GM: Set instance");
        } else {
            Debug.Log("GM: Double — destroyed");
            Destroy(gameObject);
        }
    }

    private void Start() {

        levelArray = HLP.level1; // TODO: dynamisch maken
        grid.GenerateGrid(levelArray, interval);

        rows = levelArray.GetLength(0);
        cols = levelArray.GetLength(1);

        camCtrl = Camera.main.GetComponent<CameraControl>();
        camCtrl.SetCameraStart(rows, cols);
        
        Debug.Log("GM: Starting with level " + currentLevel);

    }

    public void SpawnEntities()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // Ik wil het grid spawnen zoals ik hem zie in de 2D array,
                //met rows (y) in de -z, en columns (x) in de +x.
                Vector3 spawnLoc = new Vector3(j, 0, -i);

                if(levelArray[i, j] == 2)
                {
                    // Spawn Walls
                    GameObject w = Instantiate(wallPrefab, spawnLoc, Quaternion.identity);
                    walls.Add(w);
                } else if (levelArray[i, j] == 3)
                {
                    // Spawn player
                    GameObject p = Instantiate(playerPrefab, spawnLoc, Quaternion.identity);
                    player = p.GetComponent<Player>();
                } else if (levelArray[i, j] == 4)
                {
                    // Spawn treasures
                    GameObject t = Instantiate(treasurePrefab, spawnLoc, Quaternion.identity);
                    treasures.Add(t);
                } else if (levelArray[i, j] == 5)
                {
                    // Spawn enemies
                    GameObject e = Instantiate(enemyPrefab, spawnLoc, Quaternion.identity);
                    enemies.Add(e.GetComponent<Enemy>());
                }
            }
        }

        Debug.Log("GM: Entities spawned");

        isPlaying = true;
        StartLevel(interval);
        
    }

   
   private void StartLevel(float interval)
   {

        camCtrl.isFollowing = true;
        grid.StartBlinking(interval);
        StartCoroutine(Step(interval));
        // Start music

        Debug.Log("GM: Started level");

   }

    
    // Ik wil de volgorde kunnen bepalen waarin enemies en player bewegen.
    // Kan ook met losse coroutines op individuele agents, maar dan moet ik 
    // de state van het level bijhouden en plekken 'reserveren' voor movement.
    // Moeilijk :(
   IEnumerator Step(float interval)
   {
       while(isPlaying)
       {
            float enemyDelay = enemies.Count * 0.01f;
           yield return new WaitForSeconds(interval - enemyDelay);
           foreach (Enemy enemy in enemies)
           {
               enemy.Move();
               yield return new WaitForSeconds(0.01f);
           }

            player.Move();
       }
   }

   public void UpdateUI(string lastkey)
   {
        ui.UpdateLastKey(lastkey);
   }

   public void PlayerDeath()
   {
        ClearLevel();
        // Show loss UI
        Debug.Log("GM: You dead!");
   }

   public void CollectTreasure(GameObject treasure)
   {
        treasures.Remove(treasure);
        Destroy(treasure);

        if(treasures.Count == 0)
        {
            ClearLevel();
            // Show victory UI
            Debug.Log("GM: Level won!");
        }
   }

   private void ClearLevel()
   {
        StopAllCoroutines();
        isPlaying = false;

        camCtrl.isFollowing = false;

        ClearEnemies();
        ClearPlayer();
        ClearWalls();
        ClearTreasures();

        // Reset camera to start position

        grid.ClearGrid(interval);
   }

   private void ClearEnemies()
   {
        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        enemies.Clear();
   }

    private void ClearWalls()
    {
        foreach (GameObject w in walls)
        {
            Destroy(w);
        }
        walls.Clear();
    }

    private void ClearPlayer()
    {
        if (player != null) 
        {
            Destroy(player.gameObject);
        }
    }

    private void ClearTreasures()
    {
        foreach (GameObject t in treasures)
        {
            Destroy(t);
        }
        treasures.Clear();
    }
   

}
