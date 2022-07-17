using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyAsset;
    public int EnemiesPerRow;
    public float DistanceBetweenEnemies;

    public void SpawnGroup(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var enemy = Instantiate(EnemyAsset);
            enemy.transform.position = transform.position;

            Vector3 right = transform.right * DistanceBetweenEnemies * (i % EnemiesPerRow);
            Vector3 backward = -transform.forward * DistanceBetweenEnemies * Mathf.FloorToInt(i / EnemiesPerRow);

            enemy.transform.Translate(right + backward);
        }
    }

    public void SpawnConsecutively(int totalAmount, int amountPerWave, float interval)
    {
        _ = StartCoroutine(SpawnConsecutivelyCR());

        IEnumerator SpawnConsecutivelyCR()
        {
            while (GameLoop.instance.EnemiesLeft > 0)
            {
                int amount = amountPerWave;
                if (amount > GameLoop.instance.EnemiesLeft)  // If the wave amount is larger than there are enemies left to spawn..
                    amount = GameLoop.instance.EnemiesLeft;  // Don't spawn a larger wave than there are enemies left

                SpawnGroup(amount);

                GameLoop.instance.EnemiesLeft -= amount;
                print(GameLoop.instance.EnemiesLeft + " enemies left!");

                yield return new WaitForSeconds(interval);
            }

            print("spawned all enemies of this round!");
        }
    }
}
