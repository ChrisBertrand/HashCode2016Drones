using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2016Drone
{
    class Program
    {
        public int[,] Map;

        // drone bits
        public int Rows;
        public int Columns;
        public int DronesAvailable;
        public int MaxWeight;
        public int Deadline;

        //product bits
        public int pTypes;

        public struct Coord
        {
            public int r;
            public int c;
        }

        public struct Products
        {
            public int id;
            public int weight;
            public int count;
        }

        public List<Products> Products = new List<Products>();
        public List<Warehouse> Warehouses = new List<Warehouse>();

        public struct Warehouse
        {
            public int id;
            public Coord location;
            public List<Products> stock;
        }

        public struct Order
        {
            public int id;
            public List<Products> purchaseOrder;
            public Coord destination;
        }

        public List<Drone> Drones = new List<Drone>();
        public struct Drone
        {
            public int id;
            public Coord location;
            public double distanceTravelled;
            public List<Products> carrying;
            public Customer customer;
        }

        public List<Customer> Customers = new List<Customer>();

        public struct Customer
        {
            public int id;
            public List<Order> custOrder;
        }

        public void Load(Drone drone, Warehouse w, int itemCount, Product product)
        {
            //Instantiate the drone
            if (drone.carrying is null)
                {
                drone.carrying = new List<Products>();
                drone.location.c = w.location.c;
                drone.location.r = w.location.r;
            }
        }
          
        public void RunSimulation()
        {
            for (var i = 0; i < Deadline; i++)
            {
                foreach (Drone d in Drones)
                {
                    // Take the first order
                    var c = Customers.First();
                    int numberOfItems = c.custOrder.Count();

                    d.customer = c;


                    // Move onto the next order
                    Customers.Skip(1);
                }
            }
        }

        public void Deliver()
        {
           

        }

        public static void Main(string[] args)
        {
            Program run = new Program();
            run.Initialise();
        }

        public void Initialise()
        {
            // Get all the variables up and running, number of drones, locations etc.
            var file = File.ReadAllLines("busy_day.in");
            string DroneDetails = file.First();

            string[] ddetail = DroneDetails.Split(' ');

                Rows = Convert.ToInt32( ddetail[0]);
                Columns = Convert.ToInt32(ddetail[1]);
                DronesAvailable = Convert.ToInt32(ddetail[2]);
                Deadline = Convert.ToInt32(ddetail[3]);
                MaxWeight = Convert.ToInt32(ddetail[4]);

            Map = new int[Rows, Columns];

           pTypes = Convert.ToInt32(file.Skip(1).First());

            IEnumerable<string> ProductInfo = file.Skip(2);
            foreach (var info in ProductInfo.First().Split(' ').Select((value, i) => new { i, value } ))
            {
                Products prodType = new Products { id = info.i, weight = Convert.ToInt32(info.value) };
                Products.Add(prodType);
            }

            int wCount = Convert.ToInt32(file.Skip(3).First());

            // Get all the Warehouse info... take 2 as this is per warehouse info.
            var WarehouseInfo = file.Skip(4).Take(wCount);

            // I've got the warehouse info, each two block line is needed.
            // First grab all coords

            var wCoords = WarehouseInfo.Select((value, i) => new { i, value }).Where(item => item.i % 2 == 0).ToList();

            for (var p=0; p<wCoords.Count(); p++)
            {
                string[] coordinates = wCoords[p].value.Split(' ');

                Warehouse w = new Warehouse { id = p,
                    location = new Coord { r = Convert.ToInt32(coordinates[0]), c = Convert.ToInt32(coordinates[1])}};

                Warehouses.Add(w);
            }

            // Now add the inventory to products and warehouses
            var wCProds = WarehouseInfo.Select((value, i) => new { i, value }).Where(item => item.i % 2 == 1).ToList();

            for (var i =0; i < wCProds.Count(); i++)
            {
                string[] prodTypes = wCProds[i].value.Split(' ');

                for (var o = 0; o< prodTypes.Length; o++)
                {
                    Products Prod = Products.Where(prod => prod.id == o).FirstOrDefault();
                    Prod.count = Convert.ToInt32(prodTypes[o]);

                    Warehouse warehouse = Warehouses.Where(w => w.id == i).First();
                    warehouse.stock = new List<Products>();
                    warehouse.stock.Add(Prod);
                }
            }

            var look = 1;
        }
    }
}