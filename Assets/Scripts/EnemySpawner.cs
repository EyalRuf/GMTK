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

    public void SpawnConsecutively(int amountPerWave)
    {
        int amount = amountPerWave;
        if (amount > GameManager.instance.EnemiesLeft)  // If the wave amount is larger than there are enemies left to spawn..
            amount = GameManager.instance.EnemiesLeft;  // Don't spawn a larger wave than there are enemies left

        GameManager.instance.EnemiesLeft -= amount;
        SpawnGroup(amount);
        GameManager.AmountOfActiveEnemies += amount;
    }
}
