using System;
using System.Collections.Generic;
using System.Text;

namespace GestioneContoCorrente._4J
{
    class Intestatario
    {
        string nome;

        string cognome;

        DateTime data_di_nascita;

        string codiceFiscale;

        /// <summary>
        /// Property classe intestatario
        /// </summary>
        public string Nome { get { return nome; } set { nome = value; } }
        public string Cognome { get { return cognome; } set { cognome = value; } }
        public DateTime Data_di_nascita { get { return data_di_nascita; } set { data_di_nascita = value; } }
        public string CodiceFiscale { get { return codiceFiscale; } set { codiceFiscale = value; } }
        /// <summary>
        /// Costruttore Intestatario
        /// </summary>
        /// <param name="nome">nome intestatario</param>
        /// <param name="cognome">cognome intestatario</param>
        /// <param name="data_di_nascita">data di nascita intestatario</param>
        /// <param name="codiceFiscale">codice fiscale intestatario</param>
        public Intestatario(string nome, string cognome, DateTime data_di_nascita, string codiceFiscale)
        {
            this.nome = nome;
            this.cognome = cognome;
            this.data_di_nascita = data_di_nascita;
            this.codiceFiscale = codiceFiscale;
        }

        /// <summary>
        /// Metodo che calcola l'età
        /// </summary>
        /// <returns>Ritorna un'intero che conterrà l'età dell'intestatario</returns>
        public int Età()
        {
            if (data_di_nascita.Month == DateTime.Now.Month)
            {
                if (data_di_nascita.Day > DateTime.Now.Day)
                {
                    return DateTime.Now.Year - data_di_nascita.Year - 1;
                }
                else
                {
                    return DateTime.Now.Year - data_di_nascita.Year;
                }
            }
            else if (data_di_nascita.Month < DateTime.Now.Month)
            {
                return DateTime.Now.Year - data_di_nascita.Year;
            }
            else
            {
                return DateTime.Now.Year - data_di_nascita.Year - 1;
            }
        }
    }

    class Banca
    {
        string indirizzo;

        string nome;

        string numero_di_telefono;

        List<ContoCorrente> conti = new List<ContoCorrente>();


        /// <summary>
        /// Property Banca
        /// </summary>
        public List<ContoCorrente> Conti { get { return conti; } }
        public string Indirizzo { get { return indirizzo; } set { indirizzo = value; } }
        public string Nome { get { return nome; } set { nome = value; } }
        public string Numero_di_telefono { get { return numero_di_telefono; } set { numero_di_telefono = value; } }

        /// <summary>
        /// Costruttore Banca
        /// </summary>
        /// <param name="indirizzo">indirizzo banca</param>
        /// <param name="nome">nome banca</param>
        /// <param name="numero_di_telefono">numero di telefono della banca</param>
        public Banca(string indirizzo, string nome, string numero_di_telefono)
        {
            this.indirizzo = indirizzo;
            this.nome = nome;
            this.numero_di_telefono = numero_di_telefono;
        }

        /// <summary>
        /// Metodo che aggiunge un conto nella lista di conti
        /// </summary>
        /// <param name="c">Conto corrente da aggiungere nella lista</param>
        public void AggiungiConto(ContoCorrente c)
        {
            conti.Add(c);
        }

        /// <summary>
        /// Metodo che elimina un conto dalla lista in base all'iban
        /// </summary>
        /// <param name="iban">Iban del conto da eliminare</param>
        public void EliminaConto(string iban)
        {
            foreach (ContoCorrente c in conti)
            {
                if(c.Iban == iban)
                {
                    conti.Remove(c);
                    break;
                }    
            }
        }

        /// <summary>
        /// Metodo che ritorna un conto corrente ricercato tramite un'iban
        /// </summary>
        /// <param name="iban">iban del conto da cercare nella lista</param>
        /// <returns>Conto ricercato</returns>
        public ContoCorrente RicercaConto(string iban)
        {
            foreach (ContoCorrente c in conti)
            {
                if (c.Iban == iban)
                {
                    return c;
                }
            }

            return null;
        }

    }

    class Movimento
    {
        protected DateTime dataMovimento;
        protected double importo;

        /// <summary>
        /// Property classe movimento
        /// </summary>
        public DateTime DataMovimento { get { return dataMovimento; } set { dataMovimento = value; } }
        public double Importo { get { return importo; } set { importo = value; } }

        /// <summary>
        /// Costruttore movimento
        /// </summary>
        /// <param name="dataMovimento">data in cui è avvenuto il movimento</param>
        /// <param name="importo">importo del movimento</param>
        public Movimento(DateTime dataMovimento, double importo) 
        {
            this.dataMovimento = dataMovimento;
            this.importo = importo;
        }

    }

    class Bonifico : Movimento
    {
        string ibanDestinatario;

        double costoBonifico;

        /// <summary>
        /// Property classe bonifico
        /// </summary>
        public string IbanDestinatario { get { return ibanDestinatario; } set { ibanDestinatario = value; } }
        public double CostoBonifico { get { return costoBonifico; } set { costoBonifico = value; } }

        /// <summary>
        /// Costruttore mertodo bonifico
        /// </summary>
        /// <param name="dataMovimento">data in cui è avvenuto il bonifico</param>
        /// <param name="importo">importo del bonifico</param>
        /// <param name="ibanDestinatario">iban del conto che riceverà il bonifico</param>
        /// <param name="costoBonifico">costo di ogni bonifico</param>
        public Bonifico(DateTime dataMovimento, double importo, string ibanDestinatario, double costoBonifico) : base(dataMovimento, importo)
        {
            this.ibanDestinatario = ibanDestinatario;
            this.costoBonifico = costoBonifico;
        }

    }

    class Prelievo : Movimento
    {
        public Prelievo(DateTime dataMovimento, double saldoP) : base(dataMovimento, saldoP)
        {

        }

    }

    class Versamento : Movimento
    {

        public Versamento(DateTime dataMovimento, double saldo) : base(dataMovimento, saldo)
        {

        }

    }

    class ContoCorrente
    {
        protected Banca banca;

        protected Intestatario intestatario;

        protected int nMovimenti = 0;

        protected int maxMovimenti = 50;

        protected double saldo;

        protected string iban;

        protected double costoMovimento;

        protected double costoBonifico;

        List<Movimento> movimenti = new List<Movimento>();

        /// <summary>
        /// Property classe ContoCorrente
        /// </summary>
        public List<Movimento> Movimenti { get { return movimenti; } }
        public Banca Banca { get { return banca; } set { banca = value; } }
        public Intestatario Intestatario { get { return intestatario; } set { intestatario = value; } }
        public int NMovimenti { get { return nMovimenti; } set { nMovimenti = value; } }
        public int MaxMovimenti { get { return maxMovimenti; } set { maxMovimenti = value; } }
        public double Saldo { get { return saldo; } set { saldo = value; } }
        public string Iban { get { return iban; } set { iban = value; } }
        public double CostoMovimento { get { return costoMovimento; } set { costoMovimento = value; } }

        /// <summary>
        /// Costruttore classe ContoCorrente
        /// </summary>
        /// <param name="banca">Banca in cui si trova il conto corrente</param>
        /// <param name="intestatario">Intestatario del conto</param>
        /// <param name="saldo">saldo del conto</param>
        /// <param name="iban">Iban del conto corrente</param>
        /// <param name="costoMovimento">Costo per ogni movimento che eccede il limite massiomo di movimenti</param>
        /// <param name="costoBonifico">Costo per ogni bonifico</param>
        public ContoCorrente(Banca banca, Intestatario intestatario, double saldo, string iban, double costoMovimento, double costoBonifico)
        {
            this.intestatario = intestatario;
            this.banca = banca;
            this.saldo = saldo;
            this.iban = iban;
            this.costoMovimento = costoMovimento;
            this.costoBonifico = costoBonifico;
        }

        /// <summary>
        /// Metodo virtual che ritorna un booleano se il prelievo è possibile
        /// </summary>
        /// <param name="prelievo">importo che si vuole prelevare</param>
        /// <param name="dataPrelievo">data in cui è avvenuto un prelievo</param>
        /// <returns>Un booleano che indica se il prelievo è possibile</returns>
        public virtual bool Prelievo(double prelievo, DateTime dataPrelievo)
        {
            if (saldo > prelievo + costoMovimento)
            {
                if (maxMovimenti > nMovimenti)
                {
                    maxMovimenti--;
                    nMovimenti++;

                    movimenti.Add(new Prelievo(dataPrelievo, -prelievo));

                    saldo -= prelievo;

                    return true;
                }
                else
                {
                    nMovimenti++;

                    movimenti.Add(new Prelievo(dataPrelievo, -prelievo));

                    saldo -= prelievo - costoMovimento;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Metodo versamento che versa un importo nel saldo
        /// </summary>
        /// <param name="versamento">importo da versare</param>
        /// <param name="dataVersamento">data in cui è avvenuto il versamento</param>
        public void Versamento(double versamento, DateTime dataVersamento)
        {
            if(saldo > costoMovimento)
            {
                if (maxMovimenti > nMovimenti)
                {
                    nMovimenti++;
                    maxMovimenti--;

                    movimenti.Add(new Versamento(dataVersamento, versamento));

                    saldo += versamento;
                }
                else
                {
                    nMovimenti++;

                    movimenti.Add(new Versamento(dataVersamento, versamento));

                    saldo += versamento - costoMovimento;
                }

            }
        }

        /// <summary>
        /// Metodo che crea che un bonifico e applica la tariffa prestabilita al saldo
        /// </summary>
        /// <param name="importoBonifico">importo da mandare come bonifico</param>
        /// <param name="dataBonifico">data in cui è avvenuto il bonifico</param>
        /// <param name="ibanDestinatario">iban del destinatario</param>
        /// <returns>Il valore del bonifico se il bonifico è attuabile mentre -1 se non è possibile</returns>
        public double Bonifico(double importoBonifico, DateTime dataBonifico, string ibanDestinatario)
        {
            if (saldo > importoBonifico + costoMovimento + costoBonifico)
            {
                if (maxMovimenti > nMovimenti)
                {
                    maxMovimenti--;
                    nMovimenti++;

                    movimenti.Add(new Bonifico(dataBonifico, importoBonifico, ibanDestinatario, costoBonifico));

                    saldo -= importoBonifico - costoBonifico;

                    return importoBonifico;
                }
                else
                {
                    nMovimenti++;

                    movimenti.Add(new Bonifico(dataBonifico, importoBonifico, ibanDestinatario, costoBonifico));

                    saldo -= importoBonifico - costoMovimento - costoBonifico;

                    return importoBonifico;
                }
            }

            return -1;
        }

    }

    class ContoOnline : ContoCorrente
    {
        double maxPrelievo;
        public double MaxPrelievo { get { return maxPrelievo; } set { maxPrelievo = value; } }

        /// <summary>
        /// Costruttore ContoOnline che eredita dalla classe ContoCorrente
        /// </summary>
        /// <param name="banca">Banca in cui si trova il conto corrente</param>
        /// <param name="intestatario">Intestatario del conto</param>
        /// <param name="saldo">saldo del conto</param>
        /// <param name="iban">Iban del conto corrente</param>
        /// <param name="costoMovimento">Costo per ogni movimento che eccede il limite massiomo di movimenti</param>
        /// <param name="costoBonifico">Costo per ogni bonifico</param>
        /// <param name="maxPrelievo">valore massimo del bonifico</param>
        public ContoOnline(Banca banca, Intestatario intestatario, double saldo, string iban, double costoMovimento, double costoBonifico, double maxPrelievo) : base(banca, intestatario, saldo, iban, costoMovimento, costoBonifico)
        {
            this.maxPrelievo = maxPrelievo;
        }

        /// <summary>
        /// Metodo override del metodo virtual Prelievo della classe ContoCorrente, prima di attuare il base verifica che il prelievo sia minore del MaxPrelievo
        /// </summary>
        /// <param name="prelievo"></param>
        /// <param name="dataPrelievo"></param>
        /// <returns></returns>
        public override bool Prelievo(double prelievo, DateTime dataPrelievo)
        {
            if(prelievo < maxPrelievo)
            {
                return base.Prelievo(prelievo, dataPrelievo);
            }

            return false;
        }
    }
}
