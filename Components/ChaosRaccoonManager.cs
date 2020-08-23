using Dungeonator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blunderbeast
{

    public class ChaosRaccoonManager : OverrideBehaviorBase
    {

        public ChaosRaccoonManager()
        {
            MunchVFX = (GameObject)ResourceCache.Acquire("Global VFX/Spinfall_Poof_VFX");
            MunchAnimName = "carry";
            // ExcludedRooms = new List<string> { "gungeon_notmal_trap_flametrap4ways" };
            m_repathTimer = 0.25f;
            m_pathingTimeoutTimer = 0f;
            m_pathingTimeoutTimer2 = 0f;
        }

        public GameObject MunchVFX;
        public string MunchAnimName;

        [NonSerialized]
        private float m_repathTimer;
        private float m_pathingTimeoutTimer;
        private float m_pathingTimeoutTimer2;
        private float m_cachedSpeed;

        private DebrisObject m_targetJunk;

        public override void Start()
        {
            base.Start();
            m_cachedSpeed = m_aiActor.MovementSpeed;
            if (this.m_aiActor.CompanionOwner != null)
            {
                this.m_aiActor.CompanionOwner.OnRoomClearEvent += this.RoomCleared;
            }
        }

        public override void Destroy()
        {
            this.m_aiActor.CompanionOwner.OnRoomClearEvent -= this.RoomCleared;
            base.Destroy();
        }

        private void RoomCleared(PlayerController obj)
        {
            float value = UnityEngine.Random.Range(0.0f, 1.0f);
            bool flag = value < 0.001;
            if (flag)
            {
                PickupObject byId = PickupObjectDatabase.GetById(641);
                LootEngine.SpawnItem(byId.gameObject, obj.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
            }
            else
            {
                bool flag2 = value < 0.002;
                if (flag2)
                {
                    PickupObject byId = PickupObjectDatabase.GetById(580);
                    LootEngine.SpawnItem(byId.gameObject, obj.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
                }
                else
                {
                    bool flag3 = value < 0.103;
                    if (flag3)
                    {
                        PickupObject byId = PickupObjectDatabase.GetById(127);
                        LootEngine.SpawnItem(byId.gameObject, obj.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
                    }
                    else { return; }
                }
            }
        }

        public override void Upkeep()
        {
            DecrementTimer(ref m_repathTimer, false);
            base.Upkeep();
        }

        public override BehaviorResult Update()
        {
            if (!m_targetJunk)
            {
                m_targetJunk = null;
                if (m_repathTimer <= 0f)
                {
                    m_repathTimer = 1f;
                    if (IsJunkInATrapRoom()) { return BehaviorResult.SkipAllRemainingBehaviors; }
                }
                return base.Update();
            }
            m_aiActor.PathfindToPosition(m_targetJunk.sprite.WorldCenter, new Vector2?(m_targetJunk.sprite.WorldCenter), true, null, null, null, false);
            if (m_aiActor.Path != null && m_aiActor.Path.WillReachFinalGoal)
            {
                if (Vector2.Distance(m_targetJunk.sprite.WorldCenter, m_aiActor.CenterPosition) < 1.25f) { MunchJunk(m_targetJunk); }
                return BehaviorResult.SkipAllRemainingBehaviors;
            }
            else if (Vector2.Distance(m_targetJunk.sprite.WorldCenter, m_aiActor.CenterPosition) < 1.25f)
            {
                MunchJunk(m_targetJunk);
                return BehaviorResult.SkipAllRemainingBehaviors;
            }
            return base.Update();
        }

        private bool IsJunkInATrapRoom()
        {
            PlayerController companionOwner = m_aiActor.CompanionOwner;
            if (m_aiActor.GetAbsoluteParentRoom() == null) { return false; }
            if (!companionOwner || companionOwner.CurrentRoom == null) { return false; }
            if (companionOwner.CurrentRoom.area.PrototypeRoomNormalSubcategory != PrototypeDungeonRoom.RoomNormalSubCategory.TRAP) { return false; }
            // if (ExcludedRooms.Contains(companionOwner.CurrentRoom.GetRoomName().ToLower())) { return false; }
            if (companionOwner.CurrentRoom != m_aiActor.GetAbsoluteParentRoom()) { return false; }
            List<DebrisObject> componentsAbsoluteInRoom = companionOwner.CurrentRoom.GetComponentsAbsoluteInRoom<DebrisObject>();
            if (componentsAbsoluteInRoom == null | componentsAbsoluteInRoom.Count == 0) { return false; }
            for (int i = 0; i < componentsAbsoluteInRoom.Count; i++)
            {
                PickupObject JunkPickup = componentsAbsoluteInRoom[i].GetComponentInChildren<PickupObject>();
                if (JunkPickup)
                {
                    if (JunkPickup.PickupObjectId != 127 && JunkPickup.PickupObjectId != 148 &&
                        JunkPickup.PickupObjectId != 580 && JunkPickup.PickupObjectId != 641)
                    {
                        componentsAbsoluteInRoom.RemoveAt(i); i--;
                    }
                }
                else
                {
                    componentsAbsoluteInRoom.RemoveAt(i); i--;
                }
            }
            DebrisObject closestToPosition = BraveUtility.GetClosestToPosition(componentsAbsoluteInRoom, m_aiActor.CenterPosition, new DebrisObject[0]);
            if (closestToPosition != null) { m_targetJunk = closestToPosition; return true; }
            return false;
        }

        private void MunchJunk(DebrisObject targetJunk)
        {
            if (targetJunk != null)
            {
                if (targetJunk.transform != null && targetJunk.transform.position != null)
                {
                    GameObject eatPoof = UnityEngine.Object.Instantiate(MunchVFX);
                    if (MunchVFX && eatPoof)
                    {
                        eatPoof.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(targetJunk.transform.position + new Vector3(0.5f, 0.5f), tk2dBaseSprite.Anchor.LowerCenter);
                        eatPoof.transform.position = eatPoof.transform.position.Quantize(0.0625f);
                        eatPoof.GetComponent<tk2dBaseSprite>().UpdateZDepth();
                    }
                }
                m_aiAnimator.PlayUntilCancelled(MunchAnimName, true, null, -1f, false);
                AkSoundEngine.PostEvent("Play_NPC_BabyDragun_Munch_01", m_aiActor.gameObject);
                UnityEngine.Object.Destroy(targetJunk.gameObject);
                m_targetJunk = null;
                ScoopPlayer();
                return;
            }
            else
            {
                m_targetJunk = null;
                m_aiActor.aiAnimator.PlayUntilCancelled("idle", false, null, -1f, false);
                return;
            }
        }

        public void ScoopPlayer()
        {
            if (m_aiActor.CompanionOwner == null)
            {
                return;
            }
            else
            {
                GameManager.Instance.StartCoroutine(ScoopPlayerToSafety());
            }
        }

        private IEnumerator ScoopPlayerToSafety()
        {
            PlayerController m_owner = m_aiActor.CompanionOwner;
            RoomHandler currentRoom = m_owner.CurrentRoom;
            if (currentRoom.area.PrototypeRoomNormalSubcategory != PrototypeDungeonRoom.RoomNormalSubCategory.TRAP)
            {
                m_aiActor.aiAnimator.PlayUntilCancelled("idle", false, null, -1f, false);
                yield break;
            }
            bool hasFoundExit = false;
            float maxDistance = float.MinValue;
            // This whole thing has been converted to Vector2 to avoid player being embeded into walls by failsafe teleport system (and to prevent Raccoon from doing this too)
            Vector2 mostDistantExit = new Vector2(-1, -1);
            for (int i = 0; i < currentRoom.connectedRooms.Count; i++)
            {
                PrototypeRoomExit exitConnectedToRoom = currentRoom.GetExitConnectedToRoom(currentRoom.connectedRooms[i]);
                if (exitConnectedToRoom != null)
                {
                    IntVector2 a = exitConnectedToRoom.GetExitAttachPoint() - IntVector2.One;
                    Vector2 exitVector = (a + currentRoom.area.basePosition + DungeonData.GetIntVector2FromDirection(exitConnectedToRoom.exitDirection)).ToVector2();
                    // These adjustements should in most cases result in the Raccoon dropping the player near the exit and not inside the exit which can cause issues sometimes.
                    if (exitConnectedToRoom.exitDirection == DungeonData.Direction.WEST) { exitVector += new Vector2(2, 0.8f); }
                    if (exitConnectedToRoom.exitDirection == DungeonData.Direction.EAST)
                    {
                        exitVector -= new Vector2(2.4f, 0);
                        exitVector += new Vector2(0, 0.8f);
                    }
                    if (exitConnectedToRoom.exitDirection == DungeonData.Direction.NORTH)
                    {
                        exitVector -= new Vector2(0, 2.4f);
                        exitVector += new Vector2(0.8f, 0);
                    }
                    if (exitConnectedToRoom.exitDirection == DungeonData.Direction.SOUTH) { exitVector += new Vector2(0.8f, 2.4f); }
                    hasFoundExit = true;
                    float num = Vector2.Distance(m_owner.CenterPosition, exitVector);
                    if (num > maxDistance) { maxDistance = num; mostDistantExit = exitVector; }
                }
            }
            yield return null;
            if (!hasFoundExit) { yield break; }
            // If Raccoon gets stuck or gives up trying to path to exit, this will be set to true and checked later.
            bool hasFailed = false;
            CompanionFollowPlayerBehavior followBehavior = m_aiActor.behaviorSpeculator.MovementBehaviors[0] as CompanionFollowPlayerBehavior;
            followBehavior.TemporarilyDisabled = true;
            m_aiActor.PathableTiles = (CellTypes.FLOOR | CellTypes.PIT);
            m_aiActor.ClearPath();
            m_owner.SetInputOverride("raccoon");
            m_owner.IsEthereal = true;
            m_owner.healthHaver.IsVulnerable = false;
            float cachedSpeed = m_aiActor.MovementSpeed;
            yield return new WaitForSeconds(1f);
            m_aiActor.MovementSpeed = 4f;
            m_pathingTimeoutTimer2 = 120f;
            yield return null;
            m_aiActor.aiAnimator.PlayUntilCancelled("raccoon_move", false, null, -1f, false);
            m_aiActor.PathfindToPosition(m_owner.transform.position + new Vector3(1f, 0.1f), null, true, null, null, null, false);
            // Add another timer. If player is close to south walls, Raccoon can't get into position. 
            // If this happens he'll grab the player from the closest point he could get to and continue.            
            while (!m_aiActor.PathComplete)
            {
                DecrementTimer(ref m_pathingTimeoutTimer2, false);
                if (m_pathingTimeoutTimer2 <= 0)
                {
                    m_aiActor.ClearPath();
                    break;
                }
                yield return null;
            }
            m_aiActor.sprite.SpriteChanged += UpdatePlayerPosition;
            m_aiActor.aiAnimator.PlayUntilFinished("grab", false, null, -1f, false);
            yield return null;
            Transform attachPoint = m_aiActor.transform.Find("carry");
            while (m_aiActor.aiAnimator.IsPlaying("grab"))
            {
                Vector2 preferredPrimaryPosition = attachPoint.position.XY() + (m_owner.transform.position.XY() - m_owner.sprite.WorldBottomCenter) + new Vector2(0f, -0.125f);
                m_owner.transform.position = preferredPrimaryPosition;
                m_owner.specRigidbody.Reinitialize();
                yield return null;
            }
            m_owner.SetIsFlying(true, "raccoon", true, false);
            m_aiActor.MovementSpeed = 12f;
            m_aiActor.ClearPath();
            m_aiActor.PathfindToPosition(mostDistantExit, null, true, null, null, null, false);
            m_aiActor.aiAnimator.PlayUntilCancelled("carry", true, null, -1f, false);
            // Set timeout. If Raccoon gets stuck or for some reason won't let go of the player, he'll be forced to end this operation.
            m_pathingTimeoutTimer = 280f;
            yield return null;
            while (!m_aiActor.PathComplete)
            {
                if (!hasFailed && !m_aiActor.Path.WillReachFinalGoal) { hasFailed = true; }
                DecrementTimer(ref m_pathingTimeoutTimer, false);
                if (m_pathingTimeoutTimer <= 0)
                {
                    hasFailed = true;
                    break;
                }
                if (m_owner && m_owner.transform.position.XY() != null)
                {
                    Vector2 v = attachPoint.position.XY() + (m_owner.transform.position.XY() - m_owner.sprite.WorldBottomCenter) + new Vector2(0f, -0.125f);
                    m_owner.transform.position = v;
                    m_owner.specRigidbody.Reinitialize();
                }
                else
                {
                    m_aiActor.ClearPath();
                    hasFailed = true;
                    break;
                }
                yield return null;
            }
            yield return new WaitForSeconds(0.8f);
            m_aiActor.sprite.SpriteChanged -= UpdatePlayerPosition;
            m_aiActor.MovementSpeed = cachedSpeed;
            yield return null;
            // If Raccoon gets stuck or Raccoon gives up due to pathing fail, we'll use a graceful teleport to the intended exit with screen 
            // fade instead of just dropping the player where ever the Racoon gave up at or leaving the player in limbo forever like the original
            // code would have done. (don't want Raccoon giving up and dropping player over a pit or near trap obstacle either. This is a no no! :P )
            if (hasFailed)
            {
                Pixelator.Instance.FadeToBlack(1f, false, 0f);
                yield return new WaitForSeconds(1.1f);
                m_owner.WarpToPointAndBringCoopPartner(mostDistantExit, false, true);
                yield return new WaitForSeconds(0.1f);
                Pixelator.Instance.FadeToBlack(1f, true, 0f);
                yield return new WaitForSeconds(1f);
            }
            m_owner.healthHaver.IsVulnerable = true;
            m_owner.SetIsFlying(false, "raccoon", true, false);
            m_owner.ClearInputOverride("raccoon");
            m_owner.IsEthereal = false;
            yield return null;
            m_aiActor.PathableTiles = CellTypes.FLOOR;
            followBehavior.TemporarilyDisabled = false;
            m_aiActor.aiAnimator.PlayUntilCancelled("idle", false, null, -1f, false);
            m_pathingTimeoutTimer = 0f;
            m_pathingTimeoutTimer2 = 0f;
            yield break;
        }

        private void UpdatePlayerPosition(tk2dBaseSprite obj)
        {
            if (obj)
            {
                Transform transform = m_aiActor.transform.Find("carry");
                PlayerController m_owner = m_aiActor.CompanionOwner;
                if (transform)
                {
                    Vector2 v = transform.position.XY() + (m_owner.transform.position.XY() - m_owner.sprite.WorldBottomCenter) + new Vector2(0f, -0.125f);
                    m_owner.transform.position = v;
                    m_owner.specRigidbody.Reinitialize();
                }
            }
        }

    }
}