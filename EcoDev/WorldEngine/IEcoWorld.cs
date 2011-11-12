using System;
namespace EcoDev.Engine.WorldEngine
{
	public interface IEcoWorld
	{
		void AddPlayer(EcoDev.Engine.Entities.LivingEntityWithQualities player);
		event EventHandler<DebugInfoEventArgs> DebugInformation;
		void DestroyWorld();
		event EventHandler<EntityExitEventArgs> EntityExited;
		event EventHandler<InhabitantActionEventArgs> InhabitantPerformedAction;
		System.Collections.Generic.IEnumerable<EcoDev.Engine.Entities.LivingEntityWithQualities> Inhabitants { get; }
		void StartWorld();
		EcoDev.Engine.MapEngine.Map WorldMap { get; }
		void WriteDebugInformation(string source, string message);
		void WriteDebugInformation(string source, string message, params object[] args);
	}
}
