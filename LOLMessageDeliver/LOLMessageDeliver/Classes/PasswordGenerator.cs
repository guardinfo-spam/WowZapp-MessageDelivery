using System;
using System.Text;

namespace LOLMessageDelivery.Classes
{
    public class PasswordGenerator
    {
        const string CharsAlpha = "AaBbCcDdEeFfGgHhKkMmNnPpQqRrSsTtUuVvWwYyXxZz";
        const string CharsNumeric = "0123456789";
        const string CharsMisc = "@#";

        private readonly Random _random;

        private static PasswordGenerator _instance;

        private PasswordGenerator()
        {
            _random = new Random(Environment.TickCount);
        }


        public static PasswordGenerator Instance
        {
            get { return _instance ?? (_instance = new PasswordGenerator()); }
        }


        public string GenPassword()
        {
            var sb = new StringBuilder();

            AddChars(sb, 5, CharsAlpha);
            AddChars(sb, 2, CharsNumeric);
            sb.Append(CharsMisc);

            return sb.ToString();
        }

        private void AddChars(StringBuilder sb, int count, string source)
        {
            for (int i = 0; i < count; i++)
            {
                sb.Append(source[_random.Next(0, source.Length - 1)]);
            }
        }
    }
}
