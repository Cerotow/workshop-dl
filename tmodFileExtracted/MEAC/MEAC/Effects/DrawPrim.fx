sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
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
    float4 c = tex2D(uImage0,input.Texcoord.xy);
    c *= input.Color;
	return c;

}
float4 PixelShaderFunction2(PSInput input) : COLOR0
{
    float2 coords = input.Texcoord.xy;
    float4 c = tex2D(uImage0,coords);
    float4 c1 = tex2D(uImage1, coords + float2(0,t));

    return (c * input.Color + c * c1) * input.Color * input.Texcoord.z;

}



technique Technique1 
{
	pass Base {
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
    pass Wave
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction2();
    }
}
