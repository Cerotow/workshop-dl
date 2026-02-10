
using DDmod.Content.NPCs.Boss.先祖咒魂;
using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Projectiles.Summon;
using DDmod.Content.Projectiles.Summon.Minions;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Items.Boss.先祖咒魂
{
    public class 被诅咒的替死玩偶 : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 46;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(0,0 , 0, 0);
            Item.rare =5;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<术士召唤术2>();
            Item.shootSpeed = 0;
            Item.consumable = false;
            Item.maxStack = 1;
        }
        public override bool CanUseItem(Player player)
        {
            for (int A = 0; A < 1000; A++)
            {
                if (Main.projectile[A].active && (Main.projectile[A].type == ModContent.ProjectileType<术士召唤术2>() || Main.projectile[A].type == ModContent.ProjectileType<术士召唤术>()))
                {
                    return false;
                }
            }
            return !AnyNPCs(ModContent.NPCType<真哥布林术士>())&& !AnyNPCs(ModContent.NPCType<NPCs.Boss.先祖咒魂.先祖咒魂>());
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            NewProjectile(player.GetSource_FromAI(),position,new Vector2(0,-12),Item.shoot,0,0,-1,100,50,30);
            type = 0;
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
        public override void AddRecipes()
        {
        }
    }
}
