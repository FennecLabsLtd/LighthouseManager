using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO.Compression;

namespace LighthouseManager
{
    class Program
    {
        static readonly string ChaperoneFilename = "chaperone_info.vrchap";
        static readonly string LighthouseFilename = "lighthousedb.json";
        class OpenVRConfig
        {
            [JsonProperty("config")]
            public List<string> configPaths;

            [JsonProperty("external_drivers")]
            public List<string> externalDrivers;

            [JsonProperty("jsonid")]
            public string jsonId;

            [JsonProperty("log")]
            public List<string> logPaths;

            [JsonProperty("runtime")]
            public List<string> runtimePaths;

            public int version;
        }

        static void Main(string[] args)
        {
            // TODO: Command Line Args Mode

            if(!Directory.Exists("maps"))
            {
                Directory.CreateDirectory("maps");
            }

            bool doQuit = false;

            while(!doQuit)
            {
                doQuit = !DoMainMenu();
            }
        }

        static void PrintDebug()
        {
            Console.Clear();
            Console.WriteLine("LocalAppData: " + Environment.GetEnvironmentVariable("LocalAppData"));
            OpenVRConfig ovr = GetOpenVRConfig();

            if(ovr == null)
            {
                Console.WriteLine("OpenVR Path File: Not Found");
            } else
            {
                foreach (string s in ovr.configPaths)
                    Console.WriteLine("OpenVR Config Path: " + s);

                foreach (string s in ovr.logPaths)
                    Console.WriteLine("OpenVR Log Path: " + s);

                foreach (string s in ovr.runtimePaths)
                    Console.WriteLine("OpenVR Runtime Path: " + s);

                foreach (string s in ovr.externalDrivers)
                    Console.WriteLine("OpenVR External Drivers: " + s);
            }

            Console.WriteLine("========");
            Console.WriteLine("Press Enter to return to the main menu");
            Console.ReadLine();
        }

        static OpenVRConfig GetOpenVRConfig()
        {
            string jsonPath = Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "openvr", "openvrpaths.vrpath");

            if (!File.Exists(jsonPath))
            {
                Console.WriteLine("Cannot find OpenVR Path File in " + jsonPath + ". Have you ever run SteamVR on this PC before?");
                return null;
            }

            OpenVRConfig configData = JsonConvert.DeserializeObject<OpenVRConfig>(File.ReadAllText(jsonPath));

            if(configData.configPaths == null || configData.configPaths.Count == 0)
            {
                Console.WriteLine("Cannot find a config path entry in OpenVR Paths file.");
                return null;
            }

            Console.WriteLine("Found Config Path: " + configData.configPaths[0]);

            return configData;
        }

        static bool DoMainMenu()
        {
            Console.Clear();
            Console.WriteLine("WARNING: Please close SteamVR before doing this");
            Console.WriteLine("Welcome to the Lighthouse Manager, please select an option from the below: ");
            Console.WriteLine("1) Save Current Room Setup");
            Console.WriteLine("2) Restore Saved Room Setup");
            Console.WriteLine("3) Print Debug Data");
            Console.WriteLine("4) Quit");
            Console.WriteLine("5) View Warranty Disclaimer");
            Console.WriteLine("");

            string input = Console.ReadLine().Trim();

            switch(input)
            {
                case "1":
                    SaveMenu();
                    break;

                case "2":
                    RestoreMenu();
                    break;

                case "3":
                    PrintDebug();
                    break;

                case "4":
                    return false;

                case "5":
                    WarrantyOutput();
                    break;
            }

            return true;
        }

        static void WarrantyOutput()
        {
            Console.Clear();
            Console.WriteLine("THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.");
            Console.WriteLine("Press Enter to return to the main menu");
            Console.ReadLine();
        }

        static string GetChaperonePath(OpenVRConfig config)
        {
            return Path.Combine(config.configPaths[0], ChaperoneFilename);
        }

        static string GetLighthousePath(OpenVRConfig config)
        {
            return Path.Combine(config.configPaths[0], "lighthouse", LighthouseFilename);
        }

        static void SaveMenu()
        {
            Console.Clear();
            OpenVRConfig openVRConfig = GetOpenVRConfig();

            if (openVRConfig == null)
            {
                Console.WriteLine("Press Enter to return to the main menu");
                Console.ReadLine();
                return;
            }

            string chaperonePath = GetChaperonePath(openVRConfig);
            string lighthouseDbPath = GetLighthousePath(openVRConfig);

            bool validFilename = false;

            string filename = "";

            while (!validFilename)
            {
                Console.Write("Please enter a name for the saved room setup config: ");
                filename = Console.ReadLine().Trim() + ".rcfg";

                if (filename == ".rcfg")
                {
                    Console.WriteLine("No filename provided, please try again.");
                    validFilename = false;
                    continue;
                }

                if(File.Exists(Path.Combine("maps", filename)))
                {
                    Console.Write("The file maps/" + filename + " already exists, are you sure you wish to overwrite it? (Y/N): ");
                    string confirmation = Console.ReadLine();

                    if(confirmation.Trim().ToLower() == "y")
                    {
                        validFilename = true;
                    }
                }
            }

            if (ZipConfig(Path.Combine("maps", filename), chaperonePath, lighthouseDbPath))
            {

                Console.WriteLine("Room Setup saved to " + "maps/" + filename + ", press Enter to return to the main menu");
                Console.ReadLine();
            } else
            {
                Console.WriteLine("Error saving room setup, press Enter to return to the main menu");
                Console.ReadLine();
                return;
            }
        }

        static bool ZipConfig(string zipPath, string chaperonePath, string lighthouseDbPath)
        {
            if(!Directory.Exists("temp"))
                Directory.CreateDirectory("temp");

            File.Copy(chaperonePath, Path.Combine("temp", ChaperoneFilename), true);
            File.Copy(lighthouseDbPath, Path.Combine("temp", LighthouseFilename), true);

            if (File.Exists(zipPath))
                File.Delete(zipPath);

            try
            {
                ZipFile.CreateFromDirectory("temp", zipPath);
            } catch(Exception e)
            {
                Console.WriteLine("Error Writing File: " + e.Message);
                return false;
            }

            Directory.Delete("temp", true);

            return true;
        }

        static bool UnzipConfig(string zipPath, string chaperonePath, string lighthouseDbPath)
        {
            if (!Directory.Exists("temp"))
                Directory.CreateDirectory("temp");

            if (!File.Exists(zipPath))
            {
                Console.WriteLine("Cannot find file: " + zipPath);
                return false;
            }

            try
            {
                ZipFile.ExtractToDirectory(zipPath, "temp");

                File.Copy("temp/" + ChaperoneFilename, chaperonePath, true);
                File.Copy("temp/" + LighthouseFilename, lighthouseDbPath, true);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Extracting File: " + e.Message);
                return false;
            }

            Directory.Delete("temp", true);

            return true;
        }

        static void RestoreMenu()
        {
            Console.Clear();

            string[] maps = Directory.GetFiles("maps", "*.rcfg");

            OpenVRConfig openVRConfig = GetOpenVRConfig();

            if (openVRConfig == null)
            {
                Console.WriteLine("Press Enter to return to the main menu");
                Console.ReadLine();
                return;
            }

            string chaperonePath = GetChaperonePath(openVRConfig);
            string lighthouseDbPath = GetLighthousePath(openVRConfig);

            if (maps.Length == 0)
            {
                Console.WriteLine("No room setups found in the maps folder, press Enter to return to the main menu");
                Console.ReadLine();
                return;
            }

            bool correct = false;

            while (!correct)
            {
                Console.WriteLine("The following maps have been found:");

                int i = 1;

                foreach (string m in maps)
                {
                    Console.WriteLine(i + ") " + m.Substring(5, m.Length - 10));
                    i++;
                }

                Console.Write("Enter the number corresponding to the map you wish to restore on this PC: ");
                string input = Console.ReadLine();

                int inputInt;

                if(!int.TryParse(input, out inputInt) || inputInt > maps.Length)
                {
                    Console.WriteLine("Invalid input, please try again.");
                    continue;
                }

                inputInt--;

                Console.Write("Map '" + maps[inputInt] + " selected, is this correct? (Y/N): ");

                string confirmation = Console.ReadLine();

                if(confirmation.Trim().ToLower() != "y")
                {
                    continue;
                }

                Console.Write("WARNING: This will overwrite your existing room setup, are you sure you wish to proceed? (Y/N): ");
                confirmation = Console.ReadLine();

                if (confirmation.Trim().ToLower() != "y")
                {
                    continue;
                }

                if(UnzipConfig(maps[inputInt], chaperonePath, lighthouseDbPath))
                {
                    Console.WriteLine("Room Setup extracted successfully, your universes are now in-sync! Press enter to return to the main menu.");
                    Console.ReadLine();
                    return;
                } else
                {
                    Console.WriteLine("Failed to extract room setup, press Enter to return to the main menu.");
                    Console.ReadLine();
                    return;
                }
            }
        }
    }
}
