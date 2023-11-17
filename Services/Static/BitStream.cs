namespace Services.Static;

public class BitStream
{
    private const byte _one = (byte)0b00000001; //1
    private const byte _max = (byte)0b10000000; //255
    public byte Buffer { get; set; } = _one; //битовый буфер
    private int BufferBitCount { get => 7 - ReverseMarkerPosition; } //количество бит в буфере
    private int ReverseMarkerPosition { get; set; } = 7; //положение единички отделяющей пустые биты в буфере, от значащих (отсчёт от старшего разряда)
    public int CompleteByteLength { get => Storage.Count; } //количество полных байт
    public int BitLength { get => CompleteByteLength * 8 + BufferBitCount; } //количество установленных бит вместе с битами в буфере
    public int ByteLength { get => BufferBitCount == 0 ? CompleteByteLength : CompleteByteLength + 1; } //количество байт вместе с буфером
    public int AbsoluteBitPosition { get; private set; } = 0; //текущий читаемый бит
    public bool IsEnd { get => AbsoluteBitPosition == BitLength; } //признак конца потока
    public List<byte> Storage { get; private set; } //хранилище
    //создать пустой битовый поток
    public BitStream() 
    {
        Storage = new List<byte>(100);
    }
    //создать битовый поток с данными из стрима
    public BitStream(Stream source)
    {
        source.Position = 0;
        byte[] buffer = new byte[source.Length - 1];
        source.Read(buffer, 0, buffer.Length);
        Buffer = (byte)source.ReadByte();
        source.Position = 0;
        Storage = new List<byte>(buffer);
        for(int i = 0; i < 8; ++i) 
        {
            if (GetBitReverseIndex(Buffer, i))
            {
                ReverseMarkerPosition = i;
                break;
            }
        }                            
    }
    //создать новый байтовый поток из битового
    public Stream ToStream()
    {
        return new MemoryStream(Storage.AsEnumerable().Append(Buffer).ToArray());
    }
    //записать бит
    public void Write(bool bitValue)
    {
        AddToBuffer(bitValue);
        if (ReverseMarkerPosition == -1)
        {
            Storage.Add(Buffer);
            ResetBuffer();
        }
    }
    //записать массив бит
    public void Write(bool[] bitArray)
    {
        foreach (var bit in bitArray)
            Write(bit);
    }
    //считать бит
    public bool Read()
    {
        int localBytePostition = AbsoluteBitPosition / 8;
        int localBitPosition = AbsoluteBitPosition % 8;
        bool result;
        if (localBytePostition < Storage.Count)
            result = GetBitReverseIndex(Storage[localBytePostition], localBitPosition);
        else
            result = GetBitReverseIndex(Buffer, ReverseMarkerPosition + localBitPosition + 1);
        ++AbsoluteBitPosition;
        return result;
    }
    //сбросить битовый отступ
    public void ResetBitOffset() 
    {
        AbsoluteBitPosition = 0;
    }
    //добавить в буфер бит
    private void AddToBuffer(bool bitValue)
    {
        Buffer <<= 1;
        if (bitValue)
            Buffer += _one;
        --ReverseMarkerPosition;
    }
    //сбросить буфер
    private void ResetBuffer()
    {
        Buffer = _one;
        ReverseMarkerPosition = 7;
    }     
    //получить конкретный бит из байта (индексация идёт с 0 начиная с младшего разряда)
    public static bool GetBit(byte source, int index)
        => (source >> (index % 8) & _one) == _one;
    //получить конкретный бит из байта (индексация идёт с 0 начиная со старшего разряда)
    public static bool GetBitReverseIndex(byte source, int index)
        => (source << (index % 8) & _max) == _max;
    //установить конкретный бит в байте (индексация идёт с 0 начиная с младшего разряда)
    public static void SetBit(ref byte source, int index, bool bitValue) 
    {
        byte operand = (byte)(_one << (index % 8));
        if(bitValue)
            source |= operand;
        else
            source &= (byte)~operand;
    }
    //установить конкретный бит в байте (индексация идёт с 0 начиная со старшего разряда)
    public static void SetBitReverseIndex(ref byte source, int index, bool bitValue)
    {
        byte operand = (byte)(_max >> (index % 8));
        if (bitValue)
            source |= operand;
        else
            source &= (byte)~operand;
    }
}
