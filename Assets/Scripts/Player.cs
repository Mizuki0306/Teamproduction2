using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Brock brockScript;
    [Header("--- プレイヤーの動く速度 ---")]
    public float PlayerMoveSpeed = 2; //プレイヤーの動く速度
    [Header("--- ゲーム時間 ---")]
    public static float GameTimer;
    [Header("--- プレイヤーの位置 ---")]
    private Vector2 PlayerPosition;
    [Header("--- プレイヤーの移動可能範囲(x=left,y=right,z=bottom,w=top) ---")]
    public Vector4 PlayerRotation;
    [Header("--- 手数カウント ---")]
    public int MoveCount;

    //アニメーション
    public int currentDir;
    private Animator anim;
    private bool _holdBrock;
    private int lastDirection = 1; // 1:左, 2:右, 3:上, 4:下 (初期値は左)

    // 【追加】UpdateからFixedUpdateへ入力を渡す変数
    private float inputH;
    private float inputV;

    // 【追加】物理移動のためのRigidbody2D
    private Rigidbody2D rb;

    // インスペクターからセットできるように、GameObject型の変数を作る
    [SerializeField] private GameObject handCollider;
    [SerializeField] private GameObject playerCollider;

    void Start()
    {
        brockScript = FindFirstObjectByType<Brock>();
        MoveCount = 0;
        anim = GetComponent<Animator>();
        handCollider = transform.Find("HandCollider").gameObject;
        playerCollider = transform.Find("PlayerCollider").gameObject;

        // ★ Rigidbody2Dを自動取得
        rb = GetComponent<Rigidbody2D>();
    }

    public bool HoldBrock
    {
        get { return _holdBrock; }
        set
        {
            _holdBrock = value;
            if (anim != null)
            {
                anim.SetBool("HoldBrock", _holdBrock);
            }
        }
    }

    // ① Updateでは「入力の取得」と「アニメーション番号の決定」のみを行う
    void Update()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");

        currentDir = 0;

        if (inputH != 0 || inputV != 0)
        {
            // if-else if を使って条件式をスッキリ整理
            if (inputH < -0.5f) { currentDir = HoldBrock ? 6 : 1; lastDirection = 1; }
            else if (inputH > 0.5f) { currentDir = HoldBrock ? 5 : 2; lastDirection = 2; }

            if (inputV > 0.5f) { currentDir = HoldBrock ? 7 : 3; lastDirection = 3; }
            else if (inputV < -0.5f) { currentDir = HoldBrock ? 8 : 4; lastDirection = 4; }
        }
        else // (inputH == 0 && inputV == 0) と同じ意味
        {
            if (HoldBrock)
            {
                if (lastDirection == 1) currentDir = 10;
                else if (lastDirection == 2) currentDir = 9;
                else if (lastDirection == 3) currentDir = 11;
                else if (lastDirection == 4) currentDir = 12;
            }
            else
            {
                if (lastDirection == 1) currentDir = 13;
                else if (lastDirection == 2) currentDir = 14;
                else if (lastDirection == 3) currentDir = 15;
                else if (lastDirection == 4) currentDir = 16;
            }
        }

        if (anim != null)
        {
            anim.SetInteger("Direction", currentDir);
        }
    }

    // ② 実際の移動計算、範囲制限、位置反映を物理のタイミングで行う
    void FixedUpdate()
    {
        if (rb == null) return;

        // 斜め移動で移動速度が上がらないように正規化(normalized)して速度を計算
        Vector2 movement = new Vector2(inputH, inputV).normalized;
        rb.linearVelocity = movement * PlayerMoveSpeed;

        // ★元の仕様通り、一度PlayerPositionに現在の物理座標を入れてから計算
        PlayerPosition = rb.position;

        // 元々あった範囲制限の関数をここで呼び出す
        RangeOfMotion();

        // 制限し終わった座標を物理に返す
        rb.position = PlayerPosition;
    }

    // ③ 【残した部分】範囲制限関数
    void RangeOfMotion() // プレイヤーの移動可能範囲制御
    {
        if (PlayerPosition.x < PlayerRotation.x) PlayerPosition.x = PlayerRotation.x;
        if (PlayerPosition.x > PlayerRotation.y) PlayerPosition.x = PlayerRotation.y;
        if (PlayerPosition.y < PlayerRotation.z) PlayerPosition.y = PlayerRotation.z;
        if (PlayerPosition.y > PlayerRotation.w) PlayerPosition.y = PlayerRotation.w;
    }
}