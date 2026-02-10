using DDmod.NoContent.Config;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class SoulDrain : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            //projectile.light = 0.50f;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 2;
            Projectile.coldDamage = true;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.HoldProj(player, 24, 0, Vector2.Zero, MathHelper.PiOver4,0);
            if (Projectile.localAI[0] >= 2f)
            {
                if (Projectile.soundDelay == 0)
                {
                    Projectile.soundDelay = 30;
                    SoundStyle sound = SoundID.Item29;
                    sound.Pitch = -0.3f;
                    PlaySound(sound, Projectile.position);
                }
            }
            if (player.statMana <= 0 && Main.myPlayer == Projectile.owner)
            {
                Projectile.Kill();
            }
            bool Ok = false;
            for(int a =0;a<200;a++)
            {
                NPC npc = Main.npc[a];
                if (npc.active&&!npc.friendly&&npc.life>5&&!npc.dontTakeDamage&& new Rectangle((int)player.Dplayer().MouseWorld.X-100, (int)player.Dplayer().MouseWorld.Y-100, 200, 200).Intersects(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height))&& !npc.Dnpc().Properties.Iron)
                {
                    Ok = true;
                }
            }
            if (Projectile.ai[0] > 20 && Ok)
            {
                int A = player.ItemMana() / 2;
                if (player.ItemMana() > 0 && A < 1)
                {
                    A = 1;
                }
                player.statMana -= A;
                Projectile.ai[0] -= 20;
                if (Projectile.owner == Main.myPlayer)
                {
                    NewProjectile(Projectile.GetSource_FromThis(), player.Dplayer().MouseWorld, Vector2.Zero, 476, (int)(Projectile.damage), 0, Projectile.owner, Projectile.whoAmI,Projectile.type);
                }
            }
            int damageWithChargeAndStats = player.GetWeaponDamage(player.HeldItem);
            Projectile.damage = (int)(damageWithChargeAndStats);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            if (Main.myPlayer != Projectile.owner||Projectile.Player().dead)
            {
                return;
            }
            Player player = Main.player[Projectile.owner];
            Vector2 vector = Projectile.velocity.PerfectNormalize();
            player.velocity -= vector * 3 * Projectile.localAI[0];
            if (!Collision.CanHitLine(player.Center + Projectile.velocity.PerfectNormalize() * 50, 12, 12, player.Center, player.width / 2, player.height / 2) && ModContent.GetInstance<DDConfigServer>().Staffdamage)
            {
                if (Projectile.localAI[0] > 0.5F)
                {
                    Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(100, 50), vector * (8 + Projectile.localAI[0]), ModContent.ProjectileType<BloodBomb>(), (int)(Projectile.damage*4* Projectile.localAI[0] / 5), Projectile.knockBack, Projectile.owner, 0, 0, Projectile.localAI[0])];
                    projectile.scale = (Projectile.localAI[0]+ Projectile.localAI[0])/2;
                    projectile.hostile = true;
                }
            }
            else
            {
                Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(100, 50), vector * (8 + Projectile.localAI[0]), ModContent.ProjectileType<BloodBomb>(), (int)(Projectile.damage*4* Projectile.localAI[0]), Projectile.knockBack, Projectile.owner, 0, 0, Projectile.localAI[0])];
                projectile.scale = (Projectile.localAI[0] + Projectile.localAI[0])/2;
            }
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public float T1 = 0;
        public float T2 = 0.33f;
        public float T3 = 0.66f;
        public float H2 = 1;
        public override bool PreDraw(ref Color lightColor)
        {
            T1 += 0.033f;
            T2 += 0.033f;
            T3 += 0.033f;
            if (T1 > 0.99f) T1 = 0;
            if (T2 > 0.99f) T2 = 0;
            if (T3 > 0.99f) T3 = 0;
            Projectile.DProj().Times[0] += 0.02F;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, 0, 0f);
            Player player = Main.player[Projectile.owner];
            Color color = new Color(255, 47, 37, 0);
            Vector2 vector2 = Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 26;
            for (int a = 0; a < 5; a++)
            {
                if (Projectile.ai[0] > 5)
                {
                    texture = DDTextures.Starlight.Value;
                    Main.spriteBatch.Draw(texture, Projectile.Center + Projectile.velocity.PerfectNormalize() * 46 - Main.screenPosition, null, color * (1 - T1), Projectile.rotation + MathHelper.PiOver4, texture.Size() / 2, new Vector2(Projectile.localAI[0] / 5, Projectile.localAI[0] / 2) * T1, 0, 0f);
                }
            }
            Texture2D texture3 = DDTextures.VoidStar.Value;
            Main.spriteBatch.Draw(texture3, vector2, null, color * 0.7f, Projectile.rotation - MathHelper.PiOver4, texture3.Size() / 2, new Vector2(Projectile.localAI[0] / 6, Projectile.localAI[0] / 2.5f), 0, 0f);

            texture = DDTextures.Circle[7].Value;
            DDHelper.Compression(texture, color, Projectile.rotation - MathHelper.PiOver4, Projectile.Opacity, new Vector2(2 + Projectile.localAI[0], 1), Projectile.direction, Projectile.DProj().Times[0], BlendState.Additive);
            Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.localAI[0] / 3, 0, 0);
            if (Projectile.ai[0] > 5)
            {
                Main.spriteBatch.Draw(texture, vector2 - Projectile.velocity.PerfectNormalize() * -30 * T1, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.localAI[0] / 3 * (1 - T1), 0, 0);
                Main.spriteBatch.Draw(texture, vector2 - Projectile.velocity.PerfectNormalize() * -30 * T2, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.localAI[0] / 3 * (1 - T2), 0, 0);
                Main.spriteBatch.Draw(texture, vector2 - Projectile.velocity.PerfectNormalize() * -30 * T3, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Projectile.localAI[0] / 3 * (1 - T3), 0, 0);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);


            DDHelper.DrawExpanded(Main.GameViewMatrix.TransformationMatrix, Main.spriteBatch, player.Center - Main.screenPosition + new Vector2(0, -60), 0.5f, Projectile.localAI[0] / 2 * 0.5f, color * 0.3f, color, 1.1F, 0.6f);
            DynamicSpriteFontExtensionMethods.DrawString(
                Main.spriteBatch,
                FontAssets.MouseText.Value,
                (int)(Projectile.localAI[0] / 4 * 100) + "%",
                player.Center - Main.screenPosition - new Vector2(0, 38),
                color, 0f,
                ChatManager.GetStringSize(FontAssets.MouseText.Value, (int)(Projectile.localAI[0] / 4 * 100) + "%", Vector2.One) / 2,
                0.75F, SpriteEffects.None, 0f);
            return false;
        }
    }
}