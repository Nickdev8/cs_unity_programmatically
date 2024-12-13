using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparnScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    private List<GameObject> bullets = new List<GameObject>();
    private const int MaxBullets = 20;

    void Start()
    {
        StartCoroutine(CallSpawner());
        StartCoroutine(CheckRevengeBullets());
    }

    private IEnumerator CallSpawner()
    {
        while (true)
        {
            // Spawn a new bullet
            GameObject newBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = newBullet.AddComponent<Rigidbody>();
            }

            // Assign a random direction and fixed speed
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            rb.linearVelocity = randomDirection * 5f; // Fixed speed of 5 units/s

            bullets.Add(newBullet);

            // If the list exceeds the maximum, remove and destroy the oldest bullet
            if (bullets.Count > MaxBullets)
            {
                GameObject oldestBullet = bullets[0];
                bullets.RemoveAt(0);
                Destroy(oldestBullet);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator CheckRevengeBullets()
    {
        while (true)
        {
            List<GameObject> revengeBullets = new List<GameObject>();

            // Loop through all bullets
            foreach (var bullet in bullets)
            {
                // Generate a random number
                int randomChance = Random.Range(0, 101);
                if (randomChance >= 90)
                {
                    // Add to temporary revenge bullet list
                    revengeBullets.Add(bullet);
                }
            }

            // Process revenge bullets
            foreach (var bullet in revengeBullets)
            {
                // Create two new bullets with random directions
                for (int i = 0; i < 2; i++)
                {
                    Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                    GameObject newRevengeBullet = Instantiate(bulletPrefab, bullet.transform.position, Quaternion.identity);
                    Rigidbody rb = newRevengeBullet.GetComponent<Rigidbody>();
                    if (rb == null)
                    {
                        rb = newRevengeBullet.AddComponent<Rigidbody>();
                    }
                    rb.linearVelocity = randomDirection * 5f; // Fixed speed of 5 units/s
                    bullets.Insert(bullets.IndexOf(bullet) + 1, newRevengeBullet);
                }

                // Remove the old bullet
                bullets.Remove(bullet);
                Destroy(bullet);
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
