using DDmod.Content.NPCs.Boss.MeteorAnnihilator;
using DDmod.Content.NPCs.Boss.星心守卫;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.DDOn;
using Terraria.GameContent.Skies;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.IO;
using Terraria.ModLoader.IO;
using Filters = Terraria.Graphics.Effects.Filters;

namespace DDmod.Worlds
{
    public class 四柱背景 : ModSceneEffect
    {
        public static int A;
        public override SceneEffectPriority Priority
        {
            get
            {
                return SceneEffectPriority.Event;
            }
        }
        public override bool IsSceneEffectActive(Player player)
        {
            if (A > 0)
            {
                A = 0;
                return true;
            }
            return false;
        }

        public override void SpecialVisuals(Player player, bool isActive)
        {
            if (A == 1)
                SkyDowned.Sky("日耀", isActive);
            else if (A == 2)
                SkyDowned.Sky("星旋", isActive);
            else if (A == 3)
                SkyDowned.Sky("星云", isActive);
            else if (A == 4)
                SkyDowned.Sky("星尘", isActive);
        }
    }
    public class SkyDowned : ModSystem
    {
        public static SkyProj[] Proj = new SkyProj[400];
        public static Color BackgroundColor;
        public override void Load()
        {
            //Filters.Scene["狱火蛇Sky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0f, 0f, 0f).UseOpacity(0f), 0);
            SkyManager.Instance["狱火蛇Sky"] = new 狱火蛇Sky();
            SkyManager.Instance["星心守卫Sky"] = new 星心守卫Sky();
            //Filters.Scene["杂物Sky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0f, 0f, 0f).UseOpacity(0f), 0);
            SkyManager.Instance["杂物Sky"] = new 杂物Sky();

            SkyManager.Instance["日耀"] = new SolarSky();
            SkyManager.Instance["星旋"] = new VortexSky();
            SkyManager.Instance["星云"] = new NebulaSky();
            SkyManager.Instance["星尘"] = new StardustSky();
            for (int k = 0; k < Proj.Length; k++)
            {
                Proj[k] = new SkyProj();
                Proj[k].active = false;
            }
        }
        public static void Sky(string biomeName, bool inZone, Vector2 activationSource = default)
        {
            if (SkyManager.Instance[biomeName] != null && inZone != SkyManager.Instance[biomeName].IsActive())
            {
                if (inZone)
                    SkyManager.Instance.Activate(biomeName, activationSource);
                else
                    SkyManager.Instance.Deactivate(biomeName);
            }
        }
        public override void OnWorldLoad()
        {
        }

        public override void OnWorldUnload()
        {
        
            
        }
        public override void SaveWorldData(TagCompound tag)
        {
        }
        public override void LoadWorldData(TagCompound tag)
        {
        }

        public override void NetSend(BinaryWriter writer)
        {
        }

        public override void NetReceive(BinaryReader reader)
        {
        }
        public override void PreUpdateInvasions()
        {
        }
        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
        }
        public override void PreUpdateTime()
        {
        }
    }
}
