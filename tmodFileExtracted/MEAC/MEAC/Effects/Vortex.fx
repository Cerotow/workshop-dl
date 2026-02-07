sampler uImage0 : register(s0);
uniform float uColor;
float iTime;
float Exponent;
float Multi;
float2 UVMulti;
float Width;//1.8
float WidthCenter;//0.4
float Rotation;
float3 colorize(float3 base,float3 col,float saturation)
{
    float lum = dot(base,float3(0.3,0.6,0.1));
    float3 midColor = lerp(float3(0.5,0.5,0.5),col,saturation);
    return lum<0.5?lerp(float3(0.,0.,0.),midColor,lum*2.):lerp(midColor,float3(1.,1.,1.),(lum-0.5)*2.);
}
float4 PixelShaderFunction(float4 drawColor : COLOR0,float2 coord : TEXCOORD0) : COLOR0
{
	float2 uv1 = (coord - 0.5);
    uv1*=UVMulti;
    float2 uv2 =1.5*(coord-0.5);
    float3 col = 0;
    float2 puv;
    puv.x=atan2(uv1.y,uv1.x)/6.283;
    puv.y = length(uv1) ;
    puv.x+=puv.y*Rotation;

    float3 col0 = pow(tex2D(uImage0, float2(puv.x, puv.y + iTime)), Exponent) * Multi * smoothstep(Width, 0., abs(length(uv2) - WidthCenter) * 5.);
    col += colorize(col0,drawColor.rgb,0.8);
    
    return float4(col,max(col.r,max(col.g,col.b))*3.) * drawColor.a;
}

technique Technique1 
{
	pass Vortex
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}
