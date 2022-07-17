using System.Collections;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    #region Properties
    public static GameLoop instance;

    public static int AmountOfActiveEnemies = 0;

    [SerializeField]
    private int maxAmountOfActiveEnemies = 25;
    public Rounds currentRound;
    [Space(10)]
    public int enemiesIncreaseAmount = 2;
    public int enemiesPerWaveIncreaseAmount = 2;

    [SerializeField]
    private float timeBetweenRounds = 6f;

    [Header("Assignables")]
    public EnemySpawner[] spawners;
    private int currentSpawnerToUse = 0;
    public PlayerHUD playerHUD;

    private WaitForSeconds spawnIntervalWait;
    private WaitForSeconds timeBetweenRoundsWait;
    private WaitForFixedUpdate fixedUpdate;

    public int EnemiesLeft { get => currentRound.nrOfEnemiesLeft; set => currentRound.nrOfEnemiesLeft = value; }
    #endregion

    private void Awake() => instance = this;

    private void Start()
    {
        spawnIntervalWait = new WaitForSeconds(currentRound.spawnInterval);
        timeBetweenRoundsWait = new WaitForSeconds(timeBetweenRounds);
        fixedUpdate = new WaitForFixedUpdate();

        if (playerHUD == null)
        {
            playerHUD = FindObjectOfType<PlayerHUD>();
        }

        currentRound.nrOfEnemiesLeft = currentRound.nrOfEnemiesToSpawn;

        _ = StartCoroutine(GameLoopCR());
    }

    public IEnumerator GameLoopCR()
    {
        while (true)
        {
            //print("Start of round.");

            while (AmountOfActiveEnemies <= maxAmountOfActiveEnemies)
            {
                while (currentRound.nrOfEnemiesLeft > 0)
                {
                    spawners[currentSpawnerToUse].SpawnConsecutively(currentRound.nrOfEnemiesPerWave);
                    yield return spawnIntervalWait;
                }

                currentSpawnerToUse++;
                if (currentSpawnerToUse >= spawners.Length)
                    currentSpawnerToUse = 0;

                // STOP THIS WHEN PAUSING SPAWNING
                playerHUD?.SpawnAnim(timeBetweenRounds);

                //print("Round over. Cooldown between rounds...");
                yield return timeBetweenRoundsWait;

                NextRound();
            }

            //print(AmountOfActiveEnemies);

            yield return fixedUpdate;
        }
    }

    /// <summary>
    /// Set new round parameters
    /// </summary>
    public void NextRound()
    {
        currentRound.nrOfEnemiesToSpawn += enemiesIncreaseAmount;
        currentRound.nrOfEnemiesLeft = currentRound.nrOfEnemiesToSpawn;

        currentRound.nrOfEnemiesPerWave += enemiesPerWaveIncreaseAmount;
    }
}

[System.Serializable]
public class Rounds
{
    [Tooltip("Amount of enemeies to spawn this round")]
    public int nrOfEnemiesToSpawn = 1;
    [HideInInspector]
    public int nrOfEnemiesLeft = 1;
    [Tooltip("Amount of enemies to spawn at once per wave")]
    public int nrOfEnemiesPerWave = 1;
    [Tooltip("Time between spawns")]
    public float spawnInterval = 2f;
}
