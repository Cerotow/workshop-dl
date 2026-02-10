using DDmod.Modkey;

namespace DDmod.Content.Projectiles.Talisman
{
    public class LegendaryGel : Talismans
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
            //踩踏冷却
            if (Projectile.DProj().Times[0] > 0)
            {
                Projectile.DProj().Times[0]--;
            }
            //player.velocity.Y += 0.01f;
            //遍历npc
            for (int A = 0; A < 200; A++)
            {
                NPC npc = Main.npc[A];
                Rectangle npcRectangle = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.height, npc.height);
                Rectangle playerRectangle = new Rectangle((int)player.position.X, (int)player.position.Y + player.height, player.width, 4);
                if (npc.CanBeChasedBy(Projectile) && Projectile.DProj().Times[0] <= 0)
                {
                    //让玩家可以踩npc
                    if (playerRectangle.Intersects(npcRectangle))
                    {
                        player.immune = true;
                        player.immuneNoBlink = true;
                        player.immuneTime = 10;
                        player.velocity.Y = -8;
                        Projectile.DProj().Times[0] = 10;
                        if (Main.LocalPlayer == player)
                        {
                            npc.SimpleStrikeNPC((int)(Projectile.damage), player.direction, Main.rand.Next(100) < Projectile.CritChance, 1);
                        }
                    }
                }
            }
            //粒子
            if (Main.rand.NextBool(2))
            {
                Vector2 center = Projectile.Center;

                Vector2 direction = Main.rand.NextVector2CircularEdge(Projectile.width, Projectile.height);
                float distance = 0.3f + Main.rand.NextFloat() * 0.5f;
                Vector2 velocity = new Vector2(0f, Main.rand.NextFloat() * 4f + 1.5f);

                Dust dust = NewDustPerfect(center + direction * distance, DustID.SilverFlame, velocity.RotatedBy(Main.rand.NextFloat(-0.3F, 0.3F)));
                dust.scale = 0.5f;
                dust.fadeIn = 1.1f;
                dust.noGravity = true;
                dust.noLight = true;
                dust.alpha = 0;
                dust.color = new Color(0, 150, 200);
            }
            Projectile.Center = player.MountedCenter - new Vector2(0, player.height/2);
            Projectile.rotation = 0;
            Projectile.Player().AddBuff(BuffID.Slimed, 2, false, true);
        }
        public override bool MobileAI()
        {
            Player player = Main.player[Projectile.owner];
            //冷却好了粒子
            if (player.TPlayer().TalismanCD >= player.TPlayer().MaxTalismanCD && Main.rand.NextBool(10))
            {
                Vector2 center = Projectile.Center;

                Vector2 direction = Main.rand.NextVector2CircularEdge(Projectile.width, Projectile.height);
                float distance = 0.3f + Main.rand.NextFloat() * 0.5f;
                Vector2 velocity = new Vector2(0f, -Main.rand.NextFloat() * 0.3f - 1.5f);

                Dust dust = NewDustPerfect(center + direction * distance, DustID.SilverFlame, velocity);
                dust.scale = 0.5f;
                dust.fadeIn = 1.1f;
                dust.noGravity = true;
                dust.noLight = true;
                dust.alpha = 0;
                dust.color = new Color(0, 150, 200);
            }
            return base.MobileAI();
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
    }
}