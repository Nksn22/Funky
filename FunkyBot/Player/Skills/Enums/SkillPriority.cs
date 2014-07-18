﻿namespace FunkyBot.Skills
{
	 ///<summary>
	 ///Priority assigned to abilities
	 ///</summary>
	 public enum SkillPriority
	 {
			//None is used for non-cost reusable abilities.
			//Low is used for costing abilities.
			//High is used for buffs.

			None=0,
			Low=1,
			Medium=2,
			High=3,
	 }
}