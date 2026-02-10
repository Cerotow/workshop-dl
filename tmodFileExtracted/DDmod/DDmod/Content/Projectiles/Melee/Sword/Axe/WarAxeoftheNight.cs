using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Items;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword.Axe
{
    public class WarAxeoftheNight : ModProjectile
    {
        public static Asset<Texture2D> Trailing;
        public override void Load()
        {
            Trailing = ModContent.Request<Texture2D>(Texture + "_Trailing");
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            // Projectile.light = 0.50f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.ownerHitCheck = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            Projectile.extraUpdates = 6;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
        }
        float sp;
        float sp2;
        public override bool PreAI()
        {
            Projectile.ProjScale();
            if (Projectile.soundDelay == 0)
            {
                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/叉");
                sound.Pitch = sp * 2 - 1;
                sound.Volume = sp;
                PlaySound(sound, Projectile.position);
                Projectile.soundDelay = (int)(300 * (1.4F - sp));
            }
            Item item = Projectile.Player().ActiveItem();
            Projectile.MeleeProj().oldVels2 = TextureAssets.Item[item.type].Size().Length() / 2;
            Player player = Main.player[Projectile.owner];
            sp2++;
            if (player.controlUseItem)
            {
                if (sp <= 1F)
                {
                    sp += 0.0005F;
                }
            }
            else
            {
                sp -= 0.0015F;
                if (sp < 0)
                {
                    Projectile.Kill();
                    player.itemTime = 0;
                    player.itemAnimation = 0;
                    return false;
                }
            }
            if (sp2 % 12 == 0)
            {
                int A = NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * 10 * Projectile.scale - new Vector2(20, 20), 40, 40, 14, Projectile.velocity.X * 4, Projectile.velocity.Y * 4, 0, default, 1F);
                Main.dust[A].velocity = new Vector2(0, -1) + Projectile.velocity * sp;
            }
            if (sp2 % 4 == 0)
            {
                if (Main.netMode != 2)
                {
                    Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                    Vector2 vector = Projectile.Center + texture.Size().RotatedBy(Projectile.rotation - MathHelper.PiOver2) / 4 - Projectile.Size / 2;
                    for (int a = -Projectile.width / 16; a <= Projectile.width / 16; a++)
                    {
                        for (int b = -Projectile.height / 16; b <= Projectile.height / 16; b++)
                        {
                            if (Main.tileAxe[Main.tile[(int)vector.X / 16 + a, (int)vector.Y / 16 + b].TileType])
                            {
                                Main.player[Projectile.owner].PickTile((int)vector.X / 16 + a, (int)vector.Y / 16 + b, (int)(player.ActiveItem().GetGlobalItem<MeleeGlobalItem>().OriginalAxe(player.ActiveItem()) /2* sp));
                            }
                        }
                    }
                }
            }
            //手持弹幕
            Projectile.HoldProj(player, 26 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0].RotatedBy(-3 * player.direction), MathHelper.PiOver4, 0, true, sp / 10 * Projectile.DProj().Times[0], false, false);
            //玩家按着左键
            Projectile.HoldSword(player, 20 * 6, 60, 11480, true, true);

            Projectile.MeleeProj().oldVels[0] += Projectile.velocity.RotatedBy(1 * player.direction) * 10;
            Projectile.spriteDirection = Projectile.DProj().Times[0] == 1 ? 0 : 1;
            player.velocity.X *= 1 - 0.02F * (sp);
            return false;
        }
        int NPC;
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];
            target.AddBuff(ModContent.BuffType<EvilEntanglement>(), Main.rand.Next(100, 300));

            Vector2 vector = Main.rand.NextVector2Unit() * 20;
            NewProjectile(player.GetSource_FromAI(), target.Center + vector, -vector / 10, ModContent.ProjectileType<LightsBaneCut>(), 0, 0, player.whoAmI, target.whoAmI, 1);
            player.Dplayer().PlayerShake(12, 2);

        }
        public override bool? CanDamage()
        {
            if (sp <= 0.01F)
            {
                return false;
            }
            return null;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public Color TrailColor(float completionRatio)
        {
            float trailOpacity = Utils.GetLerpValue(0f, 0.1f, completionRatio, true) * Utils.GetLerpValue(0.7f, 0.58f, completionRatio, true);
            Color startingColor = Color.Lerp(color, color, 0.07f);
            return playerHelper.MulticolorLerp(completionRatio, new Color[]
            {
                startingColor,
            }) * trailOpacity;
        }
        public float TrailWidth(float completionRatio)
        {
            return 8 * Projectile.scale;
        }
        internal Trailing TrailDrawer;
        Color color = new Color(99, 74, 187, 0) * 0.5f;
        public override bool PreDraw(ref Color lightColor)
        {
            if (sp <= 0.01F)
            {
                return false;
            }
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            SpriteEffects spriteEffects = 0;
            if (player.direction == -1)
            {
                spriteEffects = (SpriteEffects)1;
            }
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(TrailWidth), new Trailing.VertexColorFunction(TrailColor), null, GameShaders.Misc["刀光"]);
            }
            Vector2 vector = Projectile.Player().ArmCenter();
            if (Projectile.MeleeProj().oldPlayer != Vector2.Zero)
            {
                vector = Projectile.MeleeProj().oldPlayer;
            }
            GameShaders.Misc["刀光"].SetShaderTexture(DDTextures.WhitePng);
            GameShaders.Misc["刀光"].UseColor(Projectile.GetAlpha(color * 0.6f));
            GameShaders.Misc["刀光"].UseSecondaryColor(Projectile.GetAlpha(color) * 0.6f);
            GameShaders.Misc["刀光"].Shader.Parameters["FlameColor"].SetValue(Projectile.GetAlpha(color).ToVector3());
            GameShaders.Misc["刀光"].Shader.Parameters["flipped"].SetValue(Projectile.DProj().Times[0] > 0);
            GameShaders.Misc["刀光"].Shader.Parameters["uDarkshade"].SetValue(0);
            GameShaders.Misc["刀光"].Apply(default(DrawData?));
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null);

            GameShaders.Misc["刀光"].SetShaderTexture(DDTextures.Wave);
            GameShaders.Misc["刀光"].UseColor(Projectile.GetAlpha(color));
            GameShaders.Misc["刀光"].UseSecondaryColor(Projectile.GetAlpha(color));
            GameShaders.Misc["刀光"].Shader.Parameters["FlameColor"].SetValue(Projectile.GetAlpha(color).ToVector3());
            GameShaders.Misc["刀光"].Shader.Parameters["flipped"].SetValue(Projectile.DProj().Times[0] > 0);
            GameShaders.Misc["刀光"].Shader.Parameters["uDarkshade"].SetValue(0);
            GameShaders.Misc["刀光"].Apply(default(DrawData?));
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null);

            float ro = Projectile.rotation;
            if (player.direction == -1)
            {
                ro = Projectile.rotation + MathHelper.PiOver2;
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, ro, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);

            for (int A = 0; A < Projectile.DProj().Times[4] / 4; A++)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(55, 0, 0, 0), ro, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
            }
            //Vector2 vector2 = Projectile.Center + texture.Size().RotatedBy(Projectile.rotation-MathHelper.PiOver2) / 4 - Projectile.Size / 2;
            //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, vector2 - Main.screenPosition, null, Color.White * 0.5F, 0, Vector2.Zero, Projectile.Size / 2, 0, 0f);
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector = Projectile.Center + texture.Size().RotatedBy(Projectile.rotation - MathHelper.PiOver2) / 4 - Projectile.Size / 2;
            projHitbox = new Rectangle((int)vector.X, (int)vector.Y, Projectile.width, Projectile.height);
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }
            return null;
        }
    }
}