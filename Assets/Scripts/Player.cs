using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;



public class Player : MonoBehaviour
{
    [Header("--- プレイヤーの動く速度 ---")]
    public float PlayerMoveSpeed=2; //プレイヤーの動く速度
    [Header("--- プレイヤーとブロックの距離 ---")]
    public float PlayerToBrock=2; //プレイヤーとブロックの距離
    [Header("--- プレイヤーがブロックを掴んでいるか否かの判定 ---")]
    public bool HoldBrock = false; //プレイヤーがブロックを掴んでいるか否かの判定
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPosition = transform.position;
        MoveCount = 0;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPosition = transform.position;

        // 最初は「動いていない状態（0：Idle）」として数字を設定しておく
        int currentDir = 0;

        // Aキー（左移動）
        if (Input.GetKey(KeyCode.A) && HoldBrock == false)
        {
            Debug.Log("Aキーが押されています");
            PlayerPosition.x -= PlayerMoveSpeed * Time.deltaTime;
            currentDir = 1; // 左向きのアニメーション番号「1」
        }
        // Dキー（右移動）
        else if (Input.GetKey(KeyCode.D) && HoldBrock == false)
        {
            Debug.Log("Dキーが押されています");
            PlayerPosition.x += PlayerMoveSpeed * Time.deltaTime;
            currentDir = 2; // 右向きのアニメーション番号「2」
        }
        // Wキー（上移動）
        else if (Input.GetKey(KeyCode.W) && HoldBrock == false)
        {
            PlayerPosition.y += PlayerMoveSpeed * Time.deltaTime;
            currentDir = 3; // 上向きのアニメーション番号「3」（後でRunUpを繋ぐ用）
        }
        // Sキー（下移動）
        else if (Input.GetKey(KeyCode.S) && HoldBrock == false)
        {
            PlayerPosition.y -= PlayerMoveSpeed * Time.deltaTime;
            currentDir = 4; // 下向きのアニメーション番号「4」（後でRunDownを繋ぐ用）
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
