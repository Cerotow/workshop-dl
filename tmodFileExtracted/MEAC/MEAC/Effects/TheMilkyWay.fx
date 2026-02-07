sampler uImage0 : register(s0);//Perlin
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);//DarkArea
float darkScale;
float4x4 uTransform;
struct VSInput
{
    float2 Pos : POSITION0;
    float4 Color : COLOR0;
    float3 Texcoord : TEXCOORD0;
};

struct PSInput
{
    float4 Pos : SV_POSITION;
    float4 Color : COLOR0;
    float3 Texcoord : TEXCOORD0;
};
PSInput VertexShaderFunction(VSInput input)
{
    PSInput output;
    output.Texcoord = input.Texcoord;
    output.Pos = mul(float4(input.Pos, 0, 1), uTransform);
    output.Color = input.Color;
    return output;
}


float4 PixelShaderFunction(PSInput input) : COLOR0
{
    float2 coords = input.Texcoord.xy;
    float4 c1 = tex2D(uImage0, coords) * tex2D(uImage1,coords);
    float4 c2 = tex2D(uImage0, coords) * tex2D(uImage2, float2(coords.x, coords.y / darkScale));
    float4 mix = float4(c1.rgb - c2.rgb, 1);
    return mix * input.Color;
}
technique Technique1 
{
	pass MilkyWay
	{
        VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}
