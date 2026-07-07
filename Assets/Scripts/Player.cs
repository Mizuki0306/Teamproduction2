using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;



public class Player : MonoBehaviour
{
    private Brock brockScript;
    [Header("--- プレイヤーの動く速度 ---")]
    public float PlayerMoveSpeed=2; //プレイヤーの動く速度
    [Header("--- プレイヤーとブロックの距離 ---")]
    public float PlayerToBrock=2; //プレイヤーとブロックの距離
    [Header("--- ゲーム時間 ---")]
    public static float GameTimer;
    [Header("--- プレイヤーの位置 ---")]
    private Vector2 PlayerPosition;
    [Header("--- プレイヤーの移動可能範囲(x=left,y=right,z=bottom,w=top) ---")]
    public Vector4 PlayerRotation;
    [Header("--- 手数カウント ---")]
    public int MoveCount;
    //アニメーション
    private Animator anim;
    private bool _holdBrock;
    // インスペクターからセットできるように、GameObject型の変数を作る
    [SerializeField] private GameObject handCollider;
    [SerializeField] private GameObject playerCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {// ゲームが始まった瞬間に、自動でシーン内から Player スクリプトを探してきます
        brockScript = FindFirstObjectByType<Brock>();
        PlayerPosition = transform.position;
        MoveCount = 0;
        anim = GetComponent<Animator>();
        handCollider = transform.Find("HandCollider").gameObject;
        playerCollider = transform.Find("PlayerCollider").gameObject;
    }


    // インスペクターや他のスクリプトから触る用の「窓口」
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
    } // ← ちゃんと閉じているか確認！

    // Update is called once per frame
    void Update()
    {
        PlayerPosition = transform.position;
    // 入力の取得
    float h =Input.GetAxisRaw("Horizontal");
    float v = Input.GetAxisRaw("Vertical");

        // 最初は「動いていない状態（0：Idle）」として数字を設定しておく
        int currentDir = 0;
        if (HoldBrock == false)
        {
            // --- 左右の移動とアニメーション判定 ---
            if (h < -0.5f) // 左に入力されているとき
            {
                PlayerPosition.x -= PlayerMoveSpeed * Time.deltaTime;
                currentDir = 1; // 左
            }
            else if (h > 0.5f) // 右に入力されているとき
            {
                PlayerPosition.x += PlayerMoveSpeed * Time.deltaTime;
                currentDir = 2; // 右
            }

            // --- 上下の移動とアニメーション判定 ---
            if (v > 0.5f) // 上に入力されているとき
            {
                PlayerPosition.y += PlayerMoveSpeed * Time.deltaTime;
                currentDir = 3; // 上
            }
            else if (v < -0.5f) // 下に入力されているとき
            {
                PlayerPosition.y -= PlayerMoveSpeed * Time.deltaTime;
                currentDir = 4; // 下
            }
        }
        
        if (HoldBrock == true||brockScript.currentType == Brock.BrockType.Push)
        {
            // --- 左右の移動とアニメーション判定 ---
            if (h < -0.5f&&gameObject.name== "Collider_R") // 左に入力されているかつColliderRのとき
            {
                PlayerPosition.x -= PlayerMoveSpeed * Time.deltaTime;
                currentDir = 5; // 左
            }
            else 
            if (h > 0.5f&&gameObject.name== "Collider_L") // 右に入力されているとき
            {
                PlayerPosition.x += PlayerMoveSpeed * Time.deltaTime;
                currentDir = 6; // 右
            }

            // --- 上下の移動とアニメーション判定 ---
            if (v > 0.5f&&gameObject.name== "Collider_T") // 上に入力されているとき
            {
                PlayerPosition.y += PlayerMoveSpeed * Time.deltaTime;
                currentDir = 7; // 上
            }
            if (v < -0.5f&&gameObject.name== "Collider_B") // 下に入力されているとき
            {
                PlayerPosition.y -= PlayerMoveSpeed * Time.deltaTime;
                currentDir = 8; // 下
            }
        }
        if (h == 0 && v == 0)
        {
            currentDir = 0;
        }

        // ★決まった向きの番号（0〜4）を、Animatorの「Direction」に送る
        // これにより、キーを離した時は自動的に「0」が送られてIdleに戻ります（GetKeyUpの個別処理は不要になります）
        anim.SetInteger("Direction", currentDir);

        // 移動範囲の制限処理
        RangeOfMotion();

        // 位置の反映
        transform.position = PlayerPosition;
    }

    void RangeOfMotion() // プレイヤーの移動可能範囲制御
    {
        if (PlayerPosition.x < PlayerRotation.x) // left (これより左にいけない)
        {
            PlayerPosition.x = PlayerRotation.x;
        }
        if (PlayerPosition.x > PlayerRotation.y) // right (これより右にいけない)
        {
            PlayerPosition.x = PlayerRotation.y;
        }

        // --- ここから下が修正箇所です ---
        if (PlayerPosition.y < PlayerRotation.z) // bottom (これより下(z)にいけない)
        {
            PlayerPosition.y = PlayerRotation.z; // zを代入
        }
        if (PlayerPosition.y > PlayerRotation.w) // top (これより上(w)にいけない)
        {
            PlayerPosition.y = PlayerRotation.w; // wを代入
        }
    }
}
