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
    [SerializeField] private GameObject collider_InBrockGuide;

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
        collider_InBrockGuide=transform.Find("Collider_InBrockGuide").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    // ★プレイヤーの手（HandCollider）が、ブロックの子コライダーに触れている間の処理
    private void OnTriggerStay2D(Collider2D other)
    {
        // プレイヤーがいない、またはRTボタンが押されていないなら何もしない
        if (playerScript == null) return;

        // RTボタン（あるいは指定のボタン）が押された瞬間を検知
        // ※GetAxisRaw("RT") > 0 だと、押している間中ずっとON/OFFが超高速で切り替わってしまうため、
        // 1回カチッと押した瞬間だけ判定できるように、可能なら GetButtonDown などを推奨します。
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetAxisRaw("RT") > 0.5f)
        {
            // 触れてきた相手（other）の名前が「HandCollider」のときだけ処理する
            if (other.name == "HandCollider")
            {
                // 現在の掴み状態を反転させる（掴んでいたら離す、離していたら掴む）
                playerScript.HoldBrock = !playerScript.HoldBrock;

                // 連続で一瞬に何回も切り替わるのを防ぐ簡易ガード（必要に応じて）
                Debug.Log("掴み状態が切り替わりました: " + playerScript.HoldBrock);
            }
        }
    }
}