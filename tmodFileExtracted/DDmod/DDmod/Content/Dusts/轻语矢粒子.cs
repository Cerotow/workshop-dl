using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace DDmod.Content.Dusts
{
	public class 轻语矢粒子 : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.frame = new Rectangle(0, 0, 10, 10);
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity*3;
			dust.rotation += dust.velocity.X;
			dust.scale -= 0.1f;
			dust.frame = new Rectangle(0, (int)(dust.scale * 50)%5*10, 10, 10);
			Lighting.AddLight(dust.position, 0.41f * 0.3f, 2.00f * 0.3f, 2.55f * 0.3f);
			if (dust.scale < 0.01f)
			{
				dust.active = false;
			}
			return false;
		}
	}
}