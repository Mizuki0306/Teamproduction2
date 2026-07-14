using UnityEngine;

public class Brock : MonoBehaviour
{
    public static Brock Instance { get; private set; }
    private Player playerScript;

    [Header("--- ‰Ÿ‚µˆø‚«İ’è ---")]
    public bool canPush = true;  // ‰Ÿ‚¹‚é‚©
    public bool canPull = true;  // ˆø‚¯‚é‚©

    public int brockNum;
    public BoxCollider2D BrockCollider2D;

    void Start()
    {
        playerScript = FindFirstObjectByType<Player>();
        if (playerScript != null)
        {
            playerScript.HoldBrock = false;
        }
    }
}