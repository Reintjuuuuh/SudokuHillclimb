using System;

//Parser leest grid in en maakt een 2d array met 0'en en hardcoded items. een mask bepaald door 0 en 1 welke we mogen verplaatsen
//Het grid wordt willekeurig ingevuld op alle 0 plekken met missende getallen binnen het 3x3 blok.
//search algoritme -> kies 1 van de 9 blokken,
//Maak een lijst  met nieuwe sudoku objecten waarin
//kies de beste door op elk in de lijst de evaluatiefunctie uit te voeren.

//Parsing en invullen tot sudoku object
//Algoritme
//Evaluatiefunctie

Console.Write("Wat is je naam? ");
string naam = Console.ReadLine();
Console.WriteLine($"Hallo {naam}!");