namespace Common
{
    public static class GenerateCode
    {
        public static string GenCode(string CodeOld)
        {
            string CodeNew;
            switch (CodeOld.Length)
            {
                case 3:
                    CodeNew = CodeOld + "0000000001";
                    break;
                default:
                    CodeNew = HandleCode(CodeOld);
                    break;
            }   
            return CodeNew;
        }

        private static string HandleCode(string code)
        {
            string prefix = code.Substring(0, 3);
            string numberString = code.Substring(3);
            int number = int.Parse(numberString);  
            number++;

            // Chuyển số thành chuỗi và đảm bảo có độ dài 10 ký tự với số 0 đứng trước (0000000235)
            string incrementedNumberString = number.ToString("D10");

            string result = prefix + incrementedNumberString;

            return result;
        }
    }
}
