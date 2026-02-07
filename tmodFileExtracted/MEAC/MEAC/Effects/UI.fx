sampler uImage0 : register(s0);

float t;
float omega;
float A;
float4 PixelShaderFunction(float4 c: COLOR0,float2 coord : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coord + float2(A * sin(omega * coord.y + t), A * sin(omega * coord.x + t)));
	return color*c;
}




technique Technique1 {
	pass UIWarp {
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
