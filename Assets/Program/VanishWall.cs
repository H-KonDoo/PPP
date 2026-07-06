using UnityEngine;

public class VanishWall : MonoBehaviour
{
    // 💡 物理的に跳ね返る壁のまま、触れたボールを消す場合は OnCollisionEnter2D を使います
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 衝突したオブジェクトのタグが "Ball" だった場合
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("ボールがトラップ壁に触れたため、消滅しました。");
            
            // 💡 ボールをゲーム内から完全に削除します
            Destroy(collision.gameObject);
        }
    }
}