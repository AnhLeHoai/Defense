using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawEnemy : MonoBehaviour
{
    private static bool SpawnEnemy( Vector3[] triangle, GameObject enemyPrefabs, Vector3 range)
    {
        Vector2 result;
        Vector3 enemyPosition;
        bool canSpawnHere = false;
        int attempt = 0;
        do
        {
            attempt += 1;
            var r1 = Mathf.Sqrt(Random.Range(0f, 1f));
            var r2 = Random.Range(0f, 1f);

            var m1 = 1 - r1;
            var m2 = r1 * (1 - r2);
            var m3 = r2 * r1;

            var p1 = new Vector2(triangle[0].x, triangle[0].z);
            var p2 = new Vector2(triangle[1].x, triangle[1].z);
            var p3 = new Vector2(triangle[2].x, triangle[2].z);

            result = (m1 * p1) + (m2 * p2) + (m3 * p3);
            enemyPosition = new Vector3(result.x, 3, result.y);

            canSpawnHere = canSpawn(enemyPosition, range);
            if ( attempt >= 50)
            {
                canSpawnHere = true;
                Debug.Log("Too many attempt");
            }
        } while (!canSpawnHere);
        
        if ( attempt < 50)
        {
            GameObject spawner = SimplePool.Spawn(enemyPrefabs, enemyPosition, Quaternion.Euler(0, 180, 0));
            return true;
        }

        return false;
    }

    public static int SpawnEnemies(Vector3[] triangle, GameObject enemyPrefabs, int count, Vector3 range)
    {
        int success = 0;
        for (int i = 0; i < count; i++)
        {
            if ( SpawnEnemy(triangle, enemyPrefabs, range))
            {
                success += 1;
            }
        }
        return success;
    }

    private static bool SpawnBoss(Vector3[] rectangle, GameObject enemyPrefabs, Vector3 range)
    {
        Vector3 bossPosition;
        bool canSpawnHere = false;
        int attempt = 0;

        do
        {
            var positionX = Random.Range(rectangle[0].x, rectangle[1].x);
            var positionZ = Random.Range(rectangle[1].z, rectangle[2].z);
            bossPosition = new Vector3(positionX, 3, positionZ);
            canSpawnHere = canSpawn(bossPosition, range);
            if (attempt >= 50)
            {
                canSpawnHere = true;
                Debug.Log("Too many attempt");
            }
        } while (!canSpawnHere);

        if (attempt < 50)
        {
            GameObject spawner = SimplePool.Spawn(enemyPrefabs, bossPosition, Quaternion.Euler(0, 180, 0));
            return true;
        }

        return false;
    }

    public static void SpawBosses(Vector3[] rectangle, GameObject enemyPrefabs, int count, Vector3 range)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnBoss(rectangle, enemyPrefabs, range);
        }
    }

    public static bool canSpawn(Vector3 spawnPosition, Vector3 range)
    {
        bool canSpawn = true;
        Collider[] nearCollider = Physics.OverlapBox(spawnPosition, range, Quaternion.Euler( Vector3.zero), LayerMask.GetMask("Enemy"));
        if ( nearCollider.Length != 0)
        {
            canSpawn = false;
        }
        return canSpawn;
    }
}
