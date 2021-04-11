using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

class Task1 {
	/// <summary>
	/// Entry point.
	/// </summary>
	static void Main() {
		// create a SortedSet for storing the text because it
		//	1. ignores newly added duplicates;
		// 	2. automatically sorts its contents
		SortedSet<string> set = new();

		// read the input file line by line and add each "word" to the set
		string inputFile = "deps.in";
		try {
			using StreamReader reader = new(inputFile);
			string line;
			while ((line = reader.ReadLine()) != null) {
				SplitStringToSet(line, ref set);
			}
		} catch (IOException) {
			Console.WriteLine($"[!] The file '{inputFile}' could not be read.\nCheck if the file '{inputFile}' exists in the current directory and then try again.");
			return;
		} catch (Exception e) {
			Console.WriteLine("[!] " + e.Message);
			return;
		}

		// add the ordered items to a StringBuilder, since there are potentially many concatenations to be made
		StringBuilder result = new();
		foreach (var item in set) {
			result.Append(item + "\n");
		}

		// remove the last newline
		result = result.Remove(result.Length - 1, 1);

		// print the resulting string to the output file in lexicographical order
		string outputFile = "task1.out";
		try {
			using StreamWriter writer = new(outputFile);
			writer.Write(result);
		} catch (IOException) {
			Console.WriteLine($"[!] The file '{outputFile}' could not be written.\nThis may happen if you do not have permission to write to the current directory or if the file is set as read-only.");
			return;
		}

		// notify the user about the program's execution
		Console.Write("Task 1 done! Press any key to exit...");
		Console.ReadKey();
	}

	/// <summary>
	/// Processes a line of text (by removing unnecessary spaces) and adds each non-empty
	/// "word" to the supplied SortedSet.
	/// </summary>
	/// <param name="text">the input text</param>
	/// <param name="set">a SortedSet passed by reference</param>
	static void SplitStringToSet(string text, ref SortedSet<string> set) {
		// remove extra spaces: trim + compact
		text = Regex.Replace(text.Trim(), @"\s{2,}", " ");

		foreach (string item in text.Split(' ')) {
			// add to the set every non-empty "word"
			if (item != "") {
				set.Add(item);
			}
		}
	}
}
