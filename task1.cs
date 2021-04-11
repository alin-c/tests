using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

class Program {
	static void Main() {
		// create a HashSet for storing the text
		HashSet<string> set = new();

		// read the input file line by line and add each word to the set
		// (because HashSet ignores newly added duplicates)
		using (StreamReader reader = new("deps.in")) {
			string line;
			while ((line = reader.ReadLine()) != null) {
				SplitStringToSet(line, ref set);
			}
		}

		// use a Linq query to get the sorted items from the set
		var items = from item in set
					orderby item ascending
					select item;

		// add the ordered items to a string
		string result = "";
		foreach (var item in items) {
			result += string.Concat(item, "\n");
		}
		// remove the last newline
		result = result.Remove(result.Length - 1);

		// print them to the output file in lexicographical order
		using (StreamWriter writer = new("task1.out")) {
			writer.Write(result);
		}

		Console.Write("Task 1 done! Press any key to exit...");
		Console.ReadKey();
	}

	static void SplitStringToSet(string text, ref HashSet<string> set) {
		// remove extra spaces: trim + compact
		text = Regex.Replace(text.Trim(), @"\s{2,}", " ");
		foreach (string item in text.Split(' ')) {
			set.Add(item);
		}
	}
}
