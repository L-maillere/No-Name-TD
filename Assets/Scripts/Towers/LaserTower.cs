using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : Tower
{

    void Attack(Enemy enemy)
    {
        GameObject projectile = Instantiate(projectilePrefab[level], transform.position, transform.rotation);
        projectile.transform.LookAt(enemy.transform);
        StartCoroutine(LaserHoming(projectile, enemy));
        StartCoroutine(StartCooldown());
    }

    public IEnumerator LaserHoming(GameObject projectile, Enemy enemy)
    {
        while(Vector3.Distance(enemy.transform.position, projectile.transform.position) > 0.2f)
        {
            projectile.transform.position += (enemy.transform.position - projectile.transform.position).normalized * projectileSpeed * Time.deltaTime;
            projectile.transform.LookAt(enemy.transform);
            yield return null;
        }
        enemy.ApplyDamage(projectileDamageValue);
        Destroy(projectile);
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
