sampler uImage0 : register(s0);

float4 PixelShaderFunction(float4 drawColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float a = color.r * 0.3 + color.g * 0.6 + color.b * 0.1;
    
    return float4(a,a,a,1) * drawColor;
}

technique Technique1
{
    pass Screen
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}