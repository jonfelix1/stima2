// Tubes 2 IF2211 Pengantar Strategi Algoritma~~
// Gunawan-chan
// Jon-chan

using System;
using System.Collections.Generic;

class CityNeighbor{
    private City city;
    private int weight;

    public CityNeighbor(City a, int weight){
        this.city = a;
        this.weight = weight;
    }

    public int Weight{
        get{
            return this.weight;
        }
        set{
            this.weight = value;
        }
    }
}

class City{
    private int population = 0; //buat populasi
    private int T = -1; //waktu pertama kota diinfeksi
    private List<CityNeighbor> neighbors;

    //ctor
    // Inisiasi cuman pake value, add neighbor pake addneighbors atau neighbors = 

    public City(){
        this.neighbors =  new List<CityNeighbor>();

    }
    public City(int pop){
        this.population = pop;
        this.neighbors = new List<CityNeighbor>();
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

    public List<CityNeighbor> Neighbors {
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

    public int SetInfectedTime{
        get{
            return this.T;
        }
        set{
            this.T = value;
        }
    }

    // Nambahin 1 tetangga
    public void AddNeighbor(City a, int weight){
        CityNeighbor temp = new CityNeighbor(a, weight);
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

    // Implementassin BFS disini
}

class MainProgram{
    public static void Main(String[] args){
        City a =  new City(900);

        Console.WriteLine("Testing Successful");
    }
}