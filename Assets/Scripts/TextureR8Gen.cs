using UnityEngine;

public class TextureR8Gen : MonoBehaviour
{
    public ComputeShader textureUpdater;
    public Transform targetTransform;
    private RenderTexture resultTexture;
    public Material[] materialsToUpdate; // Assign this array in the inspector
    private static readonly int Result = Shader.PropertyToID("Result");
    private static readonly int TransformPosition = Shader.PropertyToID("transformPosition");

    void Start()
    {
        // Create a 256x256 R8 texture
        resultTexture = new RenderTexture(256, 256, 0, RenderTextureFormat.R8)
        {
            enableRandomWrite = true
        };
        resultTexture.Create();

        // Initially assign the texture to all specified materials
        foreach (var mat in materialsToUpdate)
        {
            if (mat != null)
            {
                mat.mainTexture = resultTexture;
            }
        }
    }

    public void Tick()
    {
        // Convert the transform position to texture space
        Vector2 textureSpacePosition = new Vector2(targetTransform.position.x / 50f, targetTransform.position.z / 50f); // Adjust based on your needs

        // Update the texture based on the transform's position
        int kernelHandle = textureUpdater.FindKernel("CSMain");
        textureUpdater.SetTexture(kernelHandle, Result, resultTexture);
        textureUpdater.SetVector(TransformPosition, new Vector4(textureSpacePosition.x, textureSpacePosition.y, 0, 0));
        textureUpdater.Dispatch(kernelHandle, resultTexture.width / 8, resultTexture.height / 8, 1);
        
    }
}
