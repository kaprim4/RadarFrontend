using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessor
{
    public class XMLProcessor
    {
        public static TreatmentDTO deploymentData = new();
        public static void Process(string directory)
        {
            var XmlFiles = Directory.GetFiles(directory, "*.xml", SearchOption.AllDirectories);
            foreach (var XmlFile in XmlFiles)
            {
                var fileProcessor = new FileProcessor();
                DeploymentData? data = fileProcessor.Process(XmlFile);
                if (data != null)
                    deploymentData.Deployments.Add(data);
            }
        }
        
    }
}
