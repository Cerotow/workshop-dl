sampler uImage0 : register(s0);
sampler uImage1 : register(s1); 
sampler uImage2 : register(s2); 
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;

float2 uPos;
float r;//0.02
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
    float4 finalcolor = float4(0, 0, 0, 1);
	
	float2 pos = uPos;//“Boss”的位置
	float2 offset = (coords - pos);

	float2 rpos = offset * float2(uScreenResolution.x / uScreenResolution.y, 1);
	float dis = length(rpos);

	for (int i = -3; i <= 3; i++)
	{
		finalcolor += tex2D(uImage0, pos + offset + offset * r*i)*(4 - abs(i))*0.07;
	}

	return color * (1 - uOpacity) + finalcolor * uOpacity;
}

technique Technique1
{
	pass Screen
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}