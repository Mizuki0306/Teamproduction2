using UnityEngine;

public class Brock : MonoBehaviour
{
    public Player playerScript;
    public enum BrockType {Push,Pull,PushandPull }
    public BrockType currentType;
    [SerializeField] private GameObject collider_L;
    [SerializeField] private GameObject collider_R;
    [SerializeField] private GameObject collider_T;
    [SerializeField] private GameObject collider_B;
    [SerializeField] private GameObject collider_Brock;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript.HoldBrock = false;
        collider_L = transform.Find("Collider_L").gameObject;
        collider_R = transform.Find("Collider_R").gameObject; 
        collider_T = transform.Find("Collider_T").gameObject;
        collider_B = transform.Find("Collider_B").gameObject;
        collider_Brock = transform.Find("Collider_Brock").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //if ()
        //{
           // playerScript.HoldBrock = true;
        //}
}}
