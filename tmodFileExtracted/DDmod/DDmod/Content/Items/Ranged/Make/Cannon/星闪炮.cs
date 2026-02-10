using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Projectiles.Ranged.Gun;
using Terraria;

namespace DDmod.Content.Items.Ranged.Make.Cannon
{
    public class 星闪炮 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 42;
            Item.useAnimation = 42;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 40, 0, 0);
            Item.rare = 10;
            SoundStyle sound = SoundID.Item14;
            sound.Pitch = -0.8F;
            Item.UseSound = sound;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<星旋闪电球>();
            Item.shootSpeed = 8f;
            Item.useAmmo = 929;
            DDSystem.Instance.DDEquipGlow.TryGetValue("星闪炮", out int GG);
            Item.glowMask = (short)GG;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 5, true));
        }
        public bool A;
        public override void HoldItemFrame(Player player)
        {
        }
        public override void UseItemFrame(Player player)
        {
            if (Main.netMode != 2)
            {
                Texture2D texture = TextureAssets.Item[Item.type].Value;
                if (texture == null)
                {
                    return;
                }
                float frame = (Main.itemAnimations[Item.type] == null) ? 1 : Main.itemAnimations[Item.type].FrameCount;
                player.itemLocation.X = player.position.X + player.width * 0.5f - (player.direction * 2);
                player.itemLocation.Y = player.MountedCenter.Y - player.height * 0.5f;

                player.itemLocation += new Vector2(0, 14);
                player.itemLocation -= new Vector2(0, 18).RotatedBy(player.itemRotation);
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            type = Item.shoot;
            position += velocity.PerfectNormalize() * 50 - new Vector2(0, 16).RotatedBy(player.itemRotation);
            if ((Main.MouseWorld - position).Length() < 100)
            {
                Vector2 vector = Main.MouseWorld;
                vector += (Main.MouseWorld - player.MountedCenter).PerfectNormalize() * 100;
                velocity = (vector - position).PerfectNormalize() * velocity.Length();
            }
            else
            {

                velocity = (Main.MouseWorld - position).PerfectNormalize() * velocity.Length();
            }

            velocity = velocity.RotatedBy(Item.GetGlobalItem<RangedGlobalItem>().Rota * (-player.direction));

            NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);

            player.velocity += -velocity.RotatedBy(Item.GetGlobalItem<RangedGlobalItem>().Rota * -player.direction) / 2;
            for (int a = 0; a < 40; a++)
            {
                int dust = NewDust(position - new Vector2(4), 1, 1, ModContent.DustType<激光粒子>(), 0, 0, 0, new Color(29, 255, 187, 0), 0.6F);
                Main.dust[dust].velocity = velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0, 20);
                Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                Main.dust[dust].customData = 1F;
                Main.dust[dust].noGravity = true;
            }
            for (int a = -2; a <= 2; a++)
            {
                NewProjectile(player.GetSource_FromThis(), position, velocity.RotatedBy(Main.rand.NextFloat(-0.2F, 0.2F)+0.2F* a) * 8, ModContent.ProjectileType<星旋闪电>(), damage / 4, knockback, -1, 0, 1F, Main.rand.Next(40, 80));
            }
            if (Item.GetGlobalItem<RangedGlobalItem>().Rota < 1.6F)
            {
                Item.GetGlobalItem<RangedGlobalItem>().Rota += 0.8F;
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, -14);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(3456, 18).AddTile(TileID.LunarCraftingStation).Register();
        }
    }
}