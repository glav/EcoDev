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

		[TestMethod]
		public void LivingEntityShouldUpdateQualities()
		{
			var entity = new SomeEntityWithQualitities()
			{
				Agility = 1,
				Hearing = 2,
				Intelligence = 3,
				Intuition = 4,
				Reflexes = 5,
				Sight = 6,
				Speed = 7,
				Stamina = 8,
				Strength = 9,
				Touch = 10

			};
			var qualitiesToUpdate = new LivingEntityQualities()
			{
				Agility = 5,
				Hearing = 6,
				Intelligence = 7,
				Intuition = 8,
				Reflexes = 9,
				Sight = 10,
				Speed = 11,
				Stamina = 12,
				Strength = 13,
				Touch = 14

			};
			entity.UpdateQualities(qualitiesToUpdate);
			Assert.AreEqual<double>(6, entity.Agility);
			Assert.AreEqual<double>(8, entity.Hearing);
			Assert.AreEqual<double>(10, entity.Intelligence);
			Assert.AreEqual<double>(12, entity.Intuition);
			Assert.AreEqual<double>(14, entity.Reflexes);
			Assert.AreEqual<double>(16, entity.Sight);
			Assert.AreEqual<double>(18, entity.Speed);
			Assert.AreEqual<double>(20, entity.Stamina);
			Assert.AreEqual<double>(22, entity.Strength);
			Assert.AreEqual<double>(24, entity.Touch);

		}
	}

	public class SomeEntityWithQualitities : LivingEntityQualities
	{
		public void UpdateQualities(LivingEntityQualities qualities)
		{
			base.UpdateQualitities(qualities);
		}

	}

	public class DummyBuildingBlockEntity : BaseEntity
	{
		public static Guid KEY = Guid.NewGuid();

		public DummyBuildingBlockEntity()
		{
			Size = new EntitySize() { Height = 10, Thickness = 20, Width = 30 };
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
