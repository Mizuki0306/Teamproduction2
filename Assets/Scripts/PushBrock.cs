using UnityEngine;

public class PushBrock : MonoBehaviour
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
