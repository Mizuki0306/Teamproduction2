using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [Header("--- プレイヤーの動く速度 ---")]
    public float PlayerMoveSpeed = 2;
    [Header("--- ゲーム時間 ---")]
    public static float GameTimer;
    [Header("--- プレイヤーの位置 ---")]
    private Vector2 PlayerPosition;
    [Header("--- 手数カウント ---")]
    public int MoveCount;
    public int currentDir;
    private Animator anim;
    public bool _holdBrock;
    private int lastDirection = 1;
    public float inputH;
    public float inputV;
    private Rigidbody2D rb;
    [SerializeField] private GameObject handCollider;

    // ★追加：掴んだ瞬間の拘束軸（0=なし, 1=左右軸, 2=上下軸）
    private int lockedAxis = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("【エラー】Playerオブジェクトに Animator コンポーネントが見つかりません！");
        }
        rb = GetComponent<Rigidbody2D>();
        var handTransform = transform.Find("HandCollider");
        if (handTransform != null)
        {
            handCollider = handTransform.gameObject;
        }
    }

    public bool HoldBrock
    {
        get => _holdBrock;
        set
        {
            bool wasHolding = _holdBrock;
            _holdBrock = value;
            if (anim != null) anim.SetBool("HoldBrock", value);

            // ★掴んだ瞬間（false→true）だけ軸を記録
            if (!wasHolding && value)
            {
                lockedAxis = (lastDirection == 1 || lastDirection == 2) ? 1 : 2; // 1=左右軸, 2=上下軸
                Debug.Log("拘束軸を記録: " + (lockedAxis == 1 ? "左右軸" : "上下軸"));
            }
            // 離した瞬間は軸をリセット
            else if (wasHolding && !value)
            {
                lockedAxis = 0;
            }
        }
    }

    void Update()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");

        // ★掴んでいる間は、拘束軸と垂直な方向の入力を無視する
        if (HoldBrock)
        {
            if (lockedAxis == 1) // 左右軸に拘束 → 上下入力を無視
            {
                inputV = 0;
            }
            else if (lockedAxis == 2) // 上下軸に拘束 → 左右入力を無視
            {
                inputH = 0;
            }
        }

        currentDir = 0;
        if(inputH != 0 || inputV != 0)
{
            // --- 【横移動の判定】 ---
            if (inputH != 0)
            {
                if (inputH > 0 && HoldBrock && lastDirection == 1) { currentDir = 17; } // 右入力 ＆ 掴み ＆ 直前左向き
                else if (inputH < 0 && HoldBrock && lastDirection == 2) { currentDir = 18; } // 左入力 ＆ 掴み ＆ 直前右向き
                else if (inputH < 0) { currentDir = HoldBrock ? 6 : 1; lastDirection = 1; }  // 通常の左移動
                else if (inputH > 0) { currentDir = HoldBrock ? 5 : 2; lastDirection = 2; }  // 通常の右移動
            }
            // --- 【縦移動の判定】（横に入力がない時だけ処理） ---
            else if (inputV != 0)
            {
                if (inputV > 0 && HoldBrock && lastDirection == 4) { currentDir = 20; } // 上入力 ＆ 掴み ＆ 直前下向き
                                                                                        // コメントアウトされていた19番（下入力 ＆ 掴み ＆ 直前上向き）を使う場合はここに入れます
                else if (inputV < 0 && HoldBrock && lastDirection == 3) { currentDir = 19; }
                else if (inputV > 0) { currentDir = HoldBrock ? 7 : 3; lastDirection = 3; }  // 通常の上移動
                else if (inputV < 0) { currentDir = HoldBrock ? 8 : 4; lastDirection = 4; }  // 通常の下移動
            }
        }
        else
        {
            if (HoldBrock)
                currentDir = lastDirection == 1 ? 10 : lastDirection == 2 ? 9 : lastDirection == 3 ? 11 : 12;
            else
                currentDir = lastDirection == 1 ? 13 : lastDirection == 2 ? 14 : lastDirection == 3 ? 15 : 16;
        }

        if (anim != null)
        {
            anim.SetInteger("Direction", currentDir);
        }

        UpdateHandPosition();
    }

    private void UpdateHandPosition()
    {
        if (handCollider == null) return;

        // ★掴んでいる間はハンド位置を拘束軸の元の向きに固定してもよいが、
        // 一旦は既存通り lastDirection ベースのままにしておく
        Vector2 offset = lastDirection switch
        {
            1 => new Vector2(0.5f, 0f),
            2 => new Vector2(0.5f, 0f),
            3 => new Vector2(0f, 0.5f),
            4 => new Vector2(0f, -0.5f),
            _ => Vector2.zero
        };
        handCollider.transform.localPosition = offset;
    }

    void FixedUpdate()
    {
        if (rb == null) return;
        Vector2 movement = new Vector2(inputH, inputV).normalized;
        rb.linearVelocity = movement * PlayerMoveSpeed;
    }
}