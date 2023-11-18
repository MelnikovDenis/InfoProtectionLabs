using Services.Static;
namespace Services.Lab6;

public class HuffmanCompressionService
{
    public Action<string>? LogTo { get; set; } = null;
    public Dictionary<byte, bool[]> HuffmanDictionary { get; private set; } = new Dictionary<byte, bool[]>(256);
    private Node Root { get; set; } = null!;
    class Node 
    {
        public byte? Data { get; set; } = null;
        public int Frequncy { get; set; } = 0;
        public Node? LeftChild { get; set; } = null;
        public Node? RightChild { get; set; } = null;
        public Node(byte? data, Node? leftChild, Node? rightChild, int frequncy)
        {
            Data = data;
            LeftChild = leftChild;
            RightChild = rightChild;
            Frequncy = frequncy;
        }
    }
    public Stream Compress(Stream source) 
    {
        source.Position = 0;
        byte[] buffer = new byte[source.Length];
        source.Read(buffer, 0, buffer.Length);
        source.Position = 0;
        //создаём таблицу частот (то, как часто встречается байт)
        var frequencyTable = new Dictionary<byte, int>(256);
        foreach (var sourceByte in buffer)
        {
            if (frequencyTable.ContainsKey(sourceByte))
                ++frequencyTable[sourceByte];
            else
                frequencyTable.Add(sourceByte, 1);
        }
        //создаём очередь приоритетов
        var priorityQueue = new PriorityQueue<Node, int>(from kvp in frequencyTable where kvp.Value != 0 select (new Node(kvp.Key, null, null, kvp.Value), kvp.Value));
        //создаём внутри очереди приоритетов дерево хаффмана
        while (priorityQueue.Count != 1)
        {
            var left = priorityQueue.Dequeue();
            var right = priorityQueue.Dequeue();
            priorityQueue.Enqueue(new Node(null, left, right, left.Frequncy + right.Frequncy), left.Frequncy + right.Frequncy);            
        }
        //создание словаря хаффмана
        Root = priorityQueue.Dequeue();
        GetNewHuffmannDictionary(Root, new LinkedList<bool>());

        if(LogTo != null)
        {
            LogTo("Словарь Хаффмана:\n");
            foreach (var kvp in HuffmanDictionary)
            {
                LogTo($"Ключ: {kvp.Key}\tКод: ");
                foreach (bool bit in kvp.Value)
                {
                    LogTo($"{(bit ? 1 : 0)}");
                }
                LogTo($"\n");
            }
        }
        
        //кодирование и вывод в поток
        var bitStream = new BitStream();        
        foreach (var curByte in buffer) 
            bitStream.Write(HuffmanDictionary[curByte]);
        return bitStream.ToStream();
    }
    public Stream Decompress(Stream source)
    {
        source.Position = 0;
        var bitStream = new BitStream(source);
        var resultStream = new MemoryStream();
        Node? curNode = Root;
        while (!bitStream.IsEnd) 
        {
            bool curBit = bitStream.Read();            
            if (curBit)
                curNode = curNode?.RightChild;
            else
                curNode = curNode?.LeftChild;
            if (curNode == null)
                throw new Exception("В словаре нет нужного кода!");
            if(curNode.Data != null) 
            {                
                resultStream.WriteByte((byte)curNode.Data);
                curNode = Root;
            }
        }
        resultStream.Position = 0;
        return resultStream;
    }   
    public void ResetDictionary() 
    {
         HuffmanDictionary = new Dictionary<byte, bool[]>(256);
    }
    private void GetNewHuffmannDictionary(Node root, IEnumerable<bool> bitCode)
    {
        if (root.Data != null)
        {
            HuffmanDictionary!.Add((byte)root.Data, bitCode.ToArray());
        }
        else
        {
            if (root.LeftChild != null)
            {
                GetNewHuffmannDictionary(root.LeftChild, bitCode.Append(false));
            }
            if (root.RightChild != null)
            {
                GetNewHuffmannDictionary(root.RightChild, bitCode.Append(true));
            }
        }
    }
}