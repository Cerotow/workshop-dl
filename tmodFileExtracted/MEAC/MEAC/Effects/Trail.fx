sampler uShapeTex : register(s0);
sampler uColorTex : register(s1);
sampler uImage2 : register(s2);
float4x4 uTransform;
float t;

struct VSInput 
{
	float2 Pos : POSITION0;
    float4 Color : COLOR0;
	float3 Texcoord : TEXCOORD0;
};

struct PSInput 
{
	float4 Pos : SV_POSITION;
    float4 Color : COLOR0;
	float3 Texcoord : TEXCOORD0;
};
PSInput VertexShaderFunction(VSInput input) 
{
	PSInput output;
	output.Texcoord = input.Texcoord;
	output.Pos = mul(float4(input.Pos, 0, 1), uTransform);
    output.Color = input.Color;
	return output;
}

float4 PixelShaderFunction(PSInput input) : COLOR0
{
	float3 coord = input.Texcoord;
	float4 c = tex2D(uShapeTex, float2(coord.x + t , coord.y));//主纹理
	c += tex2D(uImage2, float2(1-coord.x*5, coord.y));//静态的图片
	c *= coord.z;
	c*= tex2D(uColorTex, float2(c.r,0.5));//乘颜色图
	return c;

}

float4 PixelShaderFunction2(PSInput input) : COLOR0
{
    float3 coord = input.Texcoord;
    float4 c = tex2D(uShapeTex, float2(coord.x + t, coord.y));//主纹理
    c *= coord.z;//乘上透明度
    c = tex2D(uColorTex, float2(c.r, 0.5)); //取颜色
    return c;
}

float4 PixelShaderFunction3(PSInput input) : COLOR0//用于彩虹拖尾
{
    float3 coord = input.Texcoord;
    float4 c = tex2D(uShapeTex, float2(coord.x + t, coord.y)); //主纹理
    c *= tex2D(uColorTex, float2(coord.x + t * 0.5, 0.5)); //颜色图
    float4 cclone = tex2D(uShapeTex, float2(coord.x + t, coord.y)) * 0.7; //主纹理复制
    c += cclone;
    c *= coord.z;
    c += tex2D(uImage2, coord.xy); //静态的图片
    return c;
}


technique Technique1 {
	pass Trail {
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
    pass Trail0
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction2();
    }
    pass Trail2
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction3();
    }
}
