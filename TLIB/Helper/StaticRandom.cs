using System;
using System.Collections.Generic;
using System.Text;

namespace TLIB
{
    public static class StaticRandom
    {
        public static Random r = new Random(DateTime.Now.Second);

        //
        // Zusammenfassung:
        //     Gibt eine nicht negative Zufallsganzzahl zurück.
        //
        // Rückgabewerte:
        //     Eine 32-Bit-Ganzzahl mit Vorzeichen, die größer als oder gleich 0 und kleiner
        //     ist als System.Int32.MaxValue.
        public static int Next()
        {
            return r.Next();
        }
        //
        // Zusammenfassung:
        //     Gibt eine Zufallsganzzahl zurück, die in einem angegebenen Bereich liegt.
        //
        // Parameter:
        //   minValue:
        //     Die inklusive untere Grenze der zufälligen Zahl zurückgegeben.
        //
        //   maxValue:
        //     Die exklusive obere Grenze der zufälligen Zahl zurückgegeben. maxValuemuss größer
        //     als oder gleich minValue.
        //
        // Rückgabewerte:
        //     Eine 32-Bit-Ganzzahl mit Vorzeichen größer oder gleich minValue und weniger als
        //     maxValue; ist, enthält des Bereichs der Rückgabewerte minValue , aber nicht maxValue.
        //     Wenn minValue gleich maxValue, minValue wird zurückgegeben.
        //
        // Ausnahmen:
        //   T:System.ArgumentOutOfRangeException:
        //     minValue ist größer als maxValue.
        public static int Next(int minValueInclusiv, int maxValueExclusive, int not = int.MaxValue)
        {
            int ret = 0;
            do
            {
                try
                {
                    ret = r.Next(minValueInclusiv, maxValueExclusive);
                }
                catch (Exception)
                {
                    ret = default;
                }
            } while (ret == not);

            return ret;
        }
        //
        // Zusammenfassung:
        //     Gibt eine nicht negative Zufallsganzzahl zurück, die kleiner als das angegebene
        //     Maximum ist.
        //
        // Parameter:
        //   maxValue:
        //     Die exklusive obere Grenze der Zufallszahl generiert werden soll. maxValuemuss
        //     größer als oder gleich 0 sein.
        //
        // Rückgabewerte:
        //     Eine 32-Bit-Ganzzahl mit Vorzeichen, die größer als oder gleich 0 und kleiner
        //     ist als maxValue; d. h. des Bereichs von Werten in der Regel 0 umfasst jedoch
        //     nicht maxValue. Jedoch wenn maxValue gleich 0 ist, maxValue zurückgegeben wird.
        //
        // Ausnahmen:
        //   T:System.ArgumentOutOfRangeException:
        //     maxValue ist kleiner als 0.
        public static int Next(int maxValue)
        {
            return r.Next(maxValue);
        }
        //
        // Zusammenfassung:
        //     Füllt die Elemente eines angegebenen Bytearrays mit Zufallszahlen.
        //
        // Parameter:
        //   buffer:
        //     Ein Array von Bytes, die Zufallszahlen enthalten.
        //
        // Ausnahmen:
        //   T:System.ArgumentNullException:
        //     buffer ist null.
        public static void NextBytes(byte[] buffer)
        {
            r.NextBytes(buffer);
        }
        //
        // Zusammenfassung:
        //     Gibt eine zufällige Gleitkommazahl zurück, die größer oder gleich 0,0 und kleiner
        //     als 1,0 ist.
        //
        // Rückgabewerte:
        //     Eine Gleitkommazahl mit doppelter Genauigkeit, die größer oder gleich 0,0 und
        //     kleiner als 1,0 ist.
        public static double NextDouble()
        {
            return r.NextDouble();
        }

    }

}
