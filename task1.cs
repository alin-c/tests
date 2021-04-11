using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Program {
	static void Main() {
		// create a SortedSet for storing the text because it
		//	1. ignores newly added duplicates;
		// 	2. automatically sorts its contents
		SortedSet<string> set = new();

		// read the input file line by line and add each "word" to the set
		using (StreamReader reader = new("deps.in")) {
			string line;
			while ((line = reader.ReadLine()) != null) {
				SplitStringToSet(line, ref set);
			}
		}

		// add the ordered items to a string
		string result = null;
		foreach (var item in set) {
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

	static void SplitStringToSet(string text, ref SortedSet<string> set) {
		// remove extra spaces: trim + compact
		text = Regex.Replace(text.Trim(), @"\s{2,}", " ");
		foreach (string item in text.Split(' ')) {
			set.Add(item);
		}
	}
}
