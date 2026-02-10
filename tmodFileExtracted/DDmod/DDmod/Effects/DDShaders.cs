using Terraria.Graphics.Shaders;

namespace DDmod.Effects
{
    public class DDShaders
    {
        public static void LoadShaders()
        {
            if (Main.dedServ)
            {
                return;
            }
            
            GameShaders.Misc["普通拖尾"] = new MiscShaderData(ModContent.Request<Effect>("DDmod/Effects/普通拖尾", AssetRequestMode.ImmediateLoad), "TrailPass");

            
            GameShaders.Misc["贴图拖尾"] = new MiscShaderData(ModContent.Request<Effect>("DDmod/Effects/贴图拖尾", AssetRequestMode.ImmediateLoad), "TrailPass");
            GameShaders.Misc["贴图拖尾2"] = new MiscShaderData(ModContent.Request<Effect>("DDmod/Effects/贴图拖尾2", AssetRequestMode.ImmediateLoad), "TrailPass");
            GameShaders.Misc["贴图拖尾3"] = new MiscShaderData(ModContent.Request<Effect>("DDmod/Effects/贴图拖尾3", AssetRequestMode.ImmediateLoad), "TrailPass");


            
            GameShaders.Misc["硬边缘拖尾"] = new MiscShaderData(ModContent.Request<Effect>("DDmod/Effects/硬边缘拖尾", AssetRequestMode.ImmediateLoad), "TrailPass");


            
            GameShaders.Misc["静态拖尾"] = new MiscShaderData(ModContent.Request<Effect>("DDmod/Effects/静态拖尾", AssetRequestMode.ImmediateLoad), "TrailPass");

            
            GameShaders.Misc["弓弦"] = new MiscShaderData(ModContent.Request<Effect>("DDmod/Effects/弓弦", AssetRequestMode.ImmediateLoad), "TrailPass");

           
            GameShaders.Misc["刀光"] = new MiscShaderData(ModContent.Request<Effect>("DDmod/Effects/刀光", AssetRequestMode.ImmediateLoad), "TrailPass");


            GameShaders.Misc["纯色刀光"] = new MiscShaderData(ModContent.Request<Effect>("DDmod/Effects/纯色刀光", AssetRequestMode.ImmediateLoad), "TrailPass");

         
            GameShaders.Misc["环性进度条"] = new MiscShaderData(ModContent.Request<Effect>("DDmod/Effects/环性进度条", AssetRequestMode.ImmediateLoad), "Pass0");

            
            GameShaders.Misc["环形"] = new MiscShaderData(ModContent.Request<Effect>("DDmod/Effects/环形", AssetRequestMode.ImmediateLoad), "Pass0");

          
            GameShaders.Misc["盾"] = new MiscShaderData(ModContent.Request<Effect>("DDmod/Effects/盾", AssetRequestMode.ImmediateLoad), "Pass0");

           
            GameShaders.Misc["测试盾"] = new MiscShaderData(ModContent.Request<Effect>("DDmod/Effects/测试盾", AssetRequestMode.ImmediateLoad), "Pass0");

            GameShaders.Misc["测试盾2"] = new MiscShaderData(ModContent.Request<Effect>("DDmod/Effects/测试盾2", AssetRequestMode.ImmediateLoad), "Pass0");

            GameShaders.Misc["圆形"] = new MiscShaderData(ModContent.Request<Effect>("DDmod/Effects/圆形", AssetRequestMode.ImmediateLoad), "Pass0");

            GameShaders.Misc["渲染滤镜"] = new MiscShaderData(ModContent.Request<Effect>("DDmod/Effects/渲染滤镜", AssetRequestMode.ImmediateLoad), "Pass0");


            
            GameShaders.Misc["火焰"] = new MiscShaderData(ModContent.Request<Effect>("DDmod/Effects/火焰", AssetRequestMode.ImmediateLoad), "TrailPass");

            
            GameShaders.Misc["压缩"] = new MiscShaderData(ModContent.Request<Effect>("DDmod/Effects/压缩", AssetRequestMode.ImmediateLoad), "ShieldPass");

        }
    }
}
