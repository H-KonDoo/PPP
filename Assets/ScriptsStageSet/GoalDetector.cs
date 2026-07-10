using System.Collections;
using UnityEngine;
using TMPro; // UI（TextMeshPro）用
using DG.Tweening;

public class GoalDetector : MonoBehaviour
{
    [Header("クリアに必要な衝突回数")]
    [SerializeField] private int requiredTouchCount = 3;

    [Header("クリア後、メニューに戻るまでの遅延時間（秒）")]
    [SerializeField] private float returnDelay = 3f;

    [Header("回数を表示するUIテキスト (TextMeshPro)")]
    [SerializeField] private TextMeshProUGUI countText;

    [Header("クリア時に表示するオブジェクト (TEXTやPanelなど)")]
    [SerializeField] private GameObject clearUIObject; // 💡 クリア文字のオブジェクトを登録

    [SerializeField] private StageSelectMovement clearMovement;

    private int currentTouchCount = 0; // 現在の衝突回数
    private bool isCleared = false;    // すでにクリアしたかどうかのフラグ
    private StageSelectManager stageSelectManager;

    public int GetCurrentTouchCount()
    {
        return currentTouchCount;
    }

    private void Start()
    {
        stageSelectManager = FindFirstObjectByType<StageSelectManager>();
        
        if (stageSelectManager == null)
        {
            Debug.LogError("画面内に StageSelectManager が見つかりません！");
        }

        // 💡 ゲーム開始時はクリア文字を非表示にしておく
        if (clearUIObject != null)
        {
            clearUIObject.SetActive(false);
        }

        UpdateCountUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCleared) return;

        if (other.CompareTag("Ball"))
        {
            currentTouchCount++;
            Debug.Log($"ゴールにボールが触れました！ ({currentTouchCount} / {requiredTouchCount})");

            UpdateCountUI();

            if (currentTouchCount >= requiredTouchCount)
            {
                isCleared = true;
                Debug.Log($"ステージクリア！ {returnDelay}秒後に選択画面に戻ります。");
                
                // 💡 指定回数に達したら「CLEAR」の文字を表示する
                ShowClearUI();

                StartCoroutine(ReturnToMenuRoutine());
            }
        }
    }

    private void UpdateCountUI()
    {
        if (countText != null)
        {
            countText.text = $"{currentTouchCount} / {requiredTouchCount}";
        }
    }

    // 💡 クリアテキストを表示する関数
private void ShowClearUI()
    {
        if (clearUIObject != null)
        {
            clearUIObject.SetActive(true); // 画面に出現させる
        }

        if (clearMovement != null)
        {
            clearMovement.ExpandScale();
        }
    }

    private IEnumerator ReturnToMenuRoutine()
    {
        yield return new WaitForSeconds(returnDelay);

        if (stageSelectManager != null)
        {
            stageSelectManager.BackToMenu();
        }
    }
}