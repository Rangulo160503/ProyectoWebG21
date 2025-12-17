using System.Security.Cryptography;

namespace ProyectoWebG2Api.Security
{
    public static class Contraseña
    {
        // Formato almacenado: v1$<iterations>$<saltBase64>$<hashBase64>
        private const int Iterations = 100_000;
        private const int SaltSize = 16; // 128 bits
        private const int KeySize = 32; // 256 bits

        public static string Hash(string contraseña)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                contraseña,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                KeySize);

            return $"v1${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }

        public static bool Verify(string contraseña, string stored)
        {
            var parts = stored.Split('$');
            if (parts.Length != 4 || parts[0] != "v1") return false;

            var iterations = int.Parse(parts[1]);
            var salt = Convert.FromBase64String(parts[2]);
            var hash = Convert.FromBase64String(parts[3]);

            var test = Rfc2898DeriveBytes.Pbkdf2(
               contraseña,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                hash.Length);

            return CryptographicOperations.FixedTimeEquals(hash, test);
        }
    }
}
