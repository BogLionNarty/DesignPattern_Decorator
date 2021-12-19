using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DEKORATOR_LG
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] brzydkiewyrazy = { "fifa22", "gra", "robot" }; // zakazane wyrazy
            string ciag = "List"; // zakazany ciąg 
            bool doEncrypt = true; // szyfrowanie

           
            Decorator mailbox = new DecoratorEncryption(new DecoratorCensor(new DecoratorFilter(new DekoratorPostage(new DecoratorPickUpTime(new DecoratorNumbering(new Mailbox()))), brzydkiewyrazy), ciag), doEncrypt);
           
           
            mailbox.push("Listnr1");
            mailbox.push("Lisnr");
            mailbox.push("gra");
            mailbox.push("koko");
            mailbox.push("dasd");
            mailbox.push("kulkaasdsd");
            mailbox.push("pies");
            mailbox.push("fifa");
            mailbox.push("fifa22");
            mailbox.push("koronawirus");
            
            for (int i = 0; i < mailbox.length(); i++)
            {
                Console.WriteLine(mailbox.get());
            }
            Console.WriteLine("\n.....................\n");

            Decorator mailbox2 = new DecoratorNumbering(new DecoratorEncryption(new DecoratorPickUpTime (new Mailbox()),doEncrypt)); 

            mailbox2.push("Szafka");
            mailbox2.push("List ne 2");
            for (int i = 0; i < mailbox2.length(); i++)
            {
                Console.WriteLine(mailbox2.get());
            }
            Console.WriteLine("\n.....................\n");
        }
    }

    interface IMailbox
    {
        void push(string message);
        void pop();
        string get();
        int length();
    }

    public class Mailbox : IMailbox
    {

        public LinkedList<string> mailbox = new LinkedList<string>();
        int size=0;
        public string get()
        {
            foreach (string mail in mailbox)
            {
                mailbox.RemoveFirst();
                return mail;
            }
            return " ";
        }

        public void pop()
        {
            mailbox.RemoveLast();
            size--;
        }

        public void push(string message)
        {
            mailbox.AddLast(message);
            size++;
        }
        public int length()
        {
            return size;
        }

    }
    abstract class Decorator : IMailbox
    {
        protected IMailbox obj;
        public Decorator(IMailbox obj)
        {
            this.obj = obj;
        }
        public virtual void push(string tekst)
        {
            obj.push(tekst);
        }

        public virtual void pop()
        {
            obj.pop();
        }

        public virtual string get()
        {
            return obj.get();
        }

        public int length()
        {
           return obj.length();
        }
    }

    class DekoratorPostage : Decorator
    {
        public DekoratorPostage(IMailbox obj) : base(obj)
        {
        }

        public override void push(string message)
        {
            base.push("Wysłano: " + DateTime.Now.ToString("h:mm:ss") + " " + message);
        }

        public override void pop()
        {
            base.pop();
        }

        public override string get()
        {
            return base.get();
        }
    }
    class DecoratorPickUpTime : Decorator
    {
        public DecoratorPickUpTime(IMailbox obj) : base(obj)
        {
        }

        public override void push(string message)
        {
            base.push("Odebrano: " + DateTime.Now.ToString("h:mm:ss") + " " + message);
        }

        public override void pop()
        {
            base.pop();
        }

        public override string get()
        {
            return base.get();
        }
    }
    class DecoratorNumbering : Decorator
    {
        public DecoratorNumbering(IMailbox obj) : base(obj)
        {
        }

        private int id = 1;

        public override void push(string message)
        {
            base.push("Numer to: " + id.ToString() + " " + message);
            this.id++;
        }

        public override void pop()
        {
            base.pop();
        }

        public override string get()
        {
            return base.get();
        }
    }
    class DecoratorFilter : Decorator
    {
        private string[] wyrazy;
        public DecoratorFilter(IMailbox obj, string[] brzydkie) : base(obj)
        {
            this.wyrazy = brzydkie;
        }

        public override string get()
        {
            return base.get();
        }

        public override void pop()
        {
            base.pop();
        }

        public override void push(string message)
        {
            bool isThere = false;
            for (int i = 0; i < wyrazy.Length; i++)
            {
                if (message == wyrazy[i])
                {
                    message = "Wiadomość sfiltrowana ///wyświetlam, żeby pokazać, że działa :)";
                    base.push(message);
                    isThere = true;
                    break;
                }
            }
            if (isThere == false)
            {
                base.push(message);
            }
        }
    }
    class DecoratorCensor : Decorator
    {
        private string sequence;
        public DecoratorCensor(IMailbox obj, string sequence) : base(obj)
        {
            this.sequence = sequence;
        }

        public override string get()
        {
            return base.get();
        }

        public override void pop()
        {
            base.pop();
        }

        public override void push(string message)
        {
            if (message.Contains(sequence))
            {
                message = message.Replace(sequence, "***");
                base.push(message);
            }
            else
                base.push(message);

        }
    }
    class DecoratorEncryption : Decorator
    {
        private bool doEncrypt = false;
        public DecoratorEncryption(IMailbox obj, bool doEncrypt) : base(obj)
        {
            this.doEncrypt = doEncrypt;
        }

        public override string get()
        {

            return base.get();
        }

        public override void pop()
        {
            base.pop();
        }

        public override void push(string message)
        {
            if (doEncrypt == true)
            {
                base.push(Encrypt(message));
            }
            else
                base.push(message);

        }

        public static string Encrypt(string text)
        {
            var b = Encoding.UTF8.GetBytes(text);
            var encrypted = getAes().CreateEncryptor().TransformFinalBlock(b, 0, b.Length);
            return Convert.ToBase64String(encrypted);
        }

        static Aes getAes()
        {
            var keyBytes = new byte[16];
            var skeyBytes = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            Array.Copy(skeyBytes, keyBytes, Math.Min(keyBytes.Length, skeyBytes.Length));

            Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
            aes.Key = keyBytes;
            aes.IV = keyBytes;
            return aes;
        }

    }
}


