using UnityEngine;

public class PullBrock : MonoBehaviour
{
    public Player playerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript.HoldBrock = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
