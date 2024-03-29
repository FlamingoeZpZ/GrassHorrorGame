using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public struct CircleParams
{
    public Vector2 position;
    public float radius;
}

public class GPT_ImplGrassPainter : MonoBehaviour
{
    public ComputeShader computeShader;
    public Material material;
    private RenderTexture _renderTexture;
    public float increaseSpeed = 0.01f; // Control the speed of increasing whiteness
    public float decreaseSpeed = 0.01f; // Control the speed of decreasing whiteness
    public float size = 5f; // Control the speed of decreasing whiteness
    public int grassWaterTickTimer;
    public int grassDecayTickTimer = 20;

    private int _increaseWhitenessKernelHandle;
    private int _decreaseWhitenessKernelHandle;
    private ComputeBuffer _circleBuffer;
    private readonly Vector2Int _imageSize = new Vector2Int(400, 200);
    private readonly Vector2Int _worldSize = new Vector2Int(40, 20);
    private static readonly int ImageSize = Shader.PropertyToID("ImageSize");
    private static readonly int WorldSize = Shader.PropertyToID("WorldSize");
    private static readonly int CircleBuffer = Shader.PropertyToID("CircleBuffer");
    private static readonly int Result = Shader.PropertyToID("Result");
    private static readonly int IncreaseSpeed = Shader.PropertyToID("IncreaseSpeed");
    private static readonly int DecreaseSpeed = Shader.PropertyToID("DecreaseSpeed");
    private bool _running;
    public bool isWatering;

    private void Start()
    {
        _increaseWhitenessKernelHandle = computeShader.FindKernel("IncreaseWhitenessKernel");
        _decreaseWhitenessKernelHandle = computeShader.FindKernel("DecreaseWhitenessKernel");
        _circleBuffer = new ComputeBuffer(1, sizeof(float) * 3);
        _renderTexture = new RenderTexture(_imageSize.x, _imageSize.y, 0, RenderTextureFormat.R8)
        {
            enableRandomWrite = true
        };
        _renderTexture.Create();
        _running = true;
        UpdateTexture(); // Example position (20, 10) and radius 5
        DispatchIncreaseWhitenessKernel();
        DispatchDecreaseWhitenessKernel();
    }

    private void OnDestroy()
    {
        _circleBuffer.Release();
        _running = false;
        _renderTexture.Release();
    }

    private void UpdateTexture()
    {
        computeShader.SetInts(ImageSize, _imageSize.x, _imageSize.y);
        computeShader.SetInts(WorldSize, _worldSize.x, _worldSize.y);
    }

    private async void DispatchIncreaseWhitenessKernel()
    {
        while (_running)
        {
            if (isWatering)
            {
                CircleParams circleParams;
                Vector3 position = transform.position;
                circleParams.position = new Vector2(position.x, position.z);
                circleParams.radius = size;

                _circleBuffer.SetData(new[] { circleParams });
                computeShader.SetBuffer(_increaseWhitenessKernelHandle, CircleBuffer, _circleBuffer);
                computeShader.SetTexture(_increaseWhitenessKernelHandle, Result, _renderTexture);
                computeShader.SetFloat(IncreaseSpeed, increaseSpeed);
                computeShader.Dispatch(_increaseWhitenessKernelHandle, _imageSize.x / 8, _imageSize.y / 8, 1);
                material.mainTexture = _renderTexture;
            }

            await Task.Delay(grassWaterTickTimer);
            
        }
    }

    private async void DispatchDecreaseWhitenessKernel()
    {
        while (_running)
        {

            computeShader.SetTexture(_decreaseWhitenessKernelHandle, Result, _renderTexture);
            computeShader.SetFloat(DecreaseSpeed, decreaseSpeed);
            computeShader.Dispatch(_decreaseWhitenessKernelHandle, _imageSize.x / 8, _imageSize.y / 8, 1);
            material.mainTexture = _renderTexture;
            await Task.Delay(grassDecayTickTimer);
        }
    }
    
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, size);
    }
}
