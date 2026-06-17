using Voyagoo.Contracts.Hotels;
using Voyagoo.Contracts.Restaurants;
using Voyagoo.Contracts.TourGuides;

namespace Voyagoo.Services
{
    public static class EmailTemplates
    {
        public static string GetOtpEmailTemplate(string firstName, string otpCode)
        {
            return $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8' />
                <style>
                    body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }}
                    .container {{ max-width: 600px; margin: 40px auto; background-color: #ffffff;
                                  border-radius: 10px; padding: 40px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }}
                    .header {{ text-align: center; margin-bottom: 30px; }}
                    .header h1 {{ color: #2c3e50; font-size: 28px; }}
                    .otp-box {{ background-color: #f0f4ff; border: 2px dashed #4a6cf7;
                                border-radius: 8px; padding: 20px; text-align: center; margin: 30px 0; }}
                    .otp-code {{ font-size: 42px; font-weight: bold; color: #4a6cf7; letter-spacing: 10px; }}
                    .message {{ color: #555555; font-size: 15px; line-height: 1.7; }}
                    .warning {{ color: #e74c3c; font-size: 13px; margin-top: 20px; }}
                    .footer {{ text-align: center; margin-top: 40px; color: #aaaaaa; font-size: 12px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>🌍 Voyagoo</h1>
                    </div>
                    <p class='message'>Hi <strong>{firstName}</strong>,</p>
                    <p class='message'>
                        We received a request to reset the password for your Voyagoo account.
                        Use the OTP code below to proceed:
                    </p>
                    <div class='otp-box'>
                        <div class='otp-code'>{otpCode}</div>
                    </div>
                    <p class='message'>This code is valid for <strong>10 minutes</strong>.</p>
                    <p class='warning'>
                        ⚠️ If you did not request a password reset, please ignore this email
                        or contact our support team immediately.
                    </p>
                    <div class='footer'>
                        <p>© 2025 Voyagoo. All rights reserved.</p>
                    </div>
                </div>
            </body>
            </html>";
        }

        public static string GetHotelBookingConfirmationTemplate(string firstName,CreateHotelBookingResponse booking)
        {
            var roomsRows = string.Join("", booking.Rooms.Select(r => $@"
        <tr>
            <td style='padding: 8px; border: 1px solid #ddd;'>{r.RoomType}</td>
            <td style='padding: 8px; border: 1px solid #ddd;'>{r.Quantity}</td>
            <td style='padding: 8px; border: 1px solid #ddd;'>{r.PricePerNight} EGP</td>
            <td style='padding: 8px; border: 1px solid #ddd;'>{r.Total} EGP</td>
        </tr>"));

            var featuresRows = string.Join("", booking.Features.Select(f => $@"
        <tr>
            <td style='padding: 8px; border: 1px solid #ddd;'>{f.Name}</td>
            <td style='padding: 8px; border: 1px solid #ddd;'>{f.RoomsCount}</td>
            <td style='padding: 8px; border: 1px solid #ddd;'>{f.PricePerNight} EGP</td>
            <td style='padding: 8px; border: 1px solid #ddd;'>{f.Total} EGP</td>
        </tr>"));

            return $@"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8' />
        <style>
            body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }}
            .container {{ max-width: 650px; margin: 40px auto; background-color: #ffffff;
                          border-radius: 10px; padding: 40px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }}
            .header {{ text-align: center; margin-bottom: 30px; }}
            .header h1 {{ color: #2c3e50; font-size: 28px; }}
            .booking-box {{ background-color: #f0f4ff; border-radius: 8px; padding: 20px; margin: 20px 0; }}
            .booking-box h3 {{ color: #4a6cf7; margin-bottom: 10px; }}
            table {{ width: 100%; border-collapse: collapse; margin-top: 10px; }}
            th {{ background-color: #4a6cf7; color: white; padding: 10px; text-align: left; }}
            td {{ padding: 8px; border: 1px solid #ddd; }}
            .summary-row {{ display: flex; justify-content: space-between; padding: 6px 0; }}
            .total-row {{ font-size: 18px; font-weight: bold; color: #4a6cf7;
                          border-top: 2px solid #4a6cf7; margin-top: 10px; padding-top: 10px; }}
            .footer {{ text-align: center; margin-top: 40px; color: #aaaaaa; font-size: 12px; }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <h1>🌍 Voyagoo</h1>
                <p style='color: #555;'>Booking Confirmation</p>
            </div>

            <p>Hi <strong>{firstName}</strong>,</p>
            <p>Your booking has been confirmed! Here's your summary:</p>

            <!-- Booking Info -->
            <div class='booking-box'>
                <h3>🏨 {booking.HotelName}</h3>
                <div class='summary-row'><span>Booking ID</span><span>#{booking.BookingId}</span></div>
                <div class='summary-row'><span>Check-in</span><span>{booking.CheckIn}</span></div>
                <div class='summary-row'><span>Check-out</span><span>{booking.CheckOut}</span></div>
                <div class='summary-row'><span>Nights</span><span>{booking.Nights} nights</span></div>
            </div>

            <!-- Rooms Table -->
            <h3 style='color: #2c3e50;'>🛏️ Rooms</h3>
            <table>
                <tr>
                    <th>Room Type</th>
                    <th>Quantity</th>
                    <th>Price/Night</th>
                    <th>Total</th>
                </tr>
                {roomsRows}
            </table>

            <!-- Features Table -->
            <h3 style='color: #2c3e50; margin-top: 20px;'>✨ Features & Board</h3>
            <table>
                <tr>
                    <th>Feature</th>
                    <th>Rooms</th>
                    <th>Price/Night</th>
                    <th>Total</th>
                </tr>
                {featuresRows}
            </table>

            <!-- Price Summary -->
            <div class='booking-box' style='margin-top: 20px;'>
                <h3>💰 Price Summary</h3>
                <div class='summary-row'><span>Rooms Total </span><span>{booking.RoomsTotal} EGP</span></div>
                <div class='summary-row'><span>Boards Total </span><span>{booking.BoardsTotal} EGP</span></div>
                <div class='summary-row'><span>Extras Total </span><span>{booking.ExtrasTotal} EGP</span></div>
                <div class='summary-row'><span>Subtotal </span><span> {booking.Subtotal} EGP</span></div>
                <div class='summary-row' style='color: #e74c3c;'>
                    <span>Discount  ({booking.DiscountPercentage}%)</span>
                    <span> - {booking.DiscountAmount} EGP</span>
                </div>
                <div class='summary-row' style='color: #e67e22;'>
                    <span>Service Charge ({booking.ServiceChargePercentage}%)</span>
                    <span> + {booking.ServiceChargeAmount} EGP</span>
                </div>
                <div class='summary-row total-row'>
                    <span>Total Price</span>
                    <span>{booking.TotalPrice} EGP</span>
                </div>
            </div>

            <p style='color: #555; margin-top: 20px;'>
                Thank you for choosing Voyagoo! We wish you a pleasant stay. 🌟
            </p>

            <div class='footer'>
                <p>© 2025 Voyagoo. All rights reserved.</p>
            </div>
        </div>
    </body>
    </html>";
        }

        public static string GetRestaurantBookingConfirmationTemplate(string firstName,CreateBookingResponse booking)
        {
            return $@"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8' />
        <style>
            body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }}
            .container {{ max-width: 650px; margin: 40px auto; background-color: #ffffff;
                          border-radius: 10px; padding: 40px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }}
            .header {{ text-align: center; margin-bottom: 30px; }}
            .header h1 {{ color: #2c3e50; font-size: 28px; }}
            .booking-box {{ background-color: #f0f4ff; border-radius: 8px; padding: 20px; margin: 20px 0; }}
            .booking-box h3 {{ color: #4a6cf7; margin-bottom: 10px; }}
            .summary-row {{ display: flex; justify-content: space-between; padding: 6px 0;
                            border-bottom: 1px solid #eee; }}
            .footer {{ text-align: center; margin-top: 40px; color: #aaaaaa; font-size: 12px; }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <h1>🌍 Voyagoo</h1>
                <p style='color: #555;'>Restaurant Booking Confirmation</p>
            </div>

            <p>Hi <strong>{firstName}</strong>,</p>
            <p>Your restaurant booking has been confirmed! Here's your summary:</p>

            <div class='booking-box'>
                <h3>🍽️ {booking.RestaurantName}</h3>
                <div class='summary-row'><span>Booking ID</span><span>#{booking.BookingId}</span></div>
                <div class='summary-row'><span>📍 Address</span><span>{booking.RestaurantAddress}</span></div>
                <div class='summary-row'><span>📅 Date</span><span>{booking.BookingDate}</span></div>
                <div class='summary-row'><span>👤 Guest Name</span><span>{booking.GuestName}</span></div>
                <div class='summary-row'><span>📞 Guest Phone</span><span>{booking.GuestPhone}</span></div>
            </div>

            <div class='booking-box'>
                <h3>🪑 Tables</h3>
                <div class='summary-row'><span>Tables for 2 x </span><span>{booking.TablesForTwo}</span></div>
                <div class='summary-row'><span>Tables for 4 x </span><span>{booking.TablesForFour}</span></div>
                <div class='summary-row'><span>Tables for 6 x </span><span>{booking.TablesForSix}</span></div>
            </div>

            <p style='color: #555; margin-top: 20px;'>
                Thank you for choosing Voyagoo! We wish you a wonderful dining experience. 🌟
            </p>

            <div class='footer'>
                <p>© 2025 Voyagoo. All rights reserved.</p>
            </div>
        </div>
    </body>
    </html>";
        }

        public static string GetTourGuideBookingConfirmationTemplate(string firstName,CreateTourGuideBookingResponse booking)
        {
            return $@"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8' />
        <style>
            body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }}
            .container {{ max-width: 650px; margin: 40px auto; background-color: #ffffff;
                          border-radius: 10px; padding: 40px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }}
            .header {{ text-align: center; margin-bottom: 30px; }}
            .header h1 {{ color: #2c3e50; font-size: 28px; }}
            .booking-box {{ background-color: #f0f4ff; border-radius: 8px; padding: 20px; margin: 20px 0; }}
            .booking-box h3 {{ color: #4a6cf7; margin-bottom: 10px; }}
            .summary-row {{ display: flex; justify-content: space-between; padding: 6px 0;
                            border-bottom: 1px solid #eee; }}
            .total-row {{ font-size: 18px; font-weight: bold; color: #4a6cf7;
                          border-top: 2px solid #4a6cf7; margin-top: 10px; padding-top: 10px; }}
            .footer {{ text-align: center; margin-top: 40px; color: #aaaaaa; font-size: 12px; }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <h1>🌍 Voyagoo</h1>
                <p style='color: #555;'>Tour Guide Booking Confirmation</p>
            </div>

            <p>Hi <strong>{firstName}</strong>,</p>
            <p>Your tour guide booking has been confirmed! Here's your summary:</p>

            <div class='booking-box'>
                <h3>🧭 {booking.TourGuideName}</h3>
                <div class='summary-row'><span>Booking ID</span><span>#{booking.BookingId}</span></div>
                <div class='summary-row'><span>📅 Booking Date</span><span>{booking.BookingDate}</span></div>
                <div class='summary-row'><span>🌙 Number of Days</span><span>{booking.NumberOfDays} days</span></div>
                <div class='summary-row'><span>💵 Price Per Day</span><span>{booking.PricePerDay} EGP</span></div>
            </div>

            <div class='booking-box'>
                <h3>💰 Price Summary</h3>
                <div class='summary-row'><span>Price Per Day</span><span>{booking.PricePerDay} EGP</span></div>
                <div class='summary-row'><span>Number of Days</span><span>{booking.NumberOfDays}</span></div>
                <div class='summary-row total-row'><span>Total Price</span><span>{booking.TotalPrice} EGP</span></div>
            </div>

            <p style='color: #555; margin-top: 20px;'>
                Thank you for choosing Voyagoo! We wish you an amazing experience. 🌟
            </p>

            <div class='footer'>
                <p>© 2025 Voyagoo. All rights reserved.</p>
            </div>
        </div>
    </body>
    </html>";
        }
    }
}
