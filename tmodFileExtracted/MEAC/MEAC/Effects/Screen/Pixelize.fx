sampler uImage0 : register(s0);
float2 uResolution;
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{

    float2 pixelize=uResolution.xy/2;
    coords = round(coords*pixelize)/pixelize;
    float4 color = tex2D(uImage0, coords);
    
    return color;
}

technique Technique1
{
    pass Screen
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}