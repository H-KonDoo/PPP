using UnityEngine;

public class ReverseStageController : MonoBehaviour
{
    // 💡 このステージが有効になった（始まった）瞬間に呼ばれる
    private void OnEnable()
    {
        // これから生まれてくるボール、および今あるボールすべてを左右反転モードにするための目印
        IsReverseMode = true;
        Debug.Log("左右反転ステージが開始されました！");
    }

    // 💡 このステージが消えた（メニューに戻った）瞬間に呼ばれる
    private void OnDisable()
    {
        // 左右反転モードを解除する
        IsReverseMode = false;
        Debug.Log("左右反転ステージが終了しました。");
    }

    // 💡 ゲーム全体から「今って左右反転モード？」と確認できるようにするための静的な変数（フラグ）
    public static bool IsReverseMode { get; private set; } = false;
}