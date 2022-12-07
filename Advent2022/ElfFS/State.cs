using NetTopologySuite.Index.Bintree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.ElfFS
{
    public class State
    {
        public Folder Root { get; set; }
        public Folder Current { get; set; }

        public bool EnsuresSubFoldersExist { get; set; }

        public State(bool ensuresSubFoldersExist = false)
        {
            Root = new(null, "");
            Current = Root;

            EnsuresSubFoldersExist = ensuresSubFoldersExist;
        }

        public void ExecuteCommand(string input)
        {
            if (input == "$ cd /")
            {
                Current = Root;
            }
            else if (input == "$ cd ..")
            {
                Current = Current.Parent;
            }
            else if (input.StartsWith("$ cd"))
            {
                var name = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Last();

                Current = Current.GoToSubFolder(name, EnsuresSubFoldersExist);
            }
            else if (input == "$ ls")
            {

            }
            else if (input.StartsWith("dir "))
            {
                var name = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Last();

                Current.EnsureSubFolder(name);
            }
            else
            {
                var file = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var size = long.Parse(file[0]);
                var name = file[1];

                Current.AddOrUpdateFile(name, size);
            }
        }
    }
}
