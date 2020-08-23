using System;
using UnityEngine;
using Dungeonator;


namespace Blunderbeast
{
    public class Smallbloodthirst : MonoBehaviour
    {
        public void Awake()
        {
            this.m_player = base.GetComponent<PlayerController>();
            SpeculativeRigidbody specRigidbody = this.m_player.specRigidbody;
            specRigidbody.OnPostRigidbodyMovement = (Action<SpeculativeRigidbody, Vector2, IntVector2>)Delegate.Combine(specRigidbody.OnPostRigidbodyMovement, new Action<SpeculativeRigidbody, Vector2, IntVector2>(this.HandlePostRigidbodyMovement));
            this.m_currentNumKillsRequired = GameManager.Instance.BloodthirstOptions.NumKillsForHealRequiredBase;
            this.m_currentNumKills = 0;
        }

        private void HandlePostRigidbodyMovement(SpeculativeRigidbody inSrb, Vector2 inVec2, IntVector2 inPixels)
        {
            if (!this.m_player || this.m_player.IsGhost || this.m_player.IsStealthed || Dungeon.IsGenerating || BraveTime.DeltaTime == 0f)
            {
                return;
            }

            if (this.m_player.HasPickupID(Vampirecloak.VampirecloakPickupID))
            {
                RedMatterParticleController redMatterController = GlobalSparksDoer.GetRedMatterController();
                BloodthirstSettings bloodthirstOptions = GameManager.Instance.BloodthirstOptions;
                float radius = 5f;
                float damagePerSecond = 10f;
                float SynergydamagePerSecond = 20f;
                float percentAffected = 1f;
                int gainPerHeal = 5;
                int maxRequired = 50;
                if (this.AuraAction == null)
                {
                    this.AuraAction = delegate (AIActor actor, float dist)
                    {
                        if (!actor || !actor.healthHaver)
                        {
                            return;
                        }
                        actor.HasBeenBloodthirstProcessed = true;
                        actor.CanBeBloodthirsted = (UnityEngine.Random.value < percentAffected);
                        if (actor.CanBeBloodthirsted && actor.sprite)
                        {
                            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(actor.sprite);
                            if (outlineMaterial != null)
                            {
                                outlineMaterial.SetColor("_OverrideColor", new Color(1f, 0f, 0f));
                            }
                        }

                        if (dist < radius && actor.CanBeBloodthirsted && !actor.IsGone)
                        {
                            float Synergydamage = SynergydamagePerSecond * BraveTime.DeltaTime;
                            float damage = damagePerSecond * BraveTime.DeltaTime;
                            bool isDead = actor.healthHaver.IsDead;
                            if (m_player.HasPickupID(285))
                            {
                                actor.healthHaver.ApplyDamage(Synergydamage, Vector2.zero, "Bloodthirst", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
                            }
                            if (!m_player.HasPickupID(285))
                            {
                                actor.healthHaver.ApplyDamage(damage, Vector2.zero, "Bloodthirst", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
                            }
                            if (!isDead && actor.healthHaver.IsDead)
                            {
                                this.m_currentNumKills++;
                                if (this.m_currentNumKills >= this.m_currentNumKillsRequired)
                                {
                                    this.m_currentNumKills = 0;
                                    if (this.m_player.healthHaver.GetCurrentHealthPercentage() < 1f)
                                    {
                                        this.m_player.healthHaver.ApplyHealing(0.5f);
                                        this.m_currentNumKillsRequired = Mathf.Min(maxRequired, this.m_currentNumKillsRequired + gainPerHeal);
                                        GameObject gameObject = BraveResources.Load<GameObject>("Global VFX/VFX_Healing_Sparkles_001", ".prefab");
                                        if (gameObject != null)
                                        {
                                            this.m_player.PlayEffectOnActor(gameObject, Vector3.zero, true, false, false);
                                        }
                                        AkSoundEngine.PostEvent("Play_OBJ_med_kit_01", this.gameObject);
                                    }
                                }
                            }
                            GlobalSparksDoer.DoRadialParticleBurst(3, actor.specRigidbody.HitboxPixelCollider.UnitBottomLeft, actor.specRigidbody.HitboxPixelCollider.UnitTopRight, 90f, 4f, 0f, null, null, null, GlobalSparksDoer.SparksType.RED_MATTER);
                        }
                    };
                }
                if (this.m_player != null && this.m_player.CurrentRoom != null)
                {
                    this.m_player.CurrentRoom.ApplyActionToNearbyEnemies(this.m_player.CenterPosition, 100f, this.AuraAction);
                }
                if (redMatterController)
                {
                    redMatterController.target.position = this.m_player.CenterPosition;
                    redMatterController.ProcessParticles();
                }
            }
        }



        private int m_currentNumKillsRequired;

        private int m_currentNumKills;

        private PlayerController m_player;

        private Action<AIActor, float> AuraAction;
    }
}

