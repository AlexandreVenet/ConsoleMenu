using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMenu.ClassesIHM
{
	internal static class Menu
	{
		#region Fields

		/// <summary>
		/// Texte d'aide pour la touche Entrée.
		/// </summary>
		private static string _helpTextEnter = "[ENTREE valider]";

		/// <summary>
		/// Texte d'aide pour les touches Haut/Bas.
		/// </summary>
		public static string _helpTextArrows = "[HAUT/BAS naviguer]";

		#endregion



		#region Properties

		public static string HelpTextEnter 
		{
			get => _helpTextEnter;
			set => _helpTextEnter = value;
		}

		public static string HelpTextArrows
		{
			get => _helpTextArrows;
			set => _helpTextArrows = value;
		}

		#endregion



		#region Methods

		/// <summary>
		/// Créer un menu avec une seule entrée.
		/// </summary>
		/// <param name="option">L'option pour cette entrée.</param>
		public static void Create(Option option)
		{
			// Afficher le menu

			Console.BackgroundColor = ConsoleColor.White;
			Console.ForegroundColor = ConsoleColor.Black;
			Console.WriteLine($"\n  {option.Title} ");
			Console.ResetColor();
			Console.WriteLine($"\n{_helpTextEnter}");

			ConsoleKey key = default;

			while (key != ConsoleKey.Enter)
			{
				key = Console.ReadKey(true).Key;
			}

			// Ici, la key est Enter.

			Console.Clear(); // Avant ce qui suit
			option.Action();
		}

		/// <summary>
		/// Créer un menu à partir d'un tableau d'options.
		/// <br/>La navigation réécrit le menu à la position du curseur à chaque fois.
		/// <br/>La variable de suivi est réinitialisée.
		/// </summary>
		/// <param name="menuOptions">Tableau d'options.</param>
		/// <param name="currentChoice">Variable (en référence), conservant l'index de l'option choisie.</param>
		public static void Create(Option[] menuOptions, ref int currentChoice)
		{
			Create(menuOptions, ref currentChoice, VarBehaviour.ResetValue);
		}

		/// <summary>
		/// Créer un menu à partir d'un tableau d'options.
		/// <br/>La navigation réécrit le menu à la position du curseur à chaque fois.
		/// </summary>
		/// <param name="menuOptions">Tableau d'options.</param>
		/// <param name="currentChoice">Variable (en référence), conservant l'index de l'option choisie.</param
		/// <param name="varBehaviour">Que faire avec la variable de suivi ? Conserver ou réinitialiser la valeur.</param>
		public static void Create(Option[] menuOptions, ref int currentChoice, VarBehaviour varBehaviour)
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
					Console.WriteLine($"  {menuOptions[i].Title} ");
					Console.ResetColor();
				}

				Console.WriteLine($"\n{_helpTextArrows} {_helpTextEnter}");

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
							menuOptions[currentChoice].Action();
							break;
						case VarBehaviour.ResetValue:
						default:
							// Conserver le choix utilisateur et réinitialiser ce dernier
							int savedChoice = currentChoice;
							currentChoice = 0;
							// Lancer l'action avec le choix sauvegardé ici
							menuOptions[savedChoice].Action();
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
	}
}
