using UnityEngine;

public class GravityController : MonoBehaviour
{
    [Header("このステージでの重力の向きと強さ")]
    [SerializeField] private Vector2 stageGravity = new Vector2(0f, 9.81f); // 💡 Yをプラスにすると上向きの重みになります

    private Vector2 defaultGravity; // 元々の地球の重力を保存する変数

    private void OnEnable()
    {
        // 1. Unityの現在の重力設定（通常は X:0, Y:-9.81）をバックアップとして保存
        defaultGravity = Physics2D.gravity;

        // 2. 重力をこのステージ専用の設定（上向き）に書き換える
        Physics2D.gravity = stageGravity;
        
        Debug.Log("重力が反転しました！ (上向き重り)");
    }

    private void OnDisable()
    {
        // 3. このステージが消える（非アクティブになる）ときに、重力を元の正常な状態（下向き）に戻す
        Physics2D.gravity = defaultGravity;
        
        Debug.Log("重力が元に戻りました。 (下向き重り)");
    }
}
