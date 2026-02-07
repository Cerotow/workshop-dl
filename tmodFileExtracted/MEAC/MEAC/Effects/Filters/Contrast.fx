sampler uImage0 : register(s0);

float Alpha;
float A;
float B;
float C;
float4 PixelShaderFunction(float4 drawColor : COLOR0,float2 uv : TEXCOORD0) : COLOR0
{
	float4 c = tex2D(uImage0,uv);
    return lerp(c,c*c*c*A+c*c*B+c*C,Alpha);
}

technique Technique1 
{
	pass Pass0
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
