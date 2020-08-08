using System.Security.Cryptography;

namespace Rhyze.Tests.Fixtures
{
    internal class FakeHashAlgorithm : HashAlgorithm
    {
        public override void Initialize() { }
        protected override void HashCore(byte[] array, int ibStart, int cbSize) { }

        protected override byte[] HashFinal()
        {
            return new byte[]
             {
                0x01, 0x02, 0x03,
                0x04, 0x05, 0x06,
                0x07, 0x08, 0x09,
                0x0A
             };
        }
    }
}