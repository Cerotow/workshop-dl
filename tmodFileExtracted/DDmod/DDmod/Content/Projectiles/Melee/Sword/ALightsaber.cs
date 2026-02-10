using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Sync;
using DDmod.UI.BattlePetUI.技能;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public abstract class ALightsaber : ModProjectile
    {
        public virtual Color Endcolor => new Color(255, 255, 255, 255);
        public virtual Color color => new Color(0, 50, 255, 0);
        public virtual Color color2 => new Color(255, 205, 0, 0);
        public int oldVels = 72;
        public int Width = 48;
        public Player player
        {
            get
            {
                return Main.player[Projectile.owner];
            }
        }
        public virtual void Set()
        {

        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            Projectile.width = 10;
            Projectile.height = 150;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 90000;
            Projectile.usesLocalNPCImmunity = true;

            Projectile.MeleeProj().SwordHitbox = true;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = oldVels;
            Projectile.hide = true;
            Projectile.extraUpdates = 3;
            Set();

        }
        public override void AI()
        {
            
            for (int A = -Projectile.height / 2; A < Projectile.height / 2; A += (int)(4 * Projectile.scale))
            {
                Lighting.AddLight(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), color.ToVector3());
            }
            Player player = Main.player[Projectile.owner];
            //大小加成
            Projectile.ProjScale();
            Projectile.height = (int)(Projectile.OriginalHeight() * ((float)Projectile.ai[2] / 200) * Projectile.scale);
            bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead && player.Dplayer().ForbiddenToAttack == 0;
            Projectile.MeleeProj().oldVels2 = (oldVels-16) * (float)Projectile.ai[2] / 200 + 16;
            if (!channeling)
            {
                Projectile.HoldProj(player, 12 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);

                Projectile.ai[2] -= 2;
                if (Projectile.ai[2] <= 0)
                {
                    player.itemTime = 0;
                    player.itemAnimation = 0;
                    Projectile.Kill();
                }
                return;
            }
            Projectile.localNPCHitCooldown = (int)(player.HeldItem.useAnimation / player.GetTotalAttackSpeed(Projectile.DamageType));
            if (Projectile.ai[2] < 200)
            {
                Projectile.ai[2] += 2;
                Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates + 1), 80, Projectile.DProj().Times[4] + 114514, true);

                Projectile.HoldProj(player, 12 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);
            }
            else
            {
                Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates + 1), 80, Projectile.DProj().Times[4] + 114514, true);
                Projectile.HoldProj(player, 12 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);
            }

            player.heldProj = -1;
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 20 * (Projectile.extraUpdates + 1);
                PlaySound(SoundID.Item15, Projectile.position);
            }
        }
        public override void OnHitNPC(Terraria.NPC target, HitInfo hit, int damageDone)
        {
            int Type = ModContent.DustType<电光粒子>();
            for (int A = 0; A < 5; A++)
            {
                Dust dust = Main.dust[NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, color)];
                dust.noGravity = false;
                dust.scale = Main.rand.NextFloat(0.75f, 1.25f);
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0.5f, 8f);
                dust.rotation = dust.velocity.ToRotation();
            }
            SoundStyle sound = SoundID.NPCHit53;
            sound.Pitch = 2;
            sound.Volume = 0.1F;
            PlaySound(sound, target.position);
            
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] < 2)
            {
                return false;
            }
            Vector2 vector = Projectile.Player().ArmCenter();
            if (Projectile.MeleeProj().oldPlayer != Vector2.Zero)
            {
                vector = Projectile.MeleeProj().oldPlayer;
            }
            DDHelper.BladeTrail(DDTextures.WhitePng, new Color(255,255,255,0), 0F, Projectile.DProj().Times[0] > 0,1);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Width * Projectile.scale * (float)Projectile.ai[2] / 200);
            DDHelper.BladeTrail(DDTextures.WhitePng, [color, color, new Color(255, 255, 255)], 1F, Projectile.DProj().Times[0] > 0);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Width * Projectile.scale* (float)Projectile.ai[2] / 200);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);


            Texture2D texture = DDTextures.Lightsaber.Value;
            Texture2D texture2 = DDTextures.VoidStar.Value;
            vector = new Vector2(Projectile.width, Projectile.height) / 2;


            Vector2 Center = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, Center + Projectile.velocity.PerfectNormalize() * (4 * Projectile.scale), null, new Color(0, 0, 0, 155), Projectile.rotation + MathHelper.PiOver4, new Vector2(texture.Width / 2, texture.Height - 80), new Vector2(0.2f * Projectile.scale, 0.5f * Projectile.height/150), 0, 0f);
            Main.spriteBatch.Draw(texture, Center + Projectile.velocity.PerfectNormalize() * (4 * Projectile.scale), null, color, Projectile.rotation + MathHelper.PiOver4, new Vector2(texture.Width / 2, texture.Height - 80), new Vector2(0.2f * Projectile.scale, 0.5f * Projectile.height/150), 0, 0f);
            Main.spriteBatch.Draw(texture, Center + Projectile.velocity.PerfectNormalize() * (4 * Projectile.scale), null, color2, Projectile.rotation + MathHelper.PiOver4, new Vector2(texture.Width / 2, texture.Height - 80), new Vector2(0.1f * Projectile.scale, 0.5f * Projectile.height/150), 0, 0f);

            texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, 0, 0f);

            return false;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }

    }
}