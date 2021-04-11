using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

class Task2 {
	/// <summary>
	/// Entry point.
	/// </summary>
	static void Main() {
		// create a SortedDictionary for storing the text
		// it will contain: as keys - each dependency "word";
		// 					as values - a SortedSet of dependencies
		SortedDictionary<string, SortedSet<string>> dict = new();

		// read the input file line by line and add each "word" to the dictionary
		string inputFile = "deps.in";
		try {
			using StreamReader reader = new(inputFile);
			string line;
			while ((line = reader.ReadLine()) != null) {
				SplitStringToDictionary(line, ref dict);
			}
		} catch (IOException) {
			Console.WriteLine($"[!] The file '{inputFile}' could not be read.\nCheck if the file '{inputFile}' exists in the current directory and then try again.");
			return;
		} catch (Exception e) {
			Console.WriteLine("[!] " + e.Message);
			return;
		}

		// add the secondary dependencies to the resulting dictionary
		foreach (var item in dict) {
			// 1. build a set with secondary dependencies, for every value
			SortedSet<string> temp = new();
			foreach (var value in item.Value) {
				foreach (var subvalue in dict[value]) {
					temp.Add(subvalue);
				}
			}
			// 2. add the set to the existing one corresponding to the current key
			dict[item.Key].UnionWith(temp);
		}

		// add the ordered items to a StringBuilder, since there are potentially many concatenations to be made
		StringBuilder result = new();
		foreach (var item in dict) {
			StringBuilder values = new();
			if (item.Value != null) {
				foreach (var val in item.Value) {
					values.Append(" " + val);
				}
			}
			result.Append(item.Key + values + "\n");
		}

		// remove the last newline
		result = result.Remove(result.Length - 1, 1);

		// print the resulting string to the output file in lexicographical order
		string outputFile = "task2.out";
		try {
			using StreamWriter writer = new(outputFile);
			writer.Write(result);
		} catch (Exception) {
			Console.WriteLine($"[!] The file '{outputFile}' could not be written.\nThis may happen if you do not have permission to write to the current directory or if the file is set as read-only.");
			return;
		}

		// notify the user about the program's execution
		Console.Write("Task 2 done! Press any key to exit...");
		Console.ReadKey();
	}

	/// <summary>
	/// Processes a line of text (by removing unnecessary spaces) and adds each non-empty
	/// "word" to the supplied SortedDictionary, first as key, then as corresponding primary dependencies
	/// for each key.
	/// </summary>
	/// <param name="text">the input text</param>
	/// <param name="dict">a SortedDictionary passed by reference</param>
	static void SplitStringToDictionary(string text, ref SortedDictionary<string, SortedSet<string>> dict) {
		// remove extra spaces: trim + compact
		text = Regex.Replace(text.Trim(), @"\s{2,}", " ");

		// progressively fill the dictionary with keys and primary dependencies
		string[] list = text.Split(' ');
		foreach (var item in list) {
			// ignores empty lines and adds the keys (every "word" will get its own key)
			if (item != "") {
				dict.TryAdd(item, new());
			}

			// adds the values corresponding to the key (first "word" in the text line)
			if (item != list[0]) {
				dict[list[0]].Add(item);
			}
		}
	}
}
