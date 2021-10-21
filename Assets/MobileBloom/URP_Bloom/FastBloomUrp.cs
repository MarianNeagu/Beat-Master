namespace UnityEngine.Rendering.Universal
{
    public class FastBloomUrp : ScriptableRendererFeature
    {
        [System.Serializable]
        public class FastBloomSettings
        {
            public RenderPassEvent Event = RenderPassEvent.AfterRenderingTransparents;
            public Material blitMaterial = null;
            public Color Color = Color.white;
            [Range(0, 1)]
            public float Diffuse = 2f;
            [Range(0, 5)]
            public float Amount = 1f;
            [Range(0, 1)]
            public float Threshold = 0.2f;
            [Range(0, 1)]
            public float Softness = 0f;
        }

        public FastBloomSettings settings = new FastBloomSettings();

        FastBloomUrpPass fastBloomPass;

        public override void Create()
        {
            fastBloomPass = new FastBloomUrpPass(settings.Event, settings.blitMaterial, settings.Color, settings.Diffuse, settings.Amount, settings.Threshold, settings.Softness, this.name);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            fastBloomPass.Setup(renderer.cameraColorTarget);
            renderer.EnqueuePass(fastBloomPass);
        }
    }
}

