using System;
using System.Collections.Generic;

public class BookingManager
{
	private List<Booking> bookings = new List<Booking>();

	public bool AddBooking(Booking b)
	{
		if (!CheckAvailability(b.RoomNumber, b.CheckIn, b.CheckOut))
			return false;
		bookings.Add(b);
		return true;
	}

	public bool CancelBooking(string roomNumber, string guestName)
	{
		Booking toRemove = bookings.Find(b => b.RoomNumber == roomNumber && b.GuestName == guestName);
		if (toRemove != null)
		{
			bookings.Remove(toRemove);
			return true;
		}
		return false;
	}

	public List<Booking> ListAll()
	{
		return bookings;
	}

	public bool CheckAvailability(string roomNumber, DateTime checkIn, DateTime checkOut)
	{
		foreach (var b in bookings)
		{
			if (b.RoomNumber == roomNumber &&
			   !(checkOut <= b.CheckIn || checkIn >= b.CheckOut))
			{
				return false;
			}
		}
		return true;
	}
}