using DDmod.Content.Projectiles.Magic.Staff;
using DDmod.Content.Projectiles.Magic;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria;
using Terraria.Graphics.Shaders;
using DDmod.Content.Dusts;
using DDmod.Sync;
using DDmod.Players;
using static AssGen.Assets;
using System.Reflection;

namespace DDmod.Content.Projectiles.Summon.Minions.Strengthen
{
    public class 橡果灵灯Proj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.hide = true;
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
        public int type = -1;
        public int Itemtype = -1;
        public void AC(Player player)
        {
            if (type == -1)
            {
                type = player.selectedItem;
            }
            if (type != player.selectedItem)
            {
                Projectile.Kill();
            }
            if (Itemtype == -1)
            {
                Itemtype = player.ActiveItem().type;
            }
            if (Itemtype != player.ActiveItem().type)
            {
                Projectile.Kill();
            }
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Item item = player.ActiveItem();
            if(item.type<=0)
            {
                if(Main.myPlayer==Projectile.owner)
                Projectile.Kill();
                return false;
            }
            Projectile.CritChance = player.GetWeaponCrit(player.ActiveItem());
            Projectile.damage = player.GetWeaponDamage(player.ActiveItem());

            //player.ChangeDir(Projectile.direction);
            Vector2 vector = new Vector2(1 * player.direction, 0);
            if(Projectile.localAI[1]>0)
            {
                vector = vector.RotatedBy(-0.8F * player.direction);
            }
            player.PlayerAction().PlayerArmRotation(vector.ToRotation() - MathHelper.PiOver2 - player.fullRotation, Player.CompositeArmStretchAmount.Full);
            //player.PlayerAction().PlayerArmRotationBack((player.Dplayer().MouseWorld - player.ArmCenter()).ToRotation() * player.gravDir - MathHelper.PiOver2 - player.fullRotation, Player.CompositeArmStretchAmount.Full);

            Projectile.HoldProj(player, 14, 0, vector, 0, 0, true, (Projectile.localAI[1]>0|| Projectile.ai[1] >= item.MagicItem().charging) ?0:1, false);
            if(player.Dplayer().SummonEnergy>0)
            {
                Projectile.ai[0] += player.Dplayer().SummonEnergy;
                player.Dplayer().SummonEnergy = 0;
            }
            Projectile.position += Main.OffsetsPlayerHeadgear[player.bodyFrame.Y / 56];

            player.Dplayer().ProjAnimation = true;

            if (Projectile.ai[1] < item.MagicItem().charging)
            {
                if (Projectile.ai[0] > item.useAnimation)
                {
                    Projectile.ai[1]++;
                    Projectile.ai[0] -= item.useAnimation;
                    for (int A = 0; A < 10; A++)
                    {
                        int Type = ModContent.DustType<生命粒子>();
                        Dust dust = Main.dust[NewDust(Projectile.Center-new Vector2(4,-10), 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.scale = 1f;
                        dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(2, 4);
                    }
                    SoundStyle sound = SoundID.Item82;
                    sound.Pitch = 1;
                    PlaySound(sound,Projectile.Center);
                    inertia = 0.4F;
                }
            }
            else
            {
                Projectile.ai[0] = item.useAnimation;
            }
            Lighting.AddLight(Projectile.Center,0.3F,1,0.3F);
            if (player.controlUseTile)
            {
                NPC npc = NPCdirection.FindClosest(player.Dplayer().MouseWorld, 200, false);
                if (npc != null)
                    player.MinionAttackTargetNPC = npc.whoAmI;
            }
            if (player.controlUseItem && Projectile.localAI[1] == 0 && Projectile.ai[1] >= 1)
            {
                NPC npc = NPCdirection.FindClosest(player.Dplayer().MouseWorld, 200, false);
                if (player.HasMinionAttackTargetNPC)
                {
                    npc = Main.npc[player.MinionAttackTargetNPC];
                }
                if (npc != null)
                {
                    Projectile.ai[0] = 0;
                    Projectile.localAI[1] = 30;
                    Projectile.localAI[0] = npc.whoAmI;
                    inertia = 1F;
                }
            }
            player.AddBuff(ModContent.BuffType<AcornMarker>(), 5);
            //SGreenLaser
            if (Projectile.localAI[1] > 0)
            {
                if (Projectile.ai[1] >0&& Projectile.localAI[1] % 10 == 0)
                {
                    Projectile.ai[1]--;
                    if (Projectile.owner == Main.myPlayer)
                    {
                        int A= NewProjectileChange(Projectile.GetSource_FromAI(), Projectile.Center + new Vector2(0, 10), Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(2, 5), ModContent.ProjectileType<SummonNaturalLight>(), Projectile.damage, Projectile.knockBack, -1, Projectile.localAI[0] + 1);
                        Main.projectile[A].DamageType = DamageClass.Default;
                    }
                    SoundStyle sound = SoundID.Item82;
                    PlaySound(sound, Projectile.Center);
                    for (int A = 0; A < 20; A++)
                    {
                        int Type = ModContent.DustType<生命粒子>();
                        Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4, -10), 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.scale = 1f;
                        dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(2, 4);
                    }
                }
                Projectile.localAI[1]--;

                player.itemAnimation = (int)Projectile.localAI[1];
                player.itemTime = (int)Projectile.localAI[1];
            }
            else
            {
                player.itemAnimation = 0;
                player.itemTime = 0;

            }
            AC(player);

            DDHelper.BackAndForth(-inertia, inertia, inertia / 5, ref inertiaT, ref inertiaB, false);
            inertia *= 0.96f;
            Projectile.rotation = inertiaT;
            return false;
        }
        float lastPlayerSpeedX;
        public float inertia;
        public float inertiaT;
        public bool inertiaB;
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

            Item item = player.ActiveItem();
            if (item.type <= 0)
            {
                return false;
            }
            if (player.direction == 1)
            {
                for (int A = 0; A < Projectile.ai[1]; A++)
                {
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(18, 18), Projectile.scale, 0, 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(8, 8), Projectile.scale, 0, 0f);
            }
            else
            {
                for (int A = 0; A < Projectile.ai[1]; A++)
                    Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(18, 18), Projectile.scale, (SpriteEffects)1, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(8, 8), Projectile.scale, (SpriteEffects)1, 0f);
            }

            Color color = new Color(100, 255, 100);
            DDHelper.DrawExpanded(Main.GameViewMatrix.TransformationMatrix, Main.spriteBatch, player.Center - Main.screenPosition + new Vector2(0, -50), 0.5f, Projectile.ai[0] / item.useAnimation / 2, color * 0.3f, color, 1.1F, 0.6f);
            DynamicSpriteFontExtensionMethods.DrawString(
                Main.spriteBatch,
                FontAssets.MouseText.Value,
                (int)(Projectile.ai[0] / item.useAnimation * 100) + "%(" + Projectile.ai[1] + ")",
                player.Center - Main.screenPosition - new Vector2(0, 38),
                color, 0f,
                ChatManager.GetStringSize(FontAssets.MouseText.Value, (int)(Projectile.ai[0] / item.useAnimation * 100) + "%(" + Projectile.ai[1] + ")", Vector2.One) / 2,
                0.75F, SpriteEffects.None, 0f);

            return false;
        }
        float TT;
        float TT2 = 1;

    }
}