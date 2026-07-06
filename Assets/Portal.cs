using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("ワープ先のポータルオブジェクト")]
    [SerializeField] private Transform targetPortal; 

    [Header("連続ワープを防ぐ時間（秒）")]
    [SerializeField] private float cooldownTime = 0.3f; 

    [Header("ゲートに触れてからワープするまでの遅延時間（秒）")]
    [SerializeField] private float warpDelay = 0.05f; // 💡 ここで遅らせたい秒数を設定できます

    private float lastWarpTime; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball") && Time.time > lastWarpTime + cooldownTime)
        {
            if (targetPortal != null)
            {
                // 💡 すぐにワープさせず、コルーチン（タイマー処理）を起動する
                StartCoroutine(WarpRoutine(other.transform));
            }
        }
    }

    // 💡 〇秒待ってからワープを実行する処理
    private IEnumerator WarpRoutine(Transform ballTransform)
    {
        // 指定した秒数（warpDelay）だけ待機する
        yield return new WaitForSeconds(warpDelay);

        // ボールがまだ存在している場合のみワープを実行
        if (ballTransform != null && targetPortal != null)
        {
            Debug.Log($"{gameObject.name} から {targetPortal.name} へワープしました！");

            // 瞬間移動
            ballTransform.position = targetPortal.position;

            // クールダウンを設定
            lastWarpTime = Time.time;
            Portal targetScript = targetPortal.GetComponent<Portal>();
            if (targetScript != null)
            {
                targetScript.SetCooldown(Time.time);
            }
        }
    }

    public void SetCooldown(float time)
    {
        lastWarpTime = time;
    }
}