using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using tModPorter;
using ArtificerMod.Content.Items.AccessoriesPH;

namespace ArtificerMod.Common
{
	public class ArtificerRecipesSystems : ModSystem
	{
		public override void AddRecipes()
		{
			// Introduces crafting recipes for vanilla accessories

			Recipe aglet = Recipe.Create(ItemID.Aglet);
			aglet.AddRecipeGroup("ArtificerMod:MetalBars1", 6);
			aglet.AddRecipeGroup("ArtificerMod:MetalBars4", 4);
			aglet.AddIngredient(ItemID.Gel, 5);
			aglet.AddTile(TileID.WorkBenches);
			aglet.Register();

			Recipe anklet = Recipe.Create(ItemID.AnkletoftheWind);
			anklet.AddIngredient(ItemID.Vine, 4);
			anklet.AddIngredient(ItemID.JungleSpores, 12);
			anklet.AddIngredient(ItemID.Cloud, 15);
			anklet.AddIngredient(ItemID.Gel, 10);
			anklet.AddTile(TileID.Loom);
			anklet.Register();

			Recipe cloudBottle = Recipe.Create(ItemID.CloudinaBottle);
			cloudBottle.AddIngredient(ItemID.Glass, 10);
			cloudBottle.AddIngredient(ItemID.Cloud, 45);
			cloudBottle.AddIngredient(ItemID.Feather, 5);
			cloudBottle.AddIngredient(ItemID.Gel, 5);
			cloudBottle.AddTile(TileID.SkyMill);
			cloudBottle.Register();

			Recipe hermesBoots = Recipe.Create(ItemID.HermesBoots);
			hermesBoots.AddIngredient(ItemID.Silk, 12);
			hermesBoots.AddIngredient(ItemID.TatteredCloth, 3);
			hermesBoots.AddIngredient(ItemID.SwiftnessPotion, 5);
			hermesBoots.AddIngredient(ItemID.FallenStar, 5);
			hermesBoots.AddTile(TileID.Loom);
			hermesBoots.Register();

			Recipe waterBoots = Recipe.Create(ItemID.WaterWalkingBoots);
			waterBoots.AddIngredient(ItemID.Silk, 12);
			waterBoots.AddIngredient(ItemID.TatteredCloth, 3);
			waterBoots.AddIngredient(ItemID.WaterWalkingPotion, 5);
			waterBoots.AddIngredient(ItemID.FallenStar, 5);
			waterBoots.AddTile(TileID.Loom);
			waterBoots.Register();

			Recipe horseshoe = Recipe.Create(ItemID.LuckyHorseshoe);
			horseshoe.AddRecipeGroup("ArtificerMod:MetalBars4", 10);
			horseshoe.AddIngredient(ItemID.SunplateBlock, 20);
			horseshoe.AddIngredient(ItemID.Cloud, 10);
			horseshoe.AddTile(TileID.Anvils);
			horseshoe.Register();

			Recipe lavaCharm = Recipe.Create(ItemID.LavaCharm);
			lavaCharm.AddIngredient(ItemID.Chain, 12);
			lavaCharm.AddIngredient(ItemID.HellstoneBar, 15);
			lavaCharm.AddIngredient(ItemID.LavaBucket);
			lavaCharm.AddTile(TileID.Hellforge);
			lavaCharm.AddCondition(Condition.NearLava);
			lavaCharm.AddDecraftCondition(Condition.DownedEowOrBoc);
			lavaCharm.Register();

			Recipe obsidianRose = Recipe.Create(ItemID.ObsidianRose);
			obsidianRose.AddRecipeGroup("ArtificerMod:Roses");
			obsidianRose.AddIngredient(ItemID.Obsidian, 30);
			obsidianRose.AddIngredient(ItemID.HellstoneBar, 8);
			obsidianRose.AddTile(TileID.Hellforge);
			obsidianRose.AddCondition(Condition.NearLava);
			obsidianRose.AddDecraftCondition(Condition.DownedEowOrBoc);
			obsidianRose.Register();

			Recipe depthMeter = Recipe.Create(ItemID.DepthMeter);
			depthMeter.AddRecipeGroup("ArtificerMod:MetalBars1", 10);
			depthMeter.AddRecipeGroup("ArtificerMod:MetalBars3", 8);
			depthMeter.AddRecipeGroup("ArtificerMod:MetalBars4", 6);
			depthMeter.AddTile(TileID.Tables);
			depthMeter.AddTile(TileID.Chairs);
			depthMeter.Register();

			Recipe compass = Recipe.Create(ItemID.Compass);
			compass.AddRecipeGroup("ArtificerMod:MetalBars2", 14);
			compass.AddIngredient(ItemID.MeteoriteBar, 10);
			compass.AddTile(TileID.Tables);
			compass.AddTile(TileID.Chairs);
			compass.AddDecraftCondition(Condition.DownedEowOrBoc);
			compass.Register();

			Recipe chisel = Recipe.Create(ItemID.AncientChisel);
			chisel.AddRecipeGroup("Wood", 30);
			chisel.AddIngredient(ItemID.FossilOre, 20);
			chisel.AddIngredient(ItemID.AntlionMandible, 10);
			chisel.AddTile(TileID.HeavyWorkBench);
			chisel.Register();

			Recipe sextant = Recipe.Create(ItemID.Sextant);
			sextant.AddRecipeGroup("ArtificerMod:MetalBars4", 12);
			sextant.AddIngredient(ItemID.SunplateBlock, 20);
			sextant.AddIngredient(ItemID.Coral, 5);
			sextant.AddTile(TileID.Anvils);
			sextant.Register();

			Recipe regenBand = Recipe.Create(ItemID.BandofRegeneration);
			regenBand.AddRecipeGroup("ArtificerMod:MetalBars4", 8);
			regenBand.AddRecipeGroup("ArtificerMod:MetalBars3", 12);
			regenBand.AddIngredient(ItemID.LifeCrystal);
			regenBand.AddIngredient(ItemID.FallenStar, 5);
			regenBand.AddTile(TileID.Anvils);
			regenBand.Register();

			Recipe jellyNecklace = Recipe.Create(ItemID.JellyfishNecklace);
			jellyNecklace.AddRecipeGroup("ArtificerMod:MetalBars3", 8);
			jellyNecklace.AddRecipeGroup("ArtificerMod:MetalBars4", 5);
			jellyNecklace.AddRecipeGroup("ArtificerMod:Glowsticks", 30);
			jellyNecklace.AddIngredient(ItemID.Coral, 10);
			jellyNecklace.AddTile(TileID.Anvils);
			jellyNecklace.Register();

			Recipe flowerBoots = Recipe.Create(ItemID.FlowerBoots);
			flowerBoots.AddIngredient(ItemID.Silk, 16);
			flowerBoots.AddIngredient(ItemID.TatteredCloth, 2);
			flowerBoots.AddIngredient(ItemID.JungleSpores, 10);
			flowerBoots.AddIngredient(ItemID.Vine, 2);
			flowerBoots.AddTile(TileID.Loom);
			flowerBoots.Register();

            Recipe fireBoots = Recipe.Create(ItemID.FlameWakerBoots);
            fireBoots.AddIngredient(ItemID.Silk, 16);
            fireBoots.AddIngredient(ItemID.TatteredCloth, 2);
            fireBoots.AddIngredient(ItemID.Hellstone, 15);
            fireBoots.AddTile(TileID.Loom);
			fireBoots.AddDecraftCondition(Condition.DownedEowOrBoc);
            fireBoots.Register();

            Recipe claws = Recipe.Create(ItemID.FeralClaws);
			claws.AddIngredient(ItemID.TatteredCloth, 6);
			claws.AddIngredient(ItemID.Vine, 4);
			claws.AddIngredient(ItemID.Stinger, 10);
			claws.AddTile(TileID.Loom);
			claws.Register();

			Recipe naturesGift = Recipe.Create(ItemID.NaturesGift);
			naturesGift.AddIngredient(ItemID.JungleRose);
			naturesGift.AddIngredient(ItemID.ManaCrystal);
			naturesGift.AddIngredient(ItemID.Vine, 3);
			naturesGift.AddIngredient(ItemID.JungleSpores, 15);
			naturesGift.AddCondition(Condition.InGraveyard);
			naturesGift.Register();

			Recipe turt = Recipe.Create(ItemID.FrozenTurtleShell);
			turt.AddIngredient(ItemID.TurtleShell);
			turt.AddIngredient(ItemID.ChlorophyteBar, 10);
			turt.AddRecipeGroup("ArtificerMod:Ice", 100);
			turt.AddTile(TileID.MythrilAnvil);
			turt.AddDecraftCondition(Condition.DownedMechBossAll);
			turt.Register();

			Recipe handWarmer = Recipe.Create(ItemID.HandWarmer);
			handWarmer.AddIngredient(ItemID.Silk, 12);
			handWarmer.AddIngredient(ItemID.FlinxFur, 6);
			handWarmer.AddIngredient(ItemID.WarmthPotion);
			handWarmer.AddTile(TileID.Loom);
			handWarmer.Register();

			Recipe honeycomb = Recipe.Create(ItemID.HoneyComb);
			honeycomb.AddIngredient(ItemID.BeeWax, 12);
			honeycomb.AddTile(TileID.TinkerersWorkbench);
			honeycomb.Register();

			Recipe divingHelm = Recipe.Create(ItemID.DivingHelmet);
			divingHelm.AddRecipeGroup("ArtificerMod:MetalBars1", 10);
			divingHelm.AddRecipeGroup("ArtificerMod:MetalBars2", 15);
			divingHelm.AddIngredient(ItemID.GillsPotion, 3);
			divingHelm.AddTile(TileID.Anvils);
			divingHelm.Register();

            Recipe musicBox = Recipe.Create(ItemID.MusicBox);
			musicBox.AddRecipeGroup("Wood", 10);
            musicBox.AddRecipeGroup("ArtificerMod:MetalBars3", 5);
            musicBox.AddRecipeGroup("ArtificerMod:MetalBars4", 5);
			musicBox.AddIngredient(ItemID.Diamond);
            musicBox.AddTile(TileID.WorkBenches);
            musicBox.Register();

			// Alt. bundle of balloons recipes
			Recipe balloons = Recipe.Create(ItemID.BundleofBalloons);
			balloons.AddIngredient<WeatherBottle>();
			balloons.AddIngredient(ItemID.ShinyRedBalloon, 3);
			balloons.AddTile(TileID.TinkerersWorkbench);
            balloons.Register();

			Recipe balloons2 = Recipe.Create(ItemID.HorseshoeBundle);
			balloons2.AddIngredient<WeatherBottle>();
			balloons2.AddIngredient(ItemID.ShinyRedBalloon, 3);
			balloons2.AddIngredient(ItemID.LuckyHorseshoe);
			balloons2.AddTile(TileID.TinkerersWorkbench);
            balloons2.Register();
        }

		public override void AddRecipeGroups()
		{
			RecipeGroup groupBars1 = new RecipeGroup(() => Language.GetTextValue("ItemName.CopperBar") + "/" + Language.GetTextValue("ItemName.TinBar"), new int[]
			{
		ItemID.CopperBar,
		ItemID.TinBar
			});
			RecipeGroup.RegisterGroup("ArtificerMod:MetalBars1", groupBars1);
			// TODO: Discontinue in favor of vanilla's iron/lead bar group
			RecipeGroup groupBars2 = new RecipeGroup(() => Language.GetTextValue("ItemName.IronBar") + "/" + Language.GetTextValue("ItemName.LeadBar"), new int[]
			{
		ItemID.IronBar,
		ItemID.LeadBar
			});
			RecipeGroup.RegisterGroup("ArtificerMod:MetalBars2", groupBars2);
			RecipeGroup groupBars3 = new RecipeGroup(() => Language.GetTextValue("ItemName.SilverBar") + "/" + Language.GetTextValue("ItemName.TungstenBar"), new int[]
			{
		ItemID.SilverBar,
		ItemID.TungstenBar
			});
			RecipeGroup.RegisterGroup("ArtificerMod:MetalBars3", groupBars3);
			RecipeGroup groupBars4 = new RecipeGroup(() => Language.GetTextValue("ItemName.GoldBar") + "/" + Language.GetTextValue("ItemName.PlatinumBar"), new int[]
			{
		ItemID.GoldBar,
		ItemID.PlatinumBar
			});
			RecipeGroup.RegisterGroup("ArtificerMod:MetalBars4", groupBars4);
			RecipeGroup groupBarsEvil = new RecipeGroup(() => Language.GetTextValue("ItemName.DemoniteBar") + "/" + Language.GetTextValue("ItemName.CrimtaneBar"), new int[]
			{
		ItemID.DemoniteBar,
		ItemID.CrimtaneBar
			});
			RecipeGroup.RegisterGroup("ArtificerMod:MetalBarsEvil", groupBarsEvil);
			RecipeGroup groupBarsHM3 = new RecipeGroup(() => Language.GetTextValue("ItemName.AdamantiteBar") + "/" + Language.GetTextValue("ItemName.TitaniumBar"), new int[]
			{
		ItemID.AdamantiteBar,
		ItemID.TitaniumBar
			});
			RecipeGroup.RegisterGroup("ArtificerMod:MetalBarsHM3", groupBarsHM3);

			RecipeGroup groupRoses = new RecipeGroup(() => Language.GetTextValue("ItemName.JungleRose") + "/" + Language.GetTextValue("ItemName.NaturesGift"), new int[]
			{
		ItemID.JungleRose,
		ItemID.NaturesGift
			});
			RecipeGroup.RegisterGroup("ArtificerMod:Roses", groupRoses);

			RecipeGroup iceBlocks = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Language.GetTextValue("ItemName.IceBlock"), new int[]
				{
		ItemID.IceBlock,
		ItemID.PinkIceBlock,
		ItemID.PurpleIceBlock,
		ItemID.RedIceBlock
				});
			RecipeGroup.RegisterGroup("ArtificerMod:Ice", iceBlocks);

			RecipeGroup glowsticks = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Language.GetTextValue("ItemName.Glowstick"), new int[]
				{
		ItemID.Glowstick,
		ItemID.StickyGlowstick,
		ItemID.BouncyGlowstick,
		ItemID.SpelunkerGlowstick,
		ItemID.FairyGlowstick
				});
			RecipeGroup.RegisterGroup("ArtificerMod:Glowsticks", glowsticks);

			RecipeGroup groupDmgPotions = new RecipeGroup(() => Language.GetTextValue("ItemName.WrathPotion") + "/" + Language.GetTextValue("ItemName.RagePotion"), new int[]
				{
		ItemID.WrathPotion,
		ItemID.RagePotion
				});
			RecipeGroup.RegisterGroup("ArtificerMod:DmgPotions", groupDmgPotions);

			RecipeGroup gems = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + Language.GetTextValue("Mods.ArtificerMod.CommonItemtooltip.GemsforRecipe"), new int[]
				{
		ItemID.Ruby,
		ItemID.Amber,
		ItemID.Topaz,
		ItemID.Emerald,
		ItemID.Sapphire,
		ItemID.Amethyst,
		ItemID.Diamond
				});
			RecipeGroup.RegisterGroup("ArtificerMod:Gems", gems);
		}
	}

    public class ArtificerTransmutations : GlobalItem
    {
        public override void SetStaticDefaults()
        {
            // Putrid Scent > < Flesh Knuckles
            ItemID.Sets.ShimmerTransformToItem[ItemID.PutridScent] = ItemID.FleshKnuckles;
            ItemID.Sets.ShimmerTransformToItem[ItemID.FleshKnuckles] = ItemID.PutridScent;

            // Shoe Spikes > < Climbing Claws
            ItemID.Sets.ShimmerTransformToItem[ItemID.ShoeSpikes] = ItemID.ClimbingClaws;
            ItemID.Sets.ShimmerTransformToItem[ItemID.ClimbingClaws] = ItemID.ShoeSpikes;

            // DPS Meter > Stopwatch > Lifeform Analyzer > DPS Meter
            ItemID.Sets.ShimmerTransformToItem[ItemID.DPSMeter] = ItemID.Stopwatch;
            ItemID.Sets.ShimmerTransformToItem[ItemID.Stopwatch] = ItemID.LifeformAnalyzer;
            ItemID.Sets.ShimmerTransformToItem[ItemID.LifeformAnalyzer] = ItemID.DPSMeter;
        }
    }
}