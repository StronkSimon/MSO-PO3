using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ProgrammingLearningApp
{
    
    public abstract class Exercise
    {
        public List<char> exerciseCharList { get; set; } = new List<char>();
        void loadExercise(string filePath) { }
    }

    public class PathFindingExercise : Exercise
    {
        
        public PathFindingExercise() { }
        public void loadExercise(string filePath)
        {
            StreamReader reader = new StreamReader(filePath); //translate given textfile to a charlist
            string exerciseGridFile = reader.ReadToEnd();
            reader.Close();
            foreach (char c in exerciseGridFile)
            {
                if (c != '\n' && c != '\r') // Exclude newline characters
                {
                    exerciseCharList.Add(c);
                }
            }  
        }
    }
}
