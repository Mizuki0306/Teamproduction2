using UnityEngine;

public class Brock : MonoBehaviour
{
    public static Brock Instance { get; private set; }
    private Player playerScript;

    [Header("--- ‰Ÿ‚µˆø‚«İ’è ---")]
    public bool canPush = true;
    public bool canPull = true;

    [Header("--- ƒuƒƒbƒN¯•Ê”Ô† ---")]
    public int brockNum;

    public BoxCollider2D BrockCollider2D;

    public bool IsHeld { get; set; } = false;
    public bool IsPlaced { get; set; } = false;

    void Start()
    {
        playerScript = FindFirstObjectByType<Player>();
        if (playerScript != null)
        {
            playerScript.HoldBrock = false;
        }
    }
}