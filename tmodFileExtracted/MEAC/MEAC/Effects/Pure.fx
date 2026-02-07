sampler uImage0 : register(s0);
float4 uColor;
float4 PSFunction(float4 drawColor : COLOR0,float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0,coords);
    if (any(color))
        return uColor*drawColor.a;
    else
        return float4(0, 0, 0, 0);

}

technique Technique1
{
    pass A
    {
        PixelShader = compile ps_2_0 PSFunction();
    }
}