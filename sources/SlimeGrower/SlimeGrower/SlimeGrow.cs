using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using UnityEngine;
using TUNING;

﻿using System;
using UnityEngine;
using System;
using UnityEngine;

namespace SlimeGrowerMod
{
	public class SlimeGrower : StateMachineComponent<SlimeGrower.StatesInstance>
	{
		[SerializeField]
		public CellOffset pressureSampleOffset = CellOffset.none;

		[MyCmpGet]
		private Operational operational;

		protected override void OnPrefabInit()
		{
			GetComponent<KBatchedAnimController>().randomiseLoopedOffset = true;
			base.OnPrefabInit();
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();
			smi.StartSM();
		}

		public class StatesInstance : GameStateMachine<SlimeGrower.States, SlimeGrower.StatesInstance, SlimeGrower, object>.GameInstance
		{
			private Operational operational;
			public ElementConverter converter;
			private ConduitConsumer consumer;

			public StatesInstance(SlimeGrower master)
			  : base(master)
			{
				this.operational = master.GetComponent<Operational>();
				this.converter = master.GetComponent<ElementConverter>();
				this.consumer = master.GetComponent<ConduitConsumer>();
			}

			public bool HasEnoughMass(Tag tag)
			{
				return this.converter.HasEnoughMass(tag);
			}

			public bool IsOperational {
				get {
					if (this.operational.IsOperational && this.consumer.IsConnected)
						return true;
					return false;
				}
			}
			public bool HasLight()
			{
				int cell = Grid.PosToCell(smi.master.transform.GetPosition());
				return Grid.LightCount[cell] > 0;
			}
		}

		public class States : GameStateMachine<SlimeGrower.States, SlimeGrower.StatesInstance, SlimeGrower>
		{
			public State generatingWater;
			public State stoppedGeneratingWater;
			public State stoppedGeneratingWaterTransition;
			public State noWater;
			public State noFert;
			public State gotFert;
			public State gotWater;
			public State lostFert;
			public State notoperational;
			public State noLight;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = notoperational;

				root
					.EventTransition(GameHashes.OperationalChanged, notoperational, smi => !smi.IsOperational);

				notoperational
					.QueueAnim("off")
					.EventTransition(GameHashes.OperationalChanged, noLight, smi => smi.IsOperational);

				noLight
					.QueueAnim("off")
					.Enter(smi => smi.master.operational.SetActive(false))
					.Update("NoLight", (smi, dt) => { if (smi.HasLight() && smi.HasEnoughMass(GameTags.Water)) smi.GoTo(gotFert); }, UpdateRate.SIM_1000ms);

				gotFert
					.PlayAnim("on_pre")
					.OnAnimQueueComplete(noWater);

				lostFert
					.PlayAnim("on_pst")
					.OnAnimQueueComplete(noFert);

				noFert
					.QueueAnim("off")
					.EventTransition(GameHashes.OnStorageChange, gotFert, smi => smi.HasEnoughMass(GameTags.Water))
					.Enter(smi => smi.master.operational.SetActive(false));

				noWater
					.QueueAnim("on")
					.Enter(smi => smi.master.GetComponent<PassiveElementConsumer>().EnableConsumption(true))
					.EventTransition(GameHashes.OnStorageChange, lostFert, smi => !smi.HasEnoughMass(GameTags.Water))
					.EventTransition(GameHashes.OnStorageChange, gotWater, smi => {
						if (smi.HasEnoughMass(GameTags.Water)) 
							return smi.HasEnoughMass(GameTags.Water);
						return false;
					});

				gotWater
					.PlayAnim("working_pre")
					.OnAnimQueueComplete(generatingWater);

				generatingWater
					.Enter(smi => smi.master.operational.SetActive(true))
					.Exit(smi => smi.master.operational.SetActive(false))
					.QueueAnim("working_loop", true)
					.EventTransition(GameHashes.OnStorageChange, stoppedGeneratingWater,
						smi => !smi.HasEnoughMass(GameTags.Water) || !smi.HasEnoughMass(GameTags.Water))
					.Update("GeneratingWater", (smi, dt) => { if (!smi.HasLight()) smi.GoTo(stoppedGeneratingWater); }, UpdateRate.SIM_1000ms);

				stoppedGeneratingWater
					.PlayAnim("working_pst")
					.OnAnimQueueComplete(stoppedGeneratingWaterTransition);

				stoppedGeneratingWaterTransition
					.Update("StoppedGeneratingWaterTransition", (smi, dt) => { if (!smi.HasLight()) smi.GoTo(noLight); }, UpdateRate.SIM_200ms)
					.EventTransition(GameHashes.OnStorageChange, noWater, smi => !smi.HasEnoughMass(GameTags.Water) && smi.HasLight())
					.EventTransition(GameHashes.OnStorageChange, lostFert, smi => !smi.HasEnoughMass(GameTags.Water) && smi.HasLight())
					.EventTransition(GameHashes.OnStorageChange, gotWater, smi => {
						if (smi.HasEnoughMass(GameTags.Water) && smi.HasLight())
							return smi.HasEnoughMass(GameTags.Water);
						return false;
					});
			}
		}
	}
}