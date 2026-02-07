sampler uShapeTex : register(s0);
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
	float3 coord = input.Texcoord;
	float4 c = tex2D(uShapeTex, float2(coord.x , coord.y));//Ö÷ÎÆÀí
    float4 finalColor = float4(input.Color.r,input.Color.g,0,1)*c.r;
	return finalColor;
}

technique Technique1 {
	pass Trail0 {
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
