namespace GymApp.Helpers
{
    public static class ProvinceConverter
    {
        public enum Province
        {
            LN,
            CN,
            UL,
            MN
        }
        public static string GetProvince(Province province)
        {
            switch (province)
            {
                case Province.LN:
                    return "Leinster";

                case Province.CN:
                    return "Connacht";

                case Province.UL:
                    return "Ulster";

                case Province.MN:
                    return "Connacht";
            }
            throw new Exception("Province unclear/Unavailable");
        }

        public static Province GetProvinceByName(string name)
        {
            switch (name.ToUpper())
            {
                case "LEINSTER":
                    return Province.LN;

                case "CONNACHT":
                    return Province.CN;

                case "ULSTER":
                    return Province.UL;

                case "MUNSTER":
                    return Province.MN;
            }
            throw new Exception("Province unclear/Unavailable");
        }
    }
}
