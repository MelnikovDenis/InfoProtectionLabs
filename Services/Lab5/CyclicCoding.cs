using System.Collections;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using Services.Static;
namespace Services.Lab5;
//циклическое кодирование
public class CyclicCoding
{
    public int n { get; private set; } = 7; //кол-во выходных бит
    public int k { get; private set; } = 4; //кол-во входных (информационных) бит
    public int r { get => n - k; } //кол-во проверочных бит
    public BitArray GeneratingPolynomial { get; private set; } = new BitArray(new bool[] { true, true, false, true }); //порождающий полинов g(x) = g0 + g1*x + g3*x^3
    private Dictionary<BitArray, BitArray> SyndromeTable { get; set; } = new Dictionary<BitArray, BitArray>(new BitArrayComparer());
    public CyclicCoding() 
    {
        GetSyndromeTable();
    }
    public BitArray Encode(bool[] source) 
    {     
        if (source.Length != k)
            throw new Exception($"Количество входных бит должно быть равно {k}");
        
        var extSource = new BitArray(source.Concat(new bool[r]).ToArray()); //расширенные нулями входные данные
        var registers = new BitArray(r); //регистры
        var s = new BitArray(n); //выходной код

        for (int i = 0; i < n; ++i) 
        { 
            //кодирование пока работает только для этого конкретного полинома (потом переделать)
            s[i] = extSource[i] ^ registers[0] ^ registers[2];
            registers = registers.CycleShift(-1);
            registers[0] = extSource[i];
        }
        return s;
    }
    public BitArray Decode(bool[] source, out BitArray modulo) 
    {
        if (source.Length != n)
            throw new Exception($"Количество входных бит должно быть равно {n}");

        var revSource = new BitArray(source.Reverse().ToArray()); //инвертированные входные данные
        var registers = new BitArray(r); //регистры
        var a = new BitArray(n); //выходные данные

        registers[0] = revSource[0] ^ a[0]; //подаём на вход первый символ
        var lastR0 = registers[0]; //запоминаем предыдущее значение первого символа
        for (int i = 1; i < n; ++i)
        {
            //декодирование пока работает только для этого конкретного полинома (потом переделать)
            a[i] = registers[2];
            registers[2] = registers[1];
            registers[0] = revSource[i] ^ a[i];
            registers[1] = lastR0 ^ a[i];
            lastR0 = registers[0];
        }
        modulo = registers;
        return new BitArray(a.ToBoolArray().Reverse().ToArray()); //перевёрнутый результат (расширенный нулями)
    }
    public bool[] Decode(BitArray source) 
    {
        BitArray modulo;
        var decoded = Decode(source.ToBoolArray(), out modulo);
        
        if (BitArrayExtension.BitArrayEquals(modulo, new BitArray(r, false)))
        {
            var result = new bool[k];
            for(int i = 0; i < k; ++i) 
            {
                result[i] = decoded[i];
            }
            return result;
        }
        else 
        {
            var fixedSource = new BitArray(source);
            return Decode(fixedSource.Xor(SyndromeTable[modulo]));
        }
    }
    private void GetSyndromeTable() 
    {
        var source = new bool[] { false, false, false, false };
        var s = Encode(source);
        for (int i = 0; i < s.Length; i++)
        {
            //канал передачи с ошибкой
            var errorTransmitter = s.ToBoolArray();
            errorTransmitter[i] ^= true;
            //вектор ошибок
            var errorVector = new BitArray(n, false);
            errorVector[i] = true;
            //остаток от деления
            BitArray modulo;
            Decode(errorTransmitter, out modulo);
            SyndromeTable.Add(modulo, errorVector);
        }
    }
    private class BitArrayComparer : IEqualityComparer<BitArray>
    {
        public bool Equals(BitArray x, BitArray y)
        {
            if (x == null || y == null)
                return false;
            if (x.Length != y.Length)
                return false;
            return x.ToBoolArray().SequenceEqual(y.ToBoolArray());
        }
        public int GetHashCode(BitArray obj)
        {
            uint hash = 0;
            for(int i = 0; i < obj.Length; ++i) 
            {
                hash += obj[i] ? (uint)Math.Pow(2, i) : 0;
            }
            return (int)hash;
        }
    }
}