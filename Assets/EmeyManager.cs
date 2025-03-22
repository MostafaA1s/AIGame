using UnityEngine;
using UnityEngine.UI;

public class EmeyManager : MonoBehaviour
{
    public static EmeyManager instance;
    public GameObject[] Enemies;
    public int curEnemyIndex;
    public int score;
    public Text scoreTxt;
    public AudioSource src;
    private void Start()
    {
        instance = this;
        SpawnNextEnemy();
    }

    public void IncScore()
    {
        src.Play();
        score += 5;
        scoreTxt.text = "" + score;
        SpawnNextEnemy();
    }

    public void DecScore()
    {
        score -= 5;
        scoreTxt.text = "" + score;

    }

    void SpawnNextEnemy()
    {
        if (curEnemyIndex > Enemies.Length - 1)
            curEnemyIndex = 0;

        Enemies[curEnemyIndex].SetActive(true);
        Enemies[curEnemyIndex].GetComponent<Enemy>().ResetAttack();
        curEnemyIndex++;
    }

}
