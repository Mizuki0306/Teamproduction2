using UnityEngine;

public class Hand : MonoBehaviour
{
    public Player playerScript;
    private Brock brockInRange;
    private Collider2D playerBodyCollider; // ★追加：プレイヤー本体の当たり判定

    void Start()
    {
        if (playerScript == null)
        {
            Debug.LogError("【エラー】親オブジェクトに Player スクリプトが見つかりません！");
        }
        else
        {
            // ★プレイヤー本体（親オブジェクト）のCollider2Dを取得
            playerBodyCollider = playerScript.GetComponent<BoxCollider2D>();
            if (playerBodyCollider == null)
            {
                Debug.LogWarning("【警告】Playerオブジェクトに Collider2D が見つかりません。押す動作の衝突回避ができません。");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Collider_Brock"))
        {
            brockInRange = collision.GetComponentInParent<Brock>();
            Debug.Log("ブロックが範囲内: " + (brockInRange != null ? brockInRange.name : "見つからず"));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Collider_Brock"))
        {
            Brock exited = collision.GetComponentInParent<Brock>();
            if (exited == brockInRange)
            {
                brockInRange = null;
            }
        }
    }

    void Update()
    {
        if (playerScript == null || brockInRange == null) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!playerScript.HoldBrock)
            {
                Grab();
            }
            else
            {
                Release();
            }
        }
    }

    private void Grab()
    {
        playerScript.HoldBrock = true;
        brockInRange.transform.SetParent(playerScript.transform);

        // ★掴んだ瞬間、プレイヤー本体とブロック実体の衝突判定を無効化
        // これにより「押す」動作でプレイヤーが押し返されなくなる
        SetIgnoreCollision(brockInRange, true);
        Physics2D.IgnoreCollision(playerBodyCollider, brockInRange.GetBlockCol(), true);

        Debug.Log("ブロックを掴みました: " + brockInRange.name);
    }

    private void Release()
    {
        playerScript.HoldBrock = false;

        

        if (brockInRange != null)
        {
            // ★離す瞬間、衝突判定を元に戻す
            SetIgnoreCollision(brockInRange, false);
            Physics2D.IgnoreCollision(playerBodyCollider, brockInRange.GetBlockCol(), false);

            brockInRange.transform.SetParent(null);
        }
        Debug.Log("ブロックを離しました");
    }

    private void SetIgnoreCollision(Brock brock, bool ignore)
    {
        if (playerBodyCollider == null || brock == null) return;

        Collider2D brockCollider = brock.BrockCollider2D;
        if (brockCollider != null)
        {
            Physics2D.IgnoreCollision(playerBodyCollider, brockCollider, ignore);
        }
    }
}