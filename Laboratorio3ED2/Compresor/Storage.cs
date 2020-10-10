

namespace PeliculasAPI.Helpers
{
    public class Storage
    {
        private static Storage _instance = null;

        public static Storage Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Storage();

                return _instance;
            }
        }

        
        public string nombreArchivo = "defalut";
        

    }
}