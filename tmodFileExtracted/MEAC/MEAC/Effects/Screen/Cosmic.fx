sampler uImage0 : register(s0);
texture2D tex0;
sampler2D uImage1 = sampler_state
{
    Texture = <tex0>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
float t;
float m;
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float2 coord = coords * 0.9f;
    if (max(color.r, max(color.g,color.b))>m)
    {
        return tex2D(uImage1, float2(coord.x, coord.y) + float2(t,t));
    }
    return float4(0,0,0,0);
}

technique Technique1
{
    pass Screen
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}