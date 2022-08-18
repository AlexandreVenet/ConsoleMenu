using ConsoleMenu.ClassesIHM;

namespace ConsoleMenu
{
	internal class Program
	{
		static void Main(string[] args)
		{

			// Paramètres de l'application Console
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			Console.Title = "Console Menu";

			// Démarrer l'IHM
			new IHM();

		}
	}
}