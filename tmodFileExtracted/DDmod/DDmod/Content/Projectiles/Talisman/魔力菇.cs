using DDmod.Content.Dusts;
using DDmod.Modkey;
using Terraria.ID;

namespace DDmod.Content.Projectiles.Talisman
{
    public class 魔力菇Proj : Talismans
    {
        public override void SetStaticDefaults()
        {
        }
        public override void Set()
        {
            Projectile.hide = true;
        }
        public override void PreUse()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.DProj().Times[0]++;
            if (Projectile.DProj().Times[0] % 60 == 0)
            {
                bool Crit = Main.rand.Next(100) < Projectile.CritChance;
                int r = Crit ? Projectile.damage * 2 : Projectile.damage;
                player.statMana+=r;
                player.ManaEffect(r);
                if(player.statMana>player.statManaMax2)
                {
                    player.statMana = player.statManaMax2;
                }
                Projectile.netUpdate = true;
            }
            //粒子
            if (Main.rand.NextBool(5))
            {
                int Dust = NewDust(player.position + new Vector2(-10, player.height), player.width + 20, 1, ModContent.DustType<光球粒子>(),newColor: new Color(0,155,255));
                Main.dust[Dust].scale = 0.6f;
                Main.dust[Dust].velocity = new Vector2(0, -3);
                GlobalDust.DustPlayerOwner[Dust] = player.whoAmI;
            }
            Projectile.velocity = Vector2.Zero;
            Projectile.Center = player.MountedCenter - new Vector2(0, 14).RotatedBy(player.fullRotation);
            Projectile.rotation = player.fullRotation;
            Projectile.spriteDirection = 0;
        }
        public override bool MobileAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.X*0.03F;
            Projectile.spriteDirection = 0;
            //冷却好了粒子
            if (player.TPlayer().TalismanCD >= player.TPlayer().MaxTalismanCD)
            {
                //冷却好了粒子
                if (Main.rand.NextBool(4))
                {
                    Vector2 center = Projectile.Center;

                    Vector2 direction = Main.rand.NextVector2CircularEdge(Projectile.width, Projectile.height);
                    float distance = 0.3f + Main.rand.NextFloat() * 0.5f;
                    Vector2 velocity = new Vector2(0f, -Main.rand.NextFloat() * 0.3f - 1.5f);

                    Dust dust = NewDustPerfect(center + direction * distance, ModContent.DustType<光球粒子>(), velocity, 0, new Color(0, 155, 255), 0.5f);
                    dust.fadeIn = 1.1f;
                    dust.noGravity = true;
                    dust.noLight = true;
                    dust.alpha = 0;
                }
            }
            return base.MobileAI();
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            if (Projectile.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            if (Projectile.DProj().Bool[0])
            {
                Main.spriteBatch.Draw(texture, Projectile.Center + new Vector2(0, Projectile.Player().gfxOffY) - Main.screenPosition, new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2), Color.White, Projectile.rotation, new Vector2(texture.Width, texture.Height / 2) / 2, 1, spriteEffects, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height / 2), Color.White, Projectile.rotation, new Vector2(texture.Width, texture.Height / 2) / 2, 1, spriteEffects, 0f);
            }
            return false;
        }
    }
}