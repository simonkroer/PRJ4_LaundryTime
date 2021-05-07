using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models.Booking;

namespace LaundryTime.Data
{
    public class BookingSeeder
    {
        public void CreateNewBookList(ApplicationDbContext context, DateModel date)
        {
            ApplicationDbContext _context = context;
            IDataAccessAction dataAcces = new DataAccsessAction(_context);
            BookingListModel[] BooklistM1tmp = new BookingListModel[15];

            var machines = dataAcces.Machines.GetAllMachines();
            foreach (var machine in machines)
            {
                for (int i = 8; i < 23; i++)
                {
                    int t = i - 8;
                    string time = i.ToString() + "-" + (i + 1).ToString();
                    BooklistM1tmp[t] = new BookingListModel()
                    {
                        DateModel = date,
                        Date = date.DateData,
                        Status = true,
                        Machine = machine,
                        Time = time
                    };
                    dataAcces.BookingList.Add(BooklistM1tmp[t]);
                }
            }

            dataAcces.Complete();
        }
        public DateModel CreateDateModel(ApplicationDbContext context, string dato)
        {
            ApplicationDbContext _context = context;
            IDataAccessAction dataAcces = new DataAccsessAction(_context);
            //DatePickerModel date = new DatePickerModel();
            DateTime date = DateTime.Parse(dato);
            DateModel newDateModel = new DateModel()
            {
                DateData = date.Date
            };

            _context.DateModels.Add(newDateModel);
            dataAcces.Complete();

            return newDateModel;
        }

    }
}
