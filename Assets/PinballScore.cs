using UnityEngine;
using UnityEngine.UI;

public class PinballScore : MonoBehaviour
{
    public Text scoreText; // UIテキストにスコアを表示
    private int score = 0;

    // 各オブジェクトの得点設定
    public int smallStarPoints = 1000;
    public int largeStarPoints = 20;
    public int smallCloudPoints = 15;
    public int largeCloudPoints = 30;

    private void Start()
    {
        // スコアテキスト更新関数を呼び出し
        UpdateScoreText();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 衝突時にスコア追加関数を呼び出し
        switch (collision.gameObject.tag)
        {
            case "SmallStarTag":
                AddScore(smallStarPoints);
                break;
            case "LargeStarTag":
                AddScore(largeStarPoints);
                break;
            case "SmallCloudTag":
                AddScore(smallCloudPoints);
                break;
            case "LargeCloudTag":
                AddScore(largeCloudPoints);
                break;
        }
    }

    private void AddScore(int points)
    {
        // スコアを追加する
        score += points;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        // スコアテキストを更新する
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}