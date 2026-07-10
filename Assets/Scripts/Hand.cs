using UnityEngine;

public class Hand : MonoBehaviour
{
    private Player playerScript;

    void Start()
    {
        playerScript = GetComponentInParent<Player>();
        if (playerScript == null)
        {
            Debug.LogError("【エラー】親オブジェクトに Player スクリプトが見つかりません！");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Collider_L"))
        {
            if (playerScript != null)
            {
                // ★「押した瞬間(Down)」ではなく「今押されているか」をチェック
                bool isPressingSpace = Input.GetKey(KeyCode.Space);
                bool isPressingRT = Input.GetAxisRaw("RT") > 0.5f;

                if (isPressingSpace || isPressingRT)
                {
                    // 現在の掴み状態（HoldBrock）が「まだ切り替わっていない」時だけ処理する
                    // これにより、既存の変数だけで「ボタンを押した最初の1回」に近い安全な挙動を作れます
                    if (playerScript.HoldBrock == false)
                    {
                        playerScript.HoldBrock = true;
                        Debug.Log("【既存変数のみで成功】掴みました！ 現在: " + playerScript.HoldBrock);
                    }
                }
                else
                {
                    // ボタンが離されたら、自動的に離す（またはボタンを離した時用の処理）
                    // もし「もう一度押したら離す」にしたい場合は、歩いて範囲外に出ることで離す仕様にするか、
                    // 既存の「アニメーション番号(currentDir)」などが待機状態(Idle)の時だけ受け付ける等で対応可能です。
                    if (playerScript.HoldBrock == true)
                    {
                        playerScript.HoldBrock = false;
                        Debug.Log("【既存変数のみで成功】離しました！ 現在: " + playerScript.HoldBrock);
                    }
                }
            }
        }
    }
}