using System.Globalization;

class Program
{
    public static void Main(String[] args)
    {
        //Directories in the program's Bin folder. Also where the HTML is Output.
        StreamWriter htmlWriter = new StreamWriter(@"..\..\prophunted.html");

        DirectoryInfo d = new DirectoryInfo(@"..\..\Props");
        DirectoryInfo dInv = new DirectoryInfo(@"..\..\Invalid Props");
        string directory = (@"..\..\Props");


        List<String> propList = new List<String>();
        List<String> propListPrimitive = new List<String>();
        List<String> categoryList = new List<String>();
        List<String> invalidPropList = new List<String>();
        Random rand = new Random();

        FileInfo[] Files = d.GetFiles("*.vox", SearchOption.AllDirectories); //Getting VOX Files
        FileInfo[] FilesInvalid = dInv.GetFiles("*.png", SearchOption.AllDirectories); //Getting the invalid images
        string str = "";
        string afterString = "";
        string category = "";
        string lastFolderName = "";
        string fullPath = "";

        // Creates a TextInfo based on the "en-US" culture.
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        foreach (FileInfo fileInvalid in FilesInvalid)
        {
            string invalidName = fileInvalid.ToString();

            //Find Pos of .PNG Extension
            int pngPos = invalidName.IndexOf(".");

            //Get String without .PNG extension and no Underscores
            string strInv = invalidName.Substring(0, pngPos);

            int invalidPos = strInv.IndexOf("Invalid Props");

            //Remove White space, categories, and underscores from the invalid props
            strInv = strInv.Substring(invalidPos + 14);
            strInv = strInv.Replace("_", " ");
            strInv = strInv.Replace("evertides", "");
            strInv = strInv.Replace("cullington", "");
            strInv = strInv.Replace("villa", "");
            strInv = strInv.Replace("vehicle", "");
            strInv = strInv.Replace("woonderland", "");
            strInv = strInv.Replace("quillez", "");
            strInv = strInv.Replace("lee", "");
            strInv = strInv.Replace("hub", "");
            strInv = strInv.Replace("frustrum", "");
            strInv = strInv.Replace("general", "");
            strInv = strInv.Replace("isle", "");
            strInv = strInv.Replace("marina", "");
            strInv = strInv.Replace("hollowrock", "");
            strInv = strInv.Replace("foliage", "");
            strInv = strInv.Replace("tilaggaryd", "");
            strInv = strInv.Replace("objective", "");
            strInv = strInv.Trim();

            //Add invalid props to list.
            invalidPropList.Add(textInfo.ToTitleCase(strInv));
        }

            foreach (FileInfo file in Files)
        {
            fullPath = file.ToString();

            //Get Pos Of First Underscore
            int afterFirstUnderscore = file.Name.IndexOf("_");

            // Get Strings for Category and String without Category
            //Catch for directories without prefix, just use previous folder name in this case.
            //Trim out voxboxes

            try
            {
                if (fullPath.Contains("Voxboxes") || fullPath.Contains("Voxbox's"))
                {
                    continue;
                }
                else
                {
                    category = file.Name.Substring(0, afterFirstUnderscore);
                }
            }
            catch (Exception e)
            {
                fullPath = file.ToString();

                if (fullPath.Contains("Voxboxes") || fullPath.Contains("Voxbox's"))
                {
                    continue;
                }
                else
                {
                    category = lastFolderName = Path.GetFileName(Path.GetDirectoryName(directory));
                }
            }
            afterString = file.Name.Substring(afterFirstUnderscore + 1);

            //Find Pos of .Vox Extension
            int voxPos = afterString.IndexOf(".");

            //Get String without .Vox extension and no Underscores
            str = afterString.Substring(0, voxPos);

            //Add Primitives without .Vox but with Underscores
            propListPrimitive.Add(str);

            //Remove Underscores Afterwards
            str = str.Replace("_", " ");

            //Add props to list.
            
            propList.Add(textInfo.ToTitleCase(str));
            categoryList.Add(textInfo.ToTitleCase(category));
        }

        Console.WriteLine("Find These Props: \n");

        //Start writing the HTML File
        htmlWriter.WriteLine("<!DOCTYPE html>");
        htmlWriter.WriteLine("<html>");
        htmlWriter.WriteLine("<body style=\"background-color:#2c2b3b;\">\n");
        htmlWriter.WriteLine($"<p>");



        for (int i = 0; i < 5; i++)
        {
            Console.Write("Prop ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{i}: ");
            Console.ForegroundColor = ConsoleColor.Green;

            int numberRand = rand.Next(0, propList.Count());

            string propChosen = propList[numberRand];
            string primPropChose = propListPrimitive[numberRand];
            string categoryChosen = categoryList[numberRand];
            CheckInvalid(propChosen, categoryChosen);

            //Make image with fallback incase of invalid prop
            htmlWriter.WriteLine($"\t<object data=\"Images\\{categoryChosen}_{primPropChose}.png\" type=\"image/png\">");
            htmlWriter.WriteLine($"\t\t<img src=\"Invalid.png\">");
            htmlWriter.WriteLine($"\t</object>");


            Console.Write(propChosen);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($" ({categoryChosen})");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        //Finish HTML File
        htmlWriter.WriteLine($"</p>");
        htmlWriter.WriteLine("</body");
        htmlWriter.WriteLine("</html>");

        htmlWriter.Close();

        string CheckInvalid(string passedChosen, string passedCategory)
        {
            for (int j = 0; j < invalidPropList.Count(); j++)
            {
                string data = invalidPropList[j];
                int numberRand = rand.Next(0, propList.Count());

                if (passedChosen.Equals(data))
                {
                    passedChosen = propList[numberRand];
                    RerollCategory(passedCategory, numberRand);
                    CheckInvalid(passedChosen, passedCategory);
                    return passedChosen;
                }
            }
            
            return passedChosen;
        }

        string RerollCategory(string passedCategory, int numberRand)
        {
            passedCategory = categoryList[numberRand];
            return passedCategory;
        }
    }
}