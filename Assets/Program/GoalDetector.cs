using System.Collections;
using UnityEngine;

public class GoalDetector : MonoBehaviour
{
    [Header("クリアに必要な衝突回数")]
    [SerializeField] private int requiredTouchCount = 3;

    [Header("クリア後、メニューに戻るまでの遅延時間（秒）")]
    [SerializeField] private float returnDelay = 3f;

    private int currentTouchCount = 0; // 現在の衝突回数
    private bool isCleared = false;    // すでにクリアしたかどうかのフラグ
    private StageSelectManager stageSelectManager;

    private void Start()
    {
        // 💡 画面内にある StageSelectManager を自動的に探して紐づける
        stageSelectManager = FindFirstObjectByType<StageSelectManager>();
        
        if (stageSelectManager == null)
        {
            Debug.LogError("画面内に StageSelectManager が見つかりません！");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // すでにクリア済みなら何もしない
        if (isCleared) return;

        // 衝突したオブジェクトのタグが "Ball" だった場合
        if (other.CompareTag("Ball"))
        {
            currentTouchCount++;
            Debug.Log($"ゴールにボールが触れました！ ({currentTouchCount} / {requiredTouchCount})");

            // 指定回数（3回）に達したらクリア処理を実行
            if (currentTouchCount >= requiredTouchCount)
            {
                isCleared = true;
                Debug.Log($"ステージクリア！ {returnDelay}秒後に選択画面に戻ります。");
                
                // 💡 5秒待って戻るタイマー（コルーチン）を起動
                StartCoroutine(ReturnToMenuRoutine());
            }
        }
    }

    // 💡 指定された秒数待ってからメニューに戻る処理
    private IEnumerator ReturnToMenuRoutine()
    {
        // インスペクターで設定した秒数（5秒）待つ
        yield return new WaitForSeconds(returnDelay);

        if (stageSelectManager != null)
        {
            // StageSelectManager に最初から用意されている BackToMenu 関数を呼び出す！
            stageSelectManager.BackToMenu();
        }
    }
}