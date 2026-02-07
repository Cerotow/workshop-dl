sampler uImage0 : register(s0);//Perlin
sampler uImage1 : register(s1);//PointLight

float4 PixelShaderFunction(float4 drawColor : COLOR0, float2 coord : TEXCOORD0) : COLOR0
{
    float2 coords = coord.xy;
    float c = tex2D(uImage0, coords).r * tex2D(uImage1,coords).r;
    
    float4 colorize = float4(lerp(c, 1, drawColor.r), lerp(c, 1, drawColor.g), lerp(c, 1, drawColor.b),drawColor.a);
    return c * colorize;
}
float4 PixelShaderFunction2(float4 drawColor : COLOR0, float2 coord : TEXCOORD0) : COLOR0
{
    float2 coords = coord.xy;
    float c = tex2D(uImage0, coords).r * tex2D(uImage1, coords).r;
    return c.xxxx * drawColor;
}
technique Technique1 
{
	pass MilkyWay
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
    pass MilkyWayDark
    {
        PixelShader = compile ps_3_0 PixelShaderFunction2();
    }
}
