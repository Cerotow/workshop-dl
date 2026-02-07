sampler uImage0 : register(s0);

float Threshold;
float Alpha;
bool inverse;
float4 PixelShaderFunction(float4 drawColor : COLOR0,float2 uv : TEXCOORD0) : COLOR0
{
	float4 c = tex2D(uImage0,uv);
	float4 retval;
	if(inverse)
	{
	if(max(c.r,max(c.g,c.b))>Threshold)
		retval= float4(1,0,0,1);
	else
		retval= 0;
	}
	else
	if(max(c.r,max(c.g,c.b))<Threshold)
		retval= float4(1,0,0,1);
	else
		retval= 0;
	return lerp(c,retval,Alpha);
}

technique Technique1 
{
	pass Pass0
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
