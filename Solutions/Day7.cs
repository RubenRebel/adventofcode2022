using System;
using System.Xml.Linq;

namespace AdventofCode2022.Solutions
{
    public class Day7 : SolutionBase
    {
        int totalSize = 0;
        int smallestDiskToDeleteSize = 0;
        int diskSize = 70000000;
        int spaceRequired = 30000000;

        public Day7(FileReader fReader, StringUtilities strUtils, CalculationUtilities calcUtils) : base(fReader, strUtils, calcUtils)
        {
            var deviceSpaceInput = fileReader.ConvertFileContentToString("/Users/rubenbernecker/Documents/AdventOfCode2022_Resources/devicespace.txt");
            var split = stringUtils.SplitStringsOnNewLines(deviceSpaceInput);

            // the root directory
            var root = new Node();
            var currentNode = root;

            // create tree of filesystem
            foreach (var line in split)
            {
                if (line.Contains("$"))
                {
                    if (line.Equals("$ cd .."))
                    {
                        currentNode = currentNode.previous;
                    } else if(line.Contains("$ cd ") && !line.Contains("/"))
                    {
                        var dir = line.Split(' ')[2];
                        currentNode = currentNode.next.Where(n => n.Name.Equals(dir)).FirstOrDefault();
                    }                    
                    continue;
                }

                var newNode = new Node();
                var sub = line.Split(' ');
                if (!line.Contains("dir"))
                {
                    var size = int.Parse(sub[0]);
                    newNode.Size = size;
                    newNode.Type = "file";
                    currentNode.Size += size;
                    increaseParentSize(currentNode, size);
                }
                
                newNode.Name = sub[1];
                newNode.previous = currentNode;
                currentNode.next.Add(newNode);
            }
            // end of tree creation

            appendTotalSize(root);
            LogPuzzleInformation(7, "No Space Left On Device part one");
            LogPuzzleAnswer(totalSize.ToString(), "No Space Left On Device part one");

            var unusedSpace = diskSize - root.Size;
            var spaceToFreeUp = spaceRequired - unusedSpace;
            smallestDiskToDeleteSize = root.Size;
            findSmallestDirectoryToDelete(root, spaceToFreeUp);
            LogPuzzleInformation(7, "No Space Left On Device part two");
            LogPuzzleAnswer(smallestDiskToDeleteSize.ToString(), "No Space Left On Device part two");
        }

        private void increaseParentSize(Node input, int size)
        {
            if(input.previous != null)
            {
                input.previous.Size += size;
                increaseParentSize(input.previous, size);
            }
        }

        private void findSmallestDirectoryToDelete(Node input, int spaceToFree)
        {
            if(input.Size >= spaceToFree && input.Size < smallestDiskToDeleteSize)
            {
                smallestDiskToDeleteSize = input.Size;
            }
            foreach (var childNode in input.next)
            {
                findSmallestDirectoryToDelete(childNode, spaceToFree);
            }
        }

        private void appendTotalSize(Node input)
        {
            if(input.Type == "directory" && input.Size <= 100000)
            {
                totalSize += input.Size;
            }

            foreach (var childNode in input.next)
            {
                appendTotalSize(childNode);
            }
        }
    }

    internal class Node
    {
        public string Name = "";
        public string Type = "directory";
        public int Size = 0;
        public List<Node>? next;
        public Node previous;

        public Node()
        {
            next = new List<Node>();
        }
    }
}