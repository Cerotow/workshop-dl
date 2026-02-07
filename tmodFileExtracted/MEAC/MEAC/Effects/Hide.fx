sampler uImage0 : register(s0);

float2 pos;
float r;
float2 res;
float4 PSFunction(float2 coords : TEXCOORD0) : COLOR0//圆形清除
{
    float4 color = tex2D(uImage0,coords);
    float dis=length((coords-pos)*float2(res.x/res.y,1));
    if (dis<r)
        return float4(0,0,0,0);
    else
    {
        return color;
    }
}
technique Technique1
{
    pass Hide
    {
        PixelShader = compile ps_2_0 PSFunction();
    }
}