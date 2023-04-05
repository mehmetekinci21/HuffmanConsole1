using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanConsole1
{
    /*
     * Bu örnekte, Huffman ağacı bir öncelik kuyruğu kullanılarak oluşturulur ve her karakterin Huffman kodu, 
     * Encode() yöntemi aracılığıyla hesaplanır. Ayrıca, HuffmanTree sınıfı bir Dictionary<char, string> 
     * nesnesi olan EncodedValues adlı bir özellik içerir. Bu özellik, her karakterin Huffman kodunu 
     * anahtar olarak ve karşılık gelen Huffman kodunu değer olarak içerir.
     * */
    public class HuffmanTree
    {
        private readonly List<HuffmanNode> nodes = new List<HuffmanNode>();
        public HuffmanNode Root { get; set; }
        public Dictionary<char, string> EncodedValues { get; set; }

        public void Build(string input)
        {
            // Count the frequency of each character in the input string
            var charFrequency = input.GroupBy(c => c)
                                     .Select(g => new HuffmanNode { Symbol = g.Key, Frequency = g.Count() })
                                     .OrderBy(n => n.Frequency)
                                     .ToList();

            while (charFrequency.Count > 1)
            {
                // Combine the two nodes with the smallest frequency
                var firstNode = charFrequency.First();
                var secondNode = charFrequency.ElementAt(1);

                var newNode = new HuffmanNode
                {
                    Symbol = '\0',
                    Frequency = firstNode.Frequency + secondNode.Frequency,
                    LeftChild = firstNode,
                    RightChild = secondNode
                };

                charFrequency.RemoveRange(0, 2);
                charFrequency.Add(newNode);
                charFrequency = charFrequency.OrderBy(n => n.Frequency).ToList();
            }

            Root = charFrequency.First();
            EncodedValues = new Dictionary<char, string>();
            Encode(Root, "");
        }

        private void Encode(HuffmanNode node, string code)
        {
            if (node == null) return;

            if (node.Symbol != '\0')
            {
                EncodedValues.Add(node.Symbol, code);
                return;
            }

            Encode(node.LeftChild, code + "0");
            Encode(node.RightChild, code + "1");
        }

        public string EncodeString(string input)
        {
            var encodedText = "";
            foreach (var c in input)
            {
                encodedText += EncodedValues[c];
            }
            return encodedText;
        }

        public string DecodeString(string encodedText)
        {
            var decodedText = "";
            var currentNode = Root;
            foreach (var c in encodedText)
            {
                if (c == '0')
                {
                    currentNode = currentNode.LeftChild;
                }
                else
                {
                    currentNode = currentNode.RightChild;
                }

                if (currentNode.Symbol != '\0')
                {
                    decodedText += currentNode.Symbol;
                    currentNode = Root;
                }
            }
            return decodedText;
        }
    }
}
