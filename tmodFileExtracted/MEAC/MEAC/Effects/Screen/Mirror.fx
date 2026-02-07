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

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    if (any(color))
    {
        return tex2D(uImage1, float2(coords.x,1-coords.y));
    }
        return color;
}

technique Technique1
{
    pass Screen
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}