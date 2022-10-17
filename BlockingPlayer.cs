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
		public int guardTimer = 0;
		public int parryTimer = 0;
		public bool hasShield = false;
		public bool hasGlove = false;
		public bool hasGloveBenefits = false;
		public bool hasBoot = false;
		public bool hasBootBenefits = false;
		public bool guardBashing = false;
		public int guardBashTimer = 0;
		public int guardBashCooldown = 0;
		public bool potentGuarding = true;
		public bool canPotentGuard;
		public int guardingCooldown = 0;
		public int potentGuardingCooldown = 0;
		public int parryCooldown = 0;
		public bool parryCounter = true;
		public int parryCounterCooldown = 0;
		public int screenShakeTimerGuarding = 0;
		public int screenShakeTimerParryingAttempt = 0;
		public int screenShakeTimerParrying = 0;
		
		//Resetting the Booleans is important, as some of them may not reset by themselves.
		public override void ResetEffects()
		{
			//guardTimer = 0; //Not resetting this anymore since the function stops working completely when doing so.
			hasShield = false;
			hasGlove = false;
			//hasGloveBenefits = false; //Resetting this nullifies the effect completely. :moyai:
			hasBoot = false;
			//hasBootBenefits = false; //Resetting this nullifies the effect completely. :moyai:
		}
		
		public static BlockingPlayer ModPlayer(Player Player)
		{
			return Player.GetModPlayer<BlockingPlayer>();
		}
		
		//Sounds.
		//Question: Why declare the audio like this?
		//Answer: A simple step in Code Optimization. If we were to declare this sound many times over in different places, it would get messy very quickly.
		public static readonly SoundStyle GuardRaise = new SoundStyle("Blocking/Sounds/GuardRaise");
		public static readonly SoundStyle GuardLower = new SoundStyle("Blocking/Sounds/GuardLower");
		public static readonly SoundStyle GuardBash = new SoundStyle("Blocking/Sounds/GuardBash");
		public static readonly SoundStyle ParryAttempt = new SoundStyle("Blocking/Sounds/ParryAttempt");
		public static readonly SoundStyle Parry = new SoundStyle("Blocking/Sounds/Parry");
		public static readonly SoundStyle Block = new SoundStyle("Blocking/Sounds/Block");
		public static readonly SoundStyle BlockShield = new SoundStyle("Blocking/Sounds/BlockShield");
		public static readonly SoundStyle BlockShieldBroken = new SoundStyle("Blocking/Sounds/BlockShieldBroken");
	}
}