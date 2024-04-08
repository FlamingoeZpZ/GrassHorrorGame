using UnityEngine;

public class Buckets : MonoBehaviour
{
 
    
    [field: SerializeField] public float WateringCost { get; private set; }
    [field: SerializeField] public float WaterCapacity { get; private set; }
    [field: SerializeField] public float WateringRange { get; private set; }

    [SerializeField] private int tickTime = 20;

    private Animator _animator;
    private float _currentWater;
    
    private static readonly int IsWatering = Animator.StringToHash("IsWatering");
    private static readonly int Capacity = Animator.StringToHash("WaterCapacity");

    public float WateringCapacity => _currentWater / WaterCapacity;
    public bool IsWateringCurrently { get; private set; }
    public int WateringDelay => tickTime;

    private void Awake()
    {
        _animator= GetComponent<Animator>();
        FillBucket();
    }

    public void FillBucket()
    {
        _currentWater = WaterCapacity;
        _animator.SetFloat(Capacity, WateringCapacity);
    }

    public void StartWatering()
    {
        if (WateringCapacity <= 0) return;
        _animator.SetBool(IsWatering, true);
        IsWateringCurrently = true;
    }

    public void Tick()
    {
        _currentWater -= WateringCost;
        _animator.SetFloat(Capacity, WateringCapacity);
        
        if(WateringCapacity <= 0) StopWatering();
        
    }

    public void StopWatering()
    {
        _animator.SetBool(IsWatering, false);
        IsWateringCurrently = false;
    }

}
