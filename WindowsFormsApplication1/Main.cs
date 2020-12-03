using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using System.Reflection;
using System.Security.Permissions;


namespace WindowsFormsApplication1
{

    public partial class Main : Form
    {

        string programpath = Application.StartupPath;
        bool pathed = false;


        public Main()
        {

            InitializeComponent();

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void autoLoadBtn_Click(object sender, EventArgs e)
        {
            autoLoadDo();
        }

        //the binary index (OCaml) file should be emptied, as the OCaml script actually adds the new hashtab at its shart, so not emtying it will make it grow indefinitely with different hashtabs for each added NICO.DAT 
        private bool NICO_DAT_IX_BIN_FL_CPY = false; //so keep this false to not fill the 'index' file with additinal data for NICO.DAT files at paths that do not actually exist
        private string PATH_SAVED_FL_PATHNM = "path_saved.txt";
        private int NICO_DAT_HASH_AREA_SZ = 0x8000;
        private string GAME_VERS_FL_PATHNM = "gameVers.txt";



        private long getGameVerLocDataByHash(uint hashIn)
        {
            MyGlobal.disc_version = null;
            if (File.Exists(PATH_SAVED_FL_PATHNM))
            {
                using (StreamReader sr = File.OpenText(GAME_VERS_FL_PATHNM))
                {
                    while (true)
                    {
                        string line = sr.ReadLine();
                        if (line == null) break;
                        string[] ent = line.Split(' ');
                        uint hash = 0; long size = 0; string verId = null; string name = null;
                        if (ent.Length > 0) hash = Convert.ToUInt32(ent[0], 16);
                        if (ent.Length > 1) size = Convert.ToInt64(ent[1], 16);
                        if (ent.Length > 2) verId = ent[2];
                        if (ent.Length > 3) name = ent[3];
                        string gvName = null; if (name != null) gvName = name.Replace('_', ' ');
                        if (hashIn == hash)
                        {
                            //MessageBox.Show("Match hash " + hashIn.ToString("X") + "  ent.len = " + ent.Length.ToString() + " >" + line + "<");
                            MyGlobal.disc_version = verId;
                            return size; //success
                        }
                    }
                }
            }
            return -1; //undetermined
        }

        private string gameVerStrToVerName(string gameVer)
        {
            if (gameVer == null) return "";
            if (gameVer.Length <= 0) return "";

            if (File.Exists(PATH_SAVED_FL_PATHNM))
            {
                using (StreamReader sr = File.OpenText(GAME_VERS_FL_PATHNM))
                {
                    while (true) {
                        string line = sr.ReadLine();
                        if (line == null) break;
                        string[] ent = line.Split(' ');
                        uint hash = 0; long size = 0; string verId = null; string name = null;
                        //MessageBox.Show("ent.len = " + ent.Length.ToString());
                        if (ent.Length < 4) continue; //this should save from some problems here
                        if (ent.Length > 0) if (ent[0].Length > 0) hash = Convert.ToUInt32(ent[0], 16);
                        if (ent.Length > 1) size = Convert.ToInt64(ent[1], 16);
                        if (ent.Length > 2) verId = ent[2];
                        if (ent.Length > 3) name = ent[3];
                        string gvName = null; if (name != null) gvName = name.Replace('_', ' ');
                        if (verId == gameVer) {
                            return gvName;
                        }
                    }
                }
            }

                switch (gameVer)
                {
                    case "gen": return "Gen";
                    case "opm": return "OPM Demo";
                    case "psu": return "PSU Demo";
                    case "ppr": return "PSU Preview";
                    case "preview": return "Preview";
                    case "ntsc": return "US NTSC";
                    case "pal": return "PAL";
                    default: return "Not set";
                }

        }


        static public string getDiscVerIxDir()
        {
            string discVerDir = "Resources/version_index/" + MyGlobal.disc_version + "/";
            return discVerDir;
        }

        private int fileCopyIfExistCrtDir(string dst, string src, bool overwrite)
        {
            if (src == null) return -1;
            if (dst == null) return -2;
            if (src.Length <= 0) return -3;
            if (dst.Length <= 0) return -4;
            Directory.CreateDirectory(dst);
            if (File.Exists(src)) { File.Copy(src, dst, overwrite); return 1; }
            return 0;
        }

        private int fileCopyIfExist(string dst, string src, bool overwrite)
        {
            if (src == null) return -1;
            if (dst == null) return -2;
            if (src.Length <= 0) return -3;
            if (dst.Length <= 0) return -4;
            if (File.Exists(src)) { File.Copy(src, dst, overwrite); return 1; }
            return 0;
        }

        //srcB has lower priority - if srcA does not exist, scrB is used
        public int fileCopyMultiSrcIfExistCrtDir(string dst, string srcA, string srcB, bool overwrite)
        {
            string src = srcB;
            if (srcA != null) if (srcA.Length > 0) if (File.Exists(srcA)) src = srcA;
            return fileCopyIfExist(dst, src, overwrite);
        }

        //returns the first one that exists of srcA, srcB, srcC
        static public string fileGetExistsPath(string srcA, string srcB, string srcC)
        {
            if (srcA != null) if (srcA.Length > 0) if (File.Exists(srcA)) return srcA;
            if (srcB != null) if (srcB.Length > 0) if (File.Exists(srcB)) return srcB;
            return srcC;
        }

        /*
        static void mapDrive(String driveChar, string server, string user, string password)
        {

            try
            {
                ProcessStartInfo procStartInfo;
                procStartInfo = new ProcessStartInfo();
                procStartInfo.FileName = @"C:\windows\system32\cmd.exe";
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.RedirectStandardError = true;
                procStartInfo.RedirectStandardInput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;

                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.ErrorDataReceived += cmd_Error;
                proc.OutputDataReceived += cmd_DataReceived;
                proc.EnableRaisingEvents = true;
                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();

                proc.StandardInput.WriteLine(" if exist v: (echo true) else (echo false)");
                //it should print 'false'
                proc.WaitForExit();

            }
            catch (Exception e)
            {
                // MessageBox.Show(e.Message);
            }
        }*/

        /*static void cmd_DataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine("Output from other process");
            Console.WriteLine(e.Data);
        }

        static void cmd_Error(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine("Error from other process");
            Console.WriteLine(e.Data);
        }*/


        //TODO(maybe): Maybe have a local cache of index files, saved to different folders, each named with the hash of that gameVer and be able to select between them... but would be overly-compicated to implement.

        //XXX: Some (most) funcs here use sotc_path.Text (or other parts of it) rather MyGlobal.nicoDat_path - so maybe all should be changed to use the global var.

        private void autoLoadDo()
        {
            //File.Exists(sotc_path.SelectedItem.ToString() + @"\NICO.DAT")) - this doesn't work here, as the selected item is not set
            if (!File.Exists(sotc_path.Text + @"\NICO.DAT")) //check if the file exists
            {
                MessageBox.Show("Shadow of the Colossus data not found", "ERROR");
                return;
            }

            MessageBox.Show("GEN (AutoLoad): Loading Generic/Generated game version.\nThe Viewer will refresh now, click OK and wait a moment.\nCan take 1 to 5 or more minutes to read NICO.DAT, generate the text index file, then convert it to binary, then divide it into resources groups.");

            path_state.ForeColor = Color.DarkRed;
            path_state.Text = "LOADING...";
            path_state.Visible = true;

            var processStartInfo = new ProcessStartInfo("cmd");
            processStartInfo.UseShellExecute = false;
            processStartInfo.ErrorDialog = false;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.CreateNoWindow = false; // true;

            Process process = new Process();
            process.StartInfo = processStartInfo;
            //process.OutputDataReceived += (sender2, args) => Console.WriteLine("received output: {0}", args.Data);
            //process.ErrorDataReceived += cmd_Error;
            //process.OutputDataReceived += cmd_DataReceived; //This probably works, but would have to attach a console...
            //process.EnableRaisingEvents = true;
            bool processStarted = process.Start();

            //moved to the index_gen script: File.WriteAllText(@"index/index", String.Empty); //the binary index (OCaml) file should be emptied, as the OCaml script actually adds the new hashtab at its shart, so not emtying it will make it grow indefinitely with different hashtabs for each added NICO.DAT 
            //File.WriteAllBytes(@"index/index", Properties.Resources.index_preview);
            //File.WriteAllBytes(@"index/nico.dat.index", Properties.Resources.nico_dat_preview);
            StreamWriter inputWriter = process.StandardInput;
            inputWriter.WriteLine("cd index");
            inputWriter.WriteLine("index_gen " + sotc_path.Text);
            //Generate sorted resource lists
            //inputWriter.WriteLine("cd .."); //back to main dir
            //inputWriter.WriteLine("sotcViewerResListsGen index\\nico.dat.index Resources"); //leave this to be done by the batch file
            inputWriter.WriteLine("exit");

            //process.BeginOutputReadLine();
            //process.BeginErrorReadLine();
            //process.StandardInput.WriteLine(" echo testEchoStr");
            //To avoid deadlocks, always read the output stream first and then wait.  
            string procOutput = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            string procOutSkipMs = procOutput; //Skip teh first two lines:
            int skipLen = procOutSkipMs.IndexOf("\n"); if (skipLen < 0) skipLen = 0; else skipLen += 1; procOutSkipMs = procOutSkipMs.Substring(skipLen);
            skipLen = procOutSkipMs.IndexOf("\n"); if (skipLen < 0) skipLen = 0; else skipLen += 1; procOutSkipMs = procOutSkipMs.Substring(skipLen);
            MessageBox.Show("GEN (AutoLoad):\nLoading complete!!\n" + procOutSkipMs, "Changed");

            path_state.ForeColor = Color.Green;
            path_state.Text = "LOADED!!!";

            MyGlobal.disc_version = "gen";
            label_disc_type.ForeColor = Color.Green;
            label_disc_type.Text = gameVerStrToVerName(MyGlobal.disc_version);
            saveLastOpenInfo();
        }


#if false
        //from https://www.oipapio.com/question-2317566
        private uint Crc32CAlgorithmBigCrc(string fileName)
        {
            uint hash = 0;
            byte[] buffer = null;
            FileInfo fileInfo = new FileInfo(fileName);
            long fileLength = fileInfo.Length;
            int blockSize = 1024000000;
            decimal div = fileLength / blockSize;
            int blocks = (int)Math.Floor(div);
            int restBytes = (int)(fileLength - (blocks * blockSize));
            long offsetFile = 0;
            uint interHash = 0;
            Crc32CAlgorithm Crc32CAlgorithm = new Crc32CAlgorithm();
            bool firstBlock = true;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[blockSize];
                using (BinaryReader br = new BinaryReader(fs))
                {
                    while (blocks > 0)
                    {
                        blocks -= 1;
                        fs.Seek(offsetFile, SeekOrigin.Begin);
                        buffer = br.ReadBytes(blockSize);
                        if (firstBlock)
                        {
                            firstBlock = false;
                            interHash = Crc32CAlgorithm.Compute(buffer);
                            hash = interHash;
                        }
                        else
                        {
                            hash = Crc32CAlgorithm.Append(interHash, buffer);
                        }
                        offsetFile += blockSize;
                    }
                    if (restBytes > 0)
                    {
                        Array.Resize(ref buffer, restBytes);
                        fs.Seek(offsetFile, SeekOrigin.Begin);
                        buffer = br.ReadBytes(restBytes);
                        hash = Crc32CAlgorithm.Append(interHash, buffer);
                    }
                    buffer = null;
                }
            }
            //MessageBox.Show(hash.ToString());
            //MessageBox.Show(hash.ToString("X"));
            return hash;
        }
#endif

#if false
        //from https://www.geeksforgeeks.org/hash-function-for-string-data-in-c-sharp/
        private int flHashFunc(string s, string[] array)
        {
            int total = 0;
            char[] c;
            c = s.ToCharArray();
            for (int k = 0; k <= c.GetUpperBound(0); k++) total += (int)c[k];
            return total % array.GetUpperBound(0);
        }
#endif

        //WARNING: This does not take into account endianness!
        private uint calcBufAddHash(byte[] buffer)
        {
            //uint[] wArr = Array.ConvertAll(buffer, Convert.ToUInt32); //This converts each word to byte, so no good
            uint hash = 0;
            //int bLen = buffer.Length;
            //int wLen = wArr.Length;
            int i;
            //for (i=0; i<wLen; i++) hash += wArr[i];
            var wLen = buffer.Length / sizeof(uint);
            for (i=0; i<wLen; i++)
            {
                hash += BitConverter.ToUInt32(buffer, i * sizeof(int));
            }
            return hash;
        }

        //Claculates the additive hash of the firs nrWords (4byte each) of the given file
        private uint getFileBgnHash(string fileName, int nrWords) // Crc32CAlgorithmBigCrc(string fileName)
        {
            uint hash = 0;
            byte[] buffer = null;
            FileInfo fileInfo = new FileInfo(fileName);
            long fileLength = fileInfo.Length;
            int blockSize = nrWords * 4; // 1024000000;
            long offsetFile = 0;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[blockSize];
                using (BinaryReader br = new BinaryReader(fs))
                {
                    fs.Seek(offsetFile, SeekOrigin.Begin);
                    buffer = br.ReadBytes(blockSize);
                    hash = calcBufAddHash(buffer);
                    buffer = null;
                }
            }
            //MessageBox.Show("NICO.DAT initial 0x" + blockSize.ToString("X") + " bytes hash = dec "  + hash.ToString() + " in hex 0x" + hash.ToString("X"));
            //MessageBox.Show(hash.ToString("X"));
            return hash;
        }

        private uint mkeNicoDatHash()
        {
            return getFileBgnHash(MyGlobal.nicoDat_path + @"\NICO.DAT", NICO_DAT_HASH_AREA_SZ / 4);
        }
        private int recognizeNicoDat(string flPathNm)
        {
            return 0;
        }


        private void saveLastOpenInfo()
        {
            //System.IO.File.WriteAllText(@"path_saved" + ".txt", sotc_path.SelectedItem.ToString());
            string[] pathSavedLines = new string[4];
            pathSavedLines[0] = MyGlobal.nicoDat_path;
            pathSavedLines[1] = MyGlobal.disc_version;  //on line 2
            pathSavedLines[2] = MyGlobal.nicoDat_hash.ToString("X");
            pathSavedLines[3] = MyGlobal.nicoDat_size.ToString("X");
            //System.IO.File.WriteAllText(@PATH_SAVED_FL_PATHNM, sotc_path.SelectedItem.ToString());
            //pathed = true;
            //MessageBox.Show("Saving path=" + pathSavedLines[0] + " discVer=" + pathSavedLines[1] + " hash[hex]=" + pathSavedLines[2], "Changed");
            System.IO.File.WriteAllLines(@PATH_SAVED_FL_PATHNM, pathSavedLines);
            //MessageBox.Show("Saving path=" + pathSavedLines[0] + " discVer=" + pathSavedLines[1] + " hash[hex]=" + pathSavedLines[2], "Changed");
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            nomad_link.Links[0].LinkData = "http://nomads-sotc-blog.blogspot.com.au/2011/04/welcome-to-nomads-shadow-of-colossus.html";
            
            linkLabel1.Links[0].LinkData = "http://blackbirdsotc.blogspot.com.br/";
        
            linkLabel2.Links[0].LinkData = "http://www.youtube.com/user/wwwarea";
            
            fyle_system_group.Parent = pictureBox1;

            groupBox1.Parent = pictureBox1;


            try
            {

                if (File.Exists(PATH_SAVED_FL_PATHNM)) {
                    using (StreamReader sr = File.OpenText(PATH_SAVED_FL_PATHNM))
                    {
                        sotc_path.Text = sr.ReadLine();
                        MyGlobal.nicoDat_path = sotc_path.Text;
                        MyGlobal.disc_version = sr.ReadLine();  //on line 2
                        MyGlobal.nicoDat_hash = Convert.ToUInt32(sr.ReadLine(), 16);
                    }
                    //MessageBox.Show("Old path=" + sotc_path.Text + " discVer=" + MyGlobal.disc_version + " hash[hex]=" + MyGlobal.nicoDat_hash.ToString("X"), "Changed");
                }

                uint nicoDat_crHash = mkeNicoDatHash(); //getFileBgnHash(sotc_path.Text + @"\NICO.DAT", 0x8000/4); //read the start of the initial headers which should be different for most game versions
                if (MyGlobal.disc_version != "gen") getGameVerLocDataByHash(nicoDat_crHash);

                if (MyGlobal.nicoDat_hash == nicoDat_crHash)
                { //no change
                    //MessageBox.Show("No gameVer change detected.");
                    label_disc_type.ForeColor = Color.Green;
                    label_disc_type.Text = gameVerStrToVerName(MyGlobal.disc_version);
                } else {
                    FileInfo f = new FileInfo(sotc_path.Text + @"\NICO.DAT");
                    long nico = f.Length / 1000000;
                    MyGlobal.nicoDat_size = f.Length;

                    MessageBox.Show("Path=" + sotc_path.Text + "  \ndiscVer=" + MyGlobal.disc_version + "  oldHash[hex]=" 
                        + MyGlobal.nicoDat_hash.ToString("X") + "  newHash[hex]=" + nicoDat_crHash.ToString("X")
                        + "  size[hex]=" + MyGlobal.nicoDat_size.ToString("X")
                        + "\nDifferent word hash detected (of the first 0x" + NICO_DAT_HASH_AREA_SZ.ToString("X") + " of NICO.DAT)." 
                        , "Different hash detected.");
                    MyGlobal.nicoDat_hash = nicoDat_crHash;

                    if (MyGlobal.disc_version == "gen")
                    {
                        label_disc_type.ForeColor = Color.Green;
                        label_disc_type.Text = gameVerStrToVerName(MyGlobal.disc_version);
                        autoLoadDo();
                    }
                    else
                    {
                        //moved to the index_pal / ntsc script: File.WriteAllText(@"index/index", String.Empty); //the binary index (OCaml) file should be emptied, as the OCaml script actually adds the new hashtab at its shart, so not emtying it will make it grow indefinitely with different hashtabs for each added NICO.DAT 

                        if (File.Exists(sotc_path.Text + @"\XAB"))
                        {
                                if (File.Exists(@"index\nico.dat.index"))
                                {
                                    FileInfo c = new FileInfo(@"index\nico.dat.index");
                                    long index = c.Length / 1000;
                                    if (index >= 480 && index <= 484) {; }
                                    else
                                    {
                                        MessageBox.Show("Found a different game version on the previously choosen path.\nThe Viewer will refresh now, click OK and wait a moment.");
                                        var processStartInfo = new ProcessStartInfo("cmd");

                                        processStartInfo.UseShellExecute = false;
                                        processStartInfo.ErrorDialog = false;

                                        processStartInfo.RedirectStandardError = true;
                                        processStartInfo.RedirectStandardInput = true;
                                        processStartInfo.RedirectStandardOutput = true;
                                        processStartInfo.CreateNoWindow = true;

                                        Process process = new Process();
                                        process.StartInfo = processStartInfo;
                                        bool processStarted = process.Start();

                                        if (NICO_DAT_IX_BIN_FL_CPY) File.WriteAllBytes(@"index/index", Properties.Resources.index_pal);
                                        File.WriteAllBytes(@"index/nico.dat.index", Properties.Resources.nico_dat_pal);
                                        File.WriteAllBytes(@"index/xab.index", Properties.Resources.xab);
                                        File.WriteAllBytes(@"index/xac.index", Properties.Resources.xac);
                                        StreamWriter inputWriter = process.StandardInput;
                                        inputWriter.WriteLine("cd index");
                                        inputWriter.WriteLine("index_pal " + sotc_path.Text);
                                        inputWriter.WriteLine("exit");
                                        process.WaitForExit();
                                        MessageBox.Show("PAL disc Found\nChanging complete!!", "Changed");
                                    }

                                }
                                else
                                {
                                    MessageBox.Show("Found a different game version on the previously choosen path.\nThe Viewer will refresh now, click OK and wait a moment.");
                                    var processStartInfo = new ProcessStartInfo("cmd");

                                    processStartInfo.UseShellExecute = false;
                                    processStartInfo.ErrorDialog = false;

                                    processStartInfo.RedirectStandardError = true;
                                    processStartInfo.RedirectStandardInput = true;
                                    processStartInfo.RedirectStandardOutput = true;
                                    processStartInfo.CreateNoWindow = true;

                                    Process process = new Process();
                                    process.StartInfo = processStartInfo;
                                    bool processStarted = process.Start();

                                    if (NICO_DAT_IX_BIN_FL_CPY) File.WriteAllBytes(@"index/index", Properties.Resources.index_pal);
                                    File.WriteAllBytes(@"index/nico.dat.index", Properties.Resources.nico_dat_pal);
                                    File.WriteAllBytes(@"index/xab.index", Properties.Resources.xab);
                                    File.WriteAllBytes(@"index/xac.index", Properties.Resources.xac);
                                    StreamWriter inputWriter = process.StandardInput;
                                    inputWriter.WriteLine("cd index");
                                    inputWriter.WriteLine("index_pal " + sotc_path.Text);
                                    inputWriter.WriteLine("exit");
                                    process.WaitForExit();
                                    MessageBox.Show("PAL disc Found\nChanging complete!!", "Changed");
                                }
                                MyGlobal.disc_version = "pal";
                            
                            }
                            else
                            {
                                if (nico > 2668)
                                {

                                if (MyGlobal.nicoDat_hash == 0x363AFB55)
                                {  //PSU Pv
                                    try
                                    {
                                        FileInfo c = new FileInfo(@"index\nico.dat.index");
                                        long index = c.Length / 1000;
                                        if (index >= 1350 && index <= 1352) { }
                                        else
                                        {
                                            MessageBox.Show("Found a different game version on the previously choosen path.\nThe Viewer will refresh now, click OK and wait a moment.");
                                            var processStartInfo = new ProcessStartInfo("cmd");

                                            processStartInfo.UseShellExecute = false;
                                            processStartInfo.ErrorDialog = false;

                                            processStartInfo.RedirectStandardError = true;
                                            processStartInfo.RedirectStandardInput = true;
                                            processStartInfo.RedirectStandardOutput = true;
                                            processStartInfo.CreateNoWindow = true;

                                            Process process = new Process();
                                            process.StartInfo = processStartInfo;
                                            bool processStarted = process.Start();

                                            if (NICO_DAT_IX_BIN_FL_CPY) File.WriteAllBytes(@"index/index", Properties.Resources.index_ppr);
                                            File.WriteAllBytes(@"index/nico.dat.index", Properties.Resources.nico_dat_ppr);
                                            StreamWriter inputWriter = process.StandardInput;
                                            inputWriter.WriteLine("cd index");
                                            inputWriter.WriteLine("index_ntsc " + sotc_path.Text);
                                            inputWriter.WriteLine("exit");
                                            process.WaitForExit();
                                            MessageBox.Show("PSU Preview disc Found\nChanging complete!!", "Changed");
                                        }
                                    }
                                    catch
                                    {
                                        MessageBox.Show("Found a different game version on the previously choosen path.\nThe Viewer will refresh now, click OK and wait a moment.");
                                        var processStartInfo = new ProcessStartInfo("cmd");

                                        processStartInfo.UseShellExecute = false;
                                        processStartInfo.ErrorDialog = false;

                                        processStartInfo.RedirectStandardError = true;
                                        processStartInfo.RedirectStandardInput = true;
                                        processStartInfo.RedirectStandardOutput = true;
                                        processStartInfo.CreateNoWindow = true;

                                        Process process = new Process();
                                        process.StartInfo = processStartInfo;
                                        bool processStarted = process.Start();

                                        if (NICO_DAT_IX_BIN_FL_CPY) File.WriteAllBytes(@"index/index", Properties.Resources.index_ppr);
                                        File.WriteAllBytes(@"index/nico.dat.index", Properties.Resources.nico_dat_ppr);
                                        StreamWriter inputWriter = process.StandardInput;
                                        inputWriter.WriteLine("cd index");
                                        inputWriter.WriteLine("index_ntsc " + sotc_path.Text);
                                        inputWriter.WriteLine("exit");
                                        process.WaitForExit();
                                        MessageBox.Show("PSU Preview disc Found\nChanging complete!!", "Changed");
                                    }
                                    MyGlobal.disc_version = "ppr";
                                }
                                else
                                {
                                    try
                                    {
                                        FileInfo c = new FileInfo(@"index\nico.dat.index");
                                        long index = c.Length / 1000;
                                        if (index >= 1350 && index <= 1352) { }
                                        else
                                        {
                                            MessageBox.Show("Found a different game version on the previously choosen path.\nThe Viewer will refresh now, click OK and wait a moment.");
                                            var processStartInfo = new ProcessStartInfo("cmd");

                                            processStartInfo.UseShellExecute = false;
                                            processStartInfo.ErrorDialog = false;

                                            processStartInfo.RedirectStandardError = true;
                                            processStartInfo.RedirectStandardInput = true;
                                            processStartInfo.RedirectStandardOutput = true;
                                            processStartInfo.CreateNoWindow = true;

                                            Process process = new Process();
                                            process.StartInfo = processStartInfo;
                                            bool processStarted = process.Start();

                                            if (NICO_DAT_IX_BIN_FL_CPY) File.WriteAllBytes(@"index/index", Properties.Resources.index_preview);
                                            File.WriteAllBytes(@"index/nico.dat.index", Properties.Resources.nico_dat_preview);
                                            StreamWriter inputWriter = process.StandardInput;
                                            inputWriter.WriteLine("cd index");
                                            inputWriter.WriteLine("index_ntsc " + sotc_path.Text);
                                            inputWriter.WriteLine("exit");
                                            process.WaitForExit();
                                            MessageBox.Show("PREVIEW disc Found\nChanging complete!!", "Changed");
                                        }
                                    }
                                    catch
                                    {
                                        MessageBox.Show("Found a different game version on the previously choosen path.\nThe Viewer will refresh now, click OK and wait a moment.");
                                        var processStartInfo = new ProcessStartInfo("cmd");

                                        processStartInfo.UseShellExecute = false;
                                        processStartInfo.ErrorDialog = false;

                                        processStartInfo.RedirectStandardError = true;
                                        processStartInfo.RedirectStandardInput = true;
                                        processStartInfo.RedirectStandardOutput = true;
                                        processStartInfo.CreateNoWindow = true;

                                        Process process = new Process();
                                        process.StartInfo = processStartInfo;
                                        bool processStarted = process.Start();

                                        if (NICO_DAT_IX_BIN_FL_CPY) File.WriteAllBytes(@"index/index", Properties.Resources.index_preview);
                                        File.WriteAllBytes(@"index/nico.dat.index", Properties.Resources.nico_dat_preview);
                                        StreamWriter inputWriter = process.StandardInput;
                                        inputWriter.WriteLine("cd index");
                                        inputWriter.WriteLine("index_ntsc " + sotc_path.Text);
                                        inputWriter.WriteLine("exit");
                                        process.WaitForExit();
                                        MessageBox.Show("PREVIEW disc Found\nChanging complete!!", "Changed");
                                    }
                                    MyGlobal.disc_version = "preview";
                                }
                                }
                                else
                                {
                                    if (nico < 700)
                                    {
                                        if (nico < 685)
                                        {
                                            try
                                            {
                                                FileInfo c = new FileInfo(@"index\nico.dat.index");
                                                long index = c.Length / 1000;
                                                if (index >= 378 && index <= 381) {; }
                                                else
                                                {
                                                    MessageBox.Show("Found a different game version on the previously choosen path.\nThe Viewer will refresh now, click OK and wait a moment.");
                                                    var processStartInfo = new ProcessStartInfo("cmd");

                                                    processStartInfo.UseShellExecute = false;
                                                    processStartInfo.ErrorDialog = false;

                                                    processStartInfo.RedirectStandardError = true;
                                                    processStartInfo.RedirectStandardInput = true;
                                                    processStartInfo.RedirectStandardOutput = true;
                                                    processStartInfo.CreateNoWindow = true;

                                                    Process process = new Process();
                                                    process.StartInfo = processStartInfo;
                                                    bool processStarted = process.Start();

                                                    if (NICO_DAT_IX_BIN_FL_CPY) File.WriteAllBytes(@"index/index", Properties.Resources.index_opm);
                                                    File.WriteAllBytes(@"index/nico.dat.index", Properties.Resources.nico_dat_opm);
                                                    StreamWriter inputWriter = process.StandardInput;
                                                    inputWriter.WriteLine("cd index");
                                                    inputWriter.WriteLine("index_ntsc " + sotc_path.Text);
                                                    inputWriter.WriteLine("exit");
                                                    process.WaitForExit();
                                                    MessageBox.Show("OPM disc Found\nChanging complete!!", "Changed");
                                                }

                                            }
                                            catch
                                            {
                                                MessageBox.Show("Found a different game version on the previously choosen path.\nThe Viewer will refresh now, click OK and wait a moment.");
                                                var processStartInfo = new ProcessStartInfo("cmd");

                                                processStartInfo.UseShellExecute = false;
                                                processStartInfo.ErrorDialog = false;

                                                processStartInfo.RedirectStandardError = true;
                                                processStartInfo.RedirectStandardInput = true;
                                                processStartInfo.RedirectStandardOutput = true;
                                                processStartInfo.CreateNoWindow = true;

                                                Process process = new Process();
                                                process.StartInfo = processStartInfo;
                                                bool processStarted = process.Start();

                                                if (NICO_DAT_IX_BIN_FL_CPY) File.WriteAllBytes(@"index/index", Properties.Resources.index_opm);
                                                File.WriteAllBytes(@"index/nico.dat.index", Properties.Resources.nico_dat_opm);
                                                StreamWriter inputWriter = process.StandardInput;
                                                inputWriter.WriteLine("cd index");
                                                inputWriter.WriteLine("index_ntsc " + sotc_path.Text);
                                                inputWriter.WriteLine("exit");
                                                process.WaitForExit();
                                                MessageBox.Show("OPM disc Found\nChanging complete!!", "Changed");
                                            }
                                            MyGlobal.disc_version = "opm";
                                        
                                        }
                                        else
                                        {
                                            try
                                            {
                                                FileInfo c = new FileInfo(@"index\nico.dat.index");
                                                long index = c.Length / 1000;
                                                if (index >= 382 && index <= 386) {; }
                                                else
                                                {
                                                    MessageBox.Show("Found a different game version on the previously choosen path.\nThe Viewer will refresh now, click OK and wait a moment.");
                                                    var processStartInfo = new ProcessStartInfo("cmd");

                                                    processStartInfo.UseShellExecute = false;
                                                    processStartInfo.ErrorDialog = false;

                                                    processStartInfo.RedirectStandardError = true;
                                                    processStartInfo.RedirectStandardInput = true;
                                                    processStartInfo.RedirectStandardOutput = true;
                                                    processStartInfo.CreateNoWindow = true;

                                                    Process process = new Process();
                                                    process.StartInfo = processStartInfo;
                                                    bool processStarted = process.Start();

                                                    if (NICO_DAT_IX_BIN_FL_CPY) File.WriteAllBytes(@"index/index", Properties.Resources.index_psu);
                                                    File.WriteAllBytes(@"index/nico.dat.index", Properties.Resources.nico_dat_psu);
                                                    StreamWriter inputWriter = process.StandardInput;
                                                    inputWriter.WriteLine("cd index");
                                                    inputWriter.WriteLine("index_ntsc " + sotc_path.Text);
                                                    inputWriter.WriteLine("exit");
                                                    process.WaitForExit();
                                                    MessageBox.Show("PSU disc Found\nChanging complete!!", "Changed");
                                                }

                                            }
                                            catch
                                            {
                                                MessageBox.Show("Found a different game version on the previously choosen path.\nThe Viewer will refresh now, click OK and wait a moment.");
                                                var processStartInfo = new ProcessStartInfo("cmd");

                                                processStartInfo.UseShellExecute = false;
                                                processStartInfo.ErrorDialog = false;

                                                processStartInfo.RedirectStandardError = true;
                                                processStartInfo.RedirectStandardInput = true;
                                                processStartInfo.RedirectStandardOutput = true;
                                                processStartInfo.CreateNoWindow = true;

                                                Process process = new Process();
                                                process.StartInfo = processStartInfo;
                                                bool processStarted = process.Start();

                                                if (NICO_DAT_IX_BIN_FL_CPY) File.WriteAllBytes(@"index/index", Properties.Resources.index_psu);
                                                File.WriteAllBytes(@"index/nico.dat.index", Properties.Resources.nico_dat_psu);
                                                StreamWriter inputWriter = process.StandardInput;
                                                inputWriter.WriteLine("cd index");
                                                inputWriter.WriteLine("index_ntsc " + sotc_path.Text);
                                                inputWriter.WriteLine("exit");
                                                process.WaitForExit();
                                                MessageBox.Show("PSU disc Found\nChanging complete!!", "Changed");
                                            }
                                            MyGlobal.disc_version = "psu";
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            FileInfo c = new FileInfo(@"index\nico.dat.index");
                                            long index = c.Length / 1000;
                                            if (index >= 1250 && index <= 1258) {; }
                                            else
                                            {
                                                MessageBox.Show("Found a different game version on the previously choosen path.\nThe Viewer will refresh now, click OK and wait a moment.");
                                                var processStartInfo = new ProcessStartInfo("cmd");

                                                processStartInfo.UseShellExecute = false;
                                                processStartInfo.ErrorDialog = false;

                                                processStartInfo.RedirectStandardError = true;
                                                processStartInfo.RedirectStandardInput = true;
                                                processStartInfo.RedirectStandardOutput = true;
                                                processStartInfo.CreateNoWindow = true;

                                                Process process = new Process();
                                                process.StartInfo = processStartInfo;
                                                bool processStarted = process.Start();

                                                if (NICO_DAT_IX_BIN_FL_CPY) File.WriteAllBytes(@"index/index", Properties.Resources.index_ntsc);
                                                File.WriteAllBytes(@"index/nico.dat.index", Properties.Resources.nico_dat_ntsc);
                                                StreamWriter inputWriter = process.StandardInput;
                                                inputWriter.WriteLine("cd index");
                                                inputWriter.WriteLine("index_ntsc " + sotc_path.Text);
                                                inputWriter.WriteLine("exit");
                                                process.WaitForExit();
                                                MessageBox.Show("NTSC disc Found\nChanging complete!!", "Changed");
                                            }

                                        }
                                        catch
                                        {
                                            MessageBox.Show("Found a different game version on the previously choosen path.\nThe Viewer will refresh now, click OK and wait a moment.");
                                            var processStartInfo = new ProcessStartInfo("cmd");

                                            processStartInfo.UseShellExecute = false;
                                            processStartInfo.ErrorDialog = false;

                                            processStartInfo.RedirectStandardError = true;
                                            processStartInfo.RedirectStandardInput = true;
                                            processStartInfo.RedirectStandardOutput = true;
                                            processStartInfo.CreateNoWindow = true;

                                            Process process = new Process();
                                            process.StartInfo = processStartInfo;
                                            bool processStarted = process.Start();

                                            if (NICO_DAT_IX_BIN_FL_CPY) File.WriteAllBytes(@"index/index", Properties.Resources.index_ntsc);
                                            File.WriteAllBytes(@"index/nico.dat.index", Properties.Resources.nico_dat_ntsc);
                                            StreamWriter inputWriter = process.StandardInput;
                                            inputWriter.WriteLine("cd index");
                                            inputWriter.WriteLine("index_ntsc " + sotc_path.Text);
                                            inputWriter.WriteLine("exit");
                                            process.WaitForExit();
                                            MessageBox.Show("NTSC disc Found\nChanging complete!!", "Changed");
                                        }
                                        MyGlobal.disc_version = "ntsc";
                                    }
                                }
                            }
                    }

                    label_disc_type.ForeColor = Color.Green;
                    label_disc_type.Text = gameVerStrToVerName(MyGlobal.disc_version);
                    saveLastOpenInfo();
                }
            }
            catch
            {
                sotc_path.Text = "Select a path";
            }

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void colossusin_MouseEnter(object sender, EventArgs e)
        {
            colossusout.Visible = true;
            
        }

        private void wanderin_MouseEnter(object sender, EventArgs e)
        {
            wanderout.Visible = true;
        }

        private void miscin_MouseEnter(object sender, EventArgs e)
        {
            miscout.Visible = true;
        }

        private void creditsin_MouseEnter(object sender, EventArgs e)
        {
            creditsout.Visible = true;
        }

        private void colossusout_MouseLeave(object sender, EventArgs e)
        {
            colossusout.Visible = false;
        }

        private void wanderout_MouseLeave(object sender, EventArgs e)
        {
            wanderout.Visible = false;
        }

        private void miscout_MouseLeave(object sender, EventArgs e)
        {
            miscout.Visible = false;
        }

        private void creditsout_MouseLeave(object sender, EventArgs e)
        {
            creditsout.Visible = false;
        }

        private void colossusout_Click(object sender, EventArgs e)
        {
            ColossusForm CF = new ColossusForm();
            CF.Visible = true;
         
            
        }

        private void creditsout_Click(object sender, EventArgs e)
        {
            Assembly assem = Assembly.GetExecutingAssembly();
            Version vers = assem.GetName().Version;
            DateTime buildDate = new DateTime(2000, 1, 1).AddDays(vers.Build).AddSeconds(vers.Revision * 2);
            //Console.WriteLine(vers.ToString());
            //Console.WriteLine(buildDate.ToString());
            string rev = "Rev.= " + vers.ToString() + " build date " + buildDate.ToString();

            MessageBox.Show("Thanks a lot to NOMAD, this GUI wouldn't be done without his help"
                        + "\nGUI created by MATHEUS EDUARDO GARBELINI"
                        + "\nConsole OpenGL NTO Viewer created by YOUMOOSTC"
                        + "\nThanks HEXDREL to his support"
                        + "\nWWWArea for his beta tester support"
                        + "\nWisi - some modifications and multi-version support"
                        + "\nZen - Dec-2020 updates"
                        + "\n " + rev, "Credits");
        }

       
        private void sotc_SelectedIndexChanged(object sender, EventArgs e)
        {

            
           if(sotc_path.SelectedItem.ToString() == "Browse")
           {
               if (pathed == true)
               {
                   
                   sotc_path.Items.RemoveAt(0);
               }
               var dialog = new FolderBrowserDialog();
                // this.folderBrowserDialog1.RootFolder
               if (MyGlobal.nicoDat_path != null) if(MyGlobal.nicoDat_path.Length > 0) dialog.SelectedPath = MyGlobal.nicoDat_path; //dialog.RootFolder 
               var result = dialog.ShowDialog();
               if (result == DialogResult.OK)
               {


                    if (dialog.SelectedPath.Length <= 3)
                   {
                       sotc_path.Items.Insert(0, dialog.SelectedPath.Remove(dialog.SelectedPath.Length - 1, 1));
                   }
                   else
                   {
                       sotc_path.Items.Insert(0, dialog.SelectedPath);
                   }
                   sotc_path.SelectedIndex = 0;


                    if (File.Exists(sotc_path.SelectedItem.ToString() + @"\NICO.DAT"))
                    {

                        sotc_path.Text = ".....";
                        path_state.ForeColor = Color.DarkRed;
                        path_state.Text = "CHANGING...";
                        path_state.Visible = true;

                        var processStartInfo = new ProcessStartInfo("cmd");
                        processStartInfo.UseShellExecute = false;
                        processStartInfo.ErrorDialog = false;
                        processStartInfo.RedirectStandardError = true;
                        processStartInfo.RedirectStandardInput = true;
                        processStartInfo.RedirectStandardOutput = true;
                        processStartInfo.CreateNoWindow = true;

                        Process process = new Process();
                        process.StartInfo = processStartInfo;
                        bool processStarted = process.Start();


                        //if (sotc_path.SelectedItem.ToString() != "Browse")
                        MyGlobal.nicoDat_path = sotc_path.SelectedItem.ToString();
                        //sotc_path.Text = MyGlobal.nicoDat_path;
                        //MyGlobal.disc_version = ; //set above
                        MyGlobal.nicoDat_hash = mkeNicoDatHash(); // getFileBgnHash(MyGlobal.nicoDat_path + @"\NICO.DAT", 0x8000 / 4);
                        long nicoDatRecognSz = -1;
                        //if (MyGlobal.disc_version != "gen")
                        nicoDatRecognSz = getGameVerLocDataByHash(MyGlobal.nicoDat_hash);
                        string discVerName = gameVerStrToVerName(MyGlobal.disc_version);

                        //moved to the index_pal / ntsc: File.WriteAllText(@"index/index", String.Empty); //the binary index (OCaml) file should be emptied, as the OCaml script actually adds the new hashtab at its shart, so not emtying it will make it grow indefinitely with different hashtabs for each added NICO.DAT 
                        FileInfo nico = new FileInfo(sotc_path.SelectedItem.ToString() + @"\NICO.DAT");
                        MyGlobal.nicoDat_size = nico.Length;

                        if ((nicoDatRecognSz > 0) && (MyGlobal.disc_version != null))
                        { //NICO.DAT found in the known versions list

                            string discVerDir = getDiscVerIxDir();

                            fileCopyIfExist(@"index/nico.dat.index", discVerDir + "nico.dat.index", true); //Properties.Resources.nico_dat_ntsc
                            fileCopyIfExist(@"index/xab.index", discVerDir + "xab.index", true);
                            fileCopyIfExist(@"index/xac.index", discVerDir + "xac.index", true);
                            fileCopyIfExist(@"index/xad.index", discVerDir + "xad.index", true);
                            fileCopyIfExist(@"index/xae.index", discVerDir + "xae.index", true);

                            if (NICO_DAT_IX_BIN_FL_CPY) fileCopyIfExist(@"index/index", discVerDir + "index", true); // Properties.Resources.index_ntsc

                            StreamWriter inputWriter = process.StandardInput;
                            inputWriter.WriteLine("cd index");
                            inputWriter.WriteLine("index_com " + sotc_path.SelectedItem.ToString());
                            inputWriter.WriteLine("exit");
                            //MyGlobal.disc_version = ""; set by nicoDatRecognSz() above

                        } else {

                        long tamanho = nico.Length / 1000000;

                        if (File.Exists(sotc_path.SelectedItem.ToString() + @"\XAB"))
                        {

                            if (NICO_DAT_IX_BIN_FL_CPY) File.WriteAllBytes(@"index/index", Properties.Resources.index_pal);
                            File.WriteAllBytes(@"index/nico.dat.index", Properties.Resources.nico_dat_pal);
                            File.WriteAllBytes(@"index/xab.index", Properties.Resources.xab);
                            File.WriteAllBytes(@"index/xac.index", Properties.Resources.xac);
                            StreamWriter inputWriter = process.StandardInput;
                            inputWriter.WriteLine("cd index");
                            inputWriter.WriteLine("index_pal " + sotc_path.SelectedItem.ToString());
                            inputWriter.WriteLine("exit");
                            process.WaitForExit();
                            MessageBox.Show("PAL disc Found\nChanging complete!!", "Changed");
                            MyGlobal.disc_version = "pal";
                        }
                        else
                        {

                            if (tamanho > 2700)
                            {
                                if (MyGlobal.nicoDat_hash == 0x363AFB55)
                                {

                                    path_state.ForeColor = Color.DarkRed;
                                    path_state.Text = "CHANGING...";
                                    path_state.Visible = true;
                                    if (NICO_DAT_IX_BIN_FL_CPY) File.WriteAllBytes(@"index/index", Properties.Resources.index_ppr);
                                    File.WriteAllBytes(@"index/nico.dat.index", Properties.Resources.nico_dat_ppr);
                                    StreamWriter inputWriter = process.StandardInput;
                                    inputWriter.WriteLine("cd index");
                                    inputWriter.WriteLine("index_ntsc " + sotc_path.SelectedItem.ToString());
                                    inputWriter.WriteLine("exit");
                                    process.WaitForExit();
                                    MessageBox.Show("PSU Preview disc Found\nChanging complete!!", "Changed");
                                    MyGlobal.disc_version = "ppr";
                                }
                                else
                                {

                                    path_state.ForeColor = Color.DarkRed;
                                    path_state.Text = "CHANGING...";
                                    path_state.Visible = true;
                                    if (NICO_DAT_IX_BIN_FL_CPY) File.WriteAllBytes(@"index/index", Properties.Resources.index_preview);
                                    File.WriteAllBytes(@"index/nico.dat.index", Properties.Resources.nico_dat_preview);
                                    StreamWriter inputWriter = process.StandardInput;
                                    inputWriter.WriteLine("cd index");
                                    inputWriter.WriteLine("index_ntsc " + sotc_path.SelectedItem.ToString());
                                    inputWriter.WriteLine("exit");
                                    process.WaitForExit();
                                    MessageBox.Show("PREVIEW disc Found\nChanging complete!!", "Changed");
                                    MyGlobal.disc_version = "preview";
                                }

                            }
                            else
                            {
                                if (tamanho < 700)
                                {
                                    if (tamanho < 685)
                                    {
                                        path_state.ForeColor = Color.DarkRed;
                                        path_state.Text = "CHANGING...";
                                        path_state.Visible = true;
                                        if (NICO_DAT_IX_BIN_FL_CPY) File.WriteAllBytes(@"index/index", Properties.Resources.index_opm);
                                        File.WriteAllBytes(@"index/nico.dat.index", Properties.Resources.nico_dat_opm);
                                        StreamWriter inputWriter = process.StandardInput;
                                        inputWriter.WriteLine("cd index");
                                        inputWriter.WriteLine("index_ntsc " + sotc_path.SelectedItem.ToString());
                                        inputWriter.WriteLine("exit");
                                        process.WaitForExit();
                                        MessageBox.Show("OPM DEMO disc Found\nChanging complete!!", "Changed");
                                        MyGlobal.disc_version = "opm";
                                    }
                                    else
                                    {
                                        path_state.ForeColor = Color.DarkRed;
                                        path_state.Text = "CHANGING...";
                                        path_state.Visible = true;
                                        if (NICO_DAT_IX_BIN_FL_CPY) File.WriteAllBytes(@"index/index", Properties.Resources.index_psu);
                                        File.WriteAllBytes(@"index/nico.dat.index", Properties.Resources.nico_dat_psu);
                                        StreamWriter inputWriter = process.StandardInput;
                                        inputWriter.WriteLine("cd index");
                                        inputWriter.WriteLine("index_ntsc " + sotc_path.SelectedItem.ToString());
                                        inputWriter.WriteLine("exit");
                                        process.WaitForExit();
                                        MessageBox.Show("PSU DEMO disc Found\nChanging complete!!", "Changed");
                                        MyGlobal.disc_version = "psu";
                                    }

                                }
                                else
                                {

                                    path_state.ForeColor = Color.DarkRed;
                                    path_state.Text = "CHANGING...";
                                    path_state.Visible = true;
                                    if (NICO_DAT_IX_BIN_FL_CPY) File.WriteAllBytes(@"index/index", Properties.Resources.index_ntsc);
                                    File.WriteAllBytes(@"index/nico.dat.index", Properties.Resources.nico_dat_ntsc);
                                    StreamWriter inputWriter = process.StandardInput;
                                    inputWriter.WriteLine("cd index");
                                    inputWriter.WriteLine("index_ntsc " + sotc_path.SelectedItem.ToString());
                                    inputWriter.WriteLine("exit");
                                    process.WaitForExit();
                                    MessageBox.Show("NTSC disc Found\nChanging complete!!", "Changed");
                                    MyGlobal.disc_version = "ntsc";
                                }
                            }
                        }
                    }

                        process.WaitForExit();

                        label_disc_type.ForeColor = Color.Green;
                        label_disc_type.Text = discVerName;

                        saveLastOpenInfo();
                        pathed = true;
                        path_state.ForeColor = Color.Green;
                        path_state.Text = "CHANGED!!!";
                        MessageBox.Show(discVerName + " disc Found\nChanging complete!!" + "\nPath =" + MyGlobal.nicoDat_path + "  \ndiscVer=" + MyGlobal.disc_version 
                            + "  hash[hex](sz=0x" + NICO_DAT_HASH_AREA_SZ.ToString("X") + ")=" + MyGlobal.nicoDat_hash.ToString("X") 
                            + "  size[hex]=" + MyGlobal.nicoDat_size.ToString("X")
                            , "Changed");
                    }
                    else
                    {
                        MessageBox.Show("Shadow of the Colossus data not found", "ERROR");

                    }
               }
               else
               {
                    if (MyGlobal.nicoDat_path != null) if(MyGlobal.nicoDat_path.Length > 0) sotc_path.Text = MyGlobal.nicoDat_path;
                    pathed = false;
               }

              
           }

        }

        private void nomad_link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (File.Exists("3D_Model.obj"))
            {
                Process.Start("explorer", "3D_Model.obj");
            }
            else
            {
                MessageBox.Show("No OBJ 3D file found!!","ERROR");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void miscout_Click(object sender, EventArgs e)
        {
            Misc CF = new Misc();
            CF.Visible = true;
        }

        private void wanderout_Click(object sender, EventArgs e)
        {
            Advanced CF = new Advanced();
            CF.Visible = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Number of resources listed for each version:\n |-----PAL-------|\n Models: 5241   \n Animation: 2642\n Textures: 1580 \n SKBs: 48       \n TOTAL: 9511    \n |-----NTSC------|\n Models: 5239   \n Animation: 2626\n Textures: 1399 \n SKBs: 48       \n TOTAL: 9312    \n |----PREVIEW----|\n Models: 6418   \n Animation: 2470\n Textures: 1383 \n SKBs: 48       \n TOTAL: 10319   \n |------OPM-----|\n Models: 834   \n Animation: 948\n Textures: 633 \n SKBs: 18      \n TOTAL: 2433   \n |------PSU-----|\n Models: 838   \n Animation: 944\n Textures: 639 \n SKBs: 18      \n TOTAL: 2438   \n \n Each game version has different quantity of resources listed due to the viewer\n offset index script compatibility be limited. To get a maximum of resources\n listed, use SotC PAL version. To get beta extra models, use PREVIEW version.\n", "GAME DISK VERSION STATS");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ALLMODELS h = new ALLMODELS();
            h.Close();
        }
        
        
    }
}
