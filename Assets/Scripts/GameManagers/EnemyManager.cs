using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<IEnemy> enemyList;

    void Start()
    {
        enemyList = new List<IEnemy>();

        var badRobots = FindObjectsOfType<BadRobot>();

        foreach (var item in badRobots)
        {
            enemyList.Add(item);
        }

        var batRobots= FindObjectsOfType<BatRobot>();

        foreach (var item in batRobots)
        {
            enemyList.Add(item);
        }
    }

    public void ActivateEnemies()
    {
        foreach (var item in enemyList)
        {
            item.ToggleActivation(true);
        }
    }

    public void DeactivateEnemies()
    {
        foreach (var item in enemyList)
        {
            item.ToggleActivation(false);
        }
    }
}
