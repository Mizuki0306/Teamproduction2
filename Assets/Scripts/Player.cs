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
    // インスペクターからセットできるように、GameObject型の変数を作る
    [SerializeField] private GameObject handCollider;
    [SerializeField] private GameObject playerCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ゲームが始まった瞬間に、自動でシーン内から Player スクリプトを探してきます
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
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPosition = transform.position;

        // 入力の取得
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 最初は「動いていない状態（0：Idle）」にしておく
         currentDir = 0;

        // キーが押されている場合のみ移動処理を行う
        if (h != 0 || v != 0)
        {
            // --- 左右の移動 ---
            if (h < -0.5f) // 左
            {
                PlayerPosition.x -= PlayerMoveSpeed * Time.deltaTime;
                currentDir = HoldBrock ? 6: 1; // 掴み中なら6、通常なら1
                lastDirection = 1;
            }
            if (h > 0.5f) // 右
            {
                PlayerPosition.x += PlayerMoveSpeed * Time.deltaTime;
                currentDir = HoldBrock ? 5 : 2; // 掴み中なら5、通常なら2
                lastDirection = 2;
            }

            // --- 上下の移動 ---
            if (v > 0.5f) // 上
            {
                PlayerPosition.y += PlayerMoveSpeed * Time.deltaTime;
                currentDir = HoldBrock ? 7 : 3; // 掴み中なら7、通常なら3
                lastDirection = 3;
            }
            if (v < -0.5f) // 下
            {
                PlayerPosition.y -= PlayerMoveSpeed * Time.deltaTime;
                currentDir = HoldBrock ? 8 : 4; // 掴み中なら8、通常なら4
                lastDirection = 4;
            }
        }
        if (h == 0 && v == 0)
        {
            if (HoldBrock)
            {
                if (lastDirection == 1) currentDir = 10; // 左を向いていたなら IdleLeft (13)
                else if (lastDirection == 2) currentDir = 9; // 右を向いていたなら IdleRight (14)
                else if (lastDirection == 3) currentDir = 11; // 上を向いていたなら IdleTop (15)
                else if (lastDirection == 4) currentDir = 12; // 下を向いていたなら IdleBottom (16)
            }
            else
            {
                // 通常時の待機（直前に向いていた方向によって、13〜16番を出し分ける）
                if (lastDirection == 1) currentDir = 13; // 左を向いていたなら IdleLeft (13)
                else if (lastDirection == 2) currentDir = 14; // 右を向いていたなら IdleRight (14)
                else if (lastDirection == 3) currentDir = 15; // 上を向いていたなら IdleTop (15)
                else if (lastDirection == 4) currentDir = 16; // 下を向いていたなら IdleBottom (16)
            }
        }
        // Animatorの「Direction」に番号を送る
        if (anim != null)
        {
            anim.SetInteger("Direction", currentDir);
        }

        // 移動範囲の制限処理
        RangeOfMotion();

        // 位置の反映
        transform.position = PlayerPosition;
    }

    void RangeOfMotion() // プレイヤーの移動可能範囲制御
    {
        if (PlayerPosition.x < PlayerRotation.x) // left
        {
            PlayerPosition.x = PlayerRotation.x;
        }
        if (PlayerPosition.x > PlayerRotation.y) // right
        {
            PlayerPosition.x = PlayerRotation.y;
        }
        if (PlayerPosition.y < PlayerRotation.z) // bottom
        {
            PlayerPosition.y = PlayerRotation.z;
        }
        if (PlayerPosition.y > PlayerRotation.w) // top
        {
            PlayerPosition.y = PlayerRotation.w;
        }
    }
}