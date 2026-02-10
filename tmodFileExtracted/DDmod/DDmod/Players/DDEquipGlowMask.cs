using DDmod.Content.Tiles;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace DDmod.Players
{
	internal static class DDEquipGlowMask
	{
        public static int A = 0;
        public static void GlowAdd(this Dictionary<string, int> DDGlow, string path,string name)
        {
            Array.Resize(ref TextureAssets.GlowMask, TextureAssets.GlowMask.Length + 1);
            TextureAssets.GlowMask[TextureAssets.GlowMask.Length - 1] = ModContent.Request<Texture2D>(path);
            DDGlow.Add(name, TextureAssets.GlowMask.Length - 1);
            A++;
        }
		public static void Load(Dictionary<string, int> DDGlow)
        {
            Array.Resize(ref Chest.chestTypeToIcon, Chest.chestTypeToIcon.Length + 2);
            DDGlow.GlowAdd("DDmod/Content/Items/Boss/夜光蘑菇王/夜光蘑菇王面具_HeadGlow", "夜光蘑菇王面具");
            DDGlow.GlowAdd("DDmod/Content/Items/Series/GlowingMushroom/GlowingMushroomHelm_HeadGlow", "蘑菇头");
            DDGlow.GlowAdd("DDmod/Content/Items/Series/GlowingMushroom/GlowingMushroomShirt_Body", "蘑菇衣");
            DDGlow.GlowAdd("DDmod/Content/Items/Series/GlowingMushroom/GlowingMushroomLeggings_Legs", "蘑菇腿");
            DDGlow.GlowAdd("DDmod/Content/Items/Boss/MiniBoss/枯萎的橡果之灵面具_Glow", "枯萎橡果头");
            DDGlow.GlowAdd("DDmod/Content/Items/Boss/绿岩之视/绿岩之视面具_HeadGlow", "绿岩之视面具");
            DDGlow.GlowAdd("DDmod/Content/Items/Boss/绿岩之视/绿岩护目镜_HeadGlow", "绿岩护目镜");
            DDGlow.GlowAdd("DDmod/Content/Items/Boss/绿岩之视/绿岩头盔_HeadGlow", "绿岩头盔");
            DDGlow.GlowAdd("DDmod/Content/Items/Boss/绿岩之视/绿岩面罩_HeadGlow", "绿岩面罩");
            DDGlow.GlowAdd("DDmod/Content/Items/Boss/绿岩之视/绿岩帽_HeadGlow", "绿岩帽");
            DDGlow.GlowAdd("DDmod/Content/Items/Boss/绿岩之视/绿岩盔甲_Body", "绿岩盔甲");
            DDGlow.GlowAdd("DDmod/Content/Items/Boss/绿岩之视/绿岩裤_LegsGlow", "绿岩裤");
            DDGlow.GlowAdd("DDmod/Content/Items/Melee/SwordShield/机械魔眼剑盾_Glow", "机械魔眼剑盾");
            DDGlow.GlowAdd("DDmod/Content/Items/Melee/SwordShield/流星剑盾_Glow", "流星剑盾");
            DDGlow.GlowAdd("DDmod/Content/Items/Melee/SwordShield/绿岩剑盾_Glow", "绿岩剑盾");
            DDGlow.GlowAdd("DDmod/Content/Items/Melee/Sword/Make/MeteorSword_Glow", "流星剑");
            DDGlow.GlowAdd("DDmod/Content/Items/Melee/FlyingKnife/Make/流星投刀_Glow", "流星飞刀");
            DDGlow.GlowAdd("DDmod/Content/Items/Boss/MeteorDiggerItems/MeteorShortSwordItem_Glow", "流星短剑");
            DDGlow.GlowAdd("DDmod/Content/Items/Melee/FlyingKnife/Make/绿岩飞刀_Glow","绿岩飞刀");
            DDGlow.GlowAdd("DDmod/Content/Items/Boss/天雷怒云/雷霆剑_Glow", "雷霆剑");
            DDGlow.GlowAdd("DDmod/Content/Items/Boss/先祖咒魂/诅咒双刃_Glow", "诅咒双刃");
            DDGlow.GlowAdd("DDmod/Content/Items/Boss/先祖咒魂/先祖咒魂面具_HeadGlow", "先祖咒魂面具");
            DDGlow.GlowAdd("DDmod/Content/Items/Boss/天雷怒云/天雷怒云面具_HeadGlow", "天雷怒云面具");
            DDGlow.GlowAdd("DDmod/Content/Items/Ranged/Make/Cannon/星闪炮_Glow", "星闪炮");

            TileHelp.TileGlow(DDGlow, out int G);
            A = G;
        }
		public static void UnLoad(DDSystem system)
		{
			Array.Resize(ref TextureAssets.GlowMask, TextureAssets.GlowMask.Length - A);
            system.DDEquipGlow = null;
		}
	}
}