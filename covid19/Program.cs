// Tubes 2 IF2211 Pengantar Strategi Algoritma~~
// Gunawan-chan
// Jon-chan

using System;
using System.Collections.Generic;

class City{
    private int population = 0; //buat populasi
    private int T = -1; //waktu pertama kota diinfeksi
    private List<Tuple<City,Double>> neighbors;
    private String cityname;

    //ctor
    // Inisiasi cuman pake value, add neighbor pake addneighbors atau neighbors = 

    // public City(){
    //     this.neighbors =  new List<Tuple<City,Double>>();

    // }
    public City(String name, int pop){
        this.population = pop;
        this.cityname =  name;
        this.neighbors = new List<Tuple<City,Double>>();
    }

    //cctor gk perlu
    //dtor.. emg mau isi apa?

    //method

    public int Population{
        get{
            return this.population;
        }

        set{
            this.population = value;
        }
    }

    public string CityName{
        get{
            return this.cityname;
        }
        set{
            this.cityname = value;
        }
    }

    public List<Tuple<City,Double>> Neighbors {
        get{
            return this.neighbors;
        }
        set{
            this.neighbors = value;
        }
    }

    public int NeighborsCount{
        get{
            return neighbors.Count;
        }
    }

    public int InfectedTime{
        get{
            return this.T;
        }
        set{
            this.T = value;
        }
    }

    public double Tr(City b){
        double temp = this.neighbors.Find(x => x.Item1 == b).Item2;
        return temp;
    }

    // Nambahin 1 tetangga
    public void AddNeighbor(City a, double weight){
        Tuple<City,double> temp = new Tuple<City,double>(a, weight);
        this.neighbors.Add(temp);
    }

    // Fungsi yang ada di spek
    public int t(int timenow){
        return (this.T >= 0) ? timenow - this.T : -1;
    }

    public double I(int timenow){ // Populasi masyarakat yang terkena virus
        if (this.T == -1){
            return 0;
        } else {
            return this.population/(1+(this.population-1)*Math.Exp(-0.25*this.t(timenow)));
        }
    }

}

class CityGraph{
    private List<City> citylist;

    public CityGraph(){
        this.citylist = new List<City>();
    }

    public int Size{
        get{
            return this.citylist.Count;
        }
    }

    public List<City> CityList{
        get{
            return this.citylist;
        }
    }

    public void AddCity(City a){
        this.citylist.Add(a);
    }

    public double S(String x, String y, int time){
        City a = this.CityList.Find(aa => aa.CityName == x);
        City b = this.CityList.Find(x => x.CityName == y);
        return a.I(time) * a.Tr(b);
    }

    // Implementassin BFS disini

    public void init(ref Queue<Tuple<String,String>> queuein, string a){
        City temp = this.CityList.Find(x => x.CityName == a);
        
        temp.InfectedTime = 0;

        Console.WriteLine(temp.NeighborsCount);

        //queuein.Enqueue(new Tuple<String, String>(temp.CityName, temp.Neighbors[0].Item1.CityName));

        foreach(var y in temp.Neighbors){
            queuein.Enqueue(new Tuple<String, String>(temp.CityName, y.Item1.CityName));
        }
    }
    // Fungsi menerima reference queue
    private void simulate(ref Queue<Tuple<String,String>> queuein, int time){
        Tuple<String,String> a = queuein.Dequeue();
        Console.WriteLine("hehehe " + a.Item1 + " " + a.Item2);
        //String fcity = this.CityList.Find(x => x.CityName == a.Item1).CityName;
        City tcity = this.CityList.Find(x => x.CityName == a.Item2);
        if (S(a.Item1, a.Item2, time) >= 1){ // Cek apakah terinfeksi
            int t = 0;

            while (S(a.Item1, a.Item2, t) <= 1){
                t++; // kurang efisien tapi paling gampang
            }

            if ((tcity.InfectedTime == -1) || (tcity.InfectedTime >= t)){
                tcity.InfectedTime = t; // set waktu infeksi

                foreach(var x in tcity.Neighbors){
                    queuein.Enqueue(new Tuple<String, String>(tcity.CityName, x.Item1.CityName));
                }
            } // else do nothing karena T(fcity) < T'(fcity), kotanya sudah terinfeksi sebelumnya
            
        }
    }

    public void bfs(ref Queue<Tuple<String,String>> queuein, int time){
        while (queuein.Count > 0){
            this.simulate(ref queuein , time);
        }
    }
}

class MainProgram{
    public static void Main(String[] args){
        Queue<Tuple<String,String>> sQueue = new Queue<Tuple<String, String>>();
        CityGraph ci = new CityGraph();

        int time = 30;
        
        City a, b, c, d;
        a = new City("a", 500);
        b = new City("b", 1000);
        c = new City("c", 300);
        d = new City("d", 1000);

        a.AddNeighbor(b, 0.02);
        a.AddNeighbor(c, 0.005);
        b.AddNeighbor(a, 0.005);
        b.AddNeighbor(d, 0.005);
        c.AddNeighbor(a, 0.1);
        c.AddNeighbor(b, 0.1);
        d.AddNeighbor(a, 0.05);
        d.AddNeighbor(c, 0.1);

        ci.AddCity(a);
        ci.AddCity(b);
        ci.AddCity(c);
        ci.AddCity(d);

        Console.WriteLine(a.Neighbors.Find(x => x.Item1.CityName == "b").Item2);

        Console.WriteLine(ci.CityList.Find(x => x.CityName == "a").Neighbors.Find(x => x.Item1.CityName == "b").Item2);


        Console.WriteLine(a.NeighborsCount);

        foreach (var item in a.Neighbors){
            Console.WriteLine(item.Item1.CityName);
            Console.WriteLine(item.Item2);
        }

        // Set infected time
        ci.init(ref sQueue, "a"); // Virus mulai dari b

        //ci.init(ref sQueue, "a");

        Console.WriteLine(ci.S(a.CityName, b.CityName, time));

        Console.WriteLine(sQueue.Count);

        // Dequeue blm jalan

        // Tuple<String,String> aa = sQueue.Dequeue();

        // Console.WriteLine(aa.Item1 + " " + aa.Item2);
        
        // aa = sQueue.Dequeue();

        // Console.WriteLine(aa.Item1 + " " + aa.Item2);

        ci.bfs(ref sQueue, time);

        foreach(City x in ci.CityList){
            //if (x.InfectedTime >= 0){
                Console.WriteLine(x.CityName + " " + x.InfectedTime);
            //}
        }

        Console.WriteLine("Testing Successful");
    }

}