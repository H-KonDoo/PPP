using UnityEngine;
using UnityEngine.InputSystem; // 新しいInput Systemを使用するために必要

public class StageSelectManager : MonoBehaviour
{
    [Header("ステージ1の設定")]
    [SerializeField] private GameObject stage1Prefab;    // ステージ1のプレハブ
    [SerializeField] private Collider2D areaStage1;       // ステージ1の選択エリア（コライダー）

    [Header("ステージ2の設定")]
    [SerializeField] private GameObject stage2Prefab;    // ステージ2のプレハブ
    [SerializeField] private Collider2D areaStage2;       // ステージ2の選択エリア（コライダー）

    [Header("ステージ3の設定")]
    [SerializeField] private GameObject stage3Prefab;    // ステージ3のプレハブ
    [SerializeField] private Collider2D areaStage3;       // ステージ3の選択エリア（コライダー）

    [Header("ステージ4の設定")]
    [SerializeField] private GameObject stage4Prefab;    // ステージ4のプレハブ
    [SerializeField] private Collider2D areaStage4;       // ステージ4の選択エリア（コライダー）

    [Header("ステージ5の設定")]
    [SerializeField] private GameObject stage5Prefab;    // ステージ5のプレハブ
    [SerializeField] private Collider2D areaStage5;       // ステージ5の選択エリア（コライダー）

    [Header("ステージ6の設定")]
    [SerializeField] private GameObject stage6Prefab;    // ステージ6のプレハブ
    [SerializeField] private Collider2D areaStage6;       // ステージ6の選択エリア（コライダー）
    
    [Header("ステージを生成する中心位置")]
    [SerializeField] private Transform stageSpawnPoint;

    [Header("ボール生成器の登録")]
    [SerializeField] private GameObject ballSpawner;     // インスペクターでPPPBallSpawnerを登録する枠

    private GameObject currentActiveStage; // 現在画面に生成されているステージのオブジェクト

    private void Update()
    {
        // 💡 マウスの左クリックが押された瞬間を検知（テスト用）
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // マウスの画面上のピクセル座標を取得
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            // マウスの座標をゲーム内の2D空間座標（ワールド座標）に変換
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);
            
            // ステージ選択判定の関数を呼び出す（付箋が貼られたときと同じ処理）
            OnStickyNoteDetected(mouseWorldPos);
        }
    }

    // 💡 OpenCVなどの付箋認識システム、または上記のマウスクリックから座標を受け取る関数
    public void OnStickyNoteDetected(Vector2 noteWorldPosition)
    {
        // すでにステージが生成されてゲームが始まっている場合は、重なって生成されないように無視する
        if (currentActiveStage != null) return;

        // 各ステージのエリア内に入っているか順番に判定
        if (areaStage1 != null && areaStage1.OverlapPoint(noteWorldPosition))
        {
            StartStage(stage1Prefab);
        }
        else if (areaStage2 != null && areaStage2.OverlapPoint(noteWorldPosition))
        {
            StartStage(stage2Prefab);
        }
        else if (areaStage3 != null && areaStage3.OverlapPoint(noteWorldPosition))
        {
            StartStage(stage3Prefab);
        }
        else if (areaStage4 != null && areaStage4.OverlapPoint(noteWorldPosition))
        {
            StartStage(stage4Prefab);
        }
        else if (areaStage5 != null && areaStage5.OverlapPoint(noteWorldPosition))
        {
            StartStage(stage5Prefab);
        }
        else if (areaStage6 != null && areaStage6.OverlapPoint(noteWorldPosition))
        {
            StartStage(stage6Prefab);
        }
    }

    // 💡 実際にステージを生成してゲームを開始する関数
    private void StartStage(GameObject selectedStagePrefab)
    {
        if (selectedStagePrefab == null) return;

        Debug.Log($"ステージ 【{selectedStagePrefab.name}】 を開始します！");

        // ステージ選択ボタン（全6つ）を画面から非表示にする
        SetAllAreasActive(false);

        // 指定された位置（未設定ならゼロ座標）にステージのプレハブを生成
        Vector3 spawnPos = stageSpawnPoint != null ? stageSpawnPoint.position : Vector3.zero;
        currentActiveStage = Instantiate(selectedStagePrefab, spawnPos, Quaternion.identity);

        // 💡 ステージが生成されたあとに、ボール生成器（PPPBallSpawner）をONにして位置を合わせる
        if (ballSpawner != null)
        {
            ballSpawner.SetActive(true);

            // 💡 【発射位置自動設定の処理】生成されたステージの中から "SpawnPoint" というオブジェクトを探す
            Transform customSpawnPoint = currentActiveStage.transform.Find("SpawnPoint");
            
            if (customSpawnPoint != null)
            {
                // SpawnPointが見つかったら、そこへスポーナーの座標を同期移動させる
                ballSpawner.transform.position = customSpawnPoint.position;
                Debug.Log($"{selectedStagePrefab.name} の SpawnPoint 位置へ発射口を移動しました。");
            }
            else
            {
                // ⚠️ 万が一、SpawnPointを作り忘れたステージがあった時のための保険用デフォルト位置（例: X:0, Y:3）
                ballSpawner.transform.position = new Vector3(0f, 3f, 0f);
                Debug.LogWarning($"{selectedStagePrefab.name} に SpawnPoint が見つかりません！デフォルト位置に配置しました。");
            }
        }
    }

    // 💡 必要に応じてメニュー（ステージ選択状態）に戻すための関数（リセットボタン等用）
    public void BackToMenu()
    {
        // 現在のステージを削除
        if (currentActiveStage != null)
        {
            Destroy(currentActiveStage);
        }

        // ボール生成器を再びOFFにする
        if (ballSpawner != null)
        {
            ballSpawner.SetActive(false);
        }

        // ステージ選択ボタン（全6つ）を再表示する
        SetAllAreasActive(true);
    }

    // 💡 全ての選択エリアの表示/非表示を一括で切り替えるヘルパー関数
    private void SetAllAreasActive(bool isActive)
    {
        if (areaStage1 != null) areaStage1.gameObject.SetActive(isActive);
        if (areaStage2 != null) areaStage2.gameObject.SetActive(isActive);
        if (areaStage3 != null) areaStage3.gameObject.SetActive(isActive);
        if (areaStage4 != null) areaStage4.gameObject.SetActive(isActive);
        if (areaStage5 != null) areaStage5.gameObject.SetActive(isActive);
        if (areaStage6 != null) areaStage6.gameObject.SetActive(isActive);
    }
}