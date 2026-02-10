using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class 海幽浮法杖Proj : ModProjectile
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
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.hide = true;
            Projectile.DProj().Times[2] = 0;
        }
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.CritChance = player.GetWeaponCrit(player.ActiveItem());
            Projectile.damage = player.GetWeaponDamage(player.ActiveItem());
            Vector2 vector = player.RotatedRelativePoint(player.ArmCenter(), reverseRotation: false, addGfxOffY: false);
            if (player.controlUseItem)
            {
                player.Dplayer().Realm = 10;

                if(player.Dplayer().MouseWorld.X - vector.X>0)
                {
                    Projectile.DProj().vector[0] = new Vector2(1, 0);
                }
                else
                {
                    Projectile.DProj().vector[0] = new Vector2(-1, 0);
                }
                Projectile.netUpdate = true;
                Projectile.velocity = Projectile.DProj().vector[0].PerfectNormalize();

                Projectile.DProj().Times[1] = Projectile.DProj().vector[0].ToRotation();
                if (player.Dplayer().MouseWorld.X - vector.X > 0)
                {
                    Projectile.HoldProj(player, 12, Projectile.DProj().Times[1], new Vector2(1, 0), -MathHelper.PiOver4 + player.fullRotation, 0.25F, true);
                    Projectile.Center -= new Vector2(Projectile.DProj().Times[2] / 5, Projectile.DProj().Times[2]/2 + 16);
                }
                else
                {
                    Projectile.HoldProj(player, 12, Projectile.DProj().Times[1], new Vector2(1, 0), -MathHelper.PiOver4 + MathHelper.Pi + player.fullRotation, 0.25F, true);
                    Projectile.Center -= new Vector2(-Projectile.DProj().Times[2] / 5, Projectile.DProj().Times[2]/2+16);
                }
                //if (Projectile.ai[1] < 0.5)
                {
                    float v = 0;
                    if (player.direction == -1) v = 3.14f;
                    player.itemRotation = Projectile.velocity.ToRotation() * player.gravDir + v - player.fullRotation-MathHelper.PiOver4*(Projectile.DProj().Times[2]/15) * player.direction;
                }
                //player.heldProj = -1;
                if (Projectile.DProj().Times[2] >= 20)
                {
                    if (Projectile.ai[1] < 1)
                    {
                        Projectile.ai[1] += 0.05f;
                        Projectile.ai[0] = 0;
                    }
                    else if (Projectile.ai[0] > player.ActiveItem().useAnimation)
                    {
                        if (player.statMana >= player.ItemMana())
                        {
                            Projectile.ai[0] += player.GetTotalAttackSpeed(Projectile.DamageType);
                            int A = player.ItemMana();
                            player.statMana -= A;
                            Projectile.ai[0] -= player.ActiveItem().useAnimation;

                            if (Main.myPlayer == Projectile.owner)
                            {
                                for (int a = 0; a < Main.rand.Next(2, 5); a++)
                                {
                                    float SC = Main.rand.NextFloat(0.75F, 1.25F);
                                    int proj = NewProjectile(Projectile.GetSource_FromThis(), Projectile.position + new Vector2(Projectile.width / 2-8, -8).RotatedBy(player.fullRotation), new Vector2(player.ActiveItem().shootSpeed, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<魔法水母>(), (int)(Projectile.damage* SC), 1, Projectile.owner, SC);
                                    Main.projectile[proj].netUpdate = true;
                                }
                            }
                        }
                    }
                    if(player.ownedProjectileCounts[16] >3 || player.statMana < player.ItemMana())
                    {
                        if(Projectile.ai[0] > player.ActiveItem().useAnimation)
                        {
                            Projectile.ai[0] = player.ActiveItem().useAnimation;
                        }
                        if(player.statMana < player.ItemMana())
                        {
                            Projectile.ai[1] = 0;
                        }
                    }
                }
                else
                {
                    Projectile.DProj().Times[3]++;
                    if (Projectile.DProj().Times[3] > 20)
                    {
                        Projectile.DProj().Times[2] += 4;
                    }
                }
            }
            else
            {
                Projectile.Kill();
                player.itemTime = 0;
                player.itemAnimation = 0;
            }
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {

        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            if (player.direction == 1)
            {
                if (Projectile.ai[1] < 1)
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255,255,255,0) * 0.5f* Projectile.ai[1], Projectile.rotation, Glow.Size() / 2, Projectile.scale / 4, 0, 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255,255,255,0) * 0.5f * Projectile.ai[1], Projectile.rotation, Glow.Size() / 2, Projectile.scale / 4, 0, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255,255,255,0) * Projectile.ai[1], Projectile.rotation, Glow.Size() / 2, Projectile.scale / 4, 0, 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255,255,255,0) * Projectile.ai[1], Projectile.rotation, Glow.Size() / 2, Projectile.scale / 4, 0, 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                if (Projectile.ai[1] < 1)
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * 0.5f * Projectile.ai[1], Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.scale / 4, (SpriteEffects)(-1), 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0) * 0.5f * Projectile.ai[1], Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.scale / 4, (SpriteEffects)(-1), 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255,255,255,0) * Projectile.ai[1], Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.scale / 4, (SpriteEffects)(-1), 0f);
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255,255,255,0) * Projectile.ai[1], Projectile.rotation + MathHelper.Pi, Glow.Size() / 2, Projectile.scale / 4, (SpriteEffects)(-1), 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + MathHelper.Pi, texture.Size() / 2, Projectile.scale, (SpriteEffects)(-1), 0f);
            }
            Texture2D Starlight = DDTextures.Starlight.Value;
            return false;
        }

    }

}