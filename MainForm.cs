using System;
using System.Drawing;
using System.Windows.Forms;

namespace HotelRoomBookingManager
{
	public class MainForm : Form
	{
		// Backend
		private BookingManager manager = new BookingManager();

		// Controls
		private TextBox txtGuestName;
		private TextBox txtRoomNumber;
		private DateTimePicker dtpCheckIn;
		private DateTimePicker dtpCheckOut;
		private Button btnBookRoom;
		private Button btnCancelBooking;
		private Button btnViewAllBookings;
		private Button btnExit;
		private ListBox lstBookings;
		private Label lblStatus;

		public MainForm()
		{
			// Form properties
			this.Text = "Hotel Room Booking Manager";
			this.Size = new Size(600, 450);
			this.StartPosition = FormStartPosition.CenterScreen;

			InitializeControls();
		}

		private void InitializeControls()
		{
			// Guest Name
			var lblGuest = new Label() { Text = "Guest Name:", Location = new Point(20, 20), AutoSize = true };
			this.Controls.Add(lblGuest);
			txtGuestName = new TextBox() { Location = new Point(120, 18), Width = 150 };
			this.Controls.Add(txtGuestName);

			// Room Number
			var lblRoom = new Label() { Text = "Room Number:", Location = new Point(300, 20), AutoSize = true };
			this.Controls.Add(lblRoom);
			txtRoomNumber = new TextBox() { Location = new Point(400, 18), Width = 80 };
			this.Controls.Add(txtRoomNumber);

			// Check-In
			var lblCheckIn = new Label() { Text = "Check-In:", Location = new Point(20, 60), AutoSize = true };
			this.Controls.Add(lblCheckIn);
			dtpCheckIn = new DateTimePicker() { Location = new Point(120, 58), Width = 150 };
			this.Controls.Add(dtpCheckIn);

			// Check-Out
			var lblCheckOut = new Label() { Text = "Check-Out:", Location = new Point(300, 60), AutoSize = true };
			this.Controls.Add(lblCheckOut);
			dtpCheckOut = new DateTimePicker() { Location = new Point(400, 58), Width = 150 };
			this.Controls.Add(dtpCheckOut);

			// Buttons
			btnBookRoom = new Button() { Text = "Book Room", Location = new Point(20, 100), Width = 120 };
			btnBookRoom.Click += BtnBookRoom_Click;
			this.Controls.Add(btnBookRoom);

			btnCancelBooking = new Button() { Text = "Cancel Booking", Location = new Point(160, 100), Width = 120 };
			btnCancelBooking.Click += BtnCancelBooking_Click;
			this.Controls.Add(btnCancelBooking);

			btnViewAllBookings = new Button() { Text = "View All Bookings", Location = new Point(300, 100), Width = 140 };
			btnViewAllBookings.Click += BtnViewAllBookings_Click;
			this.Controls.Add(btnViewAllBookings);

			btnExit = new Button() { Text = "Exit", Location = new Point(460, 100), Width = 80 };
			btnExit.Click += BtnExit_Click;
			this.Controls.Add(btnExit);

			// ListBox
			lstBookings = new ListBox() { Location = new Point(20, 150), Width = 540, Height = 200 };
			this.Controls.Add(lstBookings);

			// Status Label
			lblStatus = new Label() { Location = new Point(20, 360), Width = 540, Height = 30, ForeColor = Color.Green };
			this.Controls.Add(lblStatus);
		}

		// Event Handlers
		private void BtnBookRoom_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(txtGuestName.Text) || string.IsNullOrWhiteSpace(txtRoomNumber.Text))
				{
					MessageBox.Show("All fields are required.");
					return;
				}

				if (dtpCheckOut.Value <= dtpCheckIn.Value)
				{
					MessageBox.Show("Check-Out must be after Check-In.");
					return;
				}

				Booking newBooking = new Booking()
				{
					GuestName = txtGuestName.Text,
					RoomNumber = txtRoomNumber.Text,
					CheckIn = dtpCheckIn.Value,
					CheckOut = dtpCheckOut.Value
				};

				if (manager.AddBooking(newBooking))
				{
					RefreshList();
					ClearInputs();
					SetStatus($"Room {newBooking.RoomNumber} booked successfully.");
				}
				else
				{
					MessageBox.Show($"Room {newBooking.RoomNumber} is already booked during that time.");
					SetStatus("Booking failed.");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message);
			}
		}

		private void BtnCancelBooking_Click(object sender, EventArgs e)
		{
			if (manager.CancelBooking(txtRoomNumber.Text, txtGuestName.Text))
			{
				RefreshList();
				ClearInputs();
				SetStatus($"Booking for Room {txtRoomNumber.Text} cancelled.");
			}
			else
			{
				MessageBox.Show("Booking not found.");
				SetStatus("Cancel failed.");
			}
		}

		private void BtnViewAllBookings_Click(object sender, EventArgs e)
		{
			RefreshList();
		}

		private void BtnExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		// Helper methods
		private void RefreshList()
		{
			lstBookings.Items.Clear();
			foreach (var b in manager.ListAll())
			{
				lstBookings.Items.Add(b.ToString());
			}
		}

		private void ClearInputs()
		{
			txtGuestName.Clear();
			txtRoomNumber.Clear();
			dtpCheckIn.Value = DateTime.Now;
			dtpCheckOut.Value = DateTime.Now;
		}

		private void SetStatus(string message)
		{
			lblStatus.Text = message;
		}
	}
}