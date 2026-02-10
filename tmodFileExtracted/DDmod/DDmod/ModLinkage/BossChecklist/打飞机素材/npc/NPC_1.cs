
namespace DDmod.ModLinkage.BossChecklist
{
	public class NPC_1 : BNPC
	{
		public NPC_1()
		{
			type = 1;
		}
		public override void SetDefaultData()
		{
			scale = 1;
			width = 24;
			height = 24;
			Hostile = true;
			lifeMax = 50;
			fraction = 15;
			Maxframe = 2;
		}
		public override void AI()
		{
			frameCounter++;
			if (frameCounter % 10 >= 5)
			{
				frame = 0;
			}
			else
			{
				frame = 1;
			}
			//rotating += 0.6F;
			rotating = velocity.ToRotation() + MathHelper.Pi;
		}
		public override void NPCHit(ref int damage)
		{
			for (int a = 0; a < 5; a++)
			{
				BDust.NewDust(position, new Vector2(Main.rand.NextFloat(1, 3)).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), 5, 1.3F, ChecklistHelper.流星歼灭者);
			}
		}
		public override bool PreDraw(SpriteBatch sb, Texture2D texture, Color color,Rectangle rect)
		{
			sb.Draw(texture, position+rect.TopLeft() + new Vector2(width/2, height/2), new Rectangle?(new Rectangle(0, texture.Height / Maxframe * frame, texture.Width, texture.Height / Maxframe)), color, rotating, new Vector2(texture.Width-12, texture.Height / Maxframe) / 2, scale, 0, 0);
			return false;
		}
		public override bool NPCKill()
		{
			for (int a = 0; a < 25; a++)
			{
				BDust.NewDust(position, new Vector2(Main.rand.NextFloat(1, 3)).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), 5, 1.3F, ChecklistHelper.流星歼灭者);
			}
			BGore.NewGore(position, new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-6, 0)), 1,1,0, ChecklistHelper.流星歼灭者);
			BGore.NewGore(position, new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-6, 0)), 2,1,0, ChecklistHelper.流星歼灭者);

			return true;
		}
	}
}