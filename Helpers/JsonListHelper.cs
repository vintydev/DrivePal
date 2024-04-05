using DrivePal.Models;
using DrivePal.Models.ViewModels;

namespace DrivePal.Helpers
{
    public static class JSONListHelper
    {
        // Existing methods remain unchanged...

        public static string GetDrivingClassListJSONString(List<DrivingClass> drivingClasses)
        {
            var drivingClassList = new List<object>(); // Using a list of object to handle complex types

            foreach (var drivingClass in drivingClasses)
            {
                string title = drivingClass.IsReserved == false ? "Lesson - Available" : "Lesson";
                string description = "£" + drivingClass.Price.ToString();
                if (drivingClass.Learner != null)
                {
                    description += $", Learner: {drivingClass.Learner.FirstName}";
                    if (!string.IsNullOrEmpty(drivingClass.Learner.PostCode))
                    {
                        description += $", Postcode: {drivingClass.Learner.PostCode}";
                    }
                    if (!string.IsNullOrEmpty(drivingClass.Learner.Street))
                    {
                        description += $", Street: {drivingClass.Learner.Street}";
                    }
                }
                var classObj = new
                {
                    id = drivingClass.DrivingClassId,
                    start = drivingClass.DrivingClassStart,
                    end = drivingClass.DrivingClassEnd,
                    price = drivingClass.Price,
                    title = title,
                    description = description,
                    isReserved = drivingClass.IsReserved,
                    instructorId = drivingClass.InstructorId,
                    learnerId = drivingClass.LearnerId,
                    // Assuming you want to include basic details of Instructor and Learner if available
                    instructor = drivingClass.Instructor != null ? new { drivingClass.Instructor.FirstName, drivingClass.Instructor.Id } : null,
                    learner = drivingClass.Learner != null ? new { drivingClass.Learner.FirstName, drivingClass.Learner.Id } : null,
                    // You can add more properties here as needed
                };
                drivingClassList.Add(classObj);
            }

            return System.Text.Json.JsonSerializer.Serialize(drivingClassList);
        }

        public static string GetBookingListJSONString(LearnerBookingsViewModel bookingsViewModel)
        {
            var bookingList = new List<object>(); // Using a list of object to handle complex types

            foreach (var booking in bookingsViewModel.Bookings)
            {
                string title = $"Lesson with {booking.InstructorFirstName} {booking.LearnerLastName}";
                string description = $"£{booking.Price}, Instructor: {booking.InstructorLastName}, Learner: {booking.LearnerLastName}";

                var classObj = new
                {
                    id = booking.BookingId,
                    start = booking.DrivingClassStart,
                    end = booking.DrivingClassEnd,
                    price = booking.Price,
                    title = title,
                    description = description,
                    
                };
                bookingList.Add(classObj);
            }

            return System.Text.Json.JsonSerializer.Serialize(bookingList);
        }
    }
}

