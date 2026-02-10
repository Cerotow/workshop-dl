using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.NoContent.Config;
using Terraria;

namespace DDmod.Content.Projectiles.Melee.FlyingKnife.Proj
{
    public class 泰拉闪电 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 325;
        }
        public override void Load()
        {
            textureLogo = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Logo");
            
        }
        public override void SetDefaults()
        {
            int Length = 475;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = Length;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.timeLeft = Length;
            Projectile.extraUpdates = Length / 6;
        }
        Vector2[] Vector;
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
        }
        public override bool CanHitPlayer(Player target)
        {
            return Projectile.owner==Projectile.Player().whoAmI && Projectile.hostile;
        }
        public override void AI()
        {
            Player player = Projectile.Player();
            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.DProj().vector[0] = Projectile.velocity.PerfectNormalize() * 3;
                Projectile.velocity = Projectile.DProj().vector[0];
            }
            Projectile.DProj().Times[0]++;
            if (Projectile.DProj().Times[0] > 20 && Main.rand.NextBool(10) && Projectile.DProj().track > 3)
            {
                Projectile.DProj().Times[0] = 0;
                NPC npc = Projectile.FindTargetWithinRange(500, true);
                if (Projectile.hostile && player != null && Vector2.Subtract(player.Center - new Vector2(0, 70), Projectile.Center).Y > 0 && Projectile.ai[1] == 0)
                {
                    float A = Vector2.Subtract(player.Center - new Vector2(0, 70), Projectile.Center).Length() / 500;
                    if (A > 0.3F)
                    {
                        A = 0.3F;
                    }
                    Vector2 vector1 = Utils.RotatedBy(Vector2.Subtract(player.Center - new Vector2(0, 70), Projectile.Center).PerfectNormalize() * Projectile.velocity.Length(), Main.rand.NextFloat(-A, A), default);
                    Projectile.velocity = vector1;
                }
                else if (!Projectile.hostile && npc != null && Vector2.Subtract(npc.Center, Projectile.Center).Y > 0 && npc.CanBeChasedBy(Projectile, false))
                {
                    float A = Vector2.Subtract(npc.Center, Projectile.Center).Length() / 300;
                    if (A > 0.6F)
                    {
                        A = 0.6F;
                    }
                    Vector2 vector1 = Utils.RotatedBy(Vector2.Subtract(npc.Center, Projectile.Center).PerfectNormalize() * Projectile.velocity.Length(), Main.rand.NextFloat(-A, A), default);
                    Projectile.velocity = vector1;
                }
                else
                {
                    if (Projectile.hostile && player != null && Vector2.Subtract(player.Center - new Vector2(0, 70), Projectile.Center).Y > 0 && Projectile.ai[1] == 0)
                    {
                        Projectile.tileCollide = true;
                    }
                    Vector2 vector1 = Utils.RotatedBy(Projectile.DProj().vector[0].PerfectNormalize() * Projectile.velocity.Length(), Main.rand.NextFloat(-0.6F, 0.6F), default);
                    Projectile.velocity = vector1;
                }
                if (!Projectile.hostile && Projectile.position.Y > Projectile.localAI[0])
                {
                    Projectile.tileCollide = true;
                }
                Projectile.netUpdate = true;
            }
            if (Projectile.timeLeft < 2)
            {
                if (Vector == null)
                {
                    Vector = new Vector2[Projectile.oldPos.Length];
                    for (int i = 0; i < Projectile.oldPos.Length; i++)
                    {
                        Vector[i] = Projectile.oldPos[i];
                    }
                }
                //int Proj = NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MagicElectric2>(), 0, 1, Projectile.owner, 0, 0);
                //Main.projectile[Proj].oldPos = Projectile.oldPos;
                Projectile.timeLeft = 10000;
            }
            if (Projectile.timeLeft > 1000)
            {
                Projectile.extraUpdates = 15;
                Projectile.damage = 0;
                Projectile.timeLeft = 10000;
                Projectile.velocity = Vector2.Zero;
                Projectile.scale -= 0.002F;
                if (Projectile.ai[0] == 0)
                {
                    Projectile.scale -= 0.002F;
                }
                if (Projectile.scale <= 0)
                {
                    Projectile.Kill();
                }
            }
            else
            {
                if (Vector2.Subtract(Projectile.Player().Center - new Vector2(0, 70), Projectile.Center).Length() < 20 && Projectile.hostile)
                {
                    Projectile.Player().AddBuff(ModContent.BuffType<TerraPower2>(), 900);
                    float A = 0.5f;

                    int damage = Projectile.Player().statLifeMax2 / 10;
                    player.FixedDamage(damage, PlayerDeathReason.ByProjectile(Projectile.owner, Projectile.whoAmI));
                    Projectile.timeLeft = 2;
                    Projectile.netUpdate = true;
                    if (Main.netMode != 2)
                    {
                        Texture2D texture = textureLogo.Value;
                        if(colors == null)
                        {
                            colors = DDHelper.GetColors(textureLogo.Value);
                        }
                        for (int i = 0; i < colors.Length; i++)
                        {
                            float x = i % texture.Width;
                            float y = i / texture.Width;
                            if (colors[i] != new Color(0, 0, 0, 0))
                            {
                                Dust dust = Main.dust[NewDust(Projectile.Player().Center + (new Vector2(x, y) - new Vector2(texture.Width * 0.6F, texture.Height)), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, 0, new Color(71, 233, 60, 0), 1f)];
                                dust.customData = 0.75F;
                                dust.noGravity = true;
                                dust.velocity = dust.position - Projectile.Player().Center;
                                dust.velocity /= 5;
                            }
                        }
                    }
                    foreach(Projectile projectile in Main.projectile)
                    {
                        if (projectile.type == ModContent.ProjectileType<泰拉之锋Proj>() && projectile.owner == Projectile.owner && projectile.DProj().Bool[1])
                        {
                            projectile.Kill();
                        }
                    }
                    Main.LocalPlayer.Dplayer().PlayerShake(3, 10); 
                    SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
                    sound.Pitch = -0.1f;
                    PlaySound(sound, Projectile.position);
                }
            }
        }
        public static Color[] colors;
        public static Asset<Texture2D> textureLogo;
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.netUpdate = true;
            Main.LocalPlayer.Dplayer().PlayerShake(3, 10);
            SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
            sound.Pitch = -0.1f;
            PlaySound(sound, Projectile.position);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.LocalPlayer.Dplayer().PlayerShake(3, 10);
            SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
            sound.Pitch = -0.1f;
            PlaySound(sound, Projectile.position);
            Projectile.timeLeft = 2;
            Projectile.tileCollide = false;
            Projectile.netUpdate = true;
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, ModContent.DustType<光球粒子>(), newColor: new Color(71, 233, 60, 0))];
                dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(1, 8);
                dust.noGravity = false;
                dust.alpha = 100;
                dust.scale = 1.3f;
            }
            for (int i = 0; i < 6; i++)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, ModContent.DustType<星光粒子>(), newColor: new Color(141, 233, 130, 0))];
                dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(3, 24);
                dust.noGravity = false;
                dust.alpha = 100;
                dust.scale = 1.7f;
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = DDTextures.MiniVoidStar.Value;
            Vector2 vector = Projectile.Size / 2;
            if (Vector == null)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    if (Projectile.oldPos[i] != Projectile.position)
                    {
                        Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                        Main.spriteBatch.Draw(texture, vector2, null, new Color(61, 233, 40,0)*0.5f, Projectile.rotation, texture.Size() / 2, Projectile.scale / 6 + 0.15f + (float)i / 475, spriteEffects, 0f);
                        //Main.spriteBatch.Draw(texture, vector2, null, new Color(71, 233, 60, 0).Opposite() * 0.5F, Projectile.rotation, texture.Size() / 2, Projectile.scale / 10 + 0.025F + (float)i / 675 * 0.26f, spriteEffects, 0f);
                    }
                }
            }
            else
            {
                for (int i = 0; i < Vector.Length; i++)
                {
                    if (Vector[i] != Projectile.position)
                    {
                        Vector2 vector2 = Vector[i] + vector - Main.screenPosition;
                        Main.spriteBatch.Draw(texture, vector2, null, new Color(61, 233, 40, 0) * 0.5f, Projectile.rotation, texture.Size() / 2, Projectile.scale / 6 + 0.15f + (float)i / 475, spriteEffects, 0f);
                        //Main.spriteBatch.Draw(texture, vector2, null, new Color(71, 233, 60, 0).Opposite() * 0.5F, Projectile.rotation, texture.Size() / 2, Projectile.scale / 10 + 0.025F + (float)i / 675 * 0.26f, spriteEffects, 0f);
                    }
                }
            }
            return false;
        }
    }
}