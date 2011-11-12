using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Core.Common.Actions;
using EcoDev.Engine.Entities;
using EcoDev.Engine.MapEngine;

namespace EcoDev.Engine.WorldEngine
{
	internal static class ActionResponseFactory
	{
		static Dictionary<ActionToPerform, Func<LivingEntityWithQualities, EcoWorld , ActionResult, ActionResponse>> _responseList
					= new Dictionary<ActionToPerform, Func<LivingEntityWithQualities, EcoWorld, ActionResult, ActionResponse>>();
		
		static ActionResponseFactory()
		{
			// Build up a list of ActionResponses
			_responseList.Add( ActionToPerform.Move, (entity,world, actionResult) => { return new MovementActionResponse(world) { Inhabitant = entity, DecidedAction = actionResult }; });
		}

		public static ActionResponse CreateActionResponseHandler(ActionResult actionResult, LivingEntityWithQualities entity,EcoWorld world)
		{
			return _responseList[actionResult.DecidedAction](entity, world, actionResult);
		}
	}
}
