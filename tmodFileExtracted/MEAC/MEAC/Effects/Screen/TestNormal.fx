sampler uImage0 : register(s0);//传贴图
texture2D tex0;//传原图
sampler2D uImage1 = sampler_state
{
    Texture = <tex0>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
float uZPosition;
float4 PixelShaderFunction(float4 color : COLOR0,float2 coords : TEXCOORD0) : COLOR0
{
    float4 nColor = tex2D(uImage0, coords);
    float4 f;
    if (!any(nColor))
    {
       f=float4(0, 0, 0, 0);
    }
    else
    {
        float3 n = nColor.rgb;
        n -= float3(0.5,0.5,0.5);
        float k = (- uZPosition / n.b);
        float x = n.r * k + coords.x;
        float y = n.g * k + coords.y;
        //根据点向式计算与z=0平面的交点
        float2 coord = float2(x, y);
        f = tex2D(uImage1, coord);

    }
    return f;
}

technique Technique1
{
    pass Screen
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}