using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium;
using System.Threading;
using System.IO;

namespace GoogleFrequency
{
    public partial class Form1 : Form
    {
        Dictionary<string, string> dic ;
        public Form1()
        {
            InitializeComponent();

           /* IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(string.Format(@"http://takhfifan.com/tehran/"));
           // ReadDictionary(@"D:\MasMas\Projects\EnhanceErabDictionary\EnhanceErabDictionary\rr.dat", ref dic);
            while (true)
            {
                driver.Navigate().Refresh();
                if (driver.PageSource.Contains("دوسالانه ششم"))
                {

                }
            }*/
        }


        public static void ReadDictionary(string path, ref Dictionary<string, string> dic)
        {
            dic = new Dictionary<string, string>();
            StreamReader reader = new StreamReader(path);
            while (!reader.EndOfStream)
            {
                string[] d = reader.ReadLine().Split("\t".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                if (d.Length > 0)
                    if (!dic.ContainsKey(d[0]))
                        dic.Add(d[0].Trim(), d[1] + "\t" + d[2]);
            }
            reader.Close();
            reader.Dispose();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            StreamWriter wr = new StreamWriter(new FileStream(@"D:\MasMas\Projects\EnhanceErabDictionary\EnhanceErabDictionary\r1r.dat",FileMode.Create),Encoding.UTF8);
            string value = "0" ;
            IWebDriver driver = new ChromeDriver();
            //IWebDriver driver = new InternetExplorerDriver();
            try
            {
                foreach (var str in dic)
                {
                    string sttkey = str.Key;
                    if (str.Key.Contains("#"))
                        sttkey = sttkey.Replace("#", " ");

                    driver.Navigate().GoToUrl(string.Format(@"https://www.google.com"));
                    driver.FindElement(By.Name("q")).SendKeys("\"" + sttkey + "\"\n");
                    driver.FindElement(By.Name("btnG")).Click();
                    Thread.Sleep(10000);
                    if (driver.PageSource.Contains("please type the characters below"))
                    {
                        Thread.Sleep(1000);
                        while (driver.PageSource.Contains("please type the characters below"))
                            continue;
                    }
                    if (!driver.PageSource.Contains("did not match any documents") && !driver.PageSource.Contains("No results found for"))
                    {
                        try
                        {
                            value = driver.FindElement(By.Id("resultStats")).Text;
                        }
                        catch
                        {
                            wr.Close();

                        }
                    }
                    else
                        value = "0 result";
                    wr.WriteLine(str.Key + "\t" + str.Value + "\t\t" + value.Substring(value.IndexOf("About ")==-1?0:6, value.IndexOf("result")).TrimEnd());
                    //driver.FindElement(By.Name("xxx")).Click();
                    //driver.FindElement(By.ClassName("xxx")).SendKeys("سلام");

                }
            }
            catch
            {
                wr.Close();
            }
            wr.Close();
            driver.Close();       
        }
    }
}
