using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Core.Common.Actions;
using EcoDev.Engine.Entities;

namespace EcoDev.Engine.WorldEngine
{
	internal static class ActionResponseFactory
	{
		static Dictionary<ActionToPerform, Func<LivingEntityWithQualities, ActionResponse>> _responseList
					= new Dictionary<ActionToPerform, Func<LivingEntityWithQualities, ActionResponse>>();
		
		static ActionResponseFactory()
		{
			// Build up a list of ActionResponses
			_responseList.Add( ActionToPerform.Move, (entity) => { return new MovementActionResponse() { Entity = entity }; });
		}

		public static ActionResponse CreateActionResponseHandler(ActionResult actionResult, LivingEntityWithQualities entity)
		{
			return _responseList[actionResult.DecidedAction](entity);
		}
	}
}
