using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public int nextLevel;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Going Next Level");
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevel);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}
