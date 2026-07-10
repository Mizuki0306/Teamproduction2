using UnityEngine;

public class Player : MonoBehaviour
{
    private Brock brockScript;
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

    // トリガーボタン（RT）の連打防止用フラグ
    private bool isRTPressedInTrigger = false;

    void Start()
    {
        // 1. 最優先でAnimatorを取得（これでエラーを確実に防ぐ）
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("【エラー】Playerオブジェクトに Animator コンポーネントが見つかりません！");
        }

        rb = GetComponent<Rigidbody2D>();

        // 2. 安全にBrockスクリプトを探す
        brockScript = FindFirstObjectByType<Brock>();

        // 3. 子オブジェクトからHandColliderを自動検索して取得
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
            _holdBrock = value;
            if (anim != null) anim.SetBool("HoldBrock", value);
        }
    }

    void Update()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");

        currentDir = 0;

        if (inputH != 0 || inputV != 0)
        {
            if (inputH < 0) { currentDir = HoldBrock ? 6 : 1; lastDirection = 1; }
            else if (inputH > 0) { currentDir = HoldBrock ? 5 : 2; lastDirection = 2; }

            if (inputV > 0) { currentDir = HoldBrock ? 7 : 3; lastDirection = 3; }
            else if (inputV < 0) { currentDir = HoldBrock ? 8 : 4; lastDirection = 4; }
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
    }


    void FixedUpdate()
    {
        if (rb == null) return;

        Vector2 movement = new Vector2(inputH, inputV).normalized;
        rb.linearVelocity = movement * PlayerMoveSpeed;
    }


    // ★【修正】コライダー接触中にキーボード「E」またはコントローラー「RT」で掴む判定
    private void OnTriggerStay2D(Collider2D collision)
    {
        // 接触している対象が「Collider_L」という名前のブロックの場合
        if (collision.gameObject.name == "Collider_L")
        {
            float rtValue = Input.GetAxisRaw("RT");

            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || (rtValue > 0.5f && !isRTPressedInTrigger))
            {
                if (rtValue > 0.5f) isRTPressedInTrigger = true; // 連打防止

                // 状態を反転（持っていないなら持つ、持っているなら離す）
                HoldBrock = !HoldBrock;

                if (HoldBrock)
                {
                    // ★★★ 【持った時の処理】 ★★★
                    // ぶつかったブロックの親を「自分（Player）」に設定する
                    collision.gameObject.transform.SetParent(this.transform);

                    // 必要であれば、持った瞬間にプレイヤーの目の前（HandColliderの位置など）に
                    // ブロックの位置をピタッと補正するコードをここに入れると綺麗になります
                    Debug.Log("ブロックを持ちました：プレイヤーの子に設定");
                }
                else
                {
                    // ★★★ 【離した時の処理】 ★★★
                    // ブロックの親を「なし（ステージ直下）」に戻す
                    collision.gameObject.transform.SetParent(null);
                    Debug.Log("ブロックを離しました：親子関係を解除");
                }
            }

            if (rtValue < 0.1f)
            {
                isRTPressedInTrigger = false;
            }
        }
}
}