using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoLotConsoleApplication.EF;
using static System.Console;

namespace AutoLotConsoleApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WriteLine("***** Fun with ADO.NET EF *****\n");
            int carId = AddNewRecord();
            WriteLine(carId);
            ReadLine();

            PrintAllInventory();
            ReadLine();

            FunWithLinqQueries();
            ReadLine(); 
        }

        private static int AddNewRecord()
        {
            using (var context = new AutoLotEntities())
            {
                try
                {
                    var car = new Car() { Make = "Yugo", Color = "Brown", CarNickName = "Brownie" };
                    context.Cars.Add(car);
                    context.SaveChanges();
                    return car.CarId;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException.Message);
                    return 0;
                }
            }
        }

        private static void AddNewRecord(IEnumerable<Car> carsToAdd) 
        {
            using(var context = new AutoLotEntities())
            {
                context.Cars.AddRange(carsToAdd);
                context.SaveChanges();
            }
        }

        private static void PrintAllInventory()
        {
            using(var context = new AutoLotEntities())
            {
                foreach(Car c in context.Cars.Where(c => c.Make == "BMW"))
                {
                    WriteLine(c);
                }
            }
        }
        
        private static void FunWithLinqQueries()
        {
            using (var context = new AutoLotEntities())
            {
                DbSet<Car> allData = context.Cars;

                var colorMakes = from item in allData select new { item.Color, item.Make };

                foreach (var item in colorMakes)
                {
                    WriteLine(item);
                }
            }
        }

        private static void RemoveRecord(int carId)
        {
            using (var context = new AutoLotEntities())
            {
                Car carToDelete = context.Cars.Find(carId);
                if (carToDelete != null)
                {
                    context.Cars.Remove(carToDelete);

                    if(context.Entry(carToDelete).State != EntityState.Deleted)
                    {
                        throw new Exception("Unable to delete the record");
                    }
                    context.SaveChanges();
                }
            }
        }

        private static void RemoveMiltipleRecords(IEnumerable<Car> carsToRemove)
        {
            using (var context = new AutoLotEntities())
            {
                context.Cars.RemoveRange(carsToRemove);
                context.SaveChanges();
            }
        }

        private static void RemoveRecordUsingEntityState(int carId)
        {
            using(var context = new AutoLotEntities())
            {
                Car carToDelete = new Car() { CarId = carId };
                context.Entry(carToDelete).State = EntityState.Deleted;
                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Write(ex); 
                }
            }
        }

        private static void UpdateRecord(int carId)
        {
            using (var context = new AutoLotEntities())
            {
                Car carToUpdate = context.Cars.Find(carId);
                if(carToUpdate != null)
                {
                    WriteLine(context.Entry(carToUpdate).State);
                    carToUpdate.Color = "Blue";
                    WriteLine(context.Entry(carToUpdate).State);
                    context.SaveChanges();
                }
            }
        }
    }
}
