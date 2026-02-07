sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float4x4 uTransform;
float _Threshold1;//原图-改图阈值
float _Threshold2;//黑-红阈值
float2 _Time;
struct VSInput 
{
	float2 Pos : POSITION0;
    float4 Color : COLOR0;
	float2 Texcoord : TEXCOORD0;
};

struct PSInput 
{
	float4 Pos : SV_POSITION;
	float4 scPos : TEXCOORD1;
    float4 Color : COLOR0;
	float2 Texcoord : TEXCOORD0;
};
PSInput VertexShaderFunction(VSInput input) 
{
	PSInput output;
	output.Texcoord = input.Texcoord;
	output.Pos = mul(float4(input.Pos, 0, 1), uTransform);
	output.scPos = output.Pos;
    output.Color = input.Color;
	return output;
}

float4 PixelShaderFunction(PSInput input) : COLOR0
{
    float2 scCoord = input.scPos.xy+_Time;
	float2 coord=input.Texcoord;
	float4 col=tex2D(uImage0,coord)*input.Color;
	if(col.r>_Threshold1)
	{
		col=tex2D(uImage1,scCoord);
		if(col.r+col.g<_Threshold2)
			col=float4(1,0,0,1);
		else
			col=float4(0,0,0,1);
	}
	return col;
}

technique Technique1 {
	pass P0 {
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}