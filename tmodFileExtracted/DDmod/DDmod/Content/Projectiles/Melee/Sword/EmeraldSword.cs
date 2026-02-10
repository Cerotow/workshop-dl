using DDmod.Content.Items.Melee.Sword.Make;
using DDmod.NoContent.Config;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class EmeraldSword : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 54;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            //projectile.light = 0.50f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.ownerHitCheck = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            Projectile.MeleeProj().SwordHitbox = true;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];

            Projectile.MeleeProj().oldVels2 = 60;
            Projectile.extraUpdates = 8;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }
        public override bool PreAI()
        {

            //大小加成
            Projectile.ProjScaleChange();
            Player player = Main.player[Projectile.owner];
            Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
            Projectile.HoldProj(player, 44 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);

            Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates + 1), -1, 2f, true);
            bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead && Projectile.DProj().Times[4] <= 1;
            if (channeling)
            {
                Projectile.DProj().Times[2] = 0;
            }
            else
            {
                if (Projectile.DProj().Times[4] == 2)
                {
                    Projectile.DProj().Times[4]++;
                }
            }
            int useTime = (int)(player.HeldItem.useTime / player.GetTotalAttackSpeed(DamageClass.Melee));
            if (Projectile.localAI[1] >= 0 && Projectile.ai[1] == 2 && Projectile.DProj().Times[2] < useTime)
            {
                Projectile.ai[1] = 3;
                Projectile.DProj().Times[4]++;
                PlaySound(SoundID.Item1, Projectile.position);
            }
            Projectile.spriteDirection = Projectile.DProj().Times[0] > 0 ? 0 : 1;
            return false;
        }
        Color color = new Color(0, 255, 100, 120) * 0.67f;
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (ModContent.GetInstance<DDConfigClient>().SwordHit)
            {
                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/Sword");
                sound.Pitch = 0;
                PlaySound(sound, target.position);
            }
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = color;
        }
        public float TWidth()
        {
            return 22;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] < 2)
            {
                return false;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector = Projectile.Player().ArmCenter();
            if (Projectile.MeleeProj().oldPlayer != Vector2.Zero)
            {
                vector = Projectile.MeleeProj().oldPlayer;
            }
            Player player = Main.player[Projectile.owner];

            DDHelper.BladeTrail(DDTextures.WhitePng, color, 1F, Projectile.DProj().Times[0] > 0);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, player.ArmCenter() - Main.screenPosition, 88, null, Projectile.scale * TWidth());
            DDHelper.BladeTrail(DDTextures.Wave, color, 1F, Projectile.DProj().Times[0] > 0);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale * TWidth());
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Vector2 Center = Projectile.Center - Main.screenPosition;

            if (Projectile.spriteDirection == 0)
            {
                Main.spriteBatch.Draw(texture, Center, null, lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Center, null, lightColor, Projectile.rotation + MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
            }

            return false;
        }
    }
}