using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

class Task3 {
	/// <summary>
	/// Entry point.
	/// </summary>
	static void Main() {
		// create a SortedSet for storing the deps.in text because it
		//	1. ignores newly added duplicates;
		//	2. automatically sorts its contents
		SortedSet<string> dependencies = new();

		// create a List for storing the "words" from computers.in
		List<SortedSet<string>> computers = new();

		// read the input file line by line and add each "word" to the set
		string depsInputFile = "deps.in";
		string compsInputFile = "computers.in";
		try {
			using StreamReader depsReader = new(depsInputFile);
			string line;
			while ((line = depsReader.ReadLine()) != null) {
				SplitStringToSet(line, ref dependencies);
			}

			using StreamReader compsReader = new(compsInputFile);
			SortedSet<string> temp = new();
			line = "";
			while ((line = compsReader.ReadLine()) != null) {
				SplitStringToSet(line, ref temp);
				computers.Add(temp);
				temp = new();
			}
		} catch (IOException) {
			Console.WriteLine($"[!] An input file could not be read.\nCheck if the files '{depsInputFile}' and '{compsInputFile}' exist in the current directory and are readable, then try again.");
			return;
		} catch (Exception e) {
			Console.WriteLine("[!] " + e.Message);
			return;
		}

		// change the contents of computers to be the difference between the dependency set and current content
		for (var i = 0; i < computers.Count; i++) {
			computers[i] = new SortedSet<string>(dependencies.Except(computers[i]));
		}

		// add the ordered items to a StringBuilder, since there are potentially many concatenations to be made
		StringBuilder result = new();
		foreach (var item in computers) {
			result.Append(string.Join(" ", item) + "\n");
		}

		// remove the last newline
		result = result.Remove(result.Length - 1, 1);

		// print the resulting string to the output file in lexicographical order
		string outputFile = "task3.out";
		try {
			using StreamWriter writer = new(outputFile);
			writer.Write(result);
		} catch (IOException) {
			Console.WriteLine($"[!] The file '{outputFile}' could not be written.\nThis may happen if you do not have permission to write to the current directory or if the file is set as read-only.");
			return;
		}

		// notify the user about the program's execution
		Console.Write("Task 3 done! Press any key to exit...");
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

		foreach (var item in text.Split(' ')) {
			// add to the set every non-empty "word"
			if (item != "") {
				set.Add(item);
			}
		}
	}
}
