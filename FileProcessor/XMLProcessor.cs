using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessor
{
    public class XMLProcessor
    {
        public static List<DeploymentData> DeploymentData = new List<DeploymentData>();
        public static void Process(string directory)
        {
            var XmlFiles = Directory.GetFiles(directory, "*.xml", SearchOption.AllDirectories);
            foreach (var XmlFile in XmlFiles)
            {
                var fileProcessor = new FileProcessor();
                DeploymentData data = fileProcessor.Process(XmlFile);
                DeploymentData.Add(data);
            }
        }
        
    }
}
