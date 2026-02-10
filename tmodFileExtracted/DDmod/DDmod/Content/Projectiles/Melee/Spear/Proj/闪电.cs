using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.NoContent.Config;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Spear.Proj
{
    public class 闪电 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10000;
            int Length = 375;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = Length;
            Projectile.extraUpdates = Length / 20;

        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            V2.Clear();
            int count = reader.ReadInt32();
            for (int A = 0; A < count; A++)
            {
                V2.Add(reader.ReadVector2());

            }
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(V2.Count);
            for(int A=0;A< V2.Count;A++)
            {
                writer.WriteVector2(V2[A]);

            }
        }
        public Vector2[] Vector;
        public List<Vector2> V2 = new List<Vector2>();
        public override void AI()
        {
            if (V2 == null)
            {
                V2 = new List<Vector2>();
            }
            if (Projectile.ai[2] != 0)
            {
                Projectile.timeLeft = (int)Projectile.ai[2];
                Projectile.localAI[2] = Projectile.ai[2];
                Projectile.ai[2] = 0;
            }
            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.DProj().vector[0] = Projectile.velocity.PerfectNormalize() * 3;
                Projectile.velocity = Projectile.DProj().vector[0];
            }
            if (Projectile.timeLeft < 2)
            {
                Projectile.timeLeft = 10000;
            }
            else
            if (Projectile.timeLeft > 1000)
            {
                Projectile.extraUpdates = 0;
                Projectile.damage = 0;
                Projectile.timeLeft = 10000;
                Projectile.velocity = Vector2.Zero;
                Projectile.scale -= 0.04F;
                if (Projectile.scale <= 0)
                {
                    Projectile.Kill();
                }
            }
            else
            {
                Projectile.scale = Projectile.ai[1];
                V2.Add(Projectile.position);
                Projectile.DProj().Times[0]++;
                if (Projectile.DProj().Times[0] > 20 && Main.rand.NextBool(10) && Projectile.DProj().track > 3)
                {
                    Projectile.DProj().Times[0] = 0;
                    NPC npc = NPCdirection.FindClosest(Projectile.Center, 500, false);
                    //NPC npc = null;
                    if (npc != null && npc.CanBeChasedBy(Projectile, false))
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
                        Vector2 vector1 = Utils.RotatedBy(Projectile.DProj().vector[0].PerfectNormalize() * Projectile.velocity.Length(), Main.rand.NextFloat(-0.6F, 0.6F), default);
                        Projectile.velocity = vector1;
                    }
                    Projectile.netUpdate = true;
                }
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] != 1)
            {
                

                target.AddBuff(ModContent.BuffType<Charged2>(), 30);
                Projectile.timeLeft = 2;
                for (int i = 0; i < 40; i++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4, -14), 1, 1, 226)];
                    dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(1, 4);
                    dust.noGravity = false;
                    dust.alpha = 100;
                    dust.scale = Projectile.ai[1];
                }
                if (Projectile.ai[0] <= 0 && (Projectile.Center - Main.LocalPlayer.Center).Length() < 300)
                {
                    Main.LocalPlayer.Dplayer().PlayerShake(3, 10 * Projectile.scale * (1 - (Projectile.Center - Main.LocalPlayer.Center).Length() / 300));
                }
                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
                sound.Pitch = -0.1f;
                sound.Volume = .1f;
                PlaySound(sound, Projectile.position);
            }
            else
            {
                Projectile.timeLeft = 2;
                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
                sound.Volume = 0.3f;
                sound.Pitch = 0.5f;
                PlaySound(sound, Projectile.position);
                int Type = ModContent.DustType<光球粒子>();
                for (int A = 0; A < 30; A++)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(2, 254, 201, 0))];
                    dust.noGravity = true;
                    dust.scale = Main.rand.NextFloat(1F, 2.2F);
                    dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 6f);
                    dust.rotation = Projectile.rotation;
                    dust.customData = -3;
                }
            }
            Projectile.netUpdate = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[0] <= 0 && (Projectile.Center - Main.LocalPlayer.Center).Length()<300)
            {
                Main.LocalPlayer.Dplayer().PlayerShake(3, 10 * Projectile.scale * (1-(Projectile.Center - Main.LocalPlayer.Center).Length()/300));
            }
            if (Projectile.ai[0] != 1)
            {
                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
                sound.Pitch = -0.1f;
                sound.Volume = .1f;
                PlaySound(sound, Projectile.position);
                if (Projectile.timeLeft > 2 && Projectile.timeLeft < 1000)
                    Projectile.timeLeft = 2;
                Projectile.tileCollide = false;
                Projectile.netUpdate = true;
                for (int i = 0; i < 40; i++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4, -14), 1, 1, 226)];
                    dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(1, 4);
                    dust.noGravity = false;
                    dust.alpha = 100;
                    dust.scale = Projectile.ai[1];
                }
            }
            else
            {
                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
                sound.Volume = 0.3f;
                sound.Pitch = 0.5f;
                PlaySound(sound, Projectile.position);
                if (Projectile.timeLeft > 2 && Projectile.timeLeft < 1000)
                    Projectile.timeLeft = 2;
                int Type = ModContent.DustType<光球粒子>();
                for (int A = 0; A < 30; A++)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(2, 254, 201, 0))];
                    dust.noGravity = true;
                    dust.scale = Main.rand.NextFloat(1F, 2.2F);
                    dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 6f);
                    dust.rotation = Projectile.rotation;
                    dust.customData = -3;
                }
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
            Color color = new Color(0, 186, 242, 0);
            if(Projectile.ai[0] != 1)
            {
                color = new Color(2, 254, 201, 0);
            }
            Vector2 vector = Projectile.Size / 2;
            if (V2 == null)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    if (Projectile.oldPos[i] != Projectile.position)
                    {
                        Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                        Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale / 6 + 0.15f + (float)i / 375 / 4, spriteEffects, 0f);
                        Main.spriteBatch.Draw(texture, vector2, null, color.Opposite(), Projectile.rotation, texture.Size() / 2, Projectile.scale / 10 + 0.025F + (float)i / 375 / 4 * 0.6f, spriteEffects, 0f);
                    }
                }
            }
            else
            {
                float ro = 0;
                for (int i = 0; i < V2.Count; i++)
                {
                    float sc = Projectile.scale * (1F - i / Projectile.localAI[2]);
                    bool flag = false;
                    if (i > 0)
                    {
                        float r2 = (V2[i] - V2[i - 1]).ToRotation();
                        if (ro != r2)
                        {
                            ro = r2;
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                    if (V2[i] != Projectile.position)
                    {
                        Vector2 vector2 = V2[i] + vector - Main.screenPosition;
                        Main.spriteBatch.Draw(texture, vector2, null, color, ro, texture.Size() / 2, new Vector2(sc >= 0.2F ? sc : 0.2F, Projectile.scale * (1F - i / Projectile.localAI[2])) / 2, spriteEffects, 0f);
                        Main.spriteBatch.Draw(texture, vector2, null, color.Opposite(), ro, texture.Size() / 2, new Vector2(sc >= 0.2F ? sc : 0.2F, Projectile.scale * (1F - i / Projectile.localAI[2]) / 2) / 2, spriteEffects, 0f);
                    }
                }
            }
            return false;
        }
    }
}