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
		//Immune Time and received damage is only after PreHurt, so the Immune Time scripts must be here, in the PostHurt Method. (Using the Hurt Method will not work.)
		public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
		{
			if (guardTimer >= 1 && !hasShield && parryTimer == 0) //Guarding, but not Parrying and not Shield Guarding.
			{
				Player.immune = true;
				Player.immuneTime = BlockingConfig.Instance.blockingImmuneTime;
				guardingCooldown = BlockingConfig.Instance.guardingCooldown;
			}
			if (guardTimer >= 1 && hasShield && parryTimer == 0) //Shield Guarding, but not Parrying.
			{
				Player.immune = true;
				Player.immuneTime = BlockingConfig.Instance.blockingImmuneTime;
				guardingCooldown = BlockingConfig.Instance.guardingCooldown;
			}
		}
	}
}