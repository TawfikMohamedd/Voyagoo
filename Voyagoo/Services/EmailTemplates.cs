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
    }
}
