using System;
using System.Text;

namespace JC.Unpacker
{
    class TabHash
    {
        private static Int32 SHA1_BLOCK_LENGTH = 64;
        private static Int32 SHA1_DIGEST_LENGTH = 20;

        class SHA1_Ctx
        {
            public static UInt32[] state = new UInt32[5];
            public static UInt32[] count = new UInt32[2];
            public static Byte[] buffer = new Byte[SHA1_BLOCK_LENGTH];
        }

        private static UInt32 SHIFT(UInt32 dwValue, Int32 dwBits)
        {
            return (dwValue << dwBits) | (dwValue >> (32 - dwBits));
        }

        private static UInt32 BLK(Int32 i, UInt32[] dwBlock)
        {
            return dwBlock[i & 15] = SHIFT(dwBlock[(i + 13) & 15] ^ dwBlock[(i + 8) & 15] ^ dwBlock[(i + 2) & 15] ^ dwBlock[i & 15], 1);
        }

        private static UInt32 BLK0(Int32 i, UInt32[] dwBlock)
        {
            return dwBlock[i];
        }

        private static void R0(UInt32 v, ref UInt32 w, UInt32 x, UInt32 y, ref UInt32 z, Int32 i, UInt32[] dwBlock)
        {
            z += ((w & (x ^ y)) ^ y) + BLK0(i, dwBlock) + 0x5A827999 + SHIFT(v, 5);
            w = SHIFT(w, 30);
        }

        private static void R1(UInt32 v, ref UInt32 w, UInt32 x, UInt32 y, ref UInt32 z, Int32 i, UInt32[] dwBlock)
        {
            z += ((w & (x ^ y)) ^ y) + BLK(i, dwBlock) + 0x5A827999 + SHIFT(v, 5);
            w = SHIFT(w, 30);
        }

        private static void R2(UInt32 v, ref UInt32 w, UInt32 x, UInt32 y, ref UInt32 z, Int32 i, UInt32[] dwBlock)
        {
            z += (w ^ x ^ y) + BLK(i, dwBlock) + 0x6ED9EBA1 + SHIFT(v, 5);
            w = SHIFT(w, 30);
        }

        private static void R3(UInt32 v, ref UInt32 w, UInt32 x, UInt32 y, ref UInt32 z, Int32 i, UInt32[] dwBlock)
        {
            z += (((w | x) & y) | (w & x)) + BLK(i, dwBlock) + 0x8F1BBCDC + SHIFT(v, 5);
            w = SHIFT(w, 30);
        }

        private static void R4(UInt32 v, ref UInt32 w, UInt32 x, UInt32 y, ref UInt32 z, Int32 i, UInt32[] dwBlock)
        {
            z += (w ^ x ^ y) + BLK(i, dwBlock) + 0xCA62C1D6 + SHIFT(v, 5);
            w = SHIFT(w, 30);
        }

        private static void SHA1_Init()
        {
            SHA1_Ctx.state[0] = 0x67452301;
            SHA1_Ctx.state[1] = 0xEFCDAB89;
            SHA1_Ctx.state[2] = 0x98BADCFE;
            SHA1_Ctx.state[3] = 0x10325476;
            SHA1_Ctx.state[4] = 0xC3D2E1F0;
            SHA1_Ctx.count[0] = SHA1_Ctx.count[1] = 0;
        }

        private static void SHA1_Transform(Byte[] lpBuffer, Int32 dwOffset)
        {

            UInt32 a, b, c, d, e;
            UInt32[] dwBlock = new UInt32[16];

            Buffer.BlockCopy(lpBuffer, dwOffset, dwBlock, 0, 64);

            a = SHA1_Ctx.state[0];
            b = SHA1_Ctx.state[1];
            c = SHA1_Ctx.state[2];
            d = SHA1_Ctx.state[3];
            e = SHA1_Ctx.state[4];

            R0(a, ref b, c, d, ref e, 0, dwBlock); R0(e, ref a, b, c, ref d, 1, dwBlock); R0(d, ref e, a, b, ref c, 2, dwBlock); R0(c, ref d, e, a, ref b, 3, dwBlock);
            R0(b, ref c, d, e, ref a, 4, dwBlock); R0(a, ref b, c, d, ref e, 5, dwBlock); R0(e, ref a, b, c, ref d, 6, dwBlock); R0(d, ref e, a, b, ref c, 7, dwBlock);
            R0(c, ref d, e, a, ref b, 8, dwBlock); R0(b, ref c, d, e, ref a, 9, dwBlock); R0(a, ref b, c, d, ref e, 10, dwBlock); R0(e, ref a, b, c, ref d, 11, dwBlock);
            R0(d, ref e, a, b, ref c, 12, dwBlock); R0(c, ref d, e, a, ref b, 13, dwBlock); R0(b, ref c, d, e, ref a, 14, dwBlock); R0(a, ref b, c, d, ref e, 15, dwBlock);
            R1(e, ref a, b, c, ref d, 16, dwBlock); R1(d, ref e, a, b, ref c, 17, dwBlock); R1(c, ref d, e, a, ref b, 18, dwBlock); R1(b, ref c, d, e, ref a, 19, dwBlock);
            R2(a, ref b, c, d, ref e, 20, dwBlock); R2(e, ref a, b, c, ref d, 21, dwBlock); R2(d, ref e, a, b, ref c, 22, dwBlock); R2(c, ref d, e, a, ref b, 23, dwBlock);
            R2(b, ref c, d, e, ref a, 24, dwBlock); R2(a, ref b, c, d, ref e, 25, dwBlock); R2(e, ref a, b, c, ref d, 26, dwBlock); R2(d, ref e, a, b, ref c, 27, dwBlock);
            R2(c, ref d, e, a, ref b, 28, dwBlock); R2(b, ref c, d, e, ref a, 29, dwBlock); R2(a, ref b, c, d, ref e, 30, dwBlock); R2(e, ref a, b, c, ref d, 31, dwBlock);
            R2(d, ref e, a, b, ref c, 32, dwBlock); R2(c, ref d, e, a, ref b, 33, dwBlock); R2(b, ref c, d, e, ref a, 34, dwBlock); R2(a, ref b, c, d, ref e, 35, dwBlock);
            R2(e, ref a, b, c, ref d, 36, dwBlock); R2(d, ref e, a, b, ref c, 37, dwBlock); R2(c, ref d, e, a, ref b, 38, dwBlock); R2(b, ref c, d, e, ref a, 39, dwBlock);
            R3(a, ref b, c, d, ref e, 40, dwBlock); R3(e, ref a, b, c, ref d, 41, dwBlock); R3(d, ref e, a, b, ref c, 42, dwBlock); R3(c, ref d, e, a, ref b, 43, dwBlock);
            R3(b, ref c, d, e, ref a, 44, dwBlock); R3(a, ref b, c, d, ref e, 45, dwBlock); R3(e, ref a, b, c, ref d, 46, dwBlock); R3(d, ref e, a, b, ref c, 47, dwBlock);
            R3(c, ref d, e, a, ref b, 48, dwBlock); R3(b, ref c, d, e, ref a, 49, dwBlock); R3(a, ref b, c, d, ref e, 50, dwBlock); R3(e, ref a, b, c, ref d, 51, dwBlock);
            R3(d, ref e, a, b, ref c, 52, dwBlock); R3(c, ref d, e, a, ref b, 53, dwBlock); R3(b, ref c, d, e, ref a, 54, dwBlock); R3(a, ref b, c, d, ref e, 55, dwBlock);
            R3(e, ref a, b, c, ref d, 56, dwBlock); R3(d, ref e, a, b, ref c, 57, dwBlock); R3(c, ref d, e, a, ref b, 58, dwBlock); R3(b, ref c, d, e, ref a, 59, dwBlock);
            R4(a, ref b, c, d, ref e, 60, dwBlock); R4(e, ref a, b, c, ref d, 61, dwBlock); R4(d, ref e, a, b, ref c, 62, dwBlock); R4(c, ref d, e, a, ref b, 63, dwBlock);
            R4(b, ref c, d, e, ref a, 64, dwBlock); R4(a, ref b, c, d, ref e, 65, dwBlock); R4(e, ref a, b, c, ref d, 66, dwBlock); R4(d, ref e, a, b, ref c, 67, dwBlock);
            R4(c, ref d, e, a, ref b, 68, dwBlock); R4(b, ref c, d, e, ref a, 69, dwBlock); R4(a, ref b, c, d, ref e, 70, dwBlock); R4(e, ref a, b, c, ref d, 71, dwBlock);
            R4(d, ref e, a, b, ref c, 72, dwBlock); R4(c, ref d, e, a, ref b, 73, dwBlock); R4(b, ref c, d, e, ref a, 74, dwBlock); R4(a, ref b, c, d, ref e, 75, dwBlock);
            R4(e, ref a, b, c, ref d, 76, dwBlock); R4(d, ref e, a, b, ref c, 77, dwBlock); R4(c, ref d, e, a, ref b, 78, dwBlock); R4(b, ref c, d, e, ref a, 79, dwBlock);

            SHA1_Ctx.state[0] += a;
            SHA1_Ctx.state[1] += b;
            SHA1_Ctx.state[2] += c;
            SHA1_Ctx.state[3] += d;
            SHA1_Ctx.state[4] += e;
        }

        private static void SHA1_Update(Byte[] lpBuffer, Int32 dwLength)
        {
            UInt32 i, j;

            j = (SHA1_Ctx.count[0] >> 3) & 63;
            if ((SHA1_Ctx.count[0] += (UInt32)dwLength << 3) < (dwLength << 3))
            {
                SHA1_Ctx.count[1]++;
            }

            SHA1_Ctx.count[1] += (UInt32)dwLength >> 29;
            if ((j + dwLength) > 63)
            {
                Buffer.BlockCopy(lpBuffer, 0, SHA1_Ctx.buffer, (Int32)j, (Int32)(i = 64 - j));

                SHA1_Transform(SHA1_Ctx.buffer, 0);
                for (; i + 63 < dwLength; i += 64)
                {
                    SHA1_Transform(lpBuffer, (Int32)i);
                }
                j = 0;
            }
            else
            {
                i = 0;
            }

            Buffer.BlockCopy(lpBuffer, 0, SHA1_Ctx.buffer, (Int32)j, (Int32)(dwLength - i));
        }

        private static Byte[] SHA1_Final()
        {
            Int32 i;
            var finalcount = new Byte[8];
            var result = new Byte[SHA1_DIGEST_LENGTH];
            var some = new Byte[1];

            for (i = 0; i < 8; i++)
            {
                finalcount[i] = (Byte)((SHA1_Ctx.count[i >= 4 ? 0 : 1] >> ((3 - (i & 3)) * 8)) & 255);
            }

            some[0] = 0x80;
            SHA1_Update(some, 1);
            do
            {
                some[0] = 0x00;
                SHA1_Update(some, 1);
            }
            while ((SHA1_Ctx.count[0] & 504) != 448);

            SHA1_Update(finalcount, 8);
            for (i = 0; i < 20; i++)
            {
                result[i] = (Byte)((SHA1_Ctx.state[i >> 2] >> ((3 - (i & 3)) * 8)) & 255);
            }

            return result;
        }

        public static UInt32 iGetHash(String m_String)
        {
            Byte[] lpBuffer = new Byte[128];
            Array.Copy(Encoding.ASCII.GetBytes(m_String), lpBuffer, m_String.Length);

            SHA1_Init();
            SHA1_Update(lpBuffer, lpBuffer.Length);
            var lpHash = SHA1_Final();

            return BitConverter.ToUInt32(lpHash, 0);
        }
    }
}
