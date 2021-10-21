namespace UnityEngine.Rendering.Universal
{
    internal class FastBloomUrpPass : ScriptableRenderPass
    {
        public Material material;

        private RenderTargetIdentifier source;
        private RenderTargetIdentifier bloomTemp = new RenderTargetIdentifier(blurTempString);
        private RenderTargetIdentifier bloomTemp1 = new RenderTargetIdentifier(blurTemp1String);
        private RenderTargetIdentifier bloomTex = new RenderTargetIdentifier(blurTexString);
        private RenderTargetIdentifier tempCopy = new RenderTargetIdentifier(tempCopyString);

        private readonly string tag;
        private readonly Color color;
        private readonly float diffuse;
        private readonly float amount;
        private readonly float threshold;
        private readonly float softness;

        static readonly int blColorString = Shader.PropertyToID("_BloomColor");
        static readonly int blDiffuseString = Shader.PropertyToID("_BloomDiffuse");
        static readonly int blDataString = Shader.PropertyToID("_BloomData");

        static readonly int blurTempString = Shader.PropertyToID("_BlurTemp");
        static readonly int blurTemp1String = Shader.PropertyToID("_BlurTemp2");
        static readonly int blurTexString = Shader.PropertyToID("_BlurTex");
        static readonly int tempCopyString = Shader.PropertyToID("_TempCopy");

        private int numberOfPasses = 3;
        private float knee;

        public FastBloomUrpPass(RenderPassEvent renderPassEvent, Material material, Color color,
            float diffuse, float amount, float threshold, float softness, string tag)
        {
            this.renderPassEvent = renderPassEvent;
            this.tag = tag;
            this.material = material;
            this.color = color;
            this.diffuse = diffuse;
            this.amount = amount;
            this.threshold = threshold;
            this.softness = softness;
        }

        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;

            CommandBuffer cmd = CommandBufferPool.Get(tag);
            cmd.GetTemporaryRT(tempCopyString, opaqueDesc, FilterMode.Bilinear);
            if (SystemInfo.copyTextureSupport == CopyTextureSupport.None)
                cmd.Blit(source, tempCopy);
            else
                cmd.CopyTexture(source, tempCopy);

            material.SetFloat(blDiffuseString, diffuse);
            material.SetColor(blColorString, amount * color);
            numberOfPasses = Mathf.Max(Mathf.CeilToInt(diffuse * 4), 1);
            material.SetFloat(blDiffuseString, numberOfPasses > 1 ? (diffuse * 4 - Mathf.FloorToInt(diffuse * 4 - 0.001f)) * 0.5f + 0.5f : diffuse * 4);
            knee = threshold * softness;
            material.SetVector(blDataString, new Vector4(threshold, threshold - knee, 2f * knee, 1f / (4f * knee + 0.00001f)));

            if (numberOfPasses == 1)
            {
                cmd.GetTemporaryRT(blurTexString, Screen.width / 2, Screen.height / 2, 0, FilterMode.Bilinear);
                cmd.Blit(tempCopy, bloomTex, material, 0);
            }
            else if (numberOfPasses == 2)
            {
                cmd.GetTemporaryRT(blurTexString, Screen.width / 2, Screen.height / 2, 0, FilterMode.Bilinear);
                cmd.GetTemporaryRT(blurTempString, Screen.width / 4, Screen.height / 4, 0, FilterMode.Bilinear);
                cmd.Blit(tempCopy, bloomTemp, material, 0);
                cmd.Blit(bloomTemp, bloomTex, material, 0);
            }
            else if (numberOfPasses == 3)
            {
                cmd.GetTemporaryRT(blurTexString, Screen.width / 4, Screen.height / 4, 0, FilterMode.Bilinear);
                cmd.GetTemporaryRT(blurTempString, Screen.width / 8, Screen.height / 8, 0, FilterMode.Bilinear);
                cmd.Blit(tempCopy, bloomTex, material, 0);
                cmd.Blit(bloomTex, bloomTemp, material, 0);
                cmd.Blit(bloomTemp, bloomTex, material, 0);
            }
            else if (numberOfPasses == 4)
            {
                cmd.GetTemporaryRT(blurTexString, Screen.width / 4, Screen.height / 4, 0, FilterMode.Bilinear);
                cmd.GetTemporaryRT(blurTempString, Screen.width / 8, Screen.height / 8, 0, FilterMode.Bilinear);
                cmd.GetTemporaryRT(blurTemp1String, Screen.width / 16, Screen.height / 16, 0, FilterMode.Bilinear);
                cmd.Blit(tempCopy, bloomTex, material, 0);
                cmd.Blit(bloomTex, bloomTemp, material, 0);
                cmd.Blit(bloomTemp, bloomTemp1, material, 0);
                cmd.Blit(bloomTemp1, bloomTemp, material, 0);
                cmd.Blit(bloomTemp, bloomTex, material, 0);
            }

            cmd.SetGlobalTexture(blurTexString, bloomTex);
            cmd.Blit(tempCopy, source, material, 1);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempCopyString);
            cmd.ReleaseTemporaryRT(blurTempString);
            cmd.ReleaseTemporaryRT(blurTemp1String);
            cmd.ReleaseTemporaryRT(blurTexString);
        }
    }
}
