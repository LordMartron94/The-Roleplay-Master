using Newtonsoft.Json;

namespace MD.Common
{
    /// <summary>
    /// A system (repository) for dealing with saving and loading.
    /// </summary>
    /// <typeparam name="T">The data-structure type of the information stored.</typeparam>
    public class JsonRepository<T> where T: struct
    {
        private string _jsonPath;

        /// <param name="jsonPath">The path to the json file of the storage.</param>
        /// <param name="createDirectories">Should the system create the folders if they do not exist. Default true.</param>
        /// <param name="createFile">Should the system create the file if it does not exist. Default true.</param>
        public JsonRepository(string jsonPath, bool createDirectories=true, bool createFile=true)
        {
            bool valid = Verify(jsonPath, createDirectories, createFile);

            if (valid)
                _jsonPath = jsonPath;
        }

        /// <summary>
        /// Verifies that the path is valid.
        /// </summary>
        /// <param name="jsonPath">The path to the json file of the storage.</param>
        /// <param name="createDirectories">Should the system create the folders if they do not exist. Default true.</param>
        /// <param name="createFile">Should the system create the file if it does not exist. Default true.</param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException">If the directory can't be found and createDirectories is false.</exception>
        /// <exception cref="Exception">If the file is not a valid Json file.</exception>
        /// <exception cref="FileNotFoundException">If the file can't be found and createFile is false.</exception>
        private bool Verify(string jsonPath, bool createDirectories, bool createFile)
        {
            FileInfo fileInfo = new FileInfo(jsonPath);

            // Verify directory exists.
            if (!Directory.Exists(fileInfo.Directory.FullName))
            {
                if (!createDirectories)
                    throw new DirectoryNotFoundException($"Folder {fileInfo.Directory.FullName} does not exist!");

                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }

            // Verify file is valid json.
            if (fileInfo.Extension != ".json")
                throw new Exception($"File {fileInfo.FullName} is not a json file!");

            // Verify file exists.
            if (!File.Exists(fileInfo.FullName))
            {
                if (!createFile)
                    throw new FileNotFoundException($"File {fileInfo.FullName} does not exist!");

                // Close the connection directly after creating the file, or we can run into issues later.
                FileStream writer = File.Create(fileInfo.FullName);
                writer.Close();
            }

            return true;
        }

        /// <summary>
        /// Saves a data struct to the json file.
        /// </summary>
        /// <param name="data">The data struct.</param>
        public void SaveToFile(T data)
        {
            // Opens new stream writer, writes the json string to the file, and then closes the connection.
            using StreamWriter streamWriter = new StreamWriter(_jsonPath);
            
            // Convert object to valid json string.
            string jsonString = JsonConvert.SerializeObject(data, Formatting.Indented); 
            
           streamWriter.Write(jsonString);
        }

        /// <summary>
        /// Loads a data struct from the json file.
        /// </summary>
        /// <returns>The data struct.</returns>
        public T? LoadFromFile()
        {
            // Opens new stream reader, reads the json string from the file, and then closes the connection.
            using StreamReader streamReader = new StreamReader(_jsonPath);
            
            T? data = JsonConvert.DeserializeObject<T>(streamReader.ReadToEnd()); 

            return data;
        }
    }
}