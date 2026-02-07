sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
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
	float2 uv = input.Texcoord.xy;
	float4 c = tex2D(uImage0, uv);
	c += tex2D(uImage1,uv) * c;
	return c * input.Color;
}



technique Technique1 {
	pass Trail {
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
