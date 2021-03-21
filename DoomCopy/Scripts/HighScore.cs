using DoomCopy.Enteties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy
{
	public class HSItem
	{
		// Variabler och egenskaper för dem:
		private string name;
		private int points;

		public string Name { get { return name; } set { name = value; } }
		public int Points { get { return points; } set { points = value; } }

		// =======================================================================
		// HSItem(), klassens konstruktor
		// =======================================================================
		public HSItem(string name, int points)
		{
			this.name = name;
			this.points = points;
		}
	}

	// =======================================================================
	// HighScore, innehåller en lista med hsItems samt metoder för att
	// manipulera listan.
	// =======================================================================
	public class HighScore
	{
		private int maxInList = 5; // Hur många som får vara i listan
		private List<HSItem> highscore = new List<HSItem>();
		private string name = ""; // Spelarens namn

		// Används för att skriva ut vilket tecken spelaren har valt just nu:
		private string currentChar;
		private int key_index = 0; // Denna används för att mata in spelarens namn

		// =======================================================================
		// HighScore(), klassens konstruktor
		// =======================================================================
		public HighScore(int maxInList)
		{
			this.maxInList = maxInList;
		}

		// =======================================================================
		// Sort(),  metod som sorterar listan. Metoden
		// anropas av Add() när en ny person läggs till i
		// listan. Använder algoritmen shellsort
		// =======================================================================
		private void ShellSort(List<HSItem> highscore)
		{
			int n = highscore.Count;
			int gap = n / 2;
			HSItem temp;

			while (gap > 0)
			{
				for (int i = 0; i + gap < n; i++)
				{
					int j = i + gap;
					temp = highscore[j];

					while (j - gap >= 0 && temp.Points < highscore[j - gap].Points)
					{


						highscore[j] = highscore[j - gap];
						j = j - gap;
					}

					highscore[j] = temp;
				}

				gap = gap / 2;
			}

			highscore.Reverse();
		}

		private void RemoveDuplicates()
		{
			for (int i = 0; i < highscore.Count; i++)
			{
				if ((i + 1) < highscore.Count)
				{
					if (highscore[i].Points == highscore[i + 1].Points && highscore[i].Name == highscore[i + 1].Name)
					{
						highscore.RemoveAt(i);
					}
				}

			}
		}
		// =======================================================================
		// Add(), lägger till en person i highscore-listan.
		// =======================================================================
		public void Add(int points)
		{
			// Skapa en temporär variabel av typen HSItem:
			HSItem temp = new HSItem(name, points);

			// Lägg till tmp i listan.
			highscore.Add(temp);

			// Sortera listan efter att vi har lagt till en person!
			ShellSort(highscore);
			RemoveDuplicates();

			// Är det för många i listan?
			while (highscore.Count > maxInList)
			{
				highscore.RemoveAt(maxInList);
			}
		}

		// =======================================================================
		// PrintDraw(), metod för att skriva ut listan. Det finns ingen
		// PrintUpdate() då det är en helt statisk text som skrivs ut.
		// =======================================================================
		public void PrintDraw(SpriteBatch spriteBatch)
		{
			string text = "";
			text += "HIGHSCORE\n";

			for (int i = 0; i < highscore.Count; i++)
			{
				text += highscore[i].Name + " " + highscore[i].Points + "\n";
			}

			/*
			foreach (HSItem h in highscore)
			{
				/*"Your Score: " + h.Name + "\n" +
				"High Score: " + PlayerManager.HighScore;
			}*/

			Vector2 textSize = Art.Font.MeasureString(text);
            spriteBatch.DrawString(Art.Font, text, GameRoot.ScreenSize / 2 - textSize / 2, Color.Red);

		}

		// =======================================================================
		// EnterUpdate(), här matar användaren in sitt användarnamn. Precis som
		// klassiska gamla arkadspel kan man ha tre tecken A-Z i sitt namn. Detta
		// är Update-delen i spel-loopen för inmatning av highscore-namn. Metoden
		// ska fortsätta anropas av Update() så länge true returneras.
		// =======================================================================
		public bool EnterUpdate(int points)
		{
			// Vilka tecken som är möjliga:
			char[] key = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K',
					   'L', 'M', 'N', 'O', 'P',  'Q', 'R', 'S', 'T', 'U',
					   'V', 'X', 'Y', 'Z'};


			// Användaren trycker knappen nedåt, stega framlänges i key-vektorn:
			if (Input.WasKeyPressed(Keys.Down))
			{
				key_index++;
				if (key_index >= key.Length)
					key_index = 0;
			}

			// Användaren trycker knappen uppåt, stega baklänges i key-vektorn:
			if (Input.WasKeyPressed(Keys.Up))
			{
				key_index--;
				if (key_index <= 0)
					key_index = key.Length - 1;
			}

			// Användaren trycker ENTER, lägg till det valda tecknet i 
			if (Input.WasKeyPressed(Keys.Enter))
			{
				name += key[key_index].ToString();
				if (name.Length == 3)
				{
					// Återställ namnet och allt så att man kan lägga till namnet 
					// på en ny spelare:
					Add(points);
					name = "";
					currentChar = "";
					key_index = 0;
					return true; // Ange att vi är klara
				}
			}
			// Lagra det tecken som nu är valt, så att vi kan skriva ut det i
			// EnterDraw():
			currentChar = key[key_index].ToString();
			// Ange att vi inte är klara, fortsätt anropa denna metod via Update():
			return false;
		}

		// =======================================================================
		// EnterDraw(), skriver ut de tecken spelaren har matat in av sitt namn
		// (om något) samt det tecken (av tre) som just nu är valt.
		// =======================================================================
		public void EnterDraw(SpriteBatch spriteBatch)
		{
			string text = "ENTER NAME: " + name + currentChar;

			Vector2 textSize = Art.Font.MeasureString(text);
			spriteBatch.DrawString(Art.Font, text, GameRoot.ScreenSize / 2 - textSize / 2 + new Vector2(0, -50), Color.Red);
		}

		public void EnterLoading(SpriteBatch spriteBatch)
		{
			string text = "SAVING...";

			Vector2 textSize = Art.Font.MeasureString(text);
			spriteBatch.DrawString(Art.Font, text, GameRoot.ScreenSize / 2 - textSize / 2 + new Vector2(0, -50), Color.Red);
		}

		// =======================================================================
		// SaveToFile(), spara till fil.
		// =======================================================================
		public void SaveHighscore(string filename)
		{
			using (StreamWriter sw = new StreamWriter(filename))
			{
				try
				{
					string currentLine = "";
					string lastLine = "";

					foreach (HSItem item in highscore)
					{
						// Spara alla i highscore listan
						string text = item.Name + ":" + item.Points;
						currentLine = text;

						if (currentLine != lastLine)
						{
							sw.WriteLine(text);
							lastLine = text;
						}
					}
				}
				catch (Exception)
				{
					throw;
				}
			}
		}

		public void LoadHighscore(string filename)
		{
			using (StreamReader sr = new StreamReader(filename))
			{
				try
				{
					// Läs in alla rader till string-objektet row och skriv ut dem:
					string row;
					while ((row = sr.ReadLine()) != null)
					{
						if(row == "")
						{
							return;
						}

						// skapa en vektor som innehåller namn och poäng,
						// words[0] blir namnet och words[1] är poängen:
						string[] words = row.Split(':');
						int points = Convert.ToInt32(words[1]);

						// Lägg till i listan:
						HSItem temp = new HSItem(words[0], points);
						highscore.Add(temp);
					}
				}
				catch (Exception)
				{
					throw;
				}
			}
		}
	}
}
