using UnityEngine;

[ExecuteInEditMode]
public class FastBloom : MonoBehaviour{
    public Color Color = Color.white;
    [Range(0, 1)]
    public float Diffuse = 2f;
    [Range(0, 5)]
    public float Amount = 1f;
	[Range(0, 1)]
	public float Threshold = 0.2f;
    [Range(0, 1)]
    public float Softness = 0f;

    static readonly int blColorString = Shader.PropertyToID("_BloomColor");
    static readonly int blDiffuseString = Shader.PropertyToID("_BloomDiffuse");
    static readonly int blDataString = Shader.PropertyToID("_BloomData");
    static readonly int bloomTexString = Shader.PropertyToID("_BloomTex");

    public Material material=null;
    private int numberOfPasses = 3;
    private float knee;

	void  OnRenderImage (RenderTexture source ,   RenderTexture destination){

        material.SetFloat(blDiffuseString, Diffuse);
        material.SetColor(blColorString, Amount * Color);
        numberOfPasses = Mathf.Max(Mathf.CeilToInt(Diffuse * 4), 1);
        material.SetFloat(blDiffuseString, numberOfPasses > 1 ? (Diffuse * 4 - Mathf.FloorToInt(Diffuse * 4 - 0.001f)) * 0.5f + 0.5f : Diffuse * 4);
        knee = Threshold * Softness;
        material.SetVector(blDataString, new Vector4(Threshold, Threshold - knee, 2f * knee, 1f / (4f * knee + 0.00001f)));
        RenderTexture bloomTex = null;

        if (numberOfPasses == 1)
        {
            bloomTex = RenderTexture.GetTemporary(Screen.width / 2, Screen.height / 2, 0, source.format);
            Graphics.Blit(source, bloomTex, material, 0);
        }
        else if (numberOfPasses == 2)
        {
            bloomTex = RenderTexture.GetTemporary(Screen.width / 2, Screen.height / 2, 0, source.format);
            var temp1 = RenderTexture.GetTemporary(Screen.width / 4, Screen.height / 4, 0, source.format);
            Graphics.Blit(source, temp1, material, 0);
            Graphics.Blit(temp1, bloomTex, material, 0);
            RenderTexture.ReleaseTemporary(temp1);
        }
        else if (numberOfPasses == 3)
        {
            bloomTex = RenderTexture.GetTemporary(Screen.width / 4, Screen.height / 4, 0, source.format);
            var temp1 = RenderTexture.GetTemporary(Screen.width / 8, Screen.height / 8, 0, source.format);
            Graphics.Blit(source, bloomTex, material, 0);
            Graphics.Blit(bloomTex, temp1, material, 0);
            Graphics.Blit(temp1, bloomTex, material, 0);
            RenderTexture.ReleaseTemporary(temp1);
        }
        else if (numberOfPasses == 4)
        {
            bloomTex = RenderTexture.GetTemporary(Screen.width / 4, Screen.height / 4, 0, source.format);
            var temp1 = RenderTexture.GetTemporary(Screen.width / 8, Screen.height / 8, 0, source.format);
            var temp2 = RenderTexture.GetTemporary(Screen.width / 16, Screen.height / 16, 0, source.format);
            Graphics.Blit(source, bloomTex, material, 0);
            Graphics.Blit(bloomTex, temp1, material, 0);
            Graphics.Blit(temp1, temp2, material, 0);
            Graphics.Blit(temp2, temp1, material, 0);
            Graphics.Blit(temp1, bloomTex, material, 0);
            RenderTexture.ReleaseTemporary(temp1);
            RenderTexture.ReleaseTemporary(temp2);
        }

        material.SetTexture(bloomTexString, bloomTex);
        RenderTexture.ReleaseTemporary(bloomTex);

        Graphics.Blit(source, destination, material, 1);
    }
}
