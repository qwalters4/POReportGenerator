using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace POReportGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Please enter a PO: ");
            string po = Console.ReadLine();
            Console.Write("Please enter a Total: ");
            int desiredTotal = int.Parse(Console.ReadLine());
            Console.Write("Please enter the number of bad drives: ");
            int badDrives = int.Parse(Console.ReadLine());
            Console.Write("Please enter the number of SAS drives: ");
            int sasdrives = int.Parse(Console.ReadLine());
            Console.WriteLine();

            string curPath = @"\\TRUENAS\DiskTesting";

            if (Directory.Exists(Path.Combine(@curPath, "Archive", po)))
            {
                Dictionary<string, int> threeTotals = new Dictionary<string, int>();
                Dictionary<string, int> twoTotals = new Dictionary<string, int>();
                Dictionary<string, int> oneTotals = new Dictionary<string, int>();
                Dictionary<string, int> M2Totals = new Dictionary<string, int>();
                Dictionary<string, int> unknownTotals = new Dictionary<string, int>();
                List<PhysicalDisk> pending = new List<PhysicalDisk>();
                int i = 0;

                foreach (string file in Directory.EnumerateFiles(Path.Combine(@curPath, "Archive", po), "*.json"))
                {
                    PhysicalDisk temp = JsonConvert.DeserializeObject<PhysicalDisk>(File.ReadAllText(file));            
                    pending.Add(temp);
                    Console.WriteLine("Drive#: " + i);
                    i++;
                }

                string sizestr;
                int sizeint;
                foreach(PhysicalDisk p in pending)
                {
                    if (p.Size.ToString().Length >= 10)
                        sizestr = p.Size.ToString().Substring(0, p.Size.ToString().Length - 9);
                    else
                        sizestr = p.Size.ToString();
                    sizeint = int.Parse(sizestr);
                    if (p.FormFactor == "" || p.FormFactor == "Unknown" || p.FormFactor == null)
                    {
                        if (sizeint <= 250)
                        {
                            if (unknownTotals.ContainsKey(sizestr))
                                unknownTotals[sizestr]++;
                            else
                                unknownTotals.Add(sizestr, 1); 
                        }
                        else if (sizeint < 320)
                        {
                            if (unknownTotals.ContainsKey(p.ModelId.Split(' ')[0]))
                                unknownTotals[p.ModelId.Split(' ')[0]]++;
                            else
                                unknownTotals.Add(p.ModelId.Split(' ')[0], 1);
                        }
                        else
                        {
                            if (unknownTotals.ContainsKey(p.ModelId))
                                unknownTotals[p.ModelId]++;
                            else
                                unknownTotals.Add(p.ModelId, 1);
                        }
                    }
                    else if (p.FormFactor == "3.5")
                    {
                        if(sizeint <= 250)
                        {
                            if (threeTotals.ContainsKey(sizestr))
                                threeTotals[sizestr]++;
                            else
                                threeTotals.Add(sizestr, 1);
                        }
                        else if(sizeint < 320)
                        {
                            if(threeTotals.ContainsKey(p.ModelId.Split(' ')[0]))
                                threeTotals[p.ModelId.Split(' ')[0]]++;
                            else
                                threeTotals.Add(p.ModelId.Split(' ')[0], 1);
                        }
                        else
                        {
                            if (threeTotals.ContainsKey(p.ModelId))
                                threeTotals[p.ModelId]++;
                            else
                                threeTotals.Add(p.ModelId, 1);
                        }
                    }
                    else if (p.FormFactor == "2.5")
                    {
                        if (sizeint <= 250)
                        {
                            if (twoTotals.ContainsKey(sizestr))
                                twoTotals[sizestr]++;
                            else
                                twoTotals.Add(sizestr, 1);
                        }
                        else if (sizeint < 320)
                        {
                            if (twoTotals.ContainsKey(p.ModelId.Split(' ')[0]))
                                twoTotals[p.ModelId.Split(' ')[0]]++;
                            else
                                twoTotals.Add(p.ModelId.Split(' ')[0], 1);
                        }
                        else
                        {
                            if (twoTotals.ContainsKey(p.ModelId))
                                twoTotals[p.ModelId]++;
                            else
                                twoTotals.Add(p.ModelId, 1);
                        }
                    }
                    else if (p.FormFactor == "1.8")
                    {
                        if (sizeint <= 250)
                        {
                            if (oneTotals.ContainsKey(sizestr))
                                oneTotals[sizestr]++;
                            else
                                oneTotals.Add(sizestr, 1);
                        }
                        else if (sizeint < 320)
                        {
                            if (oneTotals.ContainsKey(p.ModelId.Split(' ')[0]))
                                oneTotals[p.ModelId.Split(' ')[0]]++;
                            else
                                oneTotals.Add(p.ModelId.Split(' ')[0], 1);
                        }
                        else
                        {
                            if (oneTotals.ContainsKey(p.ModelId))
                                oneTotals[p.ModelId]++;
                            else
                                oneTotals.Add(p.ModelId, 1);
                        }
                    }
                    else if (p.FormFactor.Contains("M2"))
                    {
                        if (sizeint <= 250)
                        {
                            if (M2Totals.ContainsKey(sizestr))
                                M2Totals[sizestr]++;
                            else
                                M2Totals.Add(sizestr, 1);
                        }
                        else if (sizeint < 320)
                        {
                            if (M2Totals.ContainsKey(p.ModelId.Split(' ')[0]))
                                M2Totals[p.ModelId.Split(' ')[0]]++;
                            else
                                M2Totals.Add(p.ModelId.Split(' ')[0], 1);
                        }
                        else
                        {
                            if (M2Totals.ContainsKey(p.ModelId))
                                M2Totals[p.ModelId]++;
                            else
                                M2Totals.Add(p.ModelId, 1);
                        }
                    }
                    else
                    {
                        if (sizeint <= 250)
                        {
                            if (unknownTotals.ContainsKey(sizestr))
                                unknownTotals[sizestr]++;
                            else
                                unknownTotals.Add(sizestr, 1);
                        }
                        else if (sizeint < 320)
                        {
                            if (unknownTotals.ContainsKey(p.ModelId.Split(' ')[0]))
                                unknownTotals[p.ModelId.Split(' ')[0]]++;
                            else
                                unknownTotals.Add(p.ModelId.Split(' ')[0], 1);
                        }
                        else
                        {
                            if (unknownTotals.ContainsKey(p.ModelId))
                                unknownTotals[p.ModelId]++;
                            else
                                unknownTotals.Add(p.ModelId, 1);
                        }
                    }
                }

                Console.WriteLine("\n3.5\": ");
                foreach(KeyValuePair<string,int> kv in threeTotals.OrderBy(i => i.Key))
                {
                    Console.WriteLine("\t" + kv.Key + ": " + kv.Value);
                }

                Console.WriteLine("\n2.5\": ");
                foreach(KeyValuePair<string, int> kv in twoTotals.OrderBy(i => i.Key))
                {
                    Console.WriteLine("\t" + kv.Key + ": " + kv.Value);
                }

                Console.WriteLine("\n1.8\": ");
                foreach (KeyValuePair<string, int> kv in oneTotals.OrderBy(i => i.Key))
                {
                    Console.WriteLine("\t" + kv.Key + ": " + kv.Value);
                }

                Console.WriteLine("\nM2\": ");
                foreach (KeyValuePair<string, int> kv in M2Totals.OrderBy(i => i.Key))
                {
                    Console.WriteLine("\t" + kv.Key + ": " + kv.Value);
                }

                Console.WriteLine("\nUnknown: ");
                foreach (KeyValuePair<string, int> kv in unknownTotals.OrderBy(i => i.Key))
                {
                    Console.WriteLine("\t" + kv.Key + ": " + kv.Value);
                }

                Console.WriteLine("\nDesired total: " + desiredTotal);
                Console.WriteLine("Bad drives: " + badDrives);
                Console.WriteLine("SAS Drives: " + sasdrives);
                Console.WriteLine("Actual total: " + (threeTotals.Sum(x => x.Value) + twoTotals.Sum(x => x.Value) + unknownTotals.Sum(x => x.Value) + M2Totals.Sum(x => x.Value) + oneTotals.Sum(x => x.Value)));
                Console.WriteLine("Missing: " + (desiredTotal - (sasdrives + badDrives + threeTotals.Sum(x => x.Value) + twoTotals.Sum(x => x.Value) + unknownTotals.Sum(x => x.Value) + M2Totals.Sum(x => x.Value) + oneTotals.Sum(x => x.Value))));
                //////////////////////////////////////////////////
                FileStream ostrm;
                StreamWriter writer;
                TextWriter oldOut = Console.Out;
                try
                {
                    ostrm = new FileStream((Path.Combine(@curPath, "Archive", po) + "\\InventoryReport.txt"), FileMode.OpenOrCreate, FileAccess.Write);
                    writer = new StreamWriter(ostrm);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot open InventoryReport.txt for writing");
                    Console.WriteLine(e.Message);
                    return;
                }
                Console.SetOut(writer);
                //////////////////////////////////////////////////
                Console.WriteLine("\n3.5\": ");
                foreach (KeyValuePair<string, int> kv in threeTotals.OrderBy(i => i.Key))
                {
                    Console.WriteLine("\t" + kv.Key + ": " + kv.Value);
                }

                Console.WriteLine("\n2.5\": ");
                foreach (KeyValuePair<string, int> kv in twoTotals.OrderBy(i => i.Key))
                {
                    Console.WriteLine("\t" + kv.Key + ": " + kv.Value);
                }

                Console.WriteLine("\n1.8\": ");
                foreach (KeyValuePair<string, int> kv in oneTotals.OrderBy(i => i.Key))
                {
                    Console.WriteLine("\t" + kv.Key + ": " + kv.Value);
                }

                Console.WriteLine("\nM2\": ");
                foreach (KeyValuePair<string, int> kv in M2Totals.OrderBy(i => i.Key))
                {
                    Console.WriteLine("\t" + kv.Key + ": " + kv.Value);
                }

                Console.WriteLine("\nUnknown: ");
                foreach (KeyValuePair<string, int> kv in unknownTotals.OrderBy(i => i.Key))
                {
                    Console.WriteLine("\t" + kv.Key + ": " + kv.Value);
                }

                Console.WriteLine("\nDesired total: " + desiredTotal);
                Console.WriteLine("Bad drives: " + badDrives);
                Console.WriteLine("SAS Drives: " + sasdrives);
                Console.WriteLine("Actual total: " + (threeTotals.Sum(x => x.Value) + twoTotals.Sum(x => x.Value) + unknownTotals.Sum(x => x.Value) + M2Totals.Sum(x => x.Value) + oneTotals.Sum(x => x.Value)));
                Console.WriteLine("Missing: " + (desiredTotal - (sasdrives + badDrives + threeTotals.Sum(x => x.Value) + twoTotals.Sum(x => x.Value) + unknownTotals.Sum(x => x.Value) + M2Totals.Sum(x => x.Value) + oneTotals.Sum(x => x.Value))));

                Console.SetOut(oldOut);
                writer.Close();
                ostrm.Close();

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////

                unknownTotals.Clear();
                threeTotals.Clear();
                twoTotals.Clear();
                oneTotals.Clear();
                M2Totals.Clear();

                foreach (PhysicalDisk p in pending)
                {
                    if (p.Size.ToString().Length >= 10)
                        sizestr = p.Size.ToString().Substring(0, p.Size.ToString().Length - 9);
                    else
                        sizestr = p.Size.ToString();
                    sizeint = int.Parse(sizestr);
                    if (p.FormFactor == "" || p.FormFactor == "Unknown" || p.FormFactor == null)
                    {
                        if (unknownTotals.ContainsKey(sizestr))
                            unknownTotals[sizestr]++;
                        else
                            unknownTotals.Add(sizestr, 1);
                    }
                    else if (p.FormFactor == "3.5")
                    {
                        if (threeTotals.ContainsKey(sizestr))
                            threeTotals[sizestr]++;
                        else
                            threeTotals.Add(sizestr, 1);
                    }
                    else if (p.FormFactor == "2.5")
                    {
                        if (twoTotals.ContainsKey(sizestr))
                            twoTotals[sizestr]++;
                        else
                            twoTotals.Add(sizestr, 1);
                    }
                    else if (p.FormFactor == "1.8")
                    {
                        if (oneTotals.ContainsKey(sizestr))
                            oneTotals[sizestr]++;
                        else
                            oneTotals.Add(sizestr, 1);
                    }
                    else if (p.FormFactor.Contains("M2"))
                    {
                        
                        if (M2Totals.ContainsKey(sizestr))
                            M2Totals[sizestr]++;
                        else
                            M2Totals.Add(sizestr, 1);
                    }
                    else
                    {
                        if (unknownTotals.ContainsKey(sizestr))
                            unknownTotals[sizestr]++;
                        else
                            unknownTotals.Add(sizestr, 1);
                    }
                }

                Console.WriteLine("\n3.5\": ");
                foreach (KeyValuePair<string, int> kv in threeTotals.OrderBy(i => i.Key))
                {
                    Console.WriteLine("\t" + kv.Key + ": " + kv.Value);
                }

                Console.WriteLine("\n2.5\": ");
                foreach (KeyValuePair<string, int> kv in twoTotals.OrderBy(i => i.Key))
                {
                    Console.WriteLine("\t" + kv.Key + ": " + kv.Value);
                }

                Console.WriteLine("\n1.8\": ");
                foreach (KeyValuePair<string, int> kv in oneTotals.OrderBy(i => i.Key))
                {
                    Console.WriteLine("\t" + kv.Key + ": " + kv.Value);
                }

                Console.WriteLine("\nM2\": ");
                foreach (KeyValuePair<string, int> kv in M2Totals.OrderBy(i => i.Key))
                {
                    Console.WriteLine("\t" + kv.Key + ": " + kv.Value);
                }

                Console.WriteLine("\nUnknown: ");
                foreach (KeyValuePair<string, int> kv in unknownTotals.OrderBy(i => i.Key))
                {
                    Console.WriteLine("\t" + kv.Key + ": " + kv.Value);
                }

                Console.WriteLine("\nDesired total: " + desiredTotal);
                Console.WriteLine("Bad drives: " + badDrives);
                Console.WriteLine("SAS Drives: " + sasdrives);
                Console.WriteLine("Actual total: " + (threeTotals.Sum(x => x.Value) + twoTotals.Sum(x => x.Value) + unknownTotals.Sum(x => x.Value) + M2Totals.Sum(x => x.Value) + oneTotals.Sum(x => x.Value)));
                Console.WriteLine("Missing: " + (desiredTotal - (sasdrives + badDrives + threeTotals.Sum(x => x.Value) + twoTotals.Sum(x => x.Value) + unknownTotals.Sum(x => x.Value) + M2Totals.Sum(x => x.Value) + oneTotals.Sum(x => x.Value))));

                //////////////////////////////////////////////////
                FileStream secondostrm;
                StreamWriter secondwriter;
                TextWriter secondOut = Console.Out;
                try
                {
                    secondostrm = new FileStream((Path.Combine(@curPath, "Archive", po) + "\\WhiteLabelReport.txt"), FileMode.OpenOrCreate, FileAccess.Write);
                    secondwriter = new StreamWriter(secondostrm);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot open InventoryReport.txt for writing");
                    Console.WriteLine(e.Message);
                    return;
                }
                Console.SetOut(secondwriter);
                //////////////////////////////////////////////////

                Console.WriteLine("\n3.5\": ");
                foreach (KeyValuePair<string, int> kv in threeTotals.OrderBy(i => i.Key))
                {
                    Console.WriteLine("\t" + kv.Key + ": " + kv.Value);
                }

                Console.WriteLine("\n2.5\": ");
                foreach (KeyValuePair<string, int> kv in twoTotals.OrderBy(i => i.Key))
                {
                    Console.WriteLine("\t" + kv.Key + ": " + kv.Value);
                }

                Console.WriteLine("\n1.8\": ");
                foreach (KeyValuePair<string, int> kv in oneTotals.OrderBy(i => i.Key))
                {
                    Console.WriteLine("\t" + kv.Key + ": " + kv.Value);
                }

                Console.WriteLine("\nM2\": ");
                foreach (KeyValuePair<string, int> kv in M2Totals.OrderBy(i => i.Key))
                {
                    Console.WriteLine("\t" + kv.Key + ": " + kv.Value);
                }

                Console.WriteLine("\nUnknown: ");
                foreach (KeyValuePair<string, int> kv in unknownTotals.OrderBy(i => i.Key))
                {
                    Console.WriteLine("\t" + kv.Key + ": " + kv.Value);
                }

                Console.WriteLine("\nDesired total: " + desiredTotal);
                Console.WriteLine("Bad drives: " + badDrives);
                Console.WriteLine("SAS Drives: " + sasdrives);
                Console.WriteLine("Actual total: " + (threeTotals.Sum(x => x.Value) + twoTotals.Sum(x => x.Value) + unknownTotals.Sum(x => x.Value) + M2Totals.Sum(x => x.Value) + oneTotals.Sum(x => x.Value)));
                Console.WriteLine("Missing: " + (desiredTotal - (sasdrives + badDrives + threeTotals.Sum(x => x.Value) + twoTotals.Sum(x => x.Value) + unknownTotals.Sum(x => x.Value) + M2Totals.Sum(x => x.Value) + oneTotals.Sum(x => x.Value))));

                Console.SetOut(secondOut);
                secondwriter.Close();
                secondostrm.Close();

            }
            else
                Console.WriteLine("No such PO found.\nExiting...");
        }
    }
}
