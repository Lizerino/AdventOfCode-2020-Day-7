using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day_7
{
    class Program
    {
        static void Main(string[] args)
        {
            string puzzleInput = System.IO.File.ReadAllText("E:\\Archive\\Repos\\MyCode\\Advent of Code\\2020\\Day 7\\Input.txt");

            puzzleInput = puzzleInput.Replace("bags", "bag");
            puzzleInput = puzzleInput.Replace(".", "");

            var splitByRow = puzzleInput.Split(Environment.NewLine);

            List<rule> rules = new List<rule>();

            foreach (var row in splitByRow)
            {
                var x = new rule();
                var splitIntoMainBagAndContents = row.Split("contain");
                x.nameOfContainerBag = splitIntoMainBagAndContents[0].Trim();
                x.nameOfBagsItContains = splitIntoMainBagAndContents[1].Split(',');
                for (int i = 0; i < x.nameOfBagsItContains.Length; i++)
                {
                    x.nameOfBagsItContains[i] = x.nameOfBagsItContains[i].Trim();
                }
                rules.Add(x);
            }

            // Create a list of list of lists etc of bags
            var ListOfBags = new List<bag>();

            // Look at every rule add bag to list 
            foreach (var rule in rules)
            {
                bag bag = new bag();
                bag.listOfBagsInside = new List<bag>();
                foreach (var item in rule.nameOfBagsItContains)
                {
                    bag.Name = rule.nameOfContainerBag;
                }
                ListOfBags.Add(bag);
            }

            // Look at every bag in list and add every inside bag repeat until there are no more inside bags to add
            ListOfBags = addInsideBags(rules, ListOfBags);
            //ListOfBags = addNextLevelbags(rules, ListOfBags);

            // Count the number of bags with a certain type of bag in them and then bags with that kind of bag in them etc            
            //List<bag> bagToFind = new List<bag>();
            //bagToFind.Add( new bag { Name = "shiny gold bag" });

            //var result = new List<bag>();
            //var foundbags = findBagContainingSpecificBag(ListOfBags, bagToFind, result);

            //printFoundBags(foundbags, 0);
            var bagList = ListOfBags.Where(x => x.Name.Contains("shiny gold bag")).Select(x => x.listOfBagsInside).FirstOrDefault();
            //double howManyBags = howManyBagsInThisBag(bagList, 0);                            
            double count = 0;
            howmanybags(bagList, ref count);
           

            Console.WriteLine(count);
        }

        private static void howmanybags(List<bag> baglist, ref double count)
        {
            foreach (var item in baglist)
            {
                var localviewvar = item.Name;
                count++;
                howmanybags(item.listOfBagsInside, ref count);
            }
        }

        private static double howManyBagsInThisBag(List<bag> listOfBags, double count)
        {
            if (listOfBags.Count>0)
            {
                foreach (var bag in listOfBags)
                {
                    return howManyBagsInThisBag(bag.listOfBagsInside, count);                
                }
            }
            else
            {
            return count+listOfBags.Count;            
            }
            return count;
        }

        private static void printFoundBags(List<bag> foundbags, int indent)
        {
            foreach (var bag in foundbags)
            {
                if (foundbags.Count>0)
                {
                    for (int i = 0; i < indent; i++)
                    {
                        Console.Write("-");
                    }
                    Console.WriteLine(bag.Name);
                    printFoundBags(bag.listOfBagsInside, indent+3);
                }
            }
        }

        private static List<bag> findBagContainingSpecificBag(List<bag> listOfBags, List<bag> bagToFind, List<bag> result)
        {            
            if (bagToFind.Count>0)
            {
                var foundBags = new List<bag>();
                foreach (var containerBagToFind in bagToFind)
                {
                    foreach (var bag in listOfBags)
                    {
                        foreach (var insidebag in bag.listOfBagsInside)
                        {
                            if (insidebag.Name.Contains(containerBagToFind.Name))
                            {
                                foundBags.Add(bag);
                            }
                        }
                    }
                }
                foreach (var bag in foundBags)
                {
                    result.Add(bag);
                }
                findBagContainingSpecificBag(listOfBags, foundBags,result);
            }
            return result;
        }

        private static List<bag> addNextLevelbags(List<rule> rules, List<bag> ListOfBags)
        {
            foreach (var bag in ListOfBags)
            {
                foreach (var item in bag.listOfBagsInside)
                {
                item.listOfBagsInside = addInsideBags(rules, bag.listOfBagsInside);
                addNextLevelbags(rules, bag.listOfBagsInside);                
                }
            }
            return ListOfBags;
        }

        private static List<bag> addInsideBags(List<rule> rules, List<bag> ListOfBags)
        {
            foreach (var bag in ListOfBags)
            {
                foreach (var rule in rules)
                {
                    if (bag.Name.Contains(rule.nameOfContainerBag))
                    {
                        foreach (var insidebag in rule.nameOfBagsItContains)
                        {
                            Regex regexNr = new Regex("[0-9]?", RegexOptions.IgnoreCase);
                            Regex regexLtr = new Regex("([a-zA-Z]+( [a-zA-Z]+)+)", RegexOptions.IgnoreCase);
                            var regexnrmatch = regexNr.Match(insidebag).Value.ToString();
                            var regexltrmatch = regexLtr.Match(insidebag).Value.ToString();
                            int.TryParse(regexnrmatch, out int result);

                            var bagToAdd = ListOfBags.Where(x => x.Name==regexltrmatch).FirstOrDefault();
                            
                            for (int i = 0; i < result; i++)
                            {
                                bag.listOfBagsInside.Add(bagToAdd);
                            }                                                        
                        }
                    }
                }
            }
            return ListOfBags;
        }

        private static List<rule> containsBag(rule bag, List<rule> rules)
        {
            var bags = new List<rule>();
            foreach (var rule in rules)
            {
                foreach (var containingBag in rule.nameOfBagsItContains)
                {
                    if (containingBag.Contains(bag.nameOfContainerBag))
                    {
                        bags.Add(rule);
                    }
                }

            }
            return bags;
        }
    }

    class rule
    {
        public string nameOfContainerBag { get; set; }
        public string[] nameOfBagsItContains { get; set; }
    }

    class bag
    {
        public string Name { get; set; }
        public List<bag> listOfBagsInside { get; set; }
    }
}
