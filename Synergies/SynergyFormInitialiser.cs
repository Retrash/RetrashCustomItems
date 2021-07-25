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

            AdvancedDualWieldSynergyProcessor advancedDualWieldSynergyProcessor3 = (PickupObjectDatabase.GetById(339) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            advancedDualWieldSynergyProcessor3.PartnerGunID = Leafblower.LeafblowerID;
            advancedDualWieldSynergyProcessor3.SynergyNameToCheck = "Dendrology";
            AdvancedDualWieldSynergyProcessor advancedDualWieldSynergyProcessor4 = (PickupObjectDatabase.GetById(Leafblower.LeafblowerID) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            advancedDualWieldSynergyProcessor4.PartnerGunID = 339;
            advancedDualWieldSynergyProcessor4.SynergyNameToCheck = "Dendrology";

            AdvancedHoveringGunSynergyProcessor PlagueDoctorSynergy = (PickupObjectDatabase.GetByEncounterName("Alchemical Gun") as Gun).gameObject.AddComponent<AdvancedHoveringGunSynergyProcessor>();
            PlagueDoctorSynergy.ConsumesTargetGunAmmo = false;
            PlagueDoctorSynergy.AimType = HoveringGunController.AimType.PLAYER_AIM;
            PlagueDoctorSynergy.PositionType = HoveringGunController.HoverPosition.CIRCULATE;
            PlagueDoctorSynergy.FireType = HoveringGunController.FireType.ON_FIRED_GUN;
            PlagueDoctorSynergy.UsesMultipleGuns = false;
            PlagueDoctorSynergy.TargetGunID = 207;
            PlagueDoctorSynergy.RequiredSynergy = "Plague Doctor";
            PlagueDoctorSynergy.FireCooldown = 0.3f;
            PlagueDoctorSynergy.FireDuration = 0.1f;
        }
	}
}
