using ArtificerMod.Common;
using ArtificerMod.Content.Buffs.AbilityAccH;
using ArtificerMod.Content.Buffs.AbilityAccPH;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArtificerMod.Content.Projectiles.AbilityAccH
{
	public class PrimevalAura : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;

			Projectile.DamageType = DamageClass.Default;
			Projectile.timeLeft = 600;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
            Projectile.alpha = 255;
		}

        public override bool? CanDamage()
        {
            return false;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.active || owner.dead)
            {
                Projectile.Kill();
                return;
            }

            Projectile.Center = owner.Center;
            Projectile.alpha = Utils.Clamp(Projectile.alpha + (Projectile.timeLeft > 25 ? -10 : 10), 0, 255);

            if(!owner.TryGetModPlayer(out ArtificerPlayer modPlr) || modPlr.relicEquip == null)
            {
                Projectile.timeLeft = (int)MathHelper.Min(Projectile.timeLeft, 25);
                return;
            }

            if(Main.rand.NextFloat() > 1f - Projectile.Opacity)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(490f, 490f),
                DustID.ChlorophyteWeapon, owner.velocity + Main.rand.NextVector2Circular(1f, 1f), 0, default, 1f);
                dust.noGravity = true;
            }

            float sqrRange = 500f * 500f;
            foreach(var npc in Main.ActiveNPCs)
            {
                if(npc.chaseable && Projectile.Center.DistanceSQ(npc.Center) <= sqrRange)
                {
                    npc.AddBuff(ModContent.BuffType<PrimevalSunder>(), 600);
                    owner.AddBuff(ModContent.BuffType<PrimevalHarvest>(), 600);
                    modPlr.relicRegen += 2;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.MagicPixel.Value;
            Rectangle sourceRectangle = new(0, 0, 1, 1);
            Vector2 origin = sourceRectangle.Size() / 2f;

            for (int i = 0; i < 360; i++)
            {
                Vector2 rotOffset1 = new Vector2(0, 500f).RotatedBy(MathHelper.ToRadians(i * 2f));
                Vector2 rotOffset2 = new Vector2(0, 500f).RotatedBy(MathHelper.ToRadians((i + 1) * 2f));

                Vector2 vector2 = Projectile.Center + rotOffset1;
                Vector2 v = Projectile.Center + rotOffset2 - vector2;
                float y = v.Length();
                float rotOffset = v.ToRotation();

                Color color = new Color(165, 210, 90) * 0.75f * Projectile.Opacity;

                Main.EntitySpriteDraw(texture, vector2 - Main.screenPosition, sourceRectangle, color,
                    rotOffset + (float)Math.PI / 2f, origin, new Vector2(4f, y), SpriteEffects.None);
            }

            return false;
        }
    }
}