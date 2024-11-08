using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileProcessor
{
    public class FileProcessor : IFileProcessor
    {
        public DeploymentData Process(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DeploymentData));

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                return (DeploymentData)serializer.Deserialize(fs);
            }
        }
    }
}
