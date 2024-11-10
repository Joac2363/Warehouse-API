namespace Warehouse_API.Helpers
{
    public class SKUHelper
    {
        Random rnd;
        public SKUHelper(int seed)
        {
            rnd = new Random(seed);
        }
        public SKUHelper()
        {
            rnd = new Random();
        }

        public static bool IsValidSKU(int sku)
        {
            return sku >= 10000000 && sku <= 100000000;
        }
        public void SetSeed(int seed) 
        {
            rnd = new Random(seed);
        }
        public int New() 
        {
            return rnd.Next(10000000, 100000000);
        }
        
    }
}
