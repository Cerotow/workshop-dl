using DDmod.Content.Buffs.PlayerBuffs;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class 绿岩侦察机 : Summons
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;

        }
        public override void SetDefault()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.minionSlots = 1;
            inertia = 10f;
            SearchRange = 1000;
            IgnoreTile = false;
            Minibuff = ModContent.BuffType<绿岩侦察机Buff>();
            Speed = 8;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.extraUpdates = 1;
            Projectile.GetGlobalProjectile<SummonProjectile>().ReboundSpeed = 0.5F;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override void Visual()
        {
            Projectile.rotation = Projectile.velocity.X * 0.03F;

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 12)
            {
                Projectile.frame++;
                Projectile.DProj().Times[1]++;
                Projectile.DProj().Times[2]++;
                Projectile.frameCounter = 0;
            }
            if (!target)
            {
                if (Projectile.frame == 14)
                {
                    Projectile.frame = 9;
                    Projectile.frameCounter = 0;
                }
            }
            else
            {
                if (Projectile.frame == 5)
                {
                    Projectile.frame = 0;
                    Projectile.frameCounter = 0;
                }
                if(npc.HasBuff(ModContent.BuffType<绿岩鞭Buff>()))
                {
                    Projectile.frame = 18;
                }
            }
            if (Projectile.frame >= 19)
            {
                Projectile.frame = 0;
                Projectile.frameCounter = 0;
            }
            if (Projectile.DProj().Times[1] >= 19)
            {
                Projectile.DProj().Times[1] = 0;
            }
            if (Projectile.DProj().Times[2] >= 5)
            {
                Projectile.DProj().Times[2] = 0;
            }
        }
        public override bool MobileAI()
        {
            if (Projectile.DProj().Times[0] > 0)
            {
                Projectile.DProj().Times[0]--;
            }
            if (target)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                float Sp = vector.Length() / 10;
                if (npc.HasBuff(ModContent.BuffType<绿岩鞭Buff>()))
                {
                    Sp -= 15;
                    if (Sp > Speed)
                    {
                        Sp = Speed;
                    }
                    Projectile.velocity = (Projectile.velocity * 50 + vector.PerfectNormalize() * Sp) / 51;
                    Projectile.ai[0]++;
                    if (Projectile.ai[0] % 60 == 0 && (npc.Center - Projectile.Center).Length() < 200)
                    {
                        if (npc != null)
                        {
                            int T = NewProjectile(player.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<绿岩能量>(), Projectile.damage, 0, -1, npc.whoAmI, 1);
                            Main.projectile[T].DamageType = DamageClass.Summon;
                            Main.projectile[T].tileCollide = false;
                            Main.projectile[T].DProj().NPCW = new List<byte>() { (byte)npc.whoAmI };
                            Projectile.netUpdate = true;
                        }
                    }

                }
                else
                {
                    if (Sp > Speed)
                    {
                        Sp = Speed;
                    }
                    Projectile.velocity = (Projectile.velocity * 50 + vector.PerfectNormalize() * Sp) / 51;
                }
                return false;
            }
            return true;
        }
        public override bool AttackAI()
        {
            return false;
        }

        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            AttackFrame = 60;
            Projectile.DProj().Times[0] = 60;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 Center = Projectile.Center - Main.screenPosition;
            SpriteEffects sprite = SpriteEffects.FlipHorizontally;
            if (target)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                if (vector.X < 0)
                {
                    sprite = 0;
                }
            }
            else
            {
                if (Projectile.velocity.X < 0)
                    sprite = 0;

            }

            Main.EntitySpriteDraw(texture, Center, new Rectangle(0, texture.Height / 19 * Projectile.frame, texture.Width/4, texture.Height / 19), lightColor, Projectile.rotation, new Vector2(texture.Width / 4, texture.Height / 19) / 2f - new Vector2(0, 10), Projectile.scale, sprite, 0f);
            Main.EntitySpriteDraw(texture, Center, new Rectangle(texture.Width / 4, texture.Height / 19 * Projectile.frame, texture.Width/4, texture.Height / 19), Color.White, Projectile.rotation, new Vector2(texture.Width / 4, texture.Height / 19) / 2f - new Vector2(0, 10), Projectile.scale, sprite, 0f);
            Main.EntitySpriteDraw(texture, Center, new Rectangle(texture.Width / 4*2, texture.Height / 19 * (int)Projectile.DProj().Times[1], texture.Width/4, texture.Height / 19), Color.White, Projectile.rotation, new Vector2(texture.Width / 4, texture.Height / 19) / 2f - new Vector2(0, 10), Projectile.scale, sprite, 0f);
            Main.EntitySpriteDraw(texture, Center, new Rectangle(texture.Width / 4*3, texture.Height / 19 * (int)Projectile.DProj().Times[2], texture.Width/4, texture.Height / 19), Color.White*0.5F, Projectile.rotation, new Vector2(texture.Width / 4, texture.Height / 19) / 2f - new Vector2(0, 10), Projectile.scale, sprite, 0f);

            return false;
        }
        internal Trailing TrailDrawer;
    }
}