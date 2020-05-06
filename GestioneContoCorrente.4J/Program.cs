using System;

namespace GestioneContoCorrente._4J
{
    class Program
    {
        static Banca banca = new Banca("Piazza Cavour 33", "Banca Monte Dei Paschi Di Siena", "0541 58911");
        static void Main(string[] args)
        {
            int menu;
            do
            {
                Console.WriteLine("##############################\n| 1. Crea conto corrente.    |\n| 2. Aggiungi movimento      |\n| 3. Elimina conto corrente. |\n| 4. Stampa                  |\n| 0. Esci dal programma      |\n##############################\n");
                Console.WriteLine("Seleziona l'azione da svolgere.");
                menu = int.Parse(Console.ReadLine());
                switch (menu)
                {
                    case 0:
                        break;

                    case 1:
                        CreaContoCorrente();
                        break;

                    case 2:
                        AggiungiMovimento();
                        break;

                    case 3:
                        EliminaConto();
                        break;

                    case 4:
                        Stampa();
                        break;

                    default:
                        Console.WriteLine("Hai selezionato un'azione non esistente.");
                        break;
                }
            } while (menu != 0);

            Console.WriteLine("Grazie per aver scelto la nostra banca!!!");
            
            
            Console.ReadLine();

        }

        /// <summary>
        /// Metodo che permette la creazione di un conto corrente standard o online, con la precedente creazione dell'intestatario
        /// </summary>
        public static void CreaContoCorrente()
        {
            int contoOnline = 0;
            string nome, cognome, codiceFiscale, data, iban;
            double saldo;

            try
            {
                Console.WriteLine("Creazione Intestatario.");
                Console.WriteLine("Nome: ");
                nome = Console.ReadLine();
                Console.WriteLine("Cognome: ");
                cognome = Console.ReadLine();
                Console.WriteLine("Data di nascita (yyyy/mm/gg):");
                data = Console.ReadLine();
                Console.WriteLine("Codice Fiscale: ");
                codiceFiscale = Console.ReadLine();
                Console.WriteLine("\nCreazione conto corrente");
                do
                {
                    Console.WriteLine("Seleziona tipo di conto.\n###############################\n| 0. Conto corrente standard. |\n| 1. Conto online             |\n###############################");
                    contoOnline = int.Parse(Console.ReadLine());
                    Console.WriteLine("Inserisci il tuo IBAN");
                    iban = Console.ReadLine();
                    Console.WriteLine("Inserisci il tuo saldo");
                    saldo = double.Parse(Console.ReadLine());
                    if (contoOnline == 0)
                    {
                        banca.AggiungiConto(new ContoCorrente(banca, new Intestatario(nome, cognome, DateTime.Parse(data), codiceFiscale), saldo, iban, 0.50, 1));
                    }
                    else if (contoOnline == 1)
                    {
                        banca.AggiungiConto(new ContoOnline(banca, new Intestatario(nome, cognome, DateTime.Parse(data), codiceFiscale), saldo, iban, 0.50, 1, 1000));
                    }
                    else
                    {
                        Console.WriteLine("Hai sbagliato l'azione da svolgere. Ritenta!!!");
                    }
                } while (contoOnline != 0 && contoOnline != 1);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRORE!!!");
            }
        }

        /// <summary>
        /// Metodo che aggiunge un movimento (Prelievo, Versamento, Bonifico) ad un conto corrente prestabilito, attua inoltre un controllo sulla presenza di conti corrente nella banca
        /// </summary>
        private static void AggiungiMovimento()
        {
            string iban, data, ibanDest;
            int selezione;
            double importo;

            try
            {
                if (banca.Conti.Count != 0)
                {
                    Console.WriteLine("Inserisci l'IBAN del conto corrente su cui vuoi attuare un movimento.");
                    iban = Console.ReadLine();
                    ContoCorrente c = banca.RicercaConto(iban);
                    Console.WriteLine("In che data è avvenuto il movimento.(yyyy/mm/dd)");
                    data = Console.ReadLine();
                    Console.WriteLine("Qual'è l'importo del movimento");
                    importo = double.Parse(Console.ReadLine());
                    if (c != null)
                    {
                        Console.WriteLine("Che movimento vuoi attuare.\n#################\n| 1. Prelievo   |\n| 2 Versamento  |\n| 3. Bonifico   |\n#################");
                        selezione = int.Parse(Console.ReadLine());
                        switch (selezione)
                        {
                            case 1:
                                c.Prelievo(importo, DateTime.Parse(data));
                                break;
                            case 2:
                                c.Versamento(importo, DateTime.Parse(data));
                                break;
                            case 3:
                                Console.WriteLine("Qual'è l'IBAN del destinatario?");
                                ibanDest = Console.ReadLine();
                                AttuaBonifico(banca, c, DateTime.Parse(data), ibanDest, importo);
                                break;
                            default:
                                Console.WriteLine("Hai selezionato un'azione non esistente.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERRORE!!! non è stato trovato il conto corrente desiderato");
                    }
                }
                else
                {
                    Console.WriteLine("Nella lista della banca non sono presenti conti corrente");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRORE!!!");
            }
        }

        /// <summary>
        /// Metodo che applica il bonifico se il conto destinatario si trova nella stessa banca, mentre memorizza il bonifico nel caso in cui il destinatario abbia un conto in una banca diversa
        /// </summary>
        /// <param name="b">Banca</param>
        /// <param name="c">Conto corrente del mittente</param>
        /// <param name="dataMovimento">Data in cui è avvenuto il bonifico</param>
        /// <param name="iban">Iban del destinatario</param>
        /// <param name="importoBonifico">importo mandato tramite bonifico</param>
        public static void AttuaBonifico(Banca b, ContoCorrente c, DateTime dataMovimento, string iban, double importoBonifico)
        {
            if (b.RicercaConto(iban) != null)
            {
                b.RicercaConto(iban).Versamento(c.Bonifico(importoBonifico, dataMovimento, iban), dataMovimento);
            }
            else
            {
                c.Bonifico(importoBonifico, dataMovimento, iban);
            }
        }

        /// <summary>
        /// Metodo che elimina un conto dalla banca
        /// </summary>
        private static void EliminaConto()
        {
            string iban;
            Console.WriteLine("Inserisci l'iban del conto che vuoi elliminare");
            iban = Console.ReadLine();
            ContoCorrente c = banca.RicercaConto(iban);
            if (c != null)
            {
                banca.EliminaConto(iban);
            }
            else
            {
                Console.WriteLine("L'IBAN selezionato non è presente nei conti corrente della banca");
            }
        }


        /// <summary>
        /// Metodo che stampa in base alla scelta dell'utente i conti corrente della banca o i movimenti di un dato conto corrente, con la chiamata al metodo adatto
        /// </summary>
        private static void Stampa()
        {
            int selezione;
            string iban;
            ContoCorrente c;
            try
            {
                if (banca.Conti.Count != 0)
                {
                    Console.WriteLine("Cosa vuoi stampare?\n############################################\n| 1. Stampa conti corrente                 |\n| 2. Stampa movimenti di un conto corrente |\n############################################");
                    selezione = int.Parse(Console.ReadLine());
                    switch (selezione)
                    {
                        case 1:
                            StampaContiCorrente();
                            break;

                        case 2:
                            Console.WriteLine("Inserisci l'IBAN del conto di cui vuoi vedere i movimenti");
                            iban = Console.ReadLine();
                            c = banca.RicercaConto(iban);
                            if (c != null)
                            {
                                StampaMovimenti(c);
                            }
                            else
                            {
                                Console.WriteLine("L'IBAN selezionato non fa parte della banca selezionata!");
                            }
                            break;

                        default:
                            Console.WriteLine("Non hai scelto un'azione possibile");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Non sono presenti conti nella lista della banca");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRORE!!!");
            }
        }

        /// <summary>
        /// Metodo che stampa i conti corrente
        /// </summary>
        private static void StampaContiCorrente()
        {
            foreach (ContoCorrente c in banca.Conti)
            {
                Console.WriteLine("Il conto corrente è intestatato a" + c.Intestatario.Cognome + " " + c.Intestatario.Nome + " nato il " + c.Intestatario.Data_di_nascita.ToShortDateString() + " il cui codice fiscale " + c.Intestatario.CodiceFiscale + " avente IBAN " + c.Iban + " e saldo " + c.Saldo);
            }
        }


        /// <summary>
        /// Metodo che stampa tutti i movimenti di un determinato conto ricevuto come parametro
        /// </summary>
        /// <param name="c">Conto corrente di cui si vuole stapare i movimenti</param>
        public static void StampaMovimenti(ContoCorrente c)
        {
            foreach (Movimento m in c.Movimenti)
            {
                if(m is Versamento)
                {
                    Console.WriteLine("Versamento datato il " + m.DataMovimento.ToShortDateString() + " di importo " + m.Importo);
                }
                else if(m is Prelievo)
                {
                    Console.WriteLine("Prelievo datato il " + m.DataMovimento.ToShortDateString() + " di importo " + m.Importo);
                }
                else if(m is Bonifico)
                {
                    Console.WriteLine("Bonifico datato il " + m.DataMovimento.ToShortDateString() + " di importo " + m.Importo);
                }
            }
            Console.WriteLine("| Saldo attuale " + c.Saldo);
        }

    }
}
