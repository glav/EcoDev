using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Engine.Entities;
using EcoDev.Core.Common.Actions;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EcoDev.Engine.WorldEngine
{
	public class AsyncActionExecutionEngine
	{
		LivingEntityWithQualities _entity;
		ActionContext _context;
		CancellationTokenSource _tokenSource = new CancellationTokenSource();
#if DEBUG
		public const int MAX_MILLISECONDS_ALLOWED_FOR_ENTITY_DECISION = 2000;
#else
		public const int MAX_MILLISECONDS_ALLOWED_FOR_ENTITY_DECISION = 500;
#endif

		public AsyncActionExecutionEngine(LivingEntityWithQualities entity, ActionContext context)
		{
			_entity = entity;
			_context = context;
		}

		public ActionResultContext DetermineEntityAction()
		{
			var decisionTask = new Task<ActionResultContext>(() =>
			{
				ActionResultContext result;
				try
				{
					result = new ActionResultContext(_entity.Entity.DecideActionToPerform(_context));
				}
				catch (Exception ex)
				{
					result = new ActionResultContext() { ErrorException = ex };
					_entity.IsDead = true;
				}
				return result;
			}, _tokenSource.Token);

			decisionTask.Start();
			decisionTask.Wait(MAX_MILLISECONDS_ALLOWED_FOR_ENTITY_DECISION);

			// If the decision has not been reached after the time period, signal the task to close
			if (!decisionTask.IsCompleted && !decisionTask.IsCanceled && !decisionTask.IsFaulted)
			{
				_tokenSource.Cancel();
				System.Threading.Thread.Sleep(200);
				if (decisionTask.IsCompleted || decisionTask.IsCanceled || decisionTask.IsFaulted)
				{
					decisionTask.Dispose();
				}
				return new ActionResultContext(new MovementAction());
			}
			else
			{
				return decisionTask.Result;
			}

			return new ActionResultContext(new MovementAction());

		}

	}
}
