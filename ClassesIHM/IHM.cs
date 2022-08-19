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
		/// <br/>Utilisée pour l'usage sans conserver la valeur.
		/// </summary>
		private int _userChoice;

		/// <summary>
		/// Le choix utilisateur lors de la navigation
		/// <br/>Utilisée pour l'usage avec conservation de la valeur.
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
			Console.WriteLine("A cet écran seulement, le menu conserve la valeur de l'option choisie.");

			// Tableau d'options pour le menu
			Option[] menuOptions =
			{
				new("Info et retour simple", Info),
				new("Process et retour simple", Process),
				new("Sous-menu", SousMenu),
				new("Quitter", ()=> Environment.Exit(0))
			};

			// Le menu
			Menu(menuOptions, ref _userChoiceDemarrer, VarBehaviour.KeepValue);
		}

		private void Info()
		{
			Title("Afficher une info et un retour");

			Console.WriteLine("1 + 1 = 2");

			// Menu à une seule option
			Menu(new("Retour", Demarrer));
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

			Menu(new("Retour", Demarrer));
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
			Menu(new Option[]
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

			Menu(new("Retour", SousMenu));
		}

		private void Encore()
		{
			Title("Encore un sous-menu");

			Console.WriteLine("Ok, on a compris.");

			Menu(new Option[]
			{
				new("Na !", Na),
				new("Retour", SousMenu)
			}, ref _userChoice);
		}

		private void Na()
		{
			Title("Na");

			Console.WriteLine("Na !");

			Menu(new("Retour", Encore));
		}

		#endregion



		#region Menu controls

		/// <summary>
		/// Créer un menu avec une seule entrée.
		/// </summary>
		/// <param name="option">L'option pour cette entrée.</param>
		private void Menu(Option option)
		{
			// Afficher le menu

			Console.BackgroundColor = ConsoleColor.White;
			Console.ForegroundColor = ConsoleColor.Black;
			Console.WriteLine($"\n  {option.p_title} ");
			Console.ResetColor();
			Console.WriteLine("\n[ENTREE valider]");

			ConsoleKey key = default;

			while (key != ConsoleKey.Enter)
			{
				key = Console.ReadKey(true).Key;
			}

			// Ici, la key est Enter.

			Console.Clear(); // Avant ce qui suit
			option.p_action();
		}

		/// <summary>
		/// Créer un menu à partir d'un tableau d'options.
		/// <br/>La navigation réécrit le menu à la position du curseur à chaque fois.
		/// <br/>La variable de suivi est réinitialisée.
		/// </summary>
		/// <param name="menuOptions">Tableau d'options.</param>
		/// <param name="currentChoice">Variable (en référence), conservant l'index de l'option choisie.</param>
		private void Menu(Option[] menuOptions, ref int currentChoice)
		{
			Menu(menuOptions, ref currentChoice, VarBehaviour.ResetValue);
		}

		/// <summary>
		/// Créer un menu à partir d'un tableau d'options.
		/// <br/>La navigation réécrit le menu à la position du curseur à chaque fois.
		/// </summary>
		/// <param name="menuOptions">Tableau d'options.</param>
		/// <param name="currentChoice">Variable (en référence), conservant l'index de l'option choisie.</param
		/// <param name="varBehaviour">Que faire avec la variable de suivi ? Conserver ou réinitialiser la valeur.</param>
		private void Menu(Option[] menuOptions, ref int currentChoice, VarBehaviour varBehaviour)
		{
			while (true)
			{
				// Connaître la position du curseur (tuple)

				(int Left, int Top) cursorStart = (Console.CursorLeft, Console.CursorTop);

				// Afficher le menu

				Console.WriteLine();

				for (int i = 0; i < menuOptions.Length; i++)
				{
					if (i == currentChoice)
					{
						Console.BackgroundColor = ConsoleColor.White;
						Console.ForegroundColor = ConsoleColor.Black;
					}
					else
					{
						Console.BackgroundColor = ConsoleColor.Black;
						Console.ForegroundColor = ConsoleColor.White;
					}
					Console.WriteLine($"  {menuOptions[i].p_title} ");
					Console.ResetColor();
				}

				Console.WriteLine("\n[HAUT/BAS naviguer] [ENTREE valider]");

				// Choix utilisateur

				ConsoleKey key = default;

				while (key != ConsoleKey.Enter && key != ConsoleKey.UpArrow && key != ConsoleKey.DownArrow)
				{
					key = Console.ReadKey(true).Key;
				}

				// Ici, la key est l'une de celles autorisées.

				if (key == ConsoleKey.DownArrow)
				{
					currentChoice++;
				}
				else if (key == ConsoleKey.UpArrow)
				{
					currentChoice--;
				}
				else if (key == ConsoleKey.Enter)
				{
					// Effacer (ici et pas après sinon non considéré)
					Console.Clear();

					switch (varBehaviour)
					{
						case VarBehaviour.KeepValue:
							// Lancer l'action sans toucher à la variable (sa valeur est donc inchangée)
							menuOptions[currentChoice].p_action();
							break;
						case VarBehaviour.ResetValue:
						default:
							// Conserver le choix utilisateur et réinitialiser ce dernier
							int savedChoice = currentChoice;
							currentChoice = 0;
							// Lancer l'action avec le choix sauvegardé ici
							menuOptions[savedChoice].p_action();
							break;
					}
					
					// Arrêter ici
					break;
				}

				if (currentChoice < 0)
				{
					currentChoice = menuOptions.Length - 1;
				}
				else if (currentChoice > menuOptions.Length - 1)
				{
					currentChoice = 0;
				}

				// Réécriture à la position du curseur (et non Console.Clear())

				Console.SetCursorPosition(cursorStart.Left, cursorStart.Top);
			}
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
