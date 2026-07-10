using UnityEngine;

public class BrockGuide : MonoBehaviour
{
    private Brock brockScript;
    public enum GuideType { Push, Pull, PushandPull }
    public GuideType guideType;
    public int brockNum;
    [SerializeField] private GameObject guide_Collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        brockScript = FindFirstObjectByType<Brock>();
        guide_Collider = transform.Find("GuideCollider").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
