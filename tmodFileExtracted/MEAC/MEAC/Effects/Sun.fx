sampler uImage0 : register(s0);

float uTime;
float3 uColor;
float2 uScreenResolution;
float uScale;
float2 uPos;
float uRayAlpha;
float4 PixelShaderFunction(float4 drawColor : COLOR0,float2 coord : TEXCOORD0) : COLOR0
{
    float t = uTime;
    float4 finalColor;
    float2 uv = coord;
    uv -= uPos;
    uv.x *= uScreenResolution.x / uScreenResolution.y;
    
    uv /= dot(uv, uv);
    uv *= uScale;
    
    float l = length(uv);
    float3 col = float3(l,l,l);
    float ang = atan2(uv.y, uv.x);
    
    float ray = sin(ang * 7 + t * 1.5) + cos(ang * 10. - t * 2);
    ray *= sin(ang * 15. + t * 2) * 0.5 + 0.5;
    if (ray < 0)
        ray *= 0.8;
    ray *= 0.7;
    col += ray * 0.1*uRayAlpha;
    
    finalColor = float4(col, 1.0);
    
    float3 color = uColor; //ÑÕÉ«
    float i = 1 - (finalColor.r + finalColor.g + finalColor.b) / 3;
    finalColor.rgb *= lerp(float3(1,1,1), color, i);
    return finalColor * drawColor;
}
technique Technique1 
{
	pass Sun
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}
