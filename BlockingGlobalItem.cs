using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
//	This is needed in order to use Blocking's player-specific fields.
using static Blocking.BlockingPlayer;
//using System.Reflection; //This is what allows EquipType to be referenced. But that's unneeded in this context, so I won't allow it to exist here.

namespace Blocking
{
    public class BlockingGlobalItem : GlobalItem
    {
		public override bool InstancePerEntity => true;

		public override bool AppliesToEntity(Item entity, bool lateInstantiation)
		{
			if (entity.shieldSlot != -1)
				return true;
			else if (entity.handOnSlot != -1)
				return true;
			else if (entity.shoeSlot != -1)
				return true;
			return false;
		}
		
		public override void ModifyTooltips(Item Item, List<TooltipLine> tooltips)
		{
			//Shield
			if (Item.shieldSlot > -1 && BlockingConfig.Instance.shieldLife > 0)
			{
				var line = new TooltipLine(Mod, "Blocking:Shield", "Shield Defense grants extra Max Life.");
				tooltips.Add(line);
			}
			//Glove
			if (Item.handOnSlot > -1 && BlockingConfig.Instance.gloveBenefitsParryTimer > 0 && BlockingConfig.Instance.enableGloveBenefits && BlockingConfig.Instance.enableParrying)
			{
				var line = new TooltipLine(Mod, "Blocking:Glove", "Increases Parry Time.");
				tooltips.Add(line);
			}
			if (Item.handOnSlot > -1 && BlockingConfig.Instance.gloveBenefitsBlockingPotency > 0 && BlockingConfig.Instance.enableGloveBenefits)
			{
				var line = new TooltipLine(Mod, "Blocking:Glove", "Increases the potency of Guarding.");
				tooltips.Add(line);
			}
			//Boots
			if (Item.shoeSlot > -1 && BlockingConfig.Instance.enableBootBenefits)
			{
				var line = new TooltipLine(Mod, "Blocking:Boot", "Reduces the Movement penalty of Guarding.");
				tooltips.Add(line);
			}
		}
		
		public override void UpdateEquip(Item Item, Player Player)
		{
			var modPlayer = Player.GetModPlayer<BlockingPlayer>();
			if (!modPlayer.hasShield)
			{
				modPlayer.hasShield = Item.shieldSlot > -1;
			}
			if (!modPlayer.hasGlove)
			{
				modPlayer.hasGlove = Item.handOnSlot > -1;
			}
			if (!modPlayer.hasBoot)
			{
				modPlayer.hasBoot = Item.shoeSlot > -1;
			}
			
			//Special Accessory
			if (BlockingConfig.Instance.ItemWhitelist.Contains(new ItemDefinition(Item.type)))
			{
				//Item.defense = 2;
			}

			//Shield Life
			if (Item.shieldSlot > -1 && Item.defense > 0)
			{
				Player.statLifeMax2 += Item.defense * BlockingConfig.Instance.shieldLife;
			}
			//Shield Weight
			if (Item.shieldSlot > -1 && Item.defense > 0)
			{
				//Player.velocity.X *= BlockingConfig.Instance.shieldWeight - Item.defense + 0.995f; //Temporary calculation. I will attempt to fix this soon.
			}
		}
		public override void ModifyHitNPC(Item item, Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			//	This is *very* necessary for calling upon fields from other classes.
			BlockingPlayer bp = player.GetModPlayer<BlockingPlayer>();
			//	The game can't seem to detect the BlockingPlayer hitting an NPC, thus the Counter Damage mechanic doesn't work.
			//	I've grown tired of working on this for an hour, so I'm just going to push these changes to Github and work on other projects for now.
			if (bp.counterDamage > 0)
			{
				damage += bp.counterDamage;
				bp.counterDamage -= damage;
				Main.NewText("Counter Damage spent.");
				SoundEngine.PlaySound(BlockShield with {Pitch = +0.75f, Volume = 1f}, target.position);
			}
		}
    }
}