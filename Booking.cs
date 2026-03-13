public class Booking
{
	public string RoomNumber { get; set; }
	public string GuestName { get; set; }
	public DateTime CheckIn { get; set; }
	public DateTime CheckOut { get; set; }

	public override string ToString()
	{
		return $"[{RoomNumber}] {CheckIn:g} - {CheckOut:g} {GuestName}";
	}
}