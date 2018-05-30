	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures_Algorithms.Project1;

namespace DataStructures_Algorithms.Project2
{
	// Material
	// https://en.wikipedia.org/wiki/Huffman_coding
	// https://www.siggraph.org/education/materials/HyperGraph/video/mpeg/mpegfaq/huffman_tutorial.html
	// http://www.geeksforgeeks.org/greedy-algorithms-set-3-huffman-coding/
	public class HuffmanNode
	{
		public HuffmanNode Left { get; set; } = null;
		public HuffmanNode Right { get; set; } = null;
		public char Symbol { get; set; }
		public int Value { get; set; }
	}
	public class HuffmanTree
	{
		public HuffmanNode Root { get; set; } = null;
		private Dictionary<char, string> encodeData = new Dictionary<char, string>();

		public void Build(Vector<char> vector)
		{
            //Get our frequencies
			var dict = getFrequencyTable(vector).OrderBy(values => values.Value);
			List<HuffmanNode> nodes = new List<HuffmanNode>();
            //Build nodes based on those frequencies
			foreach (KeyValuePair<char, int> nodeData in dict)
			{
				HuffmanNode node = new HuffmanNode();
				node.Symbol = nodeData.Key;
				node.Value = nodeData.Value;
				nodes.Add(node);
			}
			while (nodes.Count > 1)
			{
				//Get lowest vals
				HuffmanNode smallest = nodes[0];
				HuffmanNode nextSmallest = nodes[1];
				//Create parent
				HuffmanNode parent = new HuffmanNode();
				//Set value and children
				parent.Value = smallest.Value + nextSmallest.Value;
				parent.Left = smallest;
				parent.Right = nextSmallest;
				//Add parent to top of queue
				nodes.Add(parent);

				//Remove values from list
				nodes.Remove(smallest);
				nodes.Remove(nextSmallest);
				//Reorder list to maintain integrity
				nodes = nodes.OrderBy(values => values.Value).ToList();
			}
			Root = nodes[0];//This is top of tree

		}
        //Recursive method to build codes, starting at top node
		public Dictionary<char, string> getEncodeMap(HuffmanNode top, string result)
		{
			if (top.Left != null)
			{
				getEncodeMap(top.Left, result + "0");
			}
			if (top.Right != null)
			{
				getEncodeMap(top.Right, result + "1");
			}
            //We've reached a leaf
			if (top.Left == null)
			{
				if(!encodeData.ContainsKey(top.Symbol))
					encodeData.Add(top.Symbol, result);
			}
			return encodeData;
		}
		private Dictionary<char, int> getFrequencyTable(Vector<char> vector)
		{
            //Data structure for our frequencies table
			Dictionary<char, int> freq = new Dictionary<char, int>();
			foreach (char symbols in vector)
			{
				//Check if character exists, increment frequency
				if (freq.ContainsKey(symbols))
				{
					freq[symbols] += 1;
				}
				//Otherwise add char occurance
				else
				{
					freq.Add(symbols, 1);
				}
			}
			//Return populated table
			return freq.Keys.Count > 0 ? freq : null;
		}

	}
	public class HuffmanCoding
	{
		private HuffmanTree tree = new HuffmanTree();

		public Vector<string> Encode(Vector<char> input)
		{
			//Data store fore codes
			Vector<string> codes = new Vector<string>();
			//Generate data by building tree
			tree.Build(input);
			//If encoded successfully, root is not null
			if (tree.Root != null)
			{
				//get our code routes
				Dictionary<char, string> encodeMap = tree.getEncodeMap(tree.Root, "");
				//Loop over all characters given
				foreach (char chars in input)
				{
					//If character has a value in map
					//Add the value to results
					if (encodeMap[chars] != null)
					{
						codes.Add(encodeMap[chars]);
					}
				}
			}
			return codes;

		}

		public Vector<char> Decode(Vector<string> input)
		{
			Vector<char> characters = new Vector<char>();
			//Swap keys and values
			Dictionary<string, char> encodeMap = tree.getEncodeMap(tree.Root, "").ToDictionary(kp => kp.Value, kp => kp.Key);
			//If a tree exists, use that to decode
			if (tree.Root != null)
			{
				//For each input code check our map to see if code exists
				foreach (string codes in input)
				{
						//If codes are equal add value
						if (encodeMap.ContainsKey(codes))
						{
							characters.Add(encodeMap[codes]);
						}
				}
			}
            //return populated list
			return characters.Count > 0 ? characters : null;
		}
	}
}
