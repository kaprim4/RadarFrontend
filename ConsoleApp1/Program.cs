
using ApiService;
using Domain.DTO;
using FileProcessor;
using RadarService;

string dir = @"D:\me\ali\Narsa\Files";

//XMLProcessor.Process(dir);

//IProcess<TreatmentDTO> process = new("data");
//Config.JwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJvdGhtYW5veCIsImp0aSI6IjViODFkMDE3LTgyMTktNDI2ZS1iMTkzLTdhODRjNWFiNzY5ZSIsImlzcyI6IlJhZGFyIiwiYXVkIjpbIlJhZGFyIiwiUmFkYXIiXSwiVXNlcl9JZCI6IjUxYTY3NzY1LWZkODctNGM3My1hNWQxLTRmNGY0OTkyMzMyMiIsImV4cCI6MTczMTMyMDY1OH0.yZXT-BqCDvEFKTarih_6Drj0VK88vIZf9VLpKTyTGJo";
//var test = await process.ProcessAsync(XMLProcessor.deploymentData, RequestType.Post, EndPoint.Add, true);


TruCAMSDK.GetTextData();
Console.ReadKey();