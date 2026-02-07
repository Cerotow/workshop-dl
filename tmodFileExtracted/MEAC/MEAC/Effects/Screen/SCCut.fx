sampler uImage0 : register(s0);

float2 line0;//斜截式（y=kx+b）中的k、b。
float2 offset; //偏移量 在外面算。
float halfWidth;
float4 PSFunction(float2 coords : TEXCOORD0) : COLOR0
{
    
    float4 color = float4(0,0,0,0);
    float k = line0.x;
    float b = line0.y;
    
    float dis = abs(k * coords.x - coords.y + b) / sqrt(k*k+1);//点到直线距离公式
    
    if (dis > halfWidth)
    {
        if (coords.y>k*coords.x+b)//在直线下方
            color = tex2D(uImage0, coords - halfWidth*offset);
        else
            color = tex2D(uImage0, coords + halfWidth*offset);
    }
     return color;
}
technique Technique1
{
	
    pass KnifeEX
    {
        PixelShader = compile ps_2_0 PSFunction();
    }
}