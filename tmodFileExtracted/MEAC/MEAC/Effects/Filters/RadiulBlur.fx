sampler uImage0 : register(s0);

float Offset;
float Scale;
float Alpha;
float2 Pos;
float4 PixelShaderFunction(float4 drawColor : COLOR0,float2 uv : TEXCOORD0) : COLOR0
{
	float4 finalColor = 0;
	for(int i=0;i<9;i++)
	{
		float2 offset=uv-Pos;
		finalColor += tex2D(uImage0,uv+offset*(i*Scale+Offset));
	}
	finalColor*=Alpha*0.11;
	finalColor.a=1;
    return finalColor;
}

technique Technique1 
{
	pass Pass0
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
