using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace Blocking
{
	public class Blocking : Mod
	{
		public static ModKeybind Guard;
		public static ModKeybind GuardBash;
		public static ModKeybind TogglePotentGuard;
		public static ModKeybind ToggleParryCounter;
		public static ModKeybind Parry;
		
        public override void Load()
        {
            Guard = KeybindLoader.RegisterKeybind(this, "Guard", "LeftAlt");
            GuardBash = KeybindLoader.RegisterKeybind(this, "Guard Bash", "Mouse1");
            TogglePotentGuard = KeybindLoader.RegisterKeybind(this, "Toggle Potent Guarding", "Mouse3");
            ToggleParryCounter = KeybindLoader.RegisterKeybind(this, "Toggle Parry Counter", "Mouse3");
            Parry = KeybindLoader.RegisterKeybind(this, "Parry", "LeftAlt");
        }
        
        public override void Unload()
        {
            Guard = null;
            GuardBash = null;
			TogglePotentGuard = null;
			ToggleParryCounter = null;
            Parry = null;
        }
    }
}
