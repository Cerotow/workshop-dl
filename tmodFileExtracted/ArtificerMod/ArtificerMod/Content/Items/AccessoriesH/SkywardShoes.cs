using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.Localization;
using ArtificerMod.Common;
using Microsoft.Xna.Framework;
using System.Drawing.Drawing2D;
using ArtificerMod.Content.Items.AccessoriesPH;

namespace ArtificerMod.Content.Items.AccessoriesH
{
	[AutoloadEquip(EquipType.Shoes)]
	public class SkywardShoes : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.accessory = true;
			Item.rare = ItemRarityID.LightPurple;
			Item.value = Item.buyPrice(0, 20, 0, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.GetModPlayer<ArtificerPlayer>().bonusFlight += 0.25f;
            player.extraFall += 30;

            player.GetModPlayer<SkywardDashPlayer>().dashEquipped = true;
        }

		public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
		{
			if (equippedItem.type == ModContent.ItemType<PhasestrideBoots>())
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<DynamicBoots>()
				.AddIngredient<WingRing>()
                .AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}

    public class SkywardDashPlayer : ModPlayer
    {
        public const int dashUp = 0;
        public const int dashDown = 1;

        public int dashDir = -1;

        public bool dashEquipped;
        public int dashDelay = 0;

        public override void ResetEffects()
        {
            dashEquipped = false;

            if (dashDelay <= 0 && Player.controlUp && Player.releaseUp && Player.doubleTapCardinalTimer[1] < 15)
            {
                dashDir = dashUp;

            }
            else if (dashDelay <= 0 && Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[0] < 15)
            {
                dashDir = dashDown;

            }
            else
            {
                dashDir = -1;
            }
        }

        public override void PostUpdateEquips()
        {
            if (!dashEquipped)
            {
                return;
            }

            if (dashDelay > 45)
            {
                Player.maxFallSpeed *= 1.8f;
            }
            if (dashDelay > 25 && Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Cloud, Player.velocity.X, Player.velocity.Y, Scale: 1.2f);
                dust.velocity *= 0.5f;
            }
        }

        public override void PreUpdateMovement()
        {
            if (CanUseDash() && dashDir != -1 && dashDelay == 0)
            {
                VerticalDash();
                dashDelay = 60;
            }

            if (dashDelay > 0)
            {
                dashDelay--;
            }
                
        }

        public void VerticalDash()
        {
            if (Player != Main.LocalPlayer)
            {
                return;
            }

            float dashVelocity = 15f;
            Vector2 newVelocity = Player.velocity;

            switch (dashDir)
            {
                case dashUp when Player.velocity.Y > -dashVelocity:
                case dashDown when Player.velocity.Y < dashVelocity:
                    {
                        float dashDirection = dashDir == dashDown ? 1 : -1f;
                        newVelocity.Y = dashDirection * dashVelocity;
                        break;
                    }
                default:
                    return; 
            }
            
            dashDelay = 60;
            Player.velocity = newVelocity;

            // Dust FX
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Cloud, 0, newVelocity.Y, Scale: 1.4f);
                dust.velocity *= 0.5f;
            }

            // Expend wing time
            Player.wingTime -= 30;
            if(Player.wingTime < 0)
            {
                Player.wingTime = 0;
            }
        }

        private bool CanUseDash()
        {
            if(!dashEquipped || Player.mount.Active)
            {
                return false;
            }

            if(Player.wings == -1 || Player.wingTime <= 0)
            {
                return Player.velocity.Y == 0;
            }

            return true;
        }
    }
}
