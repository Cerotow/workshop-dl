using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Modkey;
using Terraria.ID;

namespace DDmod.Content.Projectiles.Talisman
{
    public class 药王葫芦Proj : Talismans
    {
        public override void SetStaticDefaults()
        {
        }
        public override void Load()
        {
        }
        public override void PreUse()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = MathHelper.Pi + MathHelper.PiOver4;
            if (Projectile.DProj().Bool[2])
            {
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4) - new Vector2(0, 16).RotatedBy(Projectile.rotation), 6, 1, ModContent.DustType<光球粒子>(), 0, 0, 0, new Color(255, 218, 78, 0))];
                GlobalDust.DustPlayerOwner[dust.dustIndex] = player.whoAmI;
                dust.noGravity = false;
                dust.scale = 0.8F;
                dust.velocity = new Vector2(0, 1).RotatedBy(Main.rand.NextFloat(-0.4F, 0.4F)) * Main.rand.NextFloat(1, 3);
                player.AddBuff(ModContent.BuffType<药王葫芦>(), 20);
            }
            else
            {
                Projectile.damage = player.Aplayer().LifeMax / 100;
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4) - new Vector2(0, 16).RotatedBy(Projectile.rotation), 6, 1, ModContent.DustType<光球粒子>(), 0, 0, 0, new Color(100, 255, 100, 0))];
                GlobalDust.DustPlayerOwner[dust.dustIndex] = player.whoAmI;
                dust.noGravity = false;
                dust.scale = 0.8F;
                dust.velocity = new Vector2(0, 1).RotatedBy(Main.rand.NextFloat(-0.4F, 0.4F)) * Main.rand.NextFloat(1, 3);
                Projectile.DProj().Times[0]++;
                if (Projectile.DProj().Times[0] % 30 == 0)
                {
                    bool Crit = Main.rand.Next(100) < Projectile.CritChance;
                    int r = Crit ? Projectile.damage * 2 : Projectile.damage;
                    if (player.whoAmI == Main.myPlayer)
                        player.Heal(r);
                    if (player.statMana > player.statManaMax2)
                        player.statMana = player.statManaMax2;
                }
                for (int a = 0; a < player.buffTime.Length; a++)
                {
                    if (!BuffID.Sets.NurseCannotRemoveDebuff[player.buffType[a]] && Main.debuff[player.buffType[a]])
                    {
                        player.buffTime[a] = 1;
                    }
                }
            }
            if (!Projectile.DProj().Bool[1])
            {
                Color color = new Color(100, 255, 100, 0);
                if (Projectile.DProj().Bool[2])
                {
                    color = new Color(255, 218, 78, 0);
                }
                NewDustChange(60, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0.1F, 2, Scale: Main.rand.NextFloat(0.6F, 1F),color: color);
                Projectile.Center = player.Center - new Vector2(-10, 46);
                NewDustChange(60, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0.1F, 2, Scale: Main.rand.NextFloat(0.6F, 1F), color: color);
                Projectile.DProj().Bool[1] = true;
                Projectile.netUpdate = true;
            }
            Projectile.Center = player.Center - new Vector2(-10, 46);
            Projectile.spriteDirection = 0;
            Projectile.velocity = Vector2.Zero;
        }
        public override void ExtraUse()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.DProj().Bool[1] = false;
            //True加攻击
            //false加血
            
            Projectile.DProj().Bool[2] = Projectile.Player().statLife > Projectile.Player().Aplayer().LifeMax / 2;
        }
        public override bool MobileAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.X * 0.05F;
            Projectile.spriteDirection = -player.direction;
            Projectile.DProj().Bool[1] = false;
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            SpriteEffects spriteEffects = 0;
            if (Projectile.spriteDirection == 1|| (Projectile.DProj().Bool[0]&&Projectile.velocity.X<0))
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color color = new Color(0, 155, 255, 0);
            Vector2 vector = Projectile.Size / 2;
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, 1, spriteEffects, 0f);
            if (Projectile.DProj().Bool[0])
            {
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(100,255,100,0), Projectile.rotation, texture.Size() / 2, 1, spriteEffects, 0f);
            }
                return false;
        }
    }
}