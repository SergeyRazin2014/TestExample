using System;
using System.IO;
using System.Threading;
using System.Xml.Serialization;

namespace DAL
{
    public class SerializeXML<T>
    {
        public string PathToFile { get; private set; }

        private Stream _fstream;

        private static Mutex _mutex;

        public SerializeXML(Mutex mutex)
        {
            string appData = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            PathToFile = Path.Combine(appData, "UsersData.txt");

            _mutex = mutex;
        }

        public T Desirializ()
        {
            try
            {
                _mutex.WaitOne();

                XmlSerializer xmlserializer = new XmlSerializer(typeof(T), new Type[] { typeof(T) });

                if (File.Exists(PathToFile))
                {
                    using (_fstream = File.OpenRead(PathToFile))
                    {
                        var temp = (T)xmlserializer.Deserialize(_fstream);

                        return temp;
                    }
                }
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
            throw new Exception("Ошибка при десериализации файла!");
        }

        public void SerializtionWithExchengeFile(T instanse)
        {
            try
            {
                _mutex.WaitOne();

                using (_fstream = new FileStream(PathToFile, FileMode.Create))
                {
                    XmlSerializer xmlserializer = new XmlSerializer(typeof(T), new Type[] { typeof(T) });

                    xmlserializer.Serialize(_fstream, instanse);
                }
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }
    }
}