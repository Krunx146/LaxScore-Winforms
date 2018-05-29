using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lax_score
{

    public partial class Form1 : Form
    {
        public static DateTime Datum;
        public List<Igrac> ListaIgraca1 = new List<Igrac>();
        public List<Igrac> ListaIgraca2 = new List<Igrac>();
        public List<Iskljucenja> ListaIskljucenja = new List<Iskljucenja>();
        public Igrac SelectedPlayer = null;
        public string ImeTima1;
        public string ImeTima2;
        public int Rezultat1;
        public int Rezultat2;
        public string ImeZapisnicara;
        public bool isRunning = false;
        public bool GameStarted = false;
        public int gametime;
        public int pausetime;

        #region Inicijalizacija
        //INICIJALIZACIJA
        public Form1()
        {
            InitializeComponent();
            toolTip1.SetToolTip(btnDohvatiDatum, "Dohvati danasnji datum.");
            toolTip1.SetToolTip(btnLoad1, "Ucitaj postojecu listu igraca.");
            toolTip1.SetToolTip(btnLoad2, "Ucitaj postojecu listu igraca.");
            toolTip1.SetToolTip(btnSave1, "Spremi trenutnu listu igraca.");
            toolTip1.SetToolTip(btnSave2, "Spremi trenutnu listu igraca.");
            toolTip1.SetToolTip(btnBrisiIgraca1, "Izbrisi odabranog igraca.");
            toolTip1.SetToolTip(btnBrisiIgraca2, "Izbrisi odabranog igraca.");
            toolTip1.SetToolTip(btnDodajIgraca1, "Dodaj novog igraca.");
            toolTip1.SetToolTip(btnDodajIgraca2, "Dodaj novog igraca.");

            toolTip1.SetToolTip(btnGK, "Golman obranio udarac.");
            toolTip1.SetToolTip(btnS, "Igrac pucao na gol (izvan okvira).");
            toolTip1.SetToolTip(btnSoG, "Igrac pucao u okvir gola.");
            toolTip1.SetToolTip(btnG, "Igrac zabio gol.");
            toolTip1.SetToolTip(btnAS, "Igrac asistirao prilikom gola.");
            toolTip1.SetToolTip(btnGB, "Igrac pokupio groundball.");
            toolTip1.SetToolTip(btnERR, "Igrac napravio gresku.");
            toolTip1.SetToolTip(btnIN, "Igrac presjekao loptu.");
            toolTip1.SetToolTip(btnT, "Igrac napravio turnover.");
            toolTip1.SetToolTip(btnFaceoff, "Igrac izgubio faceoff.");
            toolTip1.SetToolTip(btnFaceoffWin, "Igrac izborio faceoff.");
            toolTip1.SetToolTip(btnPenalty, "Igrac iskljucen na odredjeno vrijeme.");
            toolTip1.SetToolTip(txtPenaltyTime, "Vrijeme u sekundama za iskljucenje.");

            btnBrisiIgraca1.Enabled = false;
            btnBrisiIgraca2.Enabled = false;
            btnEndGame.Enabled = false;
            chkGameTime40.Checked = true;
            //btnEndGame.Enabled = false;
            Rezultat1 = 0;
            Rezultat2 = 0;
        }
        #endregion

        private void btnDohvatiDatum_Click(object sender, EventArgs e)
        {
            Datum = DateTime.Today;
            txtDatum.Text = Datum.ToString("d");
        }

        #region Dodavanje igraca
        //DODAVANJE IGRACA
        private void btnDodajIgraca1_Click(object sender, EventArgs e)
        {
            try
            {
                Igrac temp = new Igrac(int.Parse(txtIgracBroj1.Text), txtIgracIme1.Text, txtIgracPrezime1.Text);
                ListaIgraca1.Add(temp);
                DataBind1();
            }
            catch { }
        }
        private void btnDodajIgraca2_Click(object sender, EventArgs e)
        {
            try
            {
                Igrac temp = new Igrac(int.Parse(txtIgracBroj2.Text), txtIgracIme2.Text, txtIgracPrezime2.Text);
                ListaIgraca2.Add(temp);
                DataBind2();
            }
            catch { }
        }
        #endregion

        #region Databind
        //DATABIND
        private void SetBackgroundColor(ListBox container, string name)
        {
            switch (name)
            {
                case "Zagreb Bulldogs":
                    container.BackColor = Color.FromArgb(205, 205, 255);
                    break;
                case "Zagreb Patriots":
                    container.BackColor = Color.FromArgb(205, 205, 205);
                    break;
                case "Varazdin Royals":
                    container.BackColor = Color.FromArgb(255, 255, 150);
                    break;
                case "Split Legion":
                    container.BackColor = Color.FromArgb(255, 180, 180);
                    break;
                default:
                    container.BackColor = Color.White;
                    break;
            }
        }

        private void DataBind1()
        {
            listIgraci1.DataSource = null;
            ListaIgraca1.Sort((x, y) => x.Broj.CompareTo(y.Broj));
            listIgraci1.DataSource = ListaIgraca1;
            SetBackgroundColor(listIgraci1, txtTim1Ime.Text);
        }
        private void DataBind2()
        {
            listIgraci2.DataSource = null;
            ListaIgraca2.Sort((x, y) => x.Broj.CompareTo(y.Broj));
            listIgraci2.DataSource = ListaIgraca2;
            SetBackgroundColor(listIgraci2, txtTim2Ime.Text);
        }
        private void DataBind3()
        {
            listIskljucenja.DataSource = null;
            ListaIskljucenja.Sort((x, y) => x.Trajanje.CompareTo(y.Trajanje));
            listIskljucenja.DataSource = ListaIskljucenja;
        }
        #endregion

        #region Kraj igre
        //UCITAVANJE POSTOJECE STATISTIKE
        private List<Igrac> LoadExisting(List<Igrac> lista, string filepath)
        {
            List<Igrac> temp = null;
            //ucitavanje liste postojecih igraca
            if (System.IO.File.Exists(filepath))
            {
                temp = new List<Igrac>();
                string[] lines = System.IO.File.ReadAllLines(filepath);
                for (int i = 2; i < lines.Count(); i++)
                {
                    string[] words = lines[i].Split(',');
                    Igrac novi = new Igrac(words[0], words[1], words[2], words[3], words[4],
                        words[5], words[6], words[7], words[8], words[9], words[10], words[11], words[12], words[13], words[14]);
                    temp.Add(novi);
                }
            }
            else return lista;

            foreach(Igrac i in temp)
            {
                foreach(Igrac j in lista)
                {
                    if(i.Broj == j.Broj)
                    {
                        i.JoinPlayer(j);
                        break;
                    }
                }
            }
            return temp;
        }

        //END FUNCTION
        private void endGame()
        {
            //Kreiraj folder za utakmice
            string filenameUtakmice = AppDomain.CurrentDomain.BaseDirectory + @"\Utakmice";
            if (!System.IO.Directory.Exists(filenameUtakmice)) System.IO.Directory.CreateDirectory(filenameUtakmice);

            string filenameLog = txtDatum.Text.ToString() + " " + ImeTima1 + " VS " + ImeTima2 + ".log";
            try
            {
                //Kreiraj log datoteku
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.IO.Path.Combine(filenameUtakmice, filenameLog)))
                {
                    file.WriteLine("Zapisnicar: " + ImeZapisnicara);
                    file.WriteLine("Datum: " + txtDatum.Text);
                    file.WriteLine(ImeTima1 + " protiv " + ImeTima2);
                    file.WriteLine("Konacni rezultat: " + Rezultat1 + " : " + Rezultat2);
                    file.WriteLine("-------------------------");
                    foreach (string s in listTimeline.Items)
                    {
                        file.WriteLine(s);
                    }
                    file.Close();
                }
                //Kreiraj csv za oba tima
                string filenameTim = txtDatum.Text.ToString() + " " + ImeTima1 + " VS " + ImeTima2 + ".csv";
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.IO.Path.Combine(filenameUtakmice, filenameTim)))
                {
                    file.WriteLine("sep=,");
                    file.WriteLine("Datum: " + "," + txtDatum.Text);
                    file.WriteLine("Tim: " + "," + ImeTima1);
                    file.WriteLine("Broj,Ime,Prezime,SAV,SoG,S,G,A,GB,E,IN,T,PEN,FOW,FOT");
                    foreach (Igrac i in ListaIgraca1) file.WriteLine(i.GetPlayerString());
                    file.WriteLine("Tim: " + "," + ImeTima2);
                    file.WriteLine("Broj,Ime,Prezime,SAV,SoG,S,G,A,GB,E,IN,T,PEN,FOW,FOT");
                    foreach (Igrac i in ListaIgraca2) file.WriteLine(i.GetPlayerString());
                    file.Close();
                }

                //MAJOR UPDATE
                string filenameIzvjestaji = AppDomain.CurrentDomain.BaseDirectory + @"\Izvjestaji";
                if (!System.IO.Directory.Exists(filenameIzvjestaji)) System.IO.Directory.CreateDirectory(filenameIzvjestaji);

                //Pisanje u izvjesta za timove
                string filenameTim1 = ImeTima1 + ".csv";
                string filepathTim1 = System.IO.Path.Combine(filenameIzvjestaji, filenameTim1);
                List<Igrac> listaTim1 = LoadExisting(ListaIgraca1,filepathTim1);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepathTim1))
                {
                    file.WriteLine("sep=,");
                    file.WriteLine("Broj,Ime,Prezime,SAV,SoG,S,G,A,GB,E,IN,T,PEN,FOW,FOT");
                    foreach (Igrac i in listaTim1) file.WriteLine(i.GetPlayerString());
                    file.Close();
                }

                string filenameTim2 = ImeTima2 + ".csv";
                string filepathTim2 = System.IO.Path.Combine(filenameIzvjestaji, filenameTim2);
                List<Igrac> listaTim2 = LoadExisting(ListaIgraca2, filepathTim2);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepathTim2))
                {
                    file.WriteLine("sep=,");
                    file.WriteLine("Broj,Ime,Prezime,SAV,SoG,S,G,A,GB,E,IN,T,PEN,FOW,FOT");
                    foreach (Igrac i in listaTim2) file.WriteLine(i.GetPlayerString());
                    file.Close();
                }

                MessageBox.Show("Kreirane datoteke: " + filenameLog + ", " + filenameTim + ", " + filenameTim1 + " i " + filenameTim2 + ".");
            }
            catch
            {
                MessageBox.Show("Datoteka je otvorena u drugom programu pa nije moguce pisati u nju!");
            }
        }
        #endregion

        private void btnBrisiIgraca1_Click(object sender, EventArgs e)
        {
            ListaIgraca1.RemoveAt(listIgraci1.SelectedIndex);
            DataBind1();
        }
        private void btnBrisiIgraca2_Click(object sender, EventArgs e)
        {
            ListaIgraca2.RemoveAt(listIgraci2.SelectedIndex);
            DataBind2();
        }

        private void listIgraci1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listIgraci1.SelectedIndex == -1) btnBrisiIgraca1.Enabled = false;
            else
            {
                SelectedPlayer = ListaIgraca1[listIgraci1.SelectedIndex];
                btnBrisiIgraca1.Enabled = true;
                btnBrisiIgraca2.Enabled = false;
                listIgraci2.SelectedIndex = -1;
            }
        }

        private void listIgraci2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listIgraci2.SelectedIndex == -1) btnBrisiIgraca2.Enabled = false;
            else
            {
                SelectedPlayer = ListaIgraca2[listIgraci2.SelectedIndex];
                btnBrisiIgraca2.Enabled = true;
                btnBrisiIgraca1.Enabled = false;
                listIgraci1.SelectedIndex = -1;
            }
        }
        //UCITAVANJE IMENA TIMA
        private void SetTeamName(string filename, TextBox textbox)
        {
            string[] splitter = filename.Split('\\');
            string[] splitter2 = splitter.LastOrDefault().Split('.');
            textbox.Text = splitter2[0];
        }
        //UCITAVANJE IGRACA PREKO LISTE
        private void btnLoad1_Click(object sender, EventArgs e)
        {
            if (ListaIgraca1.Any())
            {
                DialogResult dialogResult = MessageBox.Show("Ucitavanje novog popisa ce izbrisati trenutni popis igraca. Nastaviti?", "Upozorenje", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No) return;
            }
            Stream myStream = null;
            OpenFileDialog openLoadDialog = new OpenFileDialog();
            openLoadDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openLoadDialog.Filter = "Team files (*.team)|*.team|All files (*.*)|*.*";
            openLoadDialog.FilterIndex = 1;
            openLoadDialog.RestoreDirectory = true;
            if(openLoadDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SetTeamName(openLoadDialog.FileName, txtTim1Ime);
                    if ((myStream = openLoadDialog.OpenFile()) != null)
                    {
                        ListaIgraca1.Clear();
                        txtIgraci1Dat.Text = openLoadDialog.FileName;
                        string[] lines = System.IO.File.ReadAllLines(openLoadDialog.FileName);
                        string[] words = new string[3];
                        foreach(string l in lines)
                        {
                            words = l.Split(';');
                            Igrac temp = new Igrac(int.Parse(words[0]),words[1],words[2]);
                            ListaIgraca1.Add(temp);
                        }
                        DataBind1();
                    }
                    myStream.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            if (!ListaIgraca1.Any())
            {
                MessageBox.Show("Nema igraca koje je moguce spremiti!");
                return;
            }
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Team file|*.team|Text document|*.txt";
            saveFile.Title = "Save a Team Template File";
            saveFile.ShowDialog();
            if(saveFile.FileName != "")
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(saveFile.FileName))
                {
                    foreach (Igrac i in ListaIgraca1)
                    {
                        string zapis = i.Broj + ";" + i.Ime + ";" + i.Prezime + ";";
                        file.WriteLine(zapis);
                    }
                }
            }
        }

        private void btnLoad2_Click(object sender, EventArgs e)
        {
            if (ListaIgraca2.Any())
            {
                DialogResult dialogResult = MessageBox.Show("Ucitavanje novog popisa ce izbrisati trenutni popis igraca. Nastaviti?", "Upozorenje", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No) return;
            }
            Stream myStream = null;
            OpenFileDialog openLoadDialog = new OpenFileDialog();
            openLoadDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openLoadDialog.Filter = "Team files (*.team)|*.team|All files (*.*)|*.*";
            openLoadDialog.FilterIndex = 1;
            openLoadDialog.RestoreDirectory = true;
            if (openLoadDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SetTeamName(openLoadDialog.FileName, txtTim2Ime);
                    if ((myStream = openLoadDialog.OpenFile()) != null)
                    {
                        ListaIgraca2.Clear();
                        txtIgraci2Dat.Text = openLoadDialog.FileName;
                        string[] lines = System.IO.File.ReadAllLines(openLoadDialog.FileName);
                        string[] words = new string[3];
                        foreach (string l in lines)
                        {
                            words = l.Split(';');
                            Igrac temp = new Igrac(int.Parse(words[0]), words[1], words[2]);
                            ListaIgraca2.Add(temp);
                        }
                        DataBind2();
                    }
                    myStream.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            if (!ListaIgraca2.Any())
            {
                MessageBox.Show("Nema igraca koje je moguce spremiti!");
                return;
            }
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Team file|*.team|Text document|*.txt";
            saveFile.Title = "Save a Team Template File";
            saveFile.ShowDialog();
            if (saveFile.FileName != "")
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(saveFile.FileName))
                {
                    foreach (Igrac i in ListaIgraca2)
                    {
                        string zapis = i.Broj + ";" + i.Ime + ";" + i.Prezime + ";";
                        file.WriteLine(zapis);
                    }
                }
            }
        }

        private void chkGameTime40_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGameTime40.Checked) chkGameTime60.Checked = false;
            else chkGameTime60.Checked = true;
        }

        private void chkGameTime60_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGameTime60.Checked) chkGameTime40.Checked = false;
            else chkGameTime40.Checked = true;
        }
        //MAIN BUTTON (START)
        private void btnStartPauseGame_Click(object sender, EventArgs e)
        {
            if(GameStarted == false)
            {
                try
                {                
                    ImeZapisnicara = txtZapisnicar.Text;
                    Datum = DateTime.Parse(txtDatum.Text);
                    ImeTima1 = txtTim1Ime.Text;
                    ImeTima2 = txtTim2Ime.Text;
                    GameStarted = true;
                    isRunning = true;
                    timerMain.Enabled = true;
                    timerPause.Enabled = false;
                    btnEndGame.Enabled = true;
                    btnStartPauseGame.Text = "Pause";
                    gametime = 0;
                    pausetime = 0;
                }
                catch
                {
                    MessageBox.Show("Greska prilikom unosa podataka u polja!");
                }
            }
            else
            {
                if (isRunning)
                {
                    btnStartPauseGame.Text = "Resume";
                    timerMain.Enabled = false;
                    timerPause.Enabled = true;
                    isRunning = false;
                    lbSecondTimer.Text = "00:00";
                    pausetime = 0;
                }
                else
                {
                    btnStartPauseGame.Text = "Pause";
                    timerMain.Enabled = true;
                    timerPause.Enabled = false;
                    isRunning = true;
                    lbSecondTimer.Text = "00:00";
                    pausetime = 0;
                }
            }
        }
        //TIMER TICK
        private void timerMain_Tick(object sender, EventArgs e)
        {
            gametime++;
            int gtMinutes = gametime / 60;
            int gtSeconds = gametime % 60;
            if(gtMinutes < 10 && gtSeconds < 10) lbTimer.Text = "0" + gtMinutes + ":" + "0" + gtSeconds;
            else if(gtMinutes < 10 && gtSeconds >= 10) lbTimer.Text = "0" + gtMinutes + ":" + gtSeconds;
            else if (gtMinutes >= 10 && gtSeconds < 10) lbTimer.Text = gtMinutes + ":" + "0" + gtSeconds;
            else lbTimer.Text = gtMinutes + ":" + gtSeconds;

            if (ListaIskljucenja.Any())
            {
                foreach(Iskljucenja i in ListaIskljucenja)
                {
                    i.Trajanje--;
                    DataBind3();
                    if (i.Trajanje <= 0)
                    {
                        ListaIskljucenja.Remove(i);
                        DataBind3();
                        break;
                    }
                }
            }

            if (chkGameTime40.Checked)
            {
                if (gametime == 1200 || gametime == 2400)
                {
                    btnStartPauseGame.Text = "Resume";
                    isRunning = false;
                    timerMain.Enabled = false;
                    timerPause.Enabled = true;
                    lbSecondTimer.Text = "00:00";
                    pausetime = 0;
                    if(gametime == 1200)
                    {
                        listTimeline.Items.Add("20' Zavrsetak prvog poluvremena!");
                    }
                    else if (gametime == 2400)
                    {
                        btnStartPauseGame.Enabled = false;
                        if (Rezultat1 > Rezultat2) listTimeline.Items.Add("40' Zavrsetak utakmice! " + ImeTima1 +
                             "je pobjedio " + ImeTima2 + " rezultatom " + Rezultat1 + ":" + Rezultat2 + "!");
                        else if (Rezultat1 < Rezultat2) listTimeline.Items.Add("40' Zavrsetak utakmice! " + ImeTima2 +
                             "je pobjedio " + ImeTima1 + " rezultatom " + Rezultat2 + ":" + Rezultat1 + "!");
                        else listTimeline.Items.Add("40' Zavrsetak utakmice! Konacni rezultat: nerjeseno.");
                        endGame();
                    }
                }
            }
            else
            {
                if (gametime == 900 || gametime == 1800 || gametime == 2700 || gametime == 3600)
                {
                    btnStartPauseGame.Text = "Resume";
                    isRunning = false;
                    timerMain.Enabled = false;
                    timerPause.Enabled = true;
                    lbSecondTimer.Text = "00:00";
                    pausetime = 0;
                    if (gametime == 900) listTimeline.Items.Add("15' Zavrsetak prve cetvrtine!");
                    else if (gametime == 1800) listTimeline.Items.Add("30' Zavrsetak druge cetvrtine!");
                    else if (gametime == 2700) listTimeline.Items.Add("45' Zavrsetak trece cetvrtine!");
                    else if (gametime == 3600)
                    {
                        btnStartPauseGame.Enabled = false;
                        if (Rezultat1 > Rezultat2) listTimeline.Items.Add("40' Zavrsetak utakmice! " + ImeTima1 +
                             "je pobjedio " + ImeTima2 + " rezultatom " + Rezultat1 + ":" + Rezultat2 + "!");
                        else if (Rezultat1 < Rezultat2) listTimeline.Items.Add("40' Zavrsetak utakmice! " + ImeTima2 +
                             "je pobjedio " + ImeTima1 + " rezultatom " + Rezultat2 + ":" + Rezultat1 + "!");
                        else listTimeline.Items.Add("40' Zavrsetak utakmice! Konacni rezultat: nerjeseno.");
                        endGame();
                    }
                }
            }
        }
        //PAUSE TIMER TICK
        private void timerPause_Tick(object sender, EventArgs e)
        {
            pausetime++;
            int gtMinutes = pausetime / 60;
            int gtSeconds = pausetime % 60;
            if (gtMinutes < 10 && gtSeconds < 10) lbSecondTimer.Text = "0" + gtMinutes + ":" + "0" + gtSeconds;
            else if (gtMinutes < 10 && gtSeconds >= 10) lbSecondTimer.Text = "0" + gtMinutes + ":" + gtSeconds;
            else if (gtMinutes >= 10 && gtSeconds < 10) lbSecondTimer.Text = gtMinutes + ":" + "0" + gtSeconds;
            else lbSecondTimer.Text = gtMinutes + ":" + gtSeconds;
        }
        //ZAVRSI BUTTON
        private void btnEndGame_Click(object sender, EventArgs e)
        {
            endGame();
        }

        #region Dodavanje akcija
        //DODAVANJE AKCIJA
        private void btnGK_Click(object sender, EventArgs e)
        {
            if (SelectedPlayer != null)
            {
                SelectedPlayer.Goalie = true;
                SelectedPlayer.AddSave();
                int gtMinutes = gametime / 60 + 1;
                listTimeline.Items.Add(gtMinutes + "' (" + SelectedPlayer.Broj + ") " +
                    SelectedPlayer.Ime + " " + SelectedPlayer.Prezime + " je obranio udarac!");
            }
            listTimeline.TopIndex = listTimeline.Items.Count - 1;
            DataBind1();
            DataBind2();
        }
        private void btnG_Click(object sender, EventArgs e)
        {
            if(SelectedPlayer != null)
            {
                SelectedPlayer.AddGoal();
                int gtMinutes = gametime / 60 + 1;
                listTimeline.Items.Add(gtMinutes + "' (" + SelectedPlayer.Broj + ") " + 
                    SelectedPlayer.Ime + " " + SelectedPlayer.Prezime + " je zabio gol!");
                if (listIgraci1.SelectedIndex == -1) { Rezultat2++; lbTeam2Score.Text = Rezultat2.ToString(); }
                else { Rezultat1++; lbTeam1Score.Text = Rezultat1.ToString(); }
            }
            listTimeline.TopIndex = listTimeline.Items.Count - 1;
        }

        private void btnS_Click(object sender, EventArgs e)
        {
            if (SelectedPlayer != null)
            {
                SelectedPlayer.AddShot(false);
                int gtMinutes = gametime / 60 + 1;
                listTimeline.Items.Add(gtMinutes + "' (" + SelectedPlayer.Broj + ") " +
                    SelectedPlayer.Ime + " " + SelectedPlayer.Prezime + " je pucao na gol izvan okvira!");
            }
            listTimeline.TopIndex = listTimeline.Items.Count - 1;
        }

        private void btnSoG_Click(object sender, EventArgs e)
        {
            if (SelectedPlayer != null)
            {
                SelectedPlayer.AddShot(true);
                int gtMinutes = gametime / 60 + 1;
                listTimeline.Items.Add(gtMinutes + "' (" + SelectedPlayer.Broj + ") " +
                    SelectedPlayer.Ime + " " + SelectedPlayer.Prezime + " je pucao u okvir gola!");
            }
            listTimeline.TopIndex = listTimeline.Items.Count - 1;
        }

        private void btnAS_Click(object sender, EventArgs e)
        {
            if (SelectedPlayer != null)
            {
                SelectedPlayer.AddAssist();
                int gtMinutes = gametime / 60 + 1;
                listTimeline.Items.Add(gtMinutes + "' (" + SelectedPlayer.Broj + ") " +
                    SelectedPlayer.Ime + " " + SelectedPlayer.Prezime + " je asistirao za gol!");
            }
            listTimeline.TopIndex = listTimeline.Items.Count - 1;
        }

        private void btnGB_Click(object sender, EventArgs e)
        {
            if (SelectedPlayer != null)
            {
                SelectedPlayer.AddGroundball();
                int gtMinutes = gametime / 60 + 1;
                listTimeline.Items.Add(gtMinutes + "' (" + SelectedPlayer.Broj + ") " +
                    SelectedPlayer.Ime + " " + SelectedPlayer.Prezime + " je pokupio groundball!");
            }
            listTimeline.TopIndex = listTimeline.Items.Count - 1;
        }

        private void btnERR_Click(object sender, EventArgs e)
        {
            if (SelectedPlayer != null)
            {
                SelectedPlayer.AddE();
                int gtMinutes = gametime / 60 + 1;
                listTimeline.Items.Add(gtMinutes + "' (" + SelectedPlayer.Broj + ") " +
                    SelectedPlayer.Ime + " " + SelectedPlayer.Prezime + " je napravio gresku!");
            }
            listTimeline.TopIndex = listTimeline.Items.Count - 1;
        }

        private void btnIN_Click(object sender, EventArgs e)
        {
            if (SelectedPlayer != null)
            {
                SelectedPlayer.AddIN();
                int gtMinutes = gametime / 60 + 1;
                listTimeline.Items.Add(gtMinutes + "' (" + SelectedPlayer.Broj + ") " +
                    SelectedPlayer.Ime + " " + SelectedPlayer.Prezime + " je presjekao loptu!");
            }
            listTimeline.TopIndex = listTimeline.Items.Count - 1;
        }

        private void btnT_Click(object sender, EventArgs e)
        {
            if (SelectedPlayer != null)
            {
                SelectedPlayer.AddT();
                int gtMinutes = gametime / 60 + 1;
                listTimeline.Items.Add(gtMinutes + "' (" + SelectedPlayer.Broj + ") " +
                    SelectedPlayer.Ime + " " + SelectedPlayer.Prezime + " je napravio turnover!");
            }
            listTimeline.TopIndex = listTimeline.Items.Count - 1;
        }

        private void btnFaceoff_Click(object sender, EventArgs e)
        {
            if (SelectedPlayer != null)
            {
                SelectedPlayer.AddFaceoff(false);
                int gtMinutes = gametime / 60 + 1;
                listTimeline.Items.Add(gtMinutes + "' (" + SelectedPlayer.Broj + ") " +
                    SelectedPlayer.Ime + " " + SelectedPlayer.Prezime + " je izgubio faceoff!");
            }
            listTimeline.TopIndex = listTimeline.Items.Count - 1;
        }

        private void btnFaceoffWin_Click(object sender, EventArgs e)
        {
            if (SelectedPlayer != null)
            {
                SelectedPlayer.AddFaceoff(true);
                int gtMinutes = gametime / 60 + 1;
                listTimeline.Items.Add(gtMinutes + "' (" + SelectedPlayer.Broj + ") " +
                    SelectedPlayer.Ime + " " + SelectedPlayer.Prezime + " je dobio faceoff!");
            }
            listTimeline.TopIndex = listTimeline.Items.Count - 1;
        }

        private void btnPenalty_Click(object sender, EventArgs e)
        {
            if (SelectedPlayer != null)
            {
                int penSeconds;
                try
                {
                    penSeconds = int.Parse(txtPenaltyTime.Text);
                }
                catch { return; }
                SelectedPlayer.AddPenalty(penSeconds);
                int gtMinutes = gametime / 60 + 1;
                listTimeline.Items.Add(gtMinutes + "' (" + SelectedPlayer.Broj + ") " +
                    SelectedPlayer.Ime + " " + SelectedPlayer.Prezime + " je dobio " + penSeconds + " sekundi iskljucenja.");
                Iskljucenja temp = new Iskljucenja(SelectedPlayer.Broj,SelectedPlayer.Prezime,penSeconds);
                ListaIskljucenja.Add(temp);
                DataBind3();
            }
            listTimeline.TopIndex = listTimeline.Items.Count - 1;
        }
        #endregion

        //FORM CLOSING EVENT
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isRunning)
            {
                DialogResult dialogResult = MessageBox.Show("Jeste li sigurni da zelite izaci iz utakmice dok je utakmica u tijeku?", "Upozorenje!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No) e.Cancel = true;
            }
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                DialogResult dialogResult = MessageBox.Show("Jeste li sigurni da zelite ponovo pokrenuti aplikaciju dok je utakmica u tijeku?", "Upozorenje!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes) Application.Restart();
            }
            else
            {
                Application.Restart();
            }
        }
    }
}
