using MailKit.Security;

internal class Program
{
    private static async Task Main()
    {
        string email = "thibaud.rolain@gmail.com";           // ton Gmail complet
        string appPassword = "miahnvzvhzlodksa";            // mot de passe d'application sans espaces

        using var client = new MailKit.Net.Smtp.SmtpClient();

        try
        {
            // Connexion au serveur Gmail
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            Console.WriteLine("✅ Connecté au serveur SMTP");

            // Authentification
            await client.AuthenticateAsync(email, appPassword);
            Console.WriteLine("✅ Authentification réussie !");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Échec : " + ex.Message);
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}