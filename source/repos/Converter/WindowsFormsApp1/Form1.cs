using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Linq;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public string unit1, unit2, currentunit1 = "placeholder", currentTab = "placeholder", text = "";
        public double number = 0;
        public bool isOpen = false;
        public double[] c = new double[50];

        void Convert1()
        {
            if (double.TryParse(text1.Text, out number))
                number = Convert.ToDouble(text1.Text);
        }
        void Convert2()
        {
            switch (tabControl1.SelectedTab.Name)
            {
                case "Temperature":
                    switch (unit1)
                    {
                        case "Celsius":
                            label1.Text = "°C";
                            break;
                        case "Fahrenheit":
                            label1.Text = "°F";
                            number = FtoC(number);
                            break;
                        case "Kelvin":
                            label1.Text = "K";
                            number = KtoC(number);
                            break;
                        case "Rankine":
                            label1.Text = "°R";
                            number = RtoC(number);
                            break;
                        case "Delisle":
                            label1.Text = "°De";
                            number = DetoC(number);
                            break;
                        case "Newton":
                            label1.Text = "°N";
                            number = NtoC(number);
                            break;
                        case "Réaumur":
                            label1.Text = "°Ré";
                            number = RetoC(number);
                            break;
                        case "Rømer":
                            label1.Text = "°Rø";
                            number = RotoC(number);
                            break;
                    }
                    if (number < -273.15 && unit1 != currentunit1 && text1.Text != "" && Program.   showWarnings)
                    {
                        MessageBox.Show("Warning: That temperature doesn't exist. The lowest temperature possible is -273.15°C or 0 K and it is called absolute zero");
                        currentunit1 = unit1;
                    }
                    break;
                case "Length":
                    switch (unit1)
                    {
                        case "Metre":
                            label1.Text = "m";
                            break;
                        case "Kilometre":
                            label1.Text = "km";
                            number = KMtoM(number);
                            break;
                        case "Centimetre":
                            label1.Text = "cm";
                            number = CMtoM(number);
                            break;
                        case "Milimetre":
                            label1.Text = "mm";
                            number = MMtoM(number);
                            break;
                        case "Micrometre":
                            label1.Text = "μm";
                            number = UMtoM(number);
                            break;
                        case "Nanometre":
                            label1.Text = "nm";
                            number = NMtoM(number);
                            break;
                        case "Yard":
                            label1.Text = "yd";
                            number = YDtoM(number);
                            break;
                        case "Mile":
                            label1.Text = "mi";
                            number = MItoM(number);
                            break;
                        case "Foot":
                            label1.Text = "ft";
                            number = FTtoM(number);
                            break;
                        case "Inch":
                            label1.Text = "in";
                            number = INtoM(number);
                            break;
                        case "Thou":
                            label1.Text = "mil";
                            number = MILtoM(number);
                            break;
                        case "Nautical mile":
                            label1.Text = "M";
                            number = NMItoM(number);
                            break;
                        case "Astronomical unit":
                            label1.Text = "AU";
                            number = AUtoM(number);
                            break;
                        case "Light-year":
                            label1.Text = "ly";
                            number = LYtoM(number);
                            break;
                    }
                    if (number < 0 && text1.Text != "" && unit1 != currentunit1 && Program.showWarnings)
                    {
                        MessageBox.Show("Warning: Length cannot be negative");
                        currentunit1 = unit1;
                    }
                    break;
                case "Currency":
                    switch (unit1)
                    {
                        case "Euro":
                            label1.Text = "€";
                            break;
                        case "United States dollar":
                            label1.Text = "$";
                            number = USDtoEUR(number);
                            break;
                        case "Japanese yen":
                            label1.Text = "¥";
                            number = JPYtoEUR(number);
                            break;
                        case "Bulgarian lev":
                            label1.Text = "лв";
                            number = BGNtoEUR(number);
                            break;
                        case "Czech koruna":
                            label1.Text = "Kč";
                            number = CZKtoEUR(number);
                            break;
                        case "Danish krone":
                            label1.Text = "kr.";
                            number = DKKtoEUR(number);
                            break;
                        case "Pound sterling":
                            label1.Text = "£";
                            number = GBPtoEUR(number);
                            break;
                        case "Hungarian forint":
                            label1.Text = "Ft";
                            number = HUFtoEUR(number);
                            break;
                        case "Polish złoty":
                            label1.Text = "zł";
                            number = PLNtoEUR(number);
                            break;
                        case "Romanian leu":
                            label1.Text = "RON";
                            number = RONtoEUR(number);
                            break;
                        case "Swedish krona":
                            label1.Text = "kr";
                            number = SEKtoEUR(number);
                            break;
                        case "Swiss franc":
                            label1.Text = "CHF";
                            number = CHFtoEUR(number);
                            break;
                        case "Icelandic króna":
                            label1.Text = "Íkr";
                            number = ISKtoEUR(number);
                            break;
                        case "Norwegian krone":
                            label1.Text = "kr";
                            number = NOKtoEUR(number);
                            break;
                        case "Croatian kuna":
                            label1.Text = "kn";
                            number = HRKtoEUR(number);
                            break;
                        case "Russian ruble":
                            label1.Text = "₽";
                            number = RUBtoEUR(number);
                            break;
                        case "Turkish lira":
                            label1.Text = "₺";
                            number = TRYtoEUR(number);
                            break;
                        case "Australian dollar":
                            label1.Text = "A$";
                            number = AUDtoEUR(number);
                            break;
                        case "Brazilian real":
                            label1.Text = "R$";
                            number = BRLtoEUR(number);
                            break;
                        case "Canadian dollar":
                            label1.Text = "C$";
                            number = CADtoEUR(number);
                            break;
                        case "Chinese yuan":
                            label1.Text = "¥";
                            number = CNYtoEUR(number);
                            break;
                        case "Hong Kong dollar":
                            label1.Text = "HK$";
                            number = HKDtoEUR(number);
                            break;
                        case "Indonesian rupiah":
                            label1.Text = "Rp";
                            number = IDRtoEUR(number);
                            break;
                        case "Israeli new shekel":
                            label1.Text = "₪";
                            number = ILStoEUR(number);
                            break;
                        case "Indian rupee":
                            label1.Text = "₹";
                            number = INRtoEUR(number);
                            break;
                        case "South Korean won":
                            label1.Text = "₩";
                            number = KRWtoEUR(number);
                            break;
                        case "Mexican peso":
                            label1.Text = "Mex$";
                            number = MXNtoEUR(number);
                            break;
                        case "Malaysian ringgit":
                            label1.Text = "RM";
                            number = MYRtoEUR(number);
                            break;
                        case "New Zealand dollar":
                            label1.Text = "NZ$";
                            number = NZDtoEUR(number);
                            break;
                        case "Philippine peso":
                            label1.Text = "₱";
                            number = PHPtoEUR(number);
                            break;
                        case "Singapore dollar":
                            label1.Text = "S$";
                            number = SGDtoEUR(number);
                            break;
                        case "Thai baht":
                            label1.Text = "฿";
                            number = THBtoEUR(number);
                            break;
                        case "South African rand":
                            label1.Text = "R";
                            number = ZARtoEUR(number);
                            break;
                    }
                    if (number < 0 && text1.Text != "" && unit1 != currentunit1 && Program.showWarnings)
                    {
                        MessageBox.Show("Warning: Currency can't be negative");
                        currentunit1 = unit1;
                    }
                    break;
            }
        }
        void Convert3()
        {
            switch (tabControl1.SelectedTab.Name)
            {
                case "Temperature":
                    switch (unit2)
                    {
                        case "Celsius":
                            label3.Text = "°C";
                            if (text1.Text != "")
                                text2.Text = number.ToString();
                            break;
                        case "Fahrenheit":
                            label3.Text = "°F";
                            if (text1.Text != "")
                                text2.Text = CtoF(number).ToString();
                            break;
                        case "Kelvin":
                            label3.Text = "K";
                            if (text1.Text != "")
                                text2.Text = CtoK(number).ToString();
                            break;
                        case "Rankine":
                            label3.Text = "°R";
                            if (text1.Text != "")
                                text2.Text = CtoR(number).ToString();
                            break;
                        case "Delisle":
                            label3.Text = "°De";
                            if (text1.Text != "")
                                text2.Text = CtoDe(number).ToString();
                            break;
                        case "Newton":
                            label3.Text = "°N";
                            if (text1.Text != "")
                                text2.Text = CtoN(number).ToString();
                            break;
                        case "Réaumur":
                            label3.Text = "°Ré";
                            if (text1.Text != "")
                                text2.Text = CtoRe(number).ToString();
                            break;
                        case "Rømer":
                            label3.Text = "°Rø";
                            if (text1.Text != "")
                                text2.Text = CtoRo(number).ToString();
                            break;
                    }
                    break;
                case "Length":
                    switch (unit2)
                    {
                        case "Metre":
                            label3.Text = "m";
                            if (text1.Text != "")
                                text2.Text = number.ToString();
                            break;
                        case "Kilometre":
                            label3.Text = "km";
                            if (text1.Text != "")
                                text2.Text = MtoKM(number).ToString();
                            break;
                        case "Centimetre":
                            label3.Text = "cm";
                            if (text1.Text != "")
                                text2.Text = MtoCM(number).ToString();
                            break;
                        case "Milimetre":
                            label3.Text = "mm";
                            if (text1.Text != "")
                                text2.Text = MtoMM(number).ToString();
                            break;
                        case "Micrometre":
                            label3.Text = "μm";
                            if (text1.Text != "")
                                text2.Text = MtoUM(number).ToString();
                            break;
                        case "Nanometre":
                            label3.Text = "nm";
                            if (text1.Text != "")
                                text2.Text = MtoNM(number).ToString();
                            break;
                        case "Yard":
                            label3.Text = "yd";
                            if (text1.Text != "")
                                text2.Text = MtoYD(number).ToString();
                            break;
                        case "Mile":
                            label3.Text = "mi";
                            if (text1.Text != "")
                                text2.Text = MtoMI(number).ToString();
                            break;
                        case "Foot":
                            label3.Text = "ft";
                            if (text1.Text != "")
                                text2.Text = MtoFT(number).ToString();
                            break;
                        case "Inch":
                            label3.Text = "in";
                            if (text1.Text != "")
                                text2.Text = MtoIN(number).ToString();
                            break;
                        case "Thou":
                            label3.Text = "mil";
                            if (text1.Text != "")
                                text2.Text = MtoMIL(number).ToString();
                            break;
                        case "Nautical mile":
                            label3.Text = "M";
                            if (text1.Text != "")
                                text2.Text = MtoNMI(number).ToString();
                            break;
                        case "Astronomical unit":
                            label3.Text = "AU";
                            if (text1.Text != "")
                                text2.Text = MtoAU(number).ToString();
                            break;
                        case "Light-year":
                            label3.Text = "ly";
                            if (text1.Text != "")
                                text2.Text = MtoLY(number).ToString();
                            break;
                    }
                    break;
                case "Currency":
                    switch (unit2)
                    {
                        case "Euro":
                            label3.Text = "€";
                            if (text1.Text != "")
                                text2.Text = number.ToString();
                            break;
                        case "United States dollar":
                            label3.Text = "$";
                            if (text1.Text != "")
                                text2.Text = EURtoUSD(number).ToString();
                            break;
                        case "Japanese yen":
                            label3.Text = "¥";
                            if (text1.Text != "")
                                text2.Text = EURtoJPY(number).ToString();
                            break;
                        case "Bulgarian lev":
                            label3.Text = "лв";
                            if (text1.Text != "")
                                text2.Text = EURtoBGN(number).ToString();
                            break;
                        case "Czech koruna":
                            label3.Text = "Kč";
                            if (text1.Text != "")
                                text2.Text = EURtoCZK(number).ToString();
                            break;
                        case "Danish krone":
                            label3.Text = "kr.";
                            if (text1.Text != "")
                                text2.Text = EURtoDKK(number).ToString();
                            break;
                        case "Pound sterling":
                            label3.Text = "£";
                            if (text1.Text != "")
                                text2.Text = EURtoGBP(number).ToString();
                            break;
                        case "Hungarian forint":
                            label3.Text = "Ft";
                            if (text1.Text != "")
                                text2.Text = EURtoHUF(number).ToString();
                            break;
                        case "Polish złoty":
                            label3.Text = "zł";
                            if (text1.Text != "")
                                text2.Text = EURtoPLN(number).ToString();
                            break;
                        case "Romanian leu":
                            label3.Text = "RON";
                            if (text1.Text != "")
                                text2.Text = EURtoRON(number).ToString();
                            break;
                        case "Swedish krona":
                            label3.Text = "kr";
                            if (text1.Text != "")
                                text2.Text = EURtoSEK(number).ToString();
                            break;
                        case "Swiss franc":
                            label3.Text = "CHF";
                            if (text1.Text != "")
                                text2.Text = EURtoCHF(number).ToString();
                            break;
                        case "Icelandic króna":
                            label3.Text = "Íkr";
                            if (text1.Text != "")
                                text2.Text = EURtoISK(number).ToString();
                            break;
                        case "Norwegian krone":
                            label3.Text = "kr";
                            if (text1.Text != "")
                                text2.Text = EURtoNOK(number).ToString();
                            break;
                        case "Croatian kuna":
                            label3.Text = "kn";
                            if (text1.Text != "")
                                text2.Text = EURtoHRK(number).ToString();
                            break;
                        case "Russian ruble":
                            label3.Text = "₽";
                            if (text1.Text != "")
                                text2.Text = EURtoRUB(number).ToString();
                            break;
                        case "Turkish lira":
                            label3.Text = "₺";
                            if (text1.Text != "")
                                text2.Text = EURtoTRY(number).ToString();
                            break;
                        case "Australian dollar":
                            label3.Text = "A$";
                            if (text1.Text != "")
                                text2.Text = EURtoAUD(number).ToString();
                            break;
                        case "Brazilian real":
                            label3.Text = "R$";
                            if (text1.Text != "")
                                text2.Text = EURtoBRL(number).ToString();
                            break;
                        case "Canadian dollar":
                            label3.Text = "C$";
                            if (text1.Text != "")
                                text2.Text = EURtoCAD(number).ToString();
                            break;
                        case "Chinese yuan":
                            label3.Text = "¥";
                            if (text1.Text != "")
                                text2.Text = EURtoCNY(number).ToString();
                            break;
                        case "Hong Kong dollar":
                            label3.Text = "HK$";
                            if (text1.Text != "")
                                text2.Text = EURtoHKD(number).ToString();
                            break;
                        case "Indonesian rupiah":
                            label3.Text = "Rp";
                            if (text1.Text != "")
                                text2.Text = EURtoIDR(number).ToString();
                            break;
                        case "Israeli new shekel":
                            label3.Text = "₪";
                            if (text1.Text != "")
                                text2.Text = EURtoILS(number).ToString();
                            break;
                        case "Indian rupee":
                            label3.Text = "₹";
                            if (text1.Text != "")
                                text2.Text = EURtoINR(number).ToString();
                            break;
                        case "South Korean won":
                            label3.Text = "₩";
                            if (text1.Text != "")
                                text2.Text = EURtoKRW(number).ToString();
                            break;
                        case "Mexican peso":
                            label3.Text = "Mex$";
                            if (text1.Text != "")
                                text2.Text = EURtoMXN(number).ToString();
                            break;
                        case "Malaysian ringgit":
                            label3.Text = "RM";
                            if (text1.Text != "")
                                text2.Text = EURtoMYR(number).ToString();
                            break;
                        case "New Zealand dollar":
                            label3.Text = "NZ$";
                            if (text1.Text != "")
                                text2.Text = EURtoNZD(number).ToString();
                            break;
                        case "Philippine peso":
                            label3.Text = "₱";
                            if (text1.Text != "")
                                text2.Text = EURtoPHP(number).ToString();
                            break;
                        case "Singapore dollar":
                            label3.Text = "S$";
                            if (text1.Text != "")
                                text2.Text = EURtoSGD(number).ToString();
                            break;
                        case "Thai baht":
                            label3.Text = "฿";
                            if (text1.Text != "")
                                text2.Text = EURtoTHB(number).ToString();
                            break;
                        case "South African rand":
                            label3.Text = "R";
                            if (text1.Text != "")
                                text2.Text = EURtoZAR(number).ToString();
                            break;
                    }
                    break;

            }
        }
  
        public void SwapPositionsLeft()
        {
            text1.Location = new Point(77, 101);
            label1.Location = new Point(12, 111);
            text2.Location = new Point(538, 101);
            label3.Location = new Point(473, 111);
        }

        public void SwapPositionsRight()
        {
            text1.Location = new Point(12, 101);
            label1.Location = new Point(375, 111);
            text2.Location = new Point(473, 101);
            label3.Location = new Point(841, 111);
        }

        #region TemperatureConversion

        // Converting to Celsius degrees //
        double FtoC(double x)
        {
            return (x - 32) * 5 / 9;
        }
        double KtoC(double x)
        {
            return x - 273.15;
        }
        double RtoC(double x)
        {
            return (x - 491.67) * 5 / 9;
        }
        double DetoC(double x)
        {
            return 100 - x * 2 / 3;
        }
        double NtoC(double x)
        {
            return x * 100 / 33;
        }
        double RetoC(double x)
        {
            return x * 5 / 4;
        }
        double RotoC(double x)
        {
            return (x - 7.5) * 40 / 21;
        }
        // Converting from Celsius degrees //
        double CtoF(double x)
        {
            return x * 9 / 5 + 32;
        }
        double CtoK(double x)
        {
            return x + 273.15;
        }
        double CtoR(double x)
        {
            return (x + 273.15) * 9 / 5;
        }
        double CtoDe(double x)
        {
            return (100 - x) * 3 / 2;
        }
        double CtoN(double x)
        {
            return x * 33 / 100;
        }
        double CtoRe(double x)
        {
            return x * 4 / 5;
        }
        double CtoRo(double x)
        {
            return x * 21 / 40 + 7.5;
        }

        #endregion

        #region LengthConversion

        // Converting to Metres //
        double KMtoM(double x)
        {
            return x * 1000;
        }
        double CMtoM(double x)
        {
            return x / 100;
        }
        double MMtoM(double x)
        {
            return x / 1000;
        }
        double UMtoM(double x)
        {
            return x / 1000000;
        }
        double NMtoM(double x)
        {
            return x / 1000000000;
        }
        double YDtoM(double x)
        {
            return x * 0.9144;
        }
        double MItoM(double x)
        {
            return x * 1609.344;
        }
        double FTtoM(double x)
        {
            return x * 0.3048;
        }
        double INtoM(double x)
        {
            return x * 0.0254;
        }
        double MILtoM(double x)
        {
            return x * 0.0000254;
        }
        double NMItoM(double x)
        {
            return x * 1852;
        }
        double AUtoM(double x)
        {
            return x * 149597870700;
        }
        double LYtoM(double x)
        {
            return x * 9460730472580800;
        }
        // Converting from Metres //
        double MtoKM(double x)
        {
            return x / 1000;
        }
        double MtoCM(double x)
        {
            return x * 100;
        }
        double MtoMM(double x)
        {
            return x * 1000;
        }
        double MtoUM(double x)
        {
            return x * 1000000;
        }
        double MtoNM(double x)
        {
            return x * 1000000000;
        }
        double MtoYD(double x)
        {
            return x / 0.9144;
        }
        double MtoMI(double x)
        {
            return x / 1609.344;
        }
        double MtoFT(double x)
        {
            return x / 0.3048;
        }
        double MtoIN(double x)
        {
            return x / 0.0254;
        }
        double MtoMIL(double x)
        {
            return x / 0.0000254;
        }
        double MtoNMI(double x)
        {
            return x / 1852;
        }
        double MtoAU(double x)
        {
            return x / 149597870700;
        }
        double MtoLY(double x)
        {
            return x / 9460730472580800;
        }

        #endregion

        #region CurrencyConversion
        //Converting to Euros//
        double USDtoEUR(double x)
        {
            return x / c[1];
        }
        double JPYtoEUR(double x)
        {
            return x / c[2];
        }
        double BGNtoEUR(double x)
        {
            return x / c[3];
        }
        double CZKtoEUR(double x)
        {
            return x / c[4];
        }
        double DKKtoEUR(double x)
        {
            return x / c[5];
        }
        double GBPtoEUR(double x)
        {
            return x / c[6];
        }
        double HUFtoEUR(double x)
        {
            return x / c[7];
        }
        double PLNtoEUR(double x)
        {
            return x / c[8];
        }
        double RONtoEUR(double x)
        {
            return x / c[9];
        }
        double SEKtoEUR(double x)
        {
            return x / c[10];
        }
        double CHFtoEUR(double x)
        {
            return x / c[11];
        }
        double ISKtoEUR(double x)
        {
            return x / c[12];
        }
        double NOKtoEUR(double x)
        {
            return x / c[13];
        }
        double HRKtoEUR(double x)
        {
            return x / c[14];
        }
        double RUBtoEUR(double x)
        {
            return x / c[15];
        }
        double TRYtoEUR(double x)
        {
            return x / c[16];
        }
        double AUDtoEUR(double x)
        {
            return x / c[17];
        }
        double BRLtoEUR(double x)
        {
            return x / c[18];
        }
        double CADtoEUR(double x)
        {
            return x / c[19];
        }
        double CNYtoEUR(double x)
        {
            return x / c[20];
        }
        double HKDtoEUR(double x)
        {
            return x / c[21];
        }
        double IDRtoEUR(double x)
        {
            return x / c[22];
        }
        double ILStoEUR(double x)
        {
            return x / c[23];
        }
        double INRtoEUR(double x)
        {
            return x / c[24];
        }
        double KRWtoEUR(double x)
        {
            return x / c[25];
        }
        double MXNtoEUR(double x)
        {
            return x / c[26];
        }
        double MYRtoEUR(double x)
        {
            return x / c[27];
        }
        double NZDtoEUR(double x)
        {
            return x / c[28];
        }
        double PHPtoEUR(double x)
        {
            return x / c[29];
        }
        double SGDtoEUR(double x)
        {
            return x / c[30];
        }
        double THBtoEUR(double x)
        {
            return x / c[31];
        }
        double ZARtoEUR(double x)
        {
            return x / c[32];
        }
        //Converting from Euros//
        double EURtoUSD(double x)
        {
            return x * c[1];
        }
        double EURtoJPY(double x)
        {
            return x * c[2];
        }
        double EURtoBGN(double x)
        {
            return x * c[3];
        }
        double EURtoCZK(double x)
        {
            return x * c[4];
        }
        double EURtoDKK(double x)
        {
            return x * c[5];
        }
        double EURtoGBP(double x)
        {
            return x * c[6];
        }
        double EURtoHUF(double x)
        {
            return x * c[7];
        }
        double EURtoPLN(double x)
        {
            return x * c[8];
        }
        double EURtoRON(double x)
        {
            return x * c[9];
        }
        double EURtoSEK(double x)
        {
            return x * c[10];
        }
        double EURtoCHF(double x)
        {
            return x * c[11];
        }
        double EURtoISK(double x)
        {
            return x * c[12];
        }
        double EURtoNOK(double x)
        {
            return x * c[13];
        }
        double EURtoHRK(double x)
        {
            return x * c[14];
        }
        double EURtoRUB(double x)
        {
            return x * c[15];
        }
        double EURtoTRY(double x)
        {
            return x * c[16];
        }
        double EURtoAUD(double x)
        {
            return x * c[17];
        }
        double EURtoBRL(double x)
        {
            return x * c[18];
        }
        double EURtoCAD(double x)
        {
            return x * c[19];
        }
        double EURtoCNY(double x)
        {
            return x * c[20];
        }
        double EURtoHKD(double x)
        {
            return x * c[21];
        }
        double EURtoIDR(double x)
        {
            return x * c[22];
        }
        double EURtoILS(double x)
        {
            return x * c[23];
        }
        double EURtoINR(double x)
        {
            return x * c[24];
        }
        double EURtoKRW(double x)
        {
            return x * c[25];
        }
        double EURtoMXN(double x)
        {
            return x * c[26];
        }
        double EURtoMYR(double x)
        {
            return x * c[27];
        }
        double EURtoNZD(double x)
        {
            return x * c[28];
        }
        double EURtoPHP(double x)
        {
            return x * c[29];
        }
        double EURtoSGD(double x)
        {
            return x * c[30];
        }
        double EURtoTHB(double x)
        {
            return x * c[31];
        }
        double EURtoZAR(double x)
        {
            return x * c[32];
        }
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.comboBox1, "Pick a unit");
            System.Windows.Forms.ToolTip ToolTip2 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.comboBox2, "Pick a unit");
            System.Windows.Forms.ToolTip ToolTip3 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.pictureBox1, "Reverse the units");
            ParseExchangeRate();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            label1.Text = "";
            label3.Text = "";
            if (Program.resetValues)
            {
                bool aux = Program.showWarnings;
                Program.showWarnings = false;
                text1.Text = "";
                Program.showWarnings = aux;
            }
            text2.Text = "";
            if (tabControl1.SelectedTab.Name == "Currency" && Program.currencySymbol == "Left")
            {
                SwapPositionsLeft();
            }
            if (currentTab == "Currency" && Program.currencySymbol == "Left")
            {
                SwapPositionsRight();
            }
            switch (tabControl1.SelectedTab.Name)
            {
                case "Temperature":
                    currentTab = "Temperature";
                    comboBox1.Items.Add("Celsius");
                    comboBox1.Items.Add("Fahrenheit");
                    comboBox1.Items.Add("Kelvin");
                    comboBox1.Items.Add("Rankine");
                    comboBox1.Items.Add("Delisle");
                    comboBox1.Items.Add("Newton");
                    comboBox1.Items.Add("Réaumur");
                    comboBox1.Items.Add("Rømer");
                    comboBox2.Items.Add("Celsius");
                    comboBox2.Items.Add("Fahrenheit");
                    comboBox2.Items.Add("Kelvin");
                    comboBox2.Items.Add("Rankine");
                    comboBox2.Items.Add("Delisle");
                    comboBox2.Items.Add("Newton");
                    comboBox2.Items.Add("Réaumur");
                    comboBox2.Items.Add("Rømer");
                    break;
                case "Length":
                    currentTab = "Length";
                    comboBox1.Items.Add("Metre");
                    comboBox1.Items.Add("Kilometre");
                    comboBox1.Items.Add("Centimetre");
                    comboBox1.Items.Add("Milimetre");
                    comboBox1.Items.Add("Micrometre");
                    comboBox1.Items.Add("Nanometre");
                    comboBox1.Items.Add("Yard");
                    comboBox1.Items.Add("Mile");
                    comboBox1.Items.Add("Foot");
                    comboBox1.Items.Add("Inch");
                    comboBox1.Items.Add("Thou");
                    comboBox1.Items.Add("Nautical mile");
                    comboBox1.Items.Add("Astronomical unit");
                    comboBox1.Items.Add("Light-year");
                    comboBox2.Items.Add("Metre");
                    comboBox2.Items.Add("Kilometre");
                    comboBox2.Items.Add("Centimetre");
                    comboBox2.Items.Add("Milimetre");
                    comboBox2.Items.Add("Micrometre");
                    comboBox2.Items.Add("Nanometre");
                    comboBox2.Items.Add("Yard");
                    comboBox2.Items.Add("Mile");
                    comboBox2.Items.Add("Foot");
                    comboBox2.Items.Add("Inch");
                    comboBox2.Items.Add("Thou");
                    comboBox2.Items.Add("Nautical mile");
                    comboBox2.Items.Add("Astronomical unit");
                    comboBox2.Items.Add("Light-year");
                    break;
                case "Currency":
                    currentTab = "Currency";
                    comboBox1.Items.Add("Euro");
                    comboBox1.Items.Add("United States dollar");
                    comboBox1.Items.Add("Japanese yen");
                    comboBox1.Items.Add("Bulgarian lev");
                    comboBox1.Items.Add("Czech koruna");
                    comboBox1.Items.Add("Danish krone");
                    comboBox1.Items.Add("Pound sterling");
                    comboBox1.Items.Add("Hungarian forint");
                    comboBox1.Items.Add("Polish złoty");
                    comboBox1.Items.Add("Romanian leu");
                    comboBox1.Items.Add("Swedish krona");
                    comboBox1.Items.Add("Swiss franc");
                    comboBox1.Items.Add("Icelandic króna");
                    comboBox1.Items.Add("Norwegian krone");
                    comboBox1.Items.Add("Croatian kuna");
                    comboBox1.Items.Add("Russian ruble");
                    comboBox1.Items.Add("Turkish lira");
                    comboBox1.Items.Add("Australian dollar");
                    comboBox1.Items.Add("Brazilian real");
                    comboBox1.Items.Add("Canadian dollar");
                    comboBox1.Items.Add("Chinese yuan");
                    comboBox1.Items.Add("Hong Kong dollar");
                    comboBox1.Items.Add("Indonesian rupiah");
                    comboBox1.Items.Add("Israeli new shekel");
                    comboBox1.Items.Add("Indian rupee");
                    comboBox1.Items.Add("South Korean won");
                    comboBox1.Items.Add("Mexican peso");
                    comboBox1.Items.Add("Malaysian ringgit");
                    comboBox1.Items.Add("New Zealand dollar");
                    comboBox1.Items.Add("Philippine peso");
                    comboBox1.Items.Add("Singapore dollar");
                    comboBox1.Items.Add("Thai baht");
                    comboBox1.Items.Add("South African rand");
                    comboBox2.Items.Add("Euro");
                    comboBox2.Items.Add("United States dollar");
                    comboBox2.Items.Add("Japanese yen");
                    comboBox2.Items.Add("Bulgarian lev");
                    comboBox2.Items.Add("Czech koruna");
                    comboBox2.Items.Add("Danish krone");
                    comboBox2.Items.Add("Pound sterling");
                    comboBox2.Items.Add("Hungarian forint");
                    comboBox2.Items.Add("Polish złoty");
                    comboBox2.Items.Add("Romanian leu");
                    comboBox2.Items.Add("Swedish krona");
                    comboBox2.Items.Add("Swiss franc");
                    comboBox2.Items.Add("Icelandic króna");
                    comboBox2.Items.Add("Norwegian krone");
                    comboBox2.Items.Add("Croatian kuna");
                    comboBox2.Items.Add("Russian ruble");
                    comboBox2.Items.Add("Turkish lira");
                    comboBox2.Items.Add("Australian dollar");
                    comboBox2.Items.Add("Brazilian real");
                    comboBox2.Items.Add("Canadian dollar");
                    comboBox2.Items.Add("Chinese yuan");
                    comboBox2.Items.Add("Hong Kong dollar");
                    comboBox2.Items.Add("Indonesian rupiah");
                    comboBox2.Items.Add("Israeli new shekel");
                    comboBox2.Items.Add("Indian rupee");
                    comboBox2.Items.Add("South Korean won");
                    comboBox2.Items.Add("Mexican peso");
                    comboBox2.Items.Add("Malaysian ringgit");
                    comboBox2.Items.Add("New Zealand dollar");
                    comboBox2.Items.Add("Philippine peso");
                    comboBox2.Items.Add("Singapore dollar");
                    comboBox2.Items.Add("Thai baht");
                    comboBox2.Items.Add("South African rand");
                    break;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!double.TryParse(text1.Text, out double a) && !(text1.Text.Length == 1 && text1.Text == "-") && !(text1.Text.Length == 1 && text1.Text == ".") && !(text1.Text.Length == 2 && text1.Text == "-.") && !(text1.Text == ""))
            {
                text1.Text = text;
                text1.SelectionStart = text1.Text.Length;
                if (isOpen == false)
                {
                    if(Program.showWarnings)
                        MessageBox.Show("Please enter a valid number");
                    isOpen = true;
                }
            }
            else
            {
                Convert1();
                if (text1.Text != "" && unit1 != "" && unit2 != "")
                {
                    Convert1();
                    Convert2();
                    Convert3();
                }
                isOpen = false;
                text = text1.Text;
            }

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void Temperature_Click(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            unit1 = comboBox1.SelectedItem.ToString();
            Convert2();
            if (text1.Text != "" && unit1 != "" && unit2 != "")
            {
                Convert1();
                Convert2();
                Convert3();
            }
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            Settings f = Application.OpenForms["Settings"] as Settings;
            if (f != null)
                f.BringToFront();
            else
            {
                f = new Settings(this);
                f.Show();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "" && comboBox2.Text != "")
            {
                comboBox1.Text = comboBox2.Text;
                comboBox2.SelectedIndex = 0;
            }
            else if (comboBox2.Text == "" && comboBox1.Text != "")
            {
                comboBox2.Text = comboBox1.Text;
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                string aux;
                aux = comboBox2.Text;
                comboBox2.Text = comboBox1.Text;
                comboBox1.Text = aux;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            unit2 = comboBox2.SelectedItem.ToString();
            Convert3();
            if (text1.Text != "" && unit1 != "" && unit2 != "")
            {
                Convert1();
                Convert2();
                Convert3();
            }
        }

        private void ParseExchangeRate()
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            nsmgr.AddNamespace("gesmes", "http://www.gesmes.org/xml/2002-08-01");
            nsmgr.AddNamespace("lo", "http://www.ecb.int/vocabulary/2002-08-01/eurofxref");
            XmlNodeList nodes = xdoc.SelectNodes("//lo:Cube[@currency]", nsmgr);
            int i = 1;
            foreach (XmlNode node in nodes)
            {
                c[i] = Convert.ToDouble(node.Attributes["rate"].Value);
                i++;
            }
        }
    }
}