sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float2 uScreenResolution;



float4 Blend(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0,coords);
    float4 color2 = tex2D(uImage1, coords);
    color.rgb = pow(color.rgb, 2.2);
    return float4(pow(color.rgb, 1/2.2), 1);
}
technique Technique1
{
	
    pass Blend
    {
        PixelShader = compile ps_2_0 Blend();
    }
}