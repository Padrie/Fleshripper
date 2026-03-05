using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;

    int score;

    private void OnEnable()
    {
        Enemy.OnEnemyDied += AddScore;
        FlyingEnemy.OnFlyingEnemyDied += AddScore;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDied -= AddScore;
        FlyingEnemy.OnFlyingEnemyDied -= AddScore;
    }

    public void AddScore(Enemy enemy)
    {
        if (score == 0) score = 1;
        scoreText.text = score++.ToString();
    }    
    
    public void AddScore(FlyingEnemy enemy)
    {
        if (score == 0) score = 1;
        scoreText.text = score++.ToString();
    }
}
