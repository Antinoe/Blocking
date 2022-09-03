using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Audio;


namespace Blocking
{
    public partial class BlockingPlayer : ModPlayer
    {
		public override void ModifyScreenPosition()
		{
			if (screenShakeTimerGuarding > 0)
			{
				Main.screenPosition.X += (float)Main.rand.Next((int)(0f - 1), (int)1) * BlockingConfigClient.Instance.screenShakeAmountGuarding;
				Main.screenPosition.Y += (float)Main.rand.Next((int)(0f - 1), (int)1) * BlockingConfigClient.Instance.screenShakeAmountGuarding;
			}
			if (screenShakeTimerParryingAttempt > 0)
			{
				Main.screenPosition.X += (float)Main.rand.Next((int)(0f - 1), (int)1) * BlockingConfigClient.Instance.screenShakeAmountParryingAttempt;
				Main.screenPosition.Y += (float)Main.rand.Next((int)(0f - 1), (int)1) * BlockingConfigClient.Instance.screenShakeAmountParryingAttempt;
			}
			if (screenShakeTimerParrying > 0)
			{
				Main.screenPosition.X += (float)Main.rand.Next((int)(0f - 1), (int)1) * BlockingConfigClient.Instance.screenShakeAmountParrying;
				Main.screenPosition.Y += (float)Main.rand.Next((int)(0f - 1), (int)1) * BlockingConfigClient.Instance.screenShakeAmountParrying;
			}
		}
	}
}