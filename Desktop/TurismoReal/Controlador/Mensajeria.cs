﻿using System.Net;
using System.Net.Mail;

namespace Controlador
{
    public class Mensajeria
    {
        public static void PlanificarTransporte(string receptor, string asunto, string cantidadPersonas, string lugar, string comuna, string horaIda, string horaVuelta, string dpto, string direccion)
        {
            string fromMail = "TurismoRealNoResponder@gmail.com";
            string fromPass = "gowektlbweashvlg";

            MailMessage mail = new()
            {
                From = new MailAddress(fromMail),
                Subject = asunto,
                Body = $"Transporte para {cantidadPersonas}, el día {horaIda} en {lugar}, {comuna}, con dirección {dpto}, {direccion}.\n Fecha de vuelta {horaVuelta}"
            };
            mail.To.Add(new MailAddress(receptor));
            var smtpCliente = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPass),
                EnableSsl = true
            };
            smtpCliente.UseDefaultCredentials = true;
            smtpCliente.Send(mail);
        }
    }
}
