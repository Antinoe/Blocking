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
		public override void PostUpdate() //Animations
		{
			if (guardTimer >= 1)
			{
				Player.bodyFrame.Y = Player.bodyFrame.Height * 10;
			}
			if (guardTimer >= 10)
			{
				Player.bodyFrame.Y = Player.bodyFrame.Height * 11;
			}
			if (parryTimer >= 1)
			{
				Player.bodyFrame.Y = Player.bodyFrame.Height * 1;
			}
			if (parryTimer >= 4)
			{
				Player.bodyFrame.Y = Player.bodyFrame.Height * 2;
			}
			if (parryTimer >= 7)
			{
				Player.bodyFrame.Y = Player.bodyFrame.Height * 3;
			}
			if (parryTimer >= 10)
			{
				Player.bodyFrame.Y = Player.bodyFrame.Height * 4;
			}
		}
	}
}