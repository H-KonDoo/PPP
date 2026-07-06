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
    [SerializeField] private Collider2D areaStage3; 

        [Header("ステージ4の設定")]
    [SerializeField] private GameObject stage4Prefab;    // ステージ4のプレハブ
    [SerializeField] private Collider2D areaStage4; 
    
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

        // 付箋（またはクリック）の座標がステージ1のエリア内に入っているか判定
        if (areaStage1.OverlapPoint(noteWorldPosition))
        {
            StartStage(stage1Prefab);
        }
        // 付箋（またはクリック）の座標がステージ2のエリア内に入っているか判定
        if (areaStage2.OverlapPoint(noteWorldPosition))
        {
            StartStage(stage2Prefab);
        }
        if (areaStage3.OverlapPoint(noteWorldPosition))
        {
            StartStage(stage3Prefab);
        }
        else if (areaStage4.OverlapPoint(noteWorldPosition))
        {
            StartStage(stage4Prefab);
        }
    }

    // 💡 実際にステージを生成してゲームを開始する関数
    private void StartStage(GameObject selectedStagePrefab)
    {
        Debug.Log($"ステージ 【{selectedStagePrefab.name}】 を開始します！");

        // ステージ選択ボタン（赤い四角・青い四角）を画面から非表示にする
        areaStage1.gameObject.SetActive(false);
        areaStage2.gameObject.SetActive(false);
        areaStage3.gameObject.SetActive(false);
        areaStage4.gameObject.SetActive(false);

        // 指定された位置（未設定ならゼロ座標）にステージのプレハブを生成
        Vector3 spawnPos = stageSpawnPoint != null ? stageSpawnPoint.position : Vector3.zero;
        currentActiveStage = Instantiate(selectedStagePrefab, spawnPos, Quaternion.identity);

        // 💡 ステージが生成されたあとに、ボールスピーナー（PPPBallSpawner）をONにしてボールを落とす
        if (ballSpawner != null)
        {
            ballSpawner.SetActive(true);
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

        // ボールスピーナーを再びOFFにする
        if (ballSpawner != null)
        {
            ballSpawner.SetActive(false);
        }

        // ステージ選択ボタン（赤い四角・青い四角）を再表示する
        areaStage1.gameObject.SetActive(true);
        areaStage2.gameObject.SetActive(true);
        areaStage3.gameObject.SetActive(true);
        areaStage4.gameObject.SetActive(true);
    }
}