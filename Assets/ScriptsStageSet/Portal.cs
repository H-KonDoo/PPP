using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("ワープ先のポータルオブジェクト")]
    [SerializeField] private Transform targetPortal; 

    [Header("連続ワープを防ぐ時間（秒）")]
    [SerializeField] private float cooldownTime = 0.5f; 

    private float lastWarpTime; // 最後にワープした時間

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 💡 衝突したオブジェクトのタグが "Ball" かどうか、かつクールダウンが終わっているか判定
        if (other.CompareTag("Ball") && Time.time > lastWarpTime + cooldownTime)
        {
            if (targetPortal != null)
            {
                Debug.Log($"{gameObject.name} から {targetPortal.name} へワープします！");

                // ボールの位置を出口ポータルの位置に瞬間移動
                other.transform.position = targetPortal.position;

                // 出口側のポータルにも同じ現在の時間を設定して、逆方向の即時ワープ（無限ループ）を防ぐ
                Portal targetScript = targetPortal.GetComponent<Portal>();
                if (targetScript != null)
                {
                    targetScript.SetCooldown(Time.time);
                }
            }
        }
    }

    // 外部（他のポータル）からクールダウン時間を同期するための関数
    public void SetCooldown(float time)
    {
        lastWarpTime = time;
    }
}