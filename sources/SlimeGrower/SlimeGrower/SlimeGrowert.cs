using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using UnityEngine;
using TUNING;

using KSerialization;

namespace SlimeGrowerMod
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class SlimeGrow : StateMachineComponent<SlimeGrow.StatesInstance>
	{
		public class StatesInstance : GameStateMachine<States, StatesInstance, SlimeGrow, object>.GameInstance
		{
			public StatesInstance(SlimeGrow smi)
				: base(smi)
			{
			}
		}

		public class States : GameStateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow>
		{
			public GameStateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.State generatingOxygen;
			public GameStateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.State stoppedGeneratingOxygen;
			public GameStateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.State stoppedGeneratingOxygenTransition;
			public GameStateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.State noSlimeMold;
			public GameStateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.State noWater;
			public GameStateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.State gotWater;
			public GameStateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.State gotSlimeMold;
			public GameStateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.State lostWater;
			public GameStateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.State notoperational;

			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = (StateMachine.BaseState)this.notoperational;

				this.root
					.EventTransition(GameHashes.OperationalChanged, this.notoperational, (StateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.Transition.ConditionCallback)
						(smi => !smi.IsOperational));

				this.notoperational
					.QueueAnim("off", false, (Func<SlimeGrow.StatesInstance, string>)null)
					.EventTransition(GameHashes.OperationalChanged, this.noWater, (StateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.Transition.ConditionCallback)
					(smi => smi.IsOperational));

				this.gotWater.PlayAnim("on_pre").OnAnimQueueComplete(this.noSlimeMold);
				this.lostWater.PlayAnim("on_pst").OnAnimQueueComplete(this.noWater);

				this.noWater
					.QueueAnim("off", false, (Func<SlimeGrow.StatesInstance, string>)null)
					.EventTransition(GameHashes.OnStorageChange, this.gotWater, (StateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.Transition.ConditionCallback)
						(smi => smi.HasEnoughMass(GameTags.Water)))
					.Enter((StateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.State.Callback)
						(smi => smi.master.operational.SetActive(false, false)));


				this.noSlimeMold
					.QueueAnim("on", false, (Func<SlimeGrow.StatesInstance, string>)null)
					.Enter((StateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.State.Callback)
						(smi => smi.master.GetComponent<PassiveElementConsumer>().EnableConsumption(true)))
					.EventTransition(GameHashes.OnStorageChange, this.lostWater, (StateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.Transition.ConditionCallback)
						(smi => !smi.HasEnoughMass(GameTags.Water)))
					.EventTransition(GameHashes.OnStorageChange, this.gotSlimeMold, (StateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.Transition.ConditionCallback)
						(smi => {
							if (smi.HasEnoughMass(GameTags.Water))
								return smi.HasEnoughMass(GameTags.SlimeMold);
							return false;
						}));

				this.gotSlimeMold.PlayAnim("working_pre").OnAnimQueueComplete(this.generatingOxygen);

				this.generatingOxygen
					.Enter((StateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.State.Callback)
						(smi => smi.master.operational.SetActive(true, false)))
					.Exit((StateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.State.Callback)
						(smi => smi.master.operational.SetActive(false, false)))
					.Update("GeneratingOxygen", (System.Action<SlimeGrow.StatesInstance, float>)
						((smi, dt) => {
							int cell = Grid.PosToCell(smi.master.transform.GetPosition());
							smi.converter.OutputMultiplier = Grid.LightCount[cell] <= 0 ? 1f : smi.master.lightBonusMultiplier;
						}), UpdateRate.SIM_200ms, false).QueueAnim("working_loop", true, (Func<SlimeGrow.StatesInstance, string>)null)
					.EventTransition(GameHashes.OnStorageChange, this.stoppedGeneratingOxygen, (StateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.Transition.ConditionCallback)
						(smi => !smi.HasEnoughMass(GameTags.SlimeMold) || !smi.HasEnoughMass(GameTags.Water)));

				this.stoppedGeneratingOxygen
					.PlayAnim("working_pst")
					.OnAnimQueueComplete(this.stoppedGeneratingOxygenTransition);

				this.stoppedGeneratingOxygenTransition
					.EventTransition(GameHashes.OnStorageChange, this.noSlimeMold, (StateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.Transition.ConditionCallback)
						(smi => !smi.HasEnoughMass(GameTags.SlimeMold)))
					.EventTransition(GameHashes.OnStorageChange, this.lostWater, (StateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.Transition.ConditionCallback)
						(smi => !smi.HasEnoughMass(GameTags.Water)))
					.EventTransition(GameHashes.OnStorageChange, this.gotSlimeMold, (StateMachine<SlimeGrow.States, SlimeGrow.StatesInstance, SlimeGrow, object>.Transition.ConditionCallback)
						(smi => {
							if (smi.HasEnoughMass(GameTags.SlimeMold))
								return smi.HasEnoughMass(GameTags.Water);
							return false;
						}));

		[MyCmpGet]
		private Operational operational;

		[MyCmpGet]
		private Storage storage;

		[MyCmpGet]
		private ElementConverter elementConverter;

		[MyCmpGet]
		private ElementConsumer elementConsumer;

		public Tag filterTag;

		public bool HasFilter()
		{
			return true;
		}

		public bool IsConvertable()
		{
			return this.elementConverter.HasEnoughMassToStartConverting();
		}

			public bool HasEnoughMass(Tag tag)
			{
				return this.converter.HasEnoughMass(tag);
			}

			public bool IsOperational {
				get {
					if (this.operational.IsOperational && this.dispenser.IsConnected)
						return true;
					return false;
				}
			}

			public List<Descriptor> GetDescriptors(BuildingDef def)
		{
			return null;
		}
	
	}
}