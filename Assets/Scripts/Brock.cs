using UnityEngine;

public class Brock : MonoBehaviour
{

    private Player playerScript;
    public enum BrockType { Push, Pull, PushandPull }
    public BrockType currentType;
    public int brockNum;

    public BoxCollider2D BrockCollider2D;
    public BoxCollider2D GuideCollider2D;

    private void Awake()
    {
    }
    void Start()
    {

        playerScript = FindFirstObjectByType<Player>();
        if (playerScript != null)
        {
            playerScript.HoldBrock = false;
        }
    }
    public BoxCollider2D GetBlockCol() { return BrockCollider2D; }

}