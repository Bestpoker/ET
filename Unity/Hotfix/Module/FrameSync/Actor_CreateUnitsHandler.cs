﻿using ETModel;
using UnityEngine;

namespace ETHotfix
{
	[MessageHandler]
	public class Actor_CreateUnitsHandler : AMHandler<Actor_CreateUnits>
	{
		protected override void Run(ETModel.Session session, Actor_CreateUnits message)
		{
			// 加载Unit资源
			
            //resourcesComponent.LoadBundle($"Unit.unity3d");
            

            UnitComponent unitComponent = ETModel.Game.Scene.GetComponent<UnitComponent>();
			
			foreach (UnitInfo unitInfo in message.Units)
			{
				if (unitComponent.Get(unitInfo.UnitId) != null)
				{
					continue;
				}
                //Unit unit = UnitFactory.Create(unitInfo.UnitId);
                Unit unit = ETModel.ComponentFactory.CreateWithId<Unit>(unitInfo.UnitId);

                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle($"Player.unity3d");

                GameObject prefab = resourcesComponent.GetAsset("Player.unity3d", "Player") as GameObject;
                unit.GameObject = UnityEngine.Object.Instantiate(prefab);

                resourcesComponent.UnloadBundle($"Player.unity3d");

                unitComponent.Add(unit);
                GameObject parent = GameObject.Find($"/Global/Unit");
                unit.GameObject.transform.SetParent(parent.transform, false);

                unit.Position = new Vector3(unitInfo.X / 1000f, 0, unitInfo.Z / 1000f);
				unit.IntPos = new VInt3(unitInfo.X, 0, unitInfo.Z);

				if (PlayerComponent.Instance.MyPlayer.UnitId == unit.Id)
				{
                    //ETModel.Game.Scene.GetComponent<CameraComponent>().Unit = unit;
                    Game.Scene.AddComponent<TankControllerComponent, Unit>(unit);

                }
			}

			//Game.Scene.AddComponent<OperaComponent>();

		}
	}
}
