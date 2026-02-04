using ArtificerMod.Common;
using ArtificerMod.Content.Buffs.AbilityAccPH;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArtificerMod.Content.Projectiles.AbilityAccH
{

	public class FearReaperBlade : ModProjectile
	{
		private const string ChainTexturePath = "ArtificerMod/Content/Projectiles/AbilityAccH/FearReaperChain"; 

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.netImportant = true; // This ensures that the projectile is synced when other players join the world.
			Projectile.width = 32; 
			Projectile.height = 32; 
			Projectile.friendly = true; 
			Projectile.penetrate = -1; 
			Projectile.DamageType = DamageClass.Generic;
			Projectile.timeLeft = 930;
			Projectile.ArmorPenetration = 25;
			DrawOffsetX = -1;
			DrawOriginOffsetY = 0;
			DrawOriginOffsetX = -10;
		}

		readonly float orbitSpeed = 4f;
		readonly float orbitDirection = 1;
		public override void AI()
		{
            if (Projectile.localAI[0] == 0)
            {
                if (Projectile.ai[0] == 1)
                {
                    Projectile.ai[1] = 15f;
                }
                else
                {
                    Projectile.ai[1] = 30f;
                }

                Projectile.ai[2] = 30f;

                int particleNum = Main.rand.Next(5, 11);
                DefineOrbitPos(Main.player[Projectile.owner], out Vector2 orbitPos0);
                for (int i = 0; i < particleNum; i++)
                {
                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.BlackLightningSmall, new ParticleOrchestraSettings
                    {
                        PositionInWorld = Main.rand.NextVector2FromRectangle(Projectile.Hitbox) + (orbitPos0 - Projectile.Center)
                    }, Projectile.owner);
                }

                Projectile.localAI[0] = 1;
            }

			Player owner = Main.player[Projectile.owner];
			if (!owner.active || owner.dead)
			{
				Projectile.Kill();
				return;
			}
			if (!owner.TryGetModPlayer(out ArtificerPlayer artificer) || (!artificer.fearReaper && !artificer.fCharm))
			{
				Projectile.Kill();
				return;
			}

			DefineOrbitPos(owner, out Vector2 orbitPos);

			Projectile.Center = orbitPos;
		}

		private void DefineOrbitPos(Player owner, out Vector2 orbitPos)
		{
			Projectile.ai[1] += 1f;
			Vector2 projOffset = new Vector2(1f).RotatedBy((float)Math.PI * orbitSpeed * (Projectile.ai[1] / 60f) * orbitDirection);

			orbitPos = owner.MountedCenter + projOffset * Projectile.ai[2];

			Projectile.rotation = ((float)Math.PI * orbitSpeed * (Projectile.ai[1] / 60f) * orbitDirection) + (3f * MathHelper.PiOver4);

			if(Projectile.ai[2] < 90f && Projectile.timeLeft > 65)
            {
				Projectile.ai[2]++;
            }
			else if(Projectile.ai[2] > 30f && Projectile.timeLeft <= 65)
            {
				Projectile.ai[2]--;
			}
		}

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0f;

            float reachScalar = Utils.Remap(Projectile.ai[2], 30, 90, 0.3f, 1f);
            Vector2 ownerPos = Main.player[Projectile.owner].Center;
            Vector2 startPoint = Projectile.Center + Projectile.Center.DirectionTo(ownerPos) * -50f * reachScalar;
            Vector2 endPoint = Projectile.Center + Projectile.Center.DirectionTo(ownerPos) * 100f * reachScalar;

            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(),
                startPoint, endPoint, 32f * Projectile.scale, ref collisionPoint))
            {
                return true;
            }

            return projHitbox.Intersects(targetHitbox);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.HitDirectionOverride = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : (-1);
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			// Buff DR effect of Fear Reaper
			Player owner = Main.player[Projectile.owner];
			if(owner.TryGetModPlayer(out ArtificerPlayer artificer))
            {
				artificer.reaperDRCounter++;
            }

			for (int i = 0; i < 2; i++)
			{
				ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.BlackLightningSmall, new ParticleOrchestraSettings
				{
					PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox)
				}, Projectile.owner);
			}
		}

        public override bool PreDraw(ref Color lightColor)
		{
            if (Projectile.ai[2] > 45)
            {
                Color color1 = Color.Indigo; // Colors the back of the slash
                Color color2 = new(130, 110, 180); // Colors the front of the slash
                Color color3 = Color.Lavender; // Colors the edge of the slash

                float colorScalar = Utils.Remap(Projectile.ai[2], 45, 90, 0f, 1f);
                color1 *= colorScalar;
                color2 *= colorScalar;
                color3 *= colorScalar;

                DrawSwingAura(Projectile, color1, color2, color3, rotationDir: Projectile.direction);
            }

            DrawReaperChain(Projectile);


            return true;
		}

		public static void DrawReaperChain(Projectile projectile)
		{
            Player owner = Main.player[projectile.owner];
            if (!owner.active || owner.dead)
            {
                return;
            }

            Vector2 flailPos = owner.MountedCenter;

            Asset<Texture2D> chainTexture = ModContent.Request<Texture2D>(ChainTexturePath);

            Rectangle? chainSourceRectangle = null;
            float chainHeightAdjustment = 0f;

            Vector2 chainOrigin = chainSourceRectangle.HasValue ? (chainSourceRectangle.Value.Size() / 2f) : (chainTexture.Size() / 2f);
            Vector2 chainDrawPosition = projectile.Center;
            Vector2 vectorFromProjectileToOwner = flailPos.MoveTowards(chainDrawPosition, 4f) - chainDrawPosition;
            Vector2 unitVectorFromProjectileToOwner = vectorFromProjectileToOwner.SafeNormalize(Vector2.Zero);
            float chainSegmentLength = (chainSourceRectangle.HasValue ? chainSourceRectangle.Value.Height : chainTexture.Height()) + chainHeightAdjustment;
            if (chainSegmentLength == 0)
                chainSegmentLength = 10; // When the chain texture is being loaded, the height is 0 which would cause infinite loops.
            float chainRotation = unitVectorFromProjectileToOwner.ToRotation() + MathHelper.PiOver2;
            int chainCount = 0;
            float chainLengthRemainingToDraw = vectorFromProjectileToOwner.Length() + chainSegmentLength / 2f;

            while (chainLengthRemainingToDraw > 0f)
            {
                Color chainDrawColor = Lighting.GetColor((int)chainDrawPosition.X / 16, (int)(chainDrawPosition.Y / 16f));

                var chainTextureToDraw = chainTexture;

                Main.spriteBatch.Draw(chainTextureToDraw.Value, chainDrawPosition - Main.screenPosition, chainSourceRectangle, chainDrawColor, chainRotation, chainOrigin, 1f, SpriteEffects.None, 0f);

                chainDrawPosition += unitVectorFromProjectileToOwner * chainSegmentLength;
                chainCount++;
                chainLengthRemainingToDraw -= chainSegmentLength;
            }
        }

        public static void DrawSwingAura(Projectile proj, Color color1, Color color2, Color color3, float outlineLength = 8f, bool noSparkleRotation = false, float extraScalar = 1.25f, float rotationDir = -1)
        {
            Vector2 drawPos1 = proj.Center - Main.screenPosition;
            drawPos1 += proj.Center.DirectionTo(Main.player[proj.owner].Center) * 65f;
            Asset<Texture2D> asset = TextureAssets.Projectile[ProjectileID.Excalibur];
            Rectangle rectangle = asset.Frame(1, 4);
            Vector2 origin = rectangle.Size() / 2f;

            float baseRot = proj.rotation - ((float)Math.PI * 0.4f) * rotationDir;
            if (rotationDir == -1)
            {
                baseRot += (float)Math.PI;
            }

            float sizeScale = proj.scale * extraScalar;
            SpriteEffects drawFX = ((!(rotationDir >= 0f)) ? SpriteEffects.FlipVertically : SpriteEffects.None);

            float slashProgress = Utils.Remap(proj.Opacity, 0f, 1f, 0f, 0.45f);

            float coloringAdjust = Utils.Remap(slashProgress, 0f, 0.2f, 0f, 0.9f) * Utils.Remap(slashProgress, 0.45f, 0.5f, 0.9f, 0f);
            float sizeScalar2 = 0.975f;

            float lighting = Lighting.GetColor(proj.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
            lighting = Utils.Remap(lighting, 0.2f, 1f, 0f, 1f);

            Main.spriteBatch.Draw(asset.Value, drawPos1, rectangle, color1 * lighting * coloringAdjust, baseRot + rotationDir * ((float)Math.PI / 4f) * -1f * (1f - slashProgress), origin, sizeScale, drawFX, 0f);
            Color color4 = Color.White * coloringAdjust * 0.5f;
            color4.A = (byte)((float)(int)color4.A * (1f - lighting));
            Color color5 = color4 * lighting * 0.5f;
            color5.G = (byte)((float)(int)color5.G * lighting);
            color5.B = (byte)((float)(int)color5.R * (0.25f + lighting * 0.75f));
            Main.spriteBatch.Draw(asset.Value, drawPos1, rectangle, color5 * 0.15f, baseRot + rotationDir * 0.01f, origin, sizeScale, drawFX, 0f);
            Main.spriteBatch.Draw(asset.Value, drawPos1, rectangle, color3 * lighting * coloringAdjust * 0.3f, baseRot, origin, sizeScale, drawFX, 0f);
            Main.spriteBatch.Draw(asset.Value, drawPos1, rectangle, color2 * lighting * coloringAdjust * 0.5f, baseRot, origin, sizeScale * sizeScalar2, drawFX, 0f);

            Color colorW = Color.White * Utils.Remap(proj.ai[2], 45, 90, 0f, 1f);
            Main.spriteBatch.Draw(asset.Value, drawPos1, asset.Frame(1, 4, 0, 3), colorW * 0.8f * coloringAdjust, baseRot + rotationDir * 0.01f, origin, sizeScale, drawFX, 0f);
            Main.spriteBatch.Draw(asset.Value, drawPos1, asset.Frame(1, 4, 0, 3), colorW * 0.75f * coloringAdjust, baseRot + rotationDir * -0.05f, origin, sizeScale * 0.8f, drawFX, 0f);
            Main.spriteBatch.Draw(asset.Value, drawPos1, asset.Frame(1, 4, 0, 3), colorW * 0.6f * coloringAdjust, baseRot + rotationDir * -0.1f, origin, sizeScale * 0.6f, drawFX, 0f);


            for (float i = 0f; i < outlineLength; i += 1f)
            {
                float edgeRot = baseRot + rotationDir * i * ((float)Math.PI * -2f) * 0.025f + Utils.Remap(slashProgress, 0f, 1f, 0f, (float)Math.PI / 4f) * rotationDir;
                Vector2 drawPos2 = drawPos1 + edgeRot.ToRotationVector2() * ((float)asset.Width() * 0.5f - 6f) * sizeScale;
                float edgeColorScalar = i / (outlineLength + 1f);
                VisualHelpers.VanillaSparkleFX(proj.Opacity, SpriteEffects.None, drawPos2, new Color(255, 255, 255, 0) * coloringAdjust * edgeColorScalar, color3, slashProgress, 0f, 0.5f, 0.5f, 1f, edgeRot, new Vector2(0f, Utils.Remap(slashProgress, 0f, 0.6f, 3f, 0f)) * sizeScale, Vector2.One * sizeScale);
            }
            Vector2 drawPos3 = drawPos1 + (baseRot + Utils.Remap(slashProgress, 0f, 1f, 0f, (float)Math.PI / 4f) * rotationDir).ToRotationVector2() * ((float)asset.Width() * 0.5f - 4f) * sizeScale;
            float sparkRot = (float)Math.PI / 4f;
            if (noSparkleRotation)
            {
                sparkRot = 0f;
            }
            VisualHelpers.VanillaSparkleFX(proj.Opacity, SpriteEffects.None, drawPos3, new Color(255, 255, 255, 0) * coloringAdjust * 0.5f, color3, slashProgress, 0f, 0.5f, 0.5f, 1f, sparkRot, new Vector2(Utils.Remap(slashProgress, 0f, 0.6f, 4f, 1f)) * sizeScale * 0.5f, Vector2.One * sizeScale * 0.5f);
        
		}

        public override void OnKill(int timeLeft)
        {
			int particleNum = Main.rand.Next(5, 11);
			for (int i = 0; i < particleNum; i++)
			{
				ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.BlackLightningSmall, new ParticleOrchestraSettings
				{
					PositionInWorld = Main.rand.NextVector2FromRectangle(Projectile.Hitbox)
				}, Projectile.owner);;
			}
		}
    }
}