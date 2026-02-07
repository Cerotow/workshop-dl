sampler uShapeTex : register(s0);

texture tex0;   // 声明一个纹理对象 
float t;
int useAlpha;
sampler2D uColorTex = sampler_state  // 声明一个采样器对象 
{
	Texture = <tex0>;         // 指定被采样的纹理 
	MinFilter = Linear;       // 纹理过滤方式 
	MagFilter = Linear;
	AddressU = Wrap;          // 纹理寻址模式 
	AddressV = Wrap;
};

float4 PixelShaderFunction(float2 coord : TEXCOORD0) : COLOR0
{
	
	float4 c = tex2D(uShapeTex, float2(coord.x+t , coord.y));//主纹理
	c*= tex2D(uColorTex, float2(coord.x +t ,0.5))*0.8f;//颜色图
	float4 cclone = tex2D(uShapeTex, float2(coord.x+t, coord.y));//主纹理复制
	c += cclone*0.8f;
    
    float a=0;
    float a2 = 0;
    if(coord.x<0.002)
    {
        a = coord.x*475;
    }
    else
        a = 0.95;
    
    if(coord.x<0.02)
    {
        a2 = coord.x * 50;
    }
    else
        a2 = 1;
    
    if (useAlpha==0)
        return c;
    else if (useAlpha==1)
        return c * a2;
    else
        return c * a;

}
technique Technique1 
{
	pass RainbowLaser 
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
