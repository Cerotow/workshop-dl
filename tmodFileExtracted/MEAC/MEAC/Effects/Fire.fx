sampler uImage0 : register(s0);
texture2D tex0; 
sampler2D uImage1 = sampler_state 
{
    Texture = <tex0>; 
    MinFilter = Linear; 
    MagFilter = Linear;
    AddressU = Clamp; 
    AddressV = Clamp;
};

float uTime1;
float uTime2;
float uH;
float uTi;
float4 PSFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, float2(coords.x,coords.y+uTime2));
    float a = abs(sin(color.r * uTi + uTime1));
    a -= (1-coords.y) * (1-uH);
    a = clamp(a,0,1);
    float4 color2 = tex2D(uImage1, float2(a,0.5f));
    return color2;

}
float4 PSFunction2(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, float2(coords.x, coords.y + uTime2));
    float a = abs(sin(color.r * uTi + uTime1));
    a -= (1 - coords.y) * (1 - uH);
    return float4(a,a,a,1);

}
technique Technique1
{
    pass Fire
    {
        PixelShader = compile ps_2_0 PSFunction();
    }
    pass Fire1
    {
        PixelShader = compile ps_2_0 PSFunction2();
    }
}