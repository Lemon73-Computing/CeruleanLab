using System.Numerics;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        float[] a = new float[1000];
        float[] b = new float[1000];

        // Init
        for (int i = 0; i < a.Length; i++)
        {
            a[i] = i;
            b[i] = i * 2;
        }

        Stopwatch sw = Stopwatch.StartNew(); // 時間測定
        for (int i = 0; i < 1000 * 1000; i++)
        {
            _ = CalculatorScaler(a, b); // 処理
        }
        sw.Stop(); // 測定終了
        int scalerTime = (int)sw.ElapsedMilliseconds;
        Console.WriteLine($"スカラー計算: \t{scalerTime} ms");

        sw = Stopwatch.StartNew(); // 時間測定
        for (int i = 0; i < 1000 * 1000; i++)
        {
            _ = CalculatorSIMD(a, b); // 処理
        }
        sw.Stop(); // 測定終了
        int simdTime = (int)sw.ElapsedMilliseconds;
        Console.WriteLine($"SIMD計算: \t{simdTime} ms");

        double improvement = 1.0 - (double)simdTime / scalerTime;
        Console.WriteLine($"{improvement:P2} 速度改善");
    }

    /// <summary>
    /// スカラー計算
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    static float[] CalculatorScaler(float[] a, float[] b)
    {
        float[] result = new float[1000];

        for (int i = 0; i < a.Length; i++)
        {
            result[i] = a[i] + b[i];
        }

        return result;
    }

    /// <summary>
    /// SIMD を用いた計算
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    static float[] CalculatorSIMD(float[] a, float[] b)
    {
        float[] result = new float[1000];

        int simdLength = Vector<float>.Count; // SIMD幅（例: 4, 8 など）

        int iSimd = 0;
        for (; iSimd <= a.Length - simdLength; iSimd += simdLength)
        {
            var va = new Vector<float>(a, iSimd);
            var vb = new Vector<float>(b, iSimd);

            var vr = va + vb;

            vr.CopyTo(result, iSimd);
        }

        // 余り処理
        for (; iSimd < a.Length; iSimd++)
        {
            result[iSimd] = a[iSimd] + b[iSimd];
        }

        return result;
    }
}
