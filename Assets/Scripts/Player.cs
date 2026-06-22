using Unity.VisualScripting;
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
    public float GameTimer;
    [Header("--- プレイヤーの位置 ---")]
    private Vector2 PlayerPosition;
    [Header("--- プレイヤーの移動可能範囲(x=left,y=right,z=bottom,w=top) ---")]
    public Vector4 PlayerRotation;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Move()
    {
        if (PlayerPosition.x<0)
        {
            PlayerPosition.x = 0;
        }
        if (PlayerPosition.x < 0)
        {
            PlayerPosition.x = 0;
        }
        if (PlayerPosition.x < 0)
        {
            PlayerPosition.x = 0;
        }
        if (PlayerPosition.x < 0)
        {
            PlayerPosition.x = 0;
        }
    }
}
