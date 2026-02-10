namespace DDmod.Content.Items.Sundries
{
    public class NOCozyCabin : ModItem
    {
        public Asset<Texture2D> 不温馨木屋物块;
        public Asset<Texture2D> 不温馨木屋墙;
        public Asset<Texture2D> 不温馨木屋家具;
        public override void Load()
        {
            不温馨木屋物块 = ModContent.Request<Texture2D>("DDmod/Content/Items/Sundries/不温馨木屋物块");
            不温馨木屋墙 = ModContent.Request<Texture2D>("DDmod/Content/Items/Sundries/不温馨木屋墙");
            不温馨木屋家具 = ModContent.Request<Texture2D>("DDmod/Content/Items/Sundries/不温馨木屋家具");
        }
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
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.value = Item.buyPrice(0, 0, 10, 0);
            Item.rare = 3;
            Item.UseSound = SoundID.Item2;
            Item.autoReuse = true;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
        }
        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }
        public override bool? UseItem(Player player)
        {
            if (!Main.dedServ && Main.myPlayer == player.whoAmI && player.altFunctionUse == 0&&player.itemAnimation== Item.useAnimation-1)
            {
                不温馨木屋物块 = ModContent.Request<Texture2D>("DDmod/Content/Items/Sundries/不温馨木屋物块");
                不温馨木屋墙 = ModContent.Request<Texture2D>("DDmod/Content/Items/Sundries/不温馨木屋墙");
                不温馨木屋家具 = ModContent.Request<Texture2D>("DDmod/Content/Items/Sundries/不温馨木屋家具");
            }
            if (!Main.dedServ && Main.myPlayer == player.whoAmI && player.altFunctionUse == 0&&player.itemAnimation== Item.useAnimation-2)
            {
                Point point = new Point((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16 - 不温馨木屋物块.Height() + 1);
                PlaceBlock(不温馨木屋物块.Value, point, player);
                PlaceWall(不温馨木屋墙.Value, point, player);
                PlaceFurniture(不温馨木屋家具.Value, point, player);

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendTileSquare(player.whoAmI, point.X, point.Y, 不温馨木屋物块.Width(), 不温馨木屋物块.Height());
                }
                Item.stack--;
            }
            return false;
        }
        //放置物块
        public int ColorBlock(Color color)
        {
            if (color == new Color(191, 143, 101))
            {
                return 30;
            }
            if (color == new Color(127, 87, 53))
            {
                return 19;
            }
            return -1;
        }
        public void PlaceBlock(Texture2D texture, Point Position, Player player)
        {
            Color[] colors = DDHelper.GetColors(texture);
            for (int i = 0; i < colors.Length; i++)
            {
                int x = Position.X + i % texture.Width;
                int y = Position.Y + i / texture.Width;
                int Tile = ColorBlock(colors[i]);
                if (Tile >= 0)
                {
                    for (int a = 0; a < 20; a++)
                        player.PickTile(x, y, 35);
                    if (!Main.tile[x, y].HasTile)
                    {
                        WorldGen.PlaceTile(x, y, Tile, true, true);
                    }
                }
                else
                {
                    for (int a = 0; a < 20; a++)
                        player.PickTile(x, y, 35);
                }
            }
        }
        //放置墙
        public int ColorWall(Color color)
        {
            if (color == new Color(106, 77, 51))
            {
                return 4;
            }
            return -1;
        }
        public void PlaceWall(Texture2D texture, Point Position, Player player)
        {
            Color[] colors = DDHelper.GetColors(texture);
            for (int i = 0; i < colors.Length; i++)
            {
                int x = Position.X + i % texture.Width;
                int y = Position.Y + i / texture.Width;
                int Wall = ColorWall(colors[i]);
                if (Wall >= 0)
                {
                    if(Main.wallHouse[Main.tile[x,y].WallType])
                        WorldGen.KillWall(x, y);
                    WorldGen.PlaceWall(x, y, Wall, true);
                }
                else
                {
                    if (Main.wallHouse[Main.tile[x, y].WallType])
                        WorldGen.KillWall(x, y);
                }
            }
        }
        //放置家具
        public int ColorFurniture(Color color)
        {
            //火把
            if (color == new Color(255, 255, 0))
            {
                return 4;
            }
            //椅子朝左
            if (color == new Color(255, 0, 0))
            {
                return 15;
            }
            //椅子朝右
            if (color == new Color(155, 0, 0))
            {
                return 15;
            }
            //桌子
            if (color == new Color(0, 0, 255))
            {
                return 14;
            }
            //门
            if (color == new Color(191, 143, 111))
            {
                return 10;
            }
            //工作台
            if (color == new Color(0, 255, 255))
            {
                return 18;
            }
            return -1;
        }
        public void PlaceFurniture(Texture2D texture, Point Position, Player player)
        {
            Color[] colors = DDHelper.GetColors(texture);
            for (int i = 0; i < colors.Length; i++)
            {
                int x = Position.X + i % texture.Width;
                int y = Position.Y + i / texture.Width;
                int Furniture = ColorFurniture(colors[i]);
                if (!Main.tile[x, y].HasTile)
                {
                    if (Furniture >= 0)
                    {
                        if (colors[i] == new Color(255, 0, 0))
                        {
                            WorldGen.PlaceTile(x, y, Furniture, true, true);
                            Main.tile[x, y].TileFrameX += 16;
                            Main.tile[x, y - 1].TileFrameX += 16;
                        }
                        else
                        {
                            WorldGen.PlaceTile(x, y, Furniture, true, true);
                        }
                    }
                }
            }
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(9, 65).AddIngredient(ItemID.Torch, 2).AddTile(TileID.WorkBenches).Register();
        }
    }
}
