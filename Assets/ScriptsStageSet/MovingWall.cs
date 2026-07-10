using UnityEngine;

public class MovingWall : MonoBehaviour
{
    [Header("移動する距離（X軸、Y軸）")]
    [SerializeField] private Vector2 moveDistance = new Vector2(0f, 3f); //縦に3マス動く

    [Header("移動するスピード")]
    [SerializeField] private float speed = 2f;

    private Vector2 startPosition;

    private void Start()
    {
        // ステージ開始時の初期位置を記憶する
        startPosition = transform.position;
    }

    private void Update()
    {
        // 💡 Mathf.PingPong を使うと、0 から 1 の間を自動で往復する値が作れます
        float pingPong = Mathf.PingPong(Time.time * speed, 1f);
        
        // 初期位置から、指定した距離の間をスムーズに往復移動させる
        Vector2 targetPosition = startPosition + (moveDistance * pingPong);
        transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
    }
}