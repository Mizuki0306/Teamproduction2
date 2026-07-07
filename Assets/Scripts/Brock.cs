using UnityEngine;

public class Brock : MonoBehaviour
{
    private Player playerScript;
    public enum BrockType { Push, Pull, PushandPull }
    public BrockType currentType;
    public int brockNum;
    [SerializeField] private GameObject collider_L;
    [SerializeField] private GameObject collider_R;
    [SerializeField] private GameObject collider_T;
    [SerializeField] private GameObject collider_B;
    [SerializeField] private GameObject collider_Brock;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ゲームが始まった瞬間に、自動でシーン内から Player スクリプトを探してきます
        playerScript = FindFirstObjectByType<Player>();

        if (playerScript != null)
        {
            playerScript.HoldBrock = false;
        }

        collider_L = transform.Find("Collider_L").gameObject;
        collider_R = transform.Find("Collider_R").gameObject;
        collider_T = transform.Find("Collider_T").gameObject;
        collider_B = transform.Find("Collider_B").gameObject;
        collider_Brock = transform.Find("Collider_Brock").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーが正常に取得できていない場合は処理をしない（エラー防止）
        if (playerScript == null) return;

        // 【注意】現在の条件式だと、このBrockスクリプトがアタッチされている
        // オブジェクト自身の名前が「HandCollider」かつ「Collider_L」でないと実行されません。
        // ※当たり判定（OnTriggerEnterなど）で名前を判定する場合は、書き換えが必要です。

        if (gameObject.name == "HandCollider" && gameObject.name == "Collider_L") // left
        {
            if (Input.GetAxisRaw("RT") > 0)
            {
                playerScript.HoldBrock = !playerScript.HoldBrock;
            }
            else
            {
                playerScript.HoldBrock = !playerScript.HoldBrock;
            }
        }

        if (gameObject.name == "HandCollider" && gameObject.name == "Collider_R") // right
        {
            if (Input.GetAxisRaw("RT") > 0)
            {
                playerScript.HoldBrock = !playerScript.HoldBrock;
            }
            else
            {
                playerScript.HoldBrock = !playerScript.HoldBrock;
            }
        }

        if (gameObject.name == "HandCollider" && gameObject.name == "Collider_T") // top
        {
            if (Input.GetAxisRaw("RT") > 0)
            {
                playerScript.HoldBrock = !playerScript.HoldBrock;
            }
            else
            {
                playerScript.HoldBrock = !playerScript.HoldBrock;
            }
        }

        if (gameObject.name == "HandCollider" && gameObject.name == "Collider_B") // bottom
        {
            if (Input.GetAxisRaw("RT") > 0)
            {
                playerScript.HoldBrock = !playerScript.HoldBrock;
            }
            else
            {
                playerScript.HoldBrock = !playerScript.HoldBrock;
            }
        }
    }
}