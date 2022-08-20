using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMenu.ClassesIHM
{
	/// <summary>
	/// IHM avec menu.
	/// </summary>
	internal class IHM
	{
		#region Fields

		/// <summary>
		/// Le choix utilisateur lors de la navigation. 
		/// <br/>Utilisée pour usage par réinitialisation.
		/// </summary>
		private int _userChoice;

		/// <summary>
		/// Le choix utilisateur lors de la navigation
		/// <br/>Utilisée pour usage avec conservation de valeur.
		/// </summary>
		private int _userChoiceDemarrer;

		#endregion



		#region Constructors

		/// <summary>
		/// Démarrage automatique de l'IHM.
		/// </summary>
		public IHM()
		{
			Demarrer();
		}

		#endregion



		#region Programme

		public void Demarrer()
		{
			Title("Application titre principal");

			Console.WriteLine("Bienvenue ici !");
			Console.WriteLine("Ce menu conserve la valeur de l'option choisie.");

			// Tableau d'options pour le menu
			Option[] menuOptions =
			{
				new("Info et retour simple", Info),
				new("Process et retour simple", Process),
				new("Sous-menu", SousMenu),
				new("Quitter", ()=> Environment.Exit(0))
			};

			// Le menu
			Menu.Create(menuOptions, ref _userChoiceDemarrer, VarBehaviour.KeepValue);
		}

		private void Info()
		{
			Title("Afficher une info et un retour");

			Console.WriteLine("1 + 1 = 2");

			// Menu à une seule option
			Menu.Create(new("Retour", Demarrer));
		}

		private void Process()
		{
			Title("Procédure et un retour");

			StringBuilder sb = new();

			Console.Write("Ecrire quelque chose : ");
			sb.Append(Console.ReadLine());

			Console.Write("Ecrire autre chose : ");
			sb.Append(Console.ReadLine());

			Console.WriteLine("Vous avez écrit : ");
			Console.WriteLine($"\t{sb}");

			Menu.Create(new("Retour", Demarrer));
		}

		private void SousMenu()
		{
			Title("Sous-menu");

			Console.WriteLine("Une procédure quelconque avant le menu.");
			Console.WriteLine("[ENTREE Suite]");

			ConsoleKey key = default;
			while (key != ConsoleKey.Enter)
			{
				key = Console.ReadKey(true).Key;
			}

			Console.WriteLine("Voilà qui est fait.");
			Console.WriteLine("Maintenant, noter que le menu change et non le reste.");

			// Autre syntaxe
			Menu.Create(new Option[]
			{
				new("Une chose", Chose),
				new("Encore un sous-menu", Encore),
				new("Retour", Demarrer)
			}, ref _userChoice);
		}

		private void Chose()
		{
			Title("Chose");

			Console.WriteLine("Chose !");

			Menu.Create(new("Retour", SousMenu));
		}

		private void Encore()
		{
			Title("Encore un sous-menu");

			Console.WriteLine("Ok, on a compris.");

			Menu.Create(new Option[]
			{
				new("Na !", Na),
				new("Retour", SousMenu)
			}, ref _userChoice);
		}

		private void Na()
		{
			Title("Na");

			Console.WriteLine("Na !");

			Menu.Create(new("Retour", Encore));
		}

		#endregion



		#region UI

		/// <summary>
		/// Afficher un titre entre deux traits.
		/// </summary>
		/// <param name="str">Le titre.</param>
		private void Title(string str)
		{
			Line();
			Console.WriteLine(str);
			Line();
			Console.WriteLine();
		}

		/// <summary>
		/// Afficher un trait sur 50 caractères.
		/// </summary>
		private void Line()
		{
			Console.WriteLine(new String('═', 50));
		}

		#endregion
	}
}
