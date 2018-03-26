using System;
using System.Reflection;
using System.IO;

namespace ChangingAssemblyVersion
{
    
    class Program
    {
        
        static void Main(string[] args)
        {

            // since assembly version cannot be changed during runtime with a function, only thing we can do
            // is scan if the version has changed, from assemblyserialization class introduce a bool field
            // that can be read from another function, that will than serialize the assembly and, then, if this
            // application has the permission to it, upload the hash online hehe

            // first we need a file, like AssemblyInfo-last.cs
            try
            {
                string path = Directory.GetCurrentDirectory();
                path = path.Replace("\\bin\\Debug", "\\Properties");
                TextReader realFile = new StreamReader(path + @"\AssemblyInfo.cs");
                TextReader backupFile = null;
                TextWriter writeBackupFile = null;
                try
                {
                    backupFile = new StreamReader(path + @"\AssemblyInfo-last.cs");
                }
                catch
                {
                    writeBackupFile = new StreamWriter(path + @"\AssemblyInfo-last.cs");
                }
                if(backupFile == null)
                {
                    writeBackupFile.Write(realFile.ReadToEnd());
                    writeBackupFile.Close();
                    Console.WriteLine("File has changed! -- HASH THE ASSEMBLY AND SEND");
                    // set that file has changed
                }
                else
                {
                    if(backupFile.ReadToEnd() == realFile.ReadToEnd())
                    {
                        realFile.Close();
                        backupFile.Close();
                        Console.WriteLine("File hasn't changed!");
                        // file did not change
                    }
                    else
                    {
                        backupFile.Close();
                        writeBackupFile = new StreamWriter(path + @"\AssemblyInfo-last.cs");
                        realFile.Close();
                        realFile = new StreamReader(path + @"\AssemblyInfo.cs");
                        writeBackupFile.Write(realFile.ReadToEnd());
                        writeBackupFile.Close();
                        realFile.Close();
                        Console.WriteLine("File has changed! -- HASH THE ASSEMBLY AND SEND");
                        // set that file has changed
                    }
                }
            }
            catch (UnauthorizedAccessException x)
            {
                Console.WriteLine("Couldn't access the directory due to permission issues!");
            }
            catch (NotSupportedException x)
            {
                Console.WriteLine("Oh, you find yourself doing this on a platform that doesn't support this!");
            }
            catch (DirectoryNotFoundException x)
            {
                Console.WriteLine("The directory the stream is looking for doesn't exist!");
            }
            catch (FileNotFoundException x)
            {
                Console.WriteLine("The file we were looking for doesn't exist!");
            }
            catch
            {
                Console.WriteLine("Some problem occured, couldn't specify what!");
            }

            Console.ReadKey();
        }
    }
}
