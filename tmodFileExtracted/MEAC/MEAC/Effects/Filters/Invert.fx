sampler uImage0 : register(s0);

float Alpha;

float4 PixelShaderFunction(float4 drawColor : COLOR0,float2 uv : TEXCOORD0) : COLOR0
{
	float4 c = tex2D(uImage0,uv);
	float4 i = 1-c;
	i.a=1;
    return lerp(c,i,Alpha);
}

technique Technique1 
{
	pass Pass0
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
