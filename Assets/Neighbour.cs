using UnityEngine;

public class Neighbour : MonoBehaviour
{
    private Animator sr;
    private bool isForward;

    private static readonly int Forward = Animator.StringToHash("Forward");
    [SerializeField, Range(0,1)] private float turnAt;

    // Start is called before the first frame update
    void Awake()
    {
        sr = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var transform1 = transform;
        float dot = Vector3.Dot(transform1.forward, (Player.Position - transform1.position).normalized);
        if (isForward && dot < -turnAt)
        {
            isForward = false;
            sr.SetBool(Forward, false);
        }
        else if(dot > turnAt)
        {
            isForward = true;
            sr.SetBool(Forward, true);
        }
    }
    
}
