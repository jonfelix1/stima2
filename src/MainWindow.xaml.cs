using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Msagl;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;


namespace VisualizerCovid19
{
    class City
    {
        private int population = 0; //buat populasi
        private int T = -1; //waktu pertama kota diinfeksi
        private List<Tuple<City, Double>> neighbors;
        private String cityname;

        
        public City(String name, int pop)
        {
            this.population = pop;
            this.cityname = name;
            this.neighbors = new List<Tuple<City, Double>>();
        }


        public int Population
        {
            get
            {
                return this.population;
            }

            set
            {
                this.population = value;
            }
        }

        public string CityName
        {
            get
            {
                return this.cityname;
            }
            set
            {
                this.cityname = value;
            }
        }

        public List<Tuple<City, Double>> Neighbors
        {
            get
            {
                return this.neighbors;
            }
            set
            {
                this.neighbors = value;
            }
        }

        public int NeighborsCount
        {
            get
            {
                return neighbors.Count;
            }
        }

        public int InfectedTime
        {
            get
            {
                return this.T;
            }
            set
            {
                this.T = value;
            }
        }

        public double Tr(City b)
        {
            double temp = this.neighbors.Find(x => x.Item1 == b).Item2;
            return temp;
        }

        // Nambahin 1 tetangga
        public void AddNeighbor(City a, double weight)
        {
            Tuple<City, double> temp = new Tuple<City, double>(a, weight);
            this.neighbors.Add(temp);
        }

        // Fungsi yang ada di spek
        public int t(int timenow)
        {
            return (this.T >= 0) ? timenow - this.T : -1;
        }

        public double I(int timenow)
        { // Populasi masyarakat yang terkena virus
            if (this.T == -1)
            {
                return 0;
            }
            else
            {
                return this.population / (1 + (this.population - 1) * Math.Exp(-0.25 * this.t(timenow)));
            }
        }

    }

    class CityGraph
    {
        private List<City> citylist;

        public CityGraph()
        {
            this.citylist = new List<City>();
        }

        public int Size
        {
            get
            {
                return this.citylist.Count;
            }
        }

        public List<City> CityList
        {
            get
            {
                return this.citylist;
            }
        }

        public void AddCity(City a)
        {
            this.citylist.Add(a);
        }

        public double S(String x, String y, int time)
        {
            City a = this.CityList.Find(aa => aa.CityName == x);
            City b = this.CityList.Find(bb => bb.CityName == y);
            return a.I(time) * a.Tr(b);
        }

        

        public void init(ref Queue<Tuple<String, String>> queuein, string a)
        {
            City temp = this.CityList.Find(x => x.CityName == a);

            temp.InfectedTime = 0;

            foreach (var y in temp.Neighbors)
            {
                queuein.Enqueue(new Tuple<String, String>(temp.CityName, y.Item1.CityName));
            }
        }
        // Fungsi menerima reference queue
        private void simulate(ref Queue<Tuple<String, String>> queuein, int time, ref Graph graph)
        {
            Tuple<String, String> a = queuein.Dequeue();
            City tcity = this.CityList.Find(x => x.CityName == a.Item2);

            Boolean success = false;

            if (S(a.Item1, a.Item2, time) >= 1)
            { // Cek apakah terinfeksi
                int t = 0;

                while (S(a.Item1, a.Item2, t) <= 1)
                {
                    t++; // kurang efisien tapi paling gampang
                }

                if ((tcity.InfectedTime == -1) || (tcity.InfectedTime >= t))
                {
                    tcity.InfectedTime = t; // set waktu infeksi

                    foreach (var x in tcity.Neighbors)
                    {
                        queuein.Enqueue(new Tuple<String, String>(tcity.CityName, x.Item1.CityName));
                    }
                    success = true;
                }

                if (success)
                {
                    graph.Edges.ToList().Find(x => (x.Source == a.Item1) && (x.Target == a.Item2)).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                    graph.FindNode(a.Item1).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Red;
                    graph.FindNode(a.Item2).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Red;
                }
            }
        }

        public void bfs(ref Queue<Tuple<String, String>> queuein, int time, ref Graph graph)
        {
            while (queuein.Count > 0)
            {
                this.simulate(ref queuein, time, ref graph);
            }
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Graph graph;
        int time;
        CityGraph ci;
        Queue<Tuple<String, String>> sQueue;
        String popFilename;
        String routeFilename;
        String initCity;

        public MainWindow()
        {

            InitializeComponent();
            graph = new Graph("graph");

            sQueue = new Queue<Tuple<String, String>>();
            ci = new CityGraph();


        }

       

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {

            time = Int32.Parse(this.DayInputBox.Text);

            popFilename = this.PopulationFileNameInputBox.Text;
            routeFilename = this.RoutesFileNameInputBox.Text;


            ReadPopulationFromFile(popFilename);
            ReadRoutesFromFile(routeFilename);

            // Set infected time
            ci.init(ref sQueue, initCity);
            ci.bfs(ref sQueue, time, ref graph);

            this.gViewer.Graph = graph;

        }


        private void ReadPopulationFromFile(String filename)
        {
            int rowIndex = 1;
            int rowCount;
            City tempCity;
            char[] separators = { ' '};
            String[] parseRes;
            String cityName;
            int popCount;

            StreamReader sr = new StreamReader(filename);
            try
            {   
                using (sr)
                {
                    String line;
                    while((line = sr.ReadLine()) != null)
                    {
                        
                        if(rowIndex == 1)
                        {
                            parseRes = line.Split(separators, 2);
                            rowCount = Int32.Parse(parseRes[0]);
                            initCity = parseRes[1];

                        }
                        else
                        {
                            parseRes = line.Split(separators, 2);
                            cityName = parseRes[0];
                            popCount = Int32.Parse(parseRes[1]);

                            tempCity = new City(cityName, popCount);
                            ci.AddCity(tempCity);
                        }

                        rowIndex++;
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("File tidak bisa dibaca");
                Console.WriteLine(e.Message);
            }
        }

        private void ReadRoutesFromFile(String filename)
        {
            int rowIndex = 1;
            int rowCount;
            City tempCity;
            char[] separators = { ' ' };
            String[] parseRes;
            String city1;
            String city2;
            double travelRate;

            StreamReader sr = new StreamReader(filename);
            try
            { 
                using (sr)
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                       
                        if (rowIndex == 1)
                        {
                            rowCount = Int32.Parse(line);
                        }
                        else
                        {
                            parseRes = line.Split(separators, 3);
                            city1 = parseRes[0];
                            city2 = parseRes[1];

                            travelRate = double.Parse(parseRes[2]);

                            ci.CityList.Find(x => x.CityName == city1).AddNeighbor(ci.CityList.Find(x => x.CityName == city2), travelRate);

                        }

                        rowIndex++;
                    }
                    foreach (var city in ci.CityList)
                    {
                        foreach (var item in city.Neighbors)
                        {
                            graph.AddEdge(city.CityName, item.Item1.CityName);

                        }
                    }

                }
            }
            catch (IOException e)
            {
                Console.WriteLine("File tidak bisa dibaca");
                Console.WriteLine(e.Message);
            }
        }
    }
}
