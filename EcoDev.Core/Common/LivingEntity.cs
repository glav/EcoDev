using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoDev.Core.Common.Actions;

namespace EcoDev.Core.Common
{
	public abstract class LivingEntity : BaseEntity
	{
		public override EntityBaseType EntityType
		{
			get { return EntityBaseType.LivingEntity; }
		}

		public abstract ActionResult DecideActionToPerform(ActionContext actionContext);
	}
}
