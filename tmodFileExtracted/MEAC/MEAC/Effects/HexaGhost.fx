sampler uImage0 : register(s0);
float iTime;
float4 PixelShaderFunction(float4 drawColor : COLOR0,float2 coord : TEXCOORD0) : COLOR0
{
	float2 uv=coord;
    uv-=0.5;
	float2 puv;
    puv.x=atan2(uv.y,uv.x)/6.28;
    puv.y=length(uv);
    
    puv.x+=puv.y*0.7;
    float4 final = 0;
    float4 col = tex2D(uImage0,float2(puv.x,puv.y-iTime*0.1));
    
    float threshold = length(uv)*1.9;
    threshold=pow(threshold,2.4);
    final+=(col.r<threshold?0:1)*float4(0.35,0.23,0.35,1.0);
    
    threshold = length(uv)*2.4;
    threshold=pow(threshold,2.6);
    final+=(col.r<threshold?0:1)*float4(0.22,0.1,0.22,1.0);
    
    threshold = length(uv)*3.2;
    threshold=pow(threshold,2.8);
    final+=(col.r<threshold?0:1)*float4(0.3,0.3,0.3,1.0);


    
    return final;
}

technique Technique1 
{
	pass Colorize
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
