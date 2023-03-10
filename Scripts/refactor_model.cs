using System;
using System.IO;

namespace YOLO_Training
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set the paths to the configuration file, training data, and pre-trained weights
            string cfgFilePath = "cfg/yolov3.cfg";
            string dataFilePath = "data/animals_persons.data";
            string weightsFilePath = "weights/darknet53.conv.74";
            
            // Set the path to the executable file
            string darknetExePath = "darknet.exe";
            
            // Set the number of iterations for training
            int maxIterations = 10000;
            
            // Run the training process using Darknet
            string arguments = $"detector train {dataFilePath} {cfgFilePath} {weightsFilePath} -dont_show -map -clear -gpus 0 -max_batches {maxIterations}";
            ProcessStartInfo startInfo = new ProcessStartInfo(darknetExePath, arguments);
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            Process process = new Process();
            process.StartInfo = startInfo;
            process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
            
            // Check if the training was successful
            if (process.ExitCode == 0)
            {
                Console.WriteLine("Training completed successfully.");
            }
            else
            {
                Console.WriteLine("Training failed.");
            }
        }
    }
}
