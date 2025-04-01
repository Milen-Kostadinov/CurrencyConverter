using System.Text.RegularExpressions;

namespace CurrencyConverter
{
    public partial class CurrencyConvertorGUI : Form
    {
        public CurrencyConvertorGUI()
        {
            this.WindowState = FormWindowState.Maximized;
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filePath = ofd.FileName;
                textDisplay.Text = File.ReadAllText(filePath);
                textDisplay.SelectionStart = textDisplay.Text.Length;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string currency = "";
            double currencyToBGN = 0;
            double BGNToCurrency = 0;
            Regex regex = new Regex("");
            switch (convertFrom.SelectedIndex)
            { 
                case 0:
                    currencyToBGN = 1;
                    regex = new Regex("[0-9]+((\\.|\\,)[0-9]{1,2})?\\s(лева|лев|лв\\.|лв|BGN)");
                    break;
                case 1: 
                    currencyToBGN = 1.95;
                    regex = new Regex("[0-9]+((\\.|\\,)[0-9]{1,2})?\\s(EUR|еврото|евра|евро)");
                    break;
                case 2: 
                    currencyToBGN = 1.81;
                    regex = new Regex("[0-9]+((\\.|\\,)[0-9]{1,2})?\\s(долара|(американски|щатски)\\s(долара|долар)|USD|долар)");
                    break;
                case 3:
                    currencyToBGN = 2.34;
                    regex = new Regex("[0-9]+((\\.|\\,)[0-9]{1,2})?\\s(паунда|паунд|(британски|великобритански)\\s(паунда|паунд)|GBP)");
                    break;
            }
            switch (convertTo.SelectedIndex)
            {
                case 0: BGNToCurrency = 1; currency = "BGN"; break;
                case 1: BGNToCurrency = 1.95; currency = "EUR"; break;
                case 2: BGNToCurrency = 1.81; currency = "USD"; break;
                case 3: BGNToCurrency = 2.34; currency = "GBP"; break;
            }

            textDisplay.Text = regex.Replace(textDisplay.Text, new MatchEvaluator(new MyEvaluator(currencyToBGN, BGNToCurrency, currency).Replace));
        }
        private class MyEvaluator
        {
            double currencyToBGN;
            double BGNToCurrency;
            string currency;
            public MyEvaluator(double currencyToBGN, double BGNToCurrency, string currency) 
            {
                this.currencyToBGN = currencyToBGN;
                this.BGNToCurrency = BGNToCurrency;
                this.currency = currency;
            }
            public string Replace(Match m)
            {
                Regex regex = new Regex("\\.");
                string normalizedDouble = regex.Replace(m.ToString(), ",");
                regex = new Regex("[0-9]+((\\.|\\,)[0-9]{1,2})?");
                double value = Math.Round(double.Parse(regex.Match(normalizedDouble).ToString())*currencyToBGN / BGNToCurrency, 2);
                return value.ToString() + " " + currency;
            }
        }
        private enum Currencies
        {
            BGN, USD, GBP, EUR
        }
    }
}