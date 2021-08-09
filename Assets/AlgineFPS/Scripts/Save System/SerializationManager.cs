using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;

namespace Algine
{
    public class SerializationManager
    {
        public static bool Save(string saveName , object saveData)
        {
            BinaryFormatter binaryFormatter = GetBinaryFormatter();

            if (!Directory.Exists(Application.persistentDataPath + "/saves"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/saves");
            }

            string path = Application.persistentDataPath
                + "/saves/" + saveName + ".dat";

            FileStream file = File.Create(path);
            binaryFormatter.Serialize(file, saveData);
            file.Close();

            return true;

        }

        public static object Load(string saveName)
        {
            string path = Application.persistentDataPath
                + "/saves/" + saveName + ".dat";

            if (!File.Exists(path))
            {
                return null;
            }

            BinaryFormatter binaryFormatter = GetBinaryFormatter();

            FileStream file = File.Open(
                path, FileMode.Open);
            try
            {
                object save = binaryFormatter.Deserialize(file);
                file.Close();
                return save;
            }
            catch
            {

                Debug.LogErrorFormat("Failed to load " +
                    "file at {0}", path);
                file.Close();
                return null;
            }

        }




        private static BinaryFormatter GetBinaryFormatter()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            return binaryFormatter;
        }
    }
}

