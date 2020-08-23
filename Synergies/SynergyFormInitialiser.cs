using System;
using ItemAPI;

namespace Blunderbeast
{
	// Token: 0x02000095 RID: 149
	internal class SynergyFormInitialiser
	{
		// Token: 0x06000334 RID: 820 RVA: 0x0001E460 File Offset: 0x0001C660
		public static void AddSynergyForms()
		{
			AdvancedDualWieldSynergyProcessor advancedDualWieldSynergyProcessor = (PickupObjectDatabase.GetById(9) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
			advancedDualWieldSynergyProcessor.PartnerGunID = OrnatePistol.OrnatePistolID;
			advancedDualWieldSynergyProcessor.SynergyNameToCheck = "Antiquated";
			AdvancedDualWieldSynergyProcessor advancedDualWieldSynergyProcessor2 = (PickupObjectDatabase.GetById(OrnatePistol.OrnatePistolID) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
			advancedDualWieldSynergyProcessor2.PartnerGunID = 9;
			advancedDualWieldSynergyProcessor2.SynergyNameToCheck = "Antiquated";
		}
	}
}
