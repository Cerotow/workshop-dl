sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float2 pos;
float minDis;
float i;
float2 tOffset;
float maxDis;
float4 PixelShaderFunction(float4 defColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float warpColor = tex2D(uImage1, coords + tOffset).r;
    float2 offset = coords - pos;
    float dis = length(offset);
    float r = 0;
    warpColor -= 0.5f;
    if (dis>minDis)
    {
        if (dis<maxDis)
        {
            r = (dis - minDis) * i * warpColor;
        }
        else
        {
            r = (maxDis - minDis) * i * warpColor;
        }
    }
    offset = mul(offset, float2x2(cos(r), -sin(r), sin(r), cos(r)));
    return tex2D(uImage0, pos + offset) * defColor;
}

technique Technique1
{
    pass Screen
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}