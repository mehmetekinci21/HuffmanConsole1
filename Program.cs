// See https://aka.ms/new-console-template for more information
using HuffmanConsole1;

/*
 * Bu örnek kod, klavyeden giriş dizgisindeki her bir karakterin frekansını hesaplar ve Huffman ağacını oluşturur. 
 * Sonuç olarak, her karakterin Huffman kodunu gösteren bir tablo yazdırır.
 * */
string projeDizini = Directory.GetCurrentDirectory();// Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
Console.WriteLine("\n **** Dosyal Yolu...: ***** \n" + projeDizini);


Console.WriteLine("\n Kodlanacak String veriyi giriniz --(Girilmezse varsayılan: hello world) \n");

string input1 = Console.ReadLine();

if (input1 == null || input1 == "")
    input1 = "hello world";

HuffmanTree tree = new();
tree.Build(input1);

Console.WriteLine("Symbol\tFrequency\tHuffman Code");
foreach (var node in tree.EncodedValues.OrderBy(kv => kv.Key))
{
    Console.WriteLine("{0}\t{1}\t\t{2}", node.Key, input1.Count(c => c == node.Key), node.Value);
}

/***
 * Bu kısımda input.txt dosyasını okuyarak huffman algoritması uygulanarak output.txt olarak sıkıştırılmış çıktı 
 * ve decode eden kod işlemi mevcuttur.
***/

Console.WriteLine("\n\n **** Huffman algoritması input.txt dosyasına uygulayarak compressed.bin olarak kaydetme ***\n **** compressed.bin dosyasını tekrar output.txt olarak orjinal haline çevirme ****  \n");
// Reading the input file
string inputFilePath = "input.txt";
string input2 = File.ReadAllText(inputFilePath);

// Creating and building the Huffman tree
//HuffmanTree tree = new HuffmanTree();
tree.Build(input2);

// Encoding the input text and writing to a compressed file
string encodedText = tree.EncodeString(input2);
string compressedFilePath = "compressed.bin";
File.WriteAllBytes(compressedFilePath, ConvertToBytes(encodedText));
Console.WriteLine("--1. Adım: input.txt dosyasıHuffman algoritması ile sıkıştırılatk compressed.bin olarak kaydedildi \n");

// Reading the compressed file and decoding
byte[] compressedBytes = File.ReadAllBytes(compressedFilePath);
string encodedTextFromFile = ConvertToString(compressedBytes);
string decodedText = tree.DecodeString(encodedTextFromFile);

// Writing the decoded text to an output file
string outputFilePath = "output.txt";
File.WriteAllText(outputFilePath, decodedText);
Console.WriteLine("--2. Adım: compresed.bin dosyası output.txt olarak orjinal içerikle kaydedildi \n");
Console.ReadLine();

/**
 * bit dönüşümleri için aşağıdaki metodlar hazırlanmıştır.
 * **/
static byte[] ConvertToBytes(string bits)
{
    int numBytes = bits.Length / 8;
    if (bits.Length % 8 != 0) numBytes++;

    byte[] bytes = new byte[numBytes];
    int byteIndex = 0, bitIndex = 0;

    while (bitIndex < bits.Length)
    {
        byte b = 0;
        for (int i = 0; i < 8; i++)
        {
            if (bitIndex >= bits.Length) break;

            b <<= 1;
            if (bits[bitIndex] == '1') b |= 1;

            bitIndex++;
        }
        bytes[byteIndex++] = b;
    }

    return bytes;
}

    static string ConvertToString(byte[] bytes)
    {
        string bits = "";
        foreach (byte b in bytes)
        {
            for (int i = 7; i >= 0; i--)
            {
                bits += ((b >> i) & 1) == 1 ? '1' : '0';
            }
        }
        return bits;
    }
