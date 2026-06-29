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



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPosition = transform.position;
        MoveCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPosition = transform.position;
        if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("Dキーが押されています");
            PlayerPosition.x += PlayerMoveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("Aキーが押されています");
            PlayerPosition.x -= PlayerMoveSpeed * Time.deltaTime;
        }
        /*if (Input.GetButton("Horizontal"))
        {
            PlayerPosition.x += Input.GetAxis("Horizontal") * PlayerMoveSpeed * Time.deltaTime;
        }*/
        RangeOfMotion();
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
