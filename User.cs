using SimpleHashing.Net;

namespace GeoQuiz
{
    public class User
    {
        private const int HashingIterations = 100000;

        public string Name { get; set; }
        public string Password { get; set; }
        public string Hash { get; set; }

        public bool CheckPassword(string pwd)
        {
            var simplehash = new SimpleHash();
            return simplehash.Verify(pwd, Hash) == true;
        }

        public void CreateHash()
        {
            var simplehash = new SimpleHash();
            Hash = simplehash.Compute(Password, HashingIterations);
        }
    }
}
