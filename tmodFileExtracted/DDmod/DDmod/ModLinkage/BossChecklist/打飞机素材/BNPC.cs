
namespace DDmod.ModLinkage.BossChecklist
{
    public class BNPC
	{
		public Vector2 velocity;
		public Vector2 position;
		public int width;
		public int height;
		public int type;
		public float rotating;
		public float HitTime;
		public float scale;
		public int Maxframe = 1;
		public int frame;
		public int Bosstype;
		public float frameCounter;
		/// <summary>
		/// 是不是敌对
		/// </summary>
		public bool Hostile;
		public bool active;
		public int life;
		public int lifeMax;
		/// <summary>
		/// 分数
		/// </summary>
		public int fraction;
		public Vector2 Centre => position + new Vector2(width, height) / 2;
		public Rectangle Rectangle => new Rectangle((int)position.X, (int)position.Y, width, height);

		public Asset<Texture2D>[] NPCTexture = new Asset<Texture2D>[100];
		public virtual void SetDefaultData()
        {

        }
		public void SetDefault(int type)
		{
			this.type = type;
			if (type != 0)
				NPCTexture[type] = ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/打飞机素材/npc/NPC_" + type);

			SetDefaultData();
			width = (int)(width * scale);
			height = (int)(height * scale);
			life = lifeMax;
		}
		public virtual void AI()
		{ 
		}
		public void UpdateNPC(int NPC)
		{
			if (!active || type == 0)
			{
				return;
			}
			AI();
			position += velocity;
			if (life <= 0 && active)
			{
				Kill();
			}
		}
		public virtual void NPCHit(ref int damage)
        {

        }
		public void Hit(int damage)
		{
			NPCHit(ref damage);
			life -= damage;
			HitTime = 10;
		}
		public virtual bool PreDraw(SpriteBatch sb,Texture2D texture,Color color,Rectangle rect)
        {
			return true;
        }
		public void Dawn(SpriteBatch sb,Rectangle rect,int Type)
		{
			if (!active || type == 0||Type != Bosstype)
			{
				return;
            }
            Texture2D texture = NPCTexture[type].Value;
			Color color = Color.Wheat;
			if (HitTime > 0)
			{
				HitTime--;
				color = Color.Red;
			}
			if (PreDraw(sb,texture, color,rect))
			{
				sb.Draw(texture, Centre+rect.TopLeft(), new Rectangle?(new Rectangle(0, texture.Height / Maxframe * frame, texture.Width, texture.Height / Maxframe)), color, rotating, new Vector2(texture.Width, texture.Height / Maxframe) / 2, scale, 0, 0);
            }
        }
		public virtual bool NPCKill()
        {
			return true;
        }
		public void Kill()
		{
			if (NPCKill())
			{
				Main.LocalPlayer.GetModPlayer<MeteorAnnihilatorPlayer>().fraction += fraction;
				active = false;
			}
		}
		public static void NewNPC(Vector2 position, Vector2 velocity,BNPC npc,int Bosstype)
		{
			for (int k = 0; k < 200; k++)
			{
				if (!ChecklistHelper.NPC[k].active)
				{
					int type = npc.type;
					ChecklistHelper.NPC[k] = npc;
					ChecklistHelper.NPC[k].position = position;
					ChecklistHelper.NPC[k].velocity = velocity;
					ChecklistHelper.NPC[k].Bosstype= Bosstype;
					ChecklistHelper.NPC[k].SetDefault(type);
					ChecklistHelper.NPC[k].active = true;
					break;
				}
			}
		}
	}
}