using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Logic
{
    public static class SaveLoadManager
    {
        private static readonly string Path = Application.persistentDataPath + "/savedRun.fk";
    
        public static void SaveRun(Run run) 
        {
            var bf = new BinaryFormatter();
            var stream = new FileStream(Path, FileMode.Create);

            bf.Serialize(stream, run);
            stream.Close();
        }
    
        public static Run LoadRun()
        {
            if (File.Exists(Path))
            {
                var bf = new BinaryFormatter();
                var stream = new FileStream(Path, FileMode.Open);
                var run = (Run)bf.Deserialize(stream);
                stream.Close();
                return run;
            }
            else
            {
                Debug.Log("Error loading Run");
                return null;
            }
        }
    }
}
