using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EcoDev.Core.Common;

namespace EcoDev.UnitTests
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class BaseEntityTests
	{
		public BaseEntityTests()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		[TestMethod]
		public void BaseEntityShouldProvideValidToStringOverride()
		{
			var mockEntity = new DummyBuildingBlockEntity();
			var stringOutput = mockEntity.ToString();
			Assert.IsNotNull(stringOutput);
			Assert.IsTrue(stringOutput.Contains(DummyBuildingBlockEntity.KEY.ToString()));
			Assert.IsTrue(stringOutput.Contains("Something"));
		}
	}

	public class DummyBuildingBlockEntity : BaseEntity
	{
		public static Guid KEY = Guid.NewGuid();

		public DummyBuildingBlockEntity()
		{
			Size = new EntitySize() { Height=10, Thickness=20, Width=30 };
			Name = "Something";
			LifeKey = KEY;
			Weight = 10;
		}
		public override EntityBaseType EntityType
		{
			get { return EntityBaseType.BuildingBlock; }
		}
	}

}
