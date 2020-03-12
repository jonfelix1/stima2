// Tubes 2 IF2211 Pengantar Strategi Algoritma~~
// Gunawan-chan
// Jon-chan

using System;
using System.Collections.Generic;

// class Tuple<City,int>{
//     private City city;
//     private int weight;

//     public Tuple<City,int>(City a, int weight){
//         this.city = a;
//         this.weight = weight;
//     }

//     public int Weight{
//         get{
//             return this.weight;
//         }
//         set{
//             this.weight = value;
//         }
//     }
// }

class City{
    private int population = 0; //buat populasi
    private int T = -1; //waktu pertama kota diinfeksi
    private List<Tuple<City,Double>> neighbors;

    //ctor
    // Inisiasi cuman pake value, add neighbor pake addneighbors atau neighbors = 

    public City(){
        this.neighbors =  new List<Tuple<City,Double>>();

    }
    public City(int pop){
        this.population = pop;
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
    public void AddNeighbor(City a, int weight){
        Tuple<City,double> temp = new Tuple<City,double>(a, weight);
        this.neighbors.Add(temp);
    }

    // Fungsi yang ada di spek
    public int t(int timenow){
        return (this.T >= 0) ? timenow - this.T : -1;
    }

    public double I(int timenow){
        // if (this.T < 0){
        //     return 0;
        // } else {
        //     return this.population/(1+(this.population-1)*Math.Exp(-0.25*this.t(timenow)));
        // }

        return (this.T < 0) ? 0 : this.population/(1+(this.population-1)*Math.Exp(-0.25*this.t(timenow)));
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

    public void AddCity(City a){
        this.citylist.Add(a);
    }

    public double S(City a, City b, int timenow){
        return a.I(timenow)*a.Tr(b);
    }

    // Implementassin BFS disini
    // Fungsi menerima reference queue
}

class MainProgram{
    public static void Main(String[] args){
        Queue<Tuple<City,City>> sQueue = new Queue<Tuple<City, City>>();
        City a =  new City(900);

        Console.WriteLine("Testing Successful");
    }
}