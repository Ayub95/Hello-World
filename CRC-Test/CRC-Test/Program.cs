using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MPTSRV.TrackViewer;

namespace CRC_Test
{
    enum IconTouchdown { BottomMiddle = 0, MiddleCenter = 13 }

    class Point3D
    {
        public int x;
        public int y;
        public int z;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }

    enum TankShape { None, Parallelepiped, Cylinder };

    class Program
    {
        static void Main(string[] args)
        {
            /*int fuel = GetFuelVolume1(78, 100.0);
            fuel = GetFuel(78);*/
            int fuel = GetFuelVolume(75);
            

            Console.WriteLine(fuel.ToString());
            //Console.WriteLine(b.ToString("X"));

            Console.Read();
        }

        private const double _maximumLevel = 100.0;
        private const double _radius = 50;
        private const double _radiusSquared = _radius * _radius;
        private const double _circleArea = Math.PI * _radiusSquared;
        private static int _volume = 500;

        public static int GetFuelVolume(int fuelLevel)
        {
            //if ((_shape == TankShape.None) || (fuelLevel == 0xFF))
                //return null;

            if (false)
            {
                double theta = 2 * Math.Acos(1 - fuelLevel / _radius);
                double sPart = 0.5 * _radiusSquared * (theta - Math.Sin(theta));
                return (int)((sPart / _circleArea) * _volume);
            }
            else
            {
                return (int)((fuelLevel / _maximumLevel) * _volume);
            }
        }

        public static int GetFuelVolume1(int unit, double maxUnit)
        {

            double sFull = Math.PI;// *Math.Pow(Radius, 2);                
            double x = (unit / maxUnit) * 2 * 1;

            double gamma = Math.Asin(x - 1);
            double alpha = 2 * gamma + Math.PI;
            double sTriangle = Math.Sin(2 * Math.PI - alpha) / 2;

            double sPart = (alpha / 2) + sTriangle;

            return Convert.ToInt32(500 * (sPart / sFull));
        }

        public static int GetFuel(int unit)
        {
            double R = 50;
            double gamma = 2 * Math.Acos(1 - unit / R);
            double S = 0.5 * R * R * (gamma - Math.Sin(gamma));
            double S_full = Math.PI * R * R;

            return Convert.ToInt32((S / S_full * 500));
        }

        static int getCrc(int x)
        {
            x += 6;
            return x;
        }

        byte crc80(byte[] data, byte size)
        {
            byte crc, i, j;
            crc = 0x00;
            j = 0;
            while (size-- > 0)
            {
                crc ^= data[j++];// *data++;

                for (i = 0; i < 8; i++)
                {
                    if ((crc & 0x80) > 0) crc = (byte)((crc << 1) ^ 0x31);
                    else crc <<= 1;
                }
            }

            return crc;
        }

        /*
        #define CRC8INIT 0x00
#define CRC8POLY 0x31 // = X^8+X^5+X^4+X^0

uint8_t crc8(uint8_t *data, uint16_t size)
{
    uint8_t crc, i;
    crc = CRC8INIT;

    while (size--)
    {
        crc ^= *data++;

        for (i = 0; i < 8; i++)
        {
            if (crc & 0x80) crc = (crc << 1) ^ CRC8POLY;
            else crc <<= 1;
        }
    }

    return crc;
}
         * */

        private static byte crc8(byte[] addr, int len)
        {
            byte crc = 0;

            for (int i = 0; i < len; i++)
            {
                byte inbyte = addr[i];
                for (int j = 0; j < 8; j++)
                {
                    byte mix = (byte)((crc ^ inbyte) & 0x01);
                    crc >>= 1;
                    if (mix > 0)
                        crc ^= 0x8C;

                    inbyte >>= 1;
                }
            }
            return crc;
        }

        private static byte[] crc8_tbl = 
        { 
            0,  94, 188, 226,  97,  63, 221, 131, 194, 156, 126,  32, 163, 253,  31,  65,
            157, 195,  33, 127, 252, 162,  64,  30,  95,   1, 227, 189,  62,  96, 130, 220,
            35, 125, 159, 193,  66,  28, 254, 160, 225, 191,  93,   3, 128, 222,  60,  98,
            190, 224,   2,  92, 223, 129,  99,  61, 124,  34, 192, 158,  29,  67, 161, 255,
            70,  24, 250, 164,  39, 121, 155, 197, 132, 218,  56, 102, 229, 187,  89,   7,
            219, 133, 103,  57, 186, 228,   6,  88,  25,  71, 165, 251, 120,  38, 196, 154,
            101,  59, 217, 135,   4,  90, 184, 230, 167, 249,  27,  69, 198, 152, 122,  36,
            248, 166,  68,  26, 153, 199,  37, 123,  58, 100, 134, 216,  91,   5, 231, 185,
            140, 210,  48, 110, 237, 179,  81,  15,  78,  16, 242, 172,  47, 113, 147, 205,
            17,  79, 173, 243, 112,  46, 204, 146, 211, 141, 111,  49, 178, 236,  14,  80,
            175, 241,  19,  77, 206, 144, 114,  44, 109,  51, 209, 143,  12,  82, 176, 238,
            50, 108, 142, 208,  83,  13, 239, 177, 240, 174,  76,  18, 145, 207,  45, 115,
            202, 148, 118,  40, 171, 245,  23,  73,   8,  86, 180, 234, 105,  55, 213, 139,
            87,   9, 235, 181,  54, 104, 138, 212, 149, 203,  41, 119, 244, 170,  72,  22,
            233, 183,  85,  11, 136, 214,  52, 106,  43, 117, 151, 201,  74,  20, 246, 168,
            116,  42, 200, 150,  21,  75, 169, 247, 182, 232,  10,  84, 215, 137, 107,  53
        };

        /// <summary>
        /// CRC8 массива байтов
        /// </summary>
        /// <param name="bf">массив байтов</param>
        /// <returns></returns>
        public static byte crc80(byte[] bf, int l)
        {
            byte crc;
            int len = l;// bf.Length;
            crc = 0x0;
            int i = 0;
            do
            {
                crc = crc8_tbl[bf[i++] ^ crc];
            } while (--len != 0);

            return crc;
        }
    }
}
