using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTimeWebAppWithIdentity.Data;
using LaundryTimeWebAppWithIdentity.Unit_of_Work;

namespace LaundryTimeWebAppWithIdentity.Models.Calender
{
    public class SeedDatabase
    {
        
        
        public void SeedData()
        {
            //using (var context = new ApplicationDbContext())
            //{
            //    IUnitOfWork unitOfWork = new UnitOfWork(context);

            //    UserModel Alexander = new UserModel()
            //    {
            //        FirstName = "Alexander",
            //        LastName = "Wodstrup",
            //        reservedBooking = CreateNewReservedBookingList(),
            //    };
            //    unitOfWork.Users.Add(Alexander);
            //    CreateDateModel(unitOfWork);
            //    CreateNewBookList(unitOfWork,1); // 1 er tmp
            //    unitOfWork.Complete();
            //}
            
        }

        public List<ReservedBookingListModel> CreateNewReservedBookingList()
        {
            List<ReservedBookingListModel> emptyList = new List<ReservedBookingListModel>();
            return emptyList;
        }

        public void CreateNewBookList(IUnitOfWork unitOfWork, int dateId)
        {
            BookingListModel[] BooklistM1tmp = new BookingListModel[15];
            BookingListModel[] BooklistM2tmp = new BookingListModel[15];

            for (int i = 8; i < 23; i++)
            {
                int t = i - 8;
                string time = i.ToString() +"-"+ (i + 1).ToString();
                BooklistM1tmp[t] = new BookingListModel()
                {
                    DateModel = unitOfWork.DateModelLists.Get(dateId),
                    Status = "Available",
                    Machine = "Washing Machine 1",
                    Time = time
                };
                unitOfWork.BookingLists.Add(BooklistM1tmp[t]);
                
                BooklistM2tmp[t] = new BookingListModel()
                {
                    DateModel = unitOfWork.DateModelLists.Get(dateId),
                    Status = "Available",
                    Machine = "Washing Machine 2",
                    Time = time
                };
                unitOfWork.BookingLists.Add(BooklistM2tmp[t]);
            }
            unitOfWork.Complete();
        }

        public void CreateDateModel(IUnitOfWork unitOfWork)
        {
            DatePickerModel date = new DatePickerModel();
            DateModel newDateModel = new DateModel()
            {
                Datedata = date.Datedata
            };

            unitOfWork.DateModelLists.Add(newDateModel);
            unitOfWork.Complete();
        }
    }
}
