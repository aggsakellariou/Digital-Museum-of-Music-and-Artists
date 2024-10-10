using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

public enum LightState
{
    None,
    Red,
    Green,
    Blue
}
public enum KaraokeState
{
    Off,
    On
}

namespace Digital_Museum_of_Music_and_Artists
{
    public partial class DJDeck : Form
    {
        //
        // initialization
        //
        private readonly UserRole currentUserRole;
        private readonly string username;
        private readonly UserTicket currentUserTicket;
        private readonly int money;        
        private DeckState currentState;
        private KaraokeState karaokeState;
        private PlaylistState playlistState;
        private LightState lightState;
        private DJState djState;
        private readonly Color defaultButtonColor = Color.FromArgb(192, 64, 0);
        private readonly List<string> playlistHiphop = new List<string>();
        private readonly List<string> playlistPop = new List<string>();
        private readonly List<string> playlistRock = new List<string>();
        private readonly List<string> playlistFast = new List<string>();
        private readonly List<string> playlistSlow = new List<string>();
        private readonly List<string> playlistAudience = new List<string>();
        private readonly SoundPlayer soundPlayer = new SoundPlayer();

        private readonly Dictionary<string, (string fast, string slow, string normal)> songMappingsSpeed = new Dictionary<string, (string fast, string slow, string normal)>
        {
            // Pop songs normal
            { "Pop1_Jack_Harlow_Lovin_on_Me.wav", ("Pop1_Jack_Harlow_Lovin_on_Me-1.5x.wav", "Pop1_Jack_Harlow_Lovin_on_Me-0.5x.wav", "Pop1_Jack_Harlow_Lovin_on_Me.wav") },
            { "Pop2_Dua_Lipa_Houdini.wav", ("Pop2_Dua_Lipa_Houdini-1.5x.wav", "Pop2_Dua_Lipa_Houdini-0.5x.wav", "Pop2_Dua_Lipa_Houdini.wav") },
            { "Pop3_Miley_Cyrus_Flowers.wav", ("Pop3_Miley_Cyrus_Flowers-1.5x.wav", "Pop3_Miley_Cyrus_Flowers-0.5x.wav", "Pop3_Miley_Cyrus_Flowers.wav") },
            // Pop songs fast
            { "Pop1_Jack_Harlow_Lovin_on_Me-1.5x.wav", ("Pop1_Jack_Harlow_Lovin_on_Me-1.5x.wav", "Pop1_Jack_Harlow_Lovin_on_Me-0.5x.wav", "Pop1_Jack_Harlow_Lovin_on_Me.wav") },
            { "Pop2_Dua_Lipa_Houdini-1.5x.wav", ("Pop2_Dua_Lipa_Houdini-1.5x.wav", "Pop2_Dua_Lipa_Houdini-0.5x.wav", "Pop2_Dua_Lipa_Houdini.wav") },
            { "Pop3_Miley_Cyrus_Flowers-1.5x.wav", ("Pop3_Miley_Cyrus_Flowers-1.5x.wav", "Pop3_Miley_Cyrus_Flowers-0.5x.wav", "Pop3_Miley_Cyrus_Flowers.wav") },
            // Pop songs slow
            { "Pop1_Jack_Harlow_Lovin_on_Me-0.5x.wav", ("Pop1_Jack_Harlow_Lovin_on_Me-1.5x.wav", "Pop1_Jack_Harlow_Lovin_on_Me-0.5x.wav", "Pop1_Jack_Harlow_Lovin_on_Me.wav") },
            { "Pop2_Dua_Lipa_Houdini-0.5x.wav", ("Pop2_Dua_Lipa_Houdini-1.5x.wav", "Pop2_Dua_Lipa_Houdini-0.5x.wav", "Pop2_Dua_Lipa_Houdini.wav") },
            { "Pop3_Miley_Cyrus_Flowers-0.5x.wav", ("Pop3_Miley_Cyrus_Flowers-1.5x.wav", "Pop3_Miley_Cyrus_Flowers-0.5x.wav", "Pop3_Miley_Cyrus_Flowers.wav") },
    
            // Rock songs normal
            { "Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine.wav", ("Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine-1.5x.wav", "Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine-0.5x.wav", "Rock1_Guns_N'_Roses-Sweet_Child_O'_Mine.wav") },
            { "Rock2_U2_One.wav", ("Rock2_U2_One-1.5x.wav", "Rock2_U2_One-0.5x.wav", "Rock2_U2_One.wav") },
            { "Rock3_The_Rolling_Stones-Miss_You.wav", ("Rock3_The_Rolling_Stones-Miss_You-1.5x.wav", "Rock3_The_Rolling_Stones-Miss_You-0.5x.wav", "Rock3_The_Rolling_Stones-Miss_You.wav") },
            // Rock songs fast
            { "Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine-1.5x.wav", ("Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine-1.5x.wav", "Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine-0.5x.wav", "Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine.wav") },
            { "Rock2_U2_One-1.5x.wav", ("Rock2_U2_One-1.5x.wav", "Rock2_U2_One-0.5x.wav" , "Rock2_U2_One.wav") },
            { "Rock3_The_Rolling_Stones_Miss_You-1.5x.wav", ("Rock3_The_Rolling_Stones-Miss_You-1.5x.wav", "Rock3_The_Rolling_Stones-Miss_You-0.5x.wav", "Rock3_The_Rolling_Stones-Miss_You.wav") },
            // Rock songs slow
            { "Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine-0.5x.wav", ("Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine-1.5x.wav", "Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine-0.5x.wav", "Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine.wav") },
            { "Rock2_U2_One-0.5x.wav", ("Rock2_U2_One-1.5x.wav", "Rock2_U2_One-0.5x.wav", "Rock2_U2_One.wav") },
            { "Rock3_The_Rolling_Stones_Miss_You-0.5x.wav", ("Rock3_The_Rolling_Stones_Miss_You-1.5x.wav", "Rock3_The_Rolling_Stones_Miss_You-0.5x.wav", "Rock3_The_Rolling_Stones-Miss_You.wav") },
    
            // Hip-hop songs normal
            { "HipHop1_California_Love.wav", ("HipHop1_California_Love-1.5x.wav", "HipHop1_California_Love-0.5x.wav", "HipHop1_California_Love.wav") },
            { "HipHop2_50_Cent_In_Da_Club.wav", ("HipHop2_50_Cent_In_Da_Club-1.5x.wav", "HipHop2_50_Cent_In_Da_Club-0.5x.wav", "HipHop2_50_Cent_In_Da_Club.wav") },
            { "HipHop3_Ice_Cube_It_Was_A_Good_Day.wav", ("HipHop3_Ice_Cube_It_Was_A_Good_Day-1.5x.wav", "HipHop3_Ice_Cube_It_Was_A_Good_Day-0.5x.wav", "HipHop3_Ice_Cube_It_Was_A_Good_Day.wav") },
            // Hip-hop songs fast
            { "HipHop1_California_Love-1.5x.wav", ("HipHop1_California_Love-1.5x.wav", "HipHop1_California_Love-0.5x.wav", "HipHop1_California_Love.wav") },
            { "HipHop2_50_Cent_In_Da_Club-1.5x.wav", ("HipHop2_50_Cent_In_Da_Club-1.5x.wav", "HipHop2_50_Cent_In_Da_Club-0.5x.wav", "HipHop2_50_Cent_In_Da_Club.wav") },
            { "HipHop3_Ice_Cube_It_Was_A_Good_Day-1.5x.wav", ("HipHop3_Ice_Cube_It_Was_A_Good_Day-1.5x.wav", "HipHop3_Ice_Cube_It_Was_A_Good_Day-0.5x.wav", "HipHop3_Ice_Cube_It_Was_A_Good_Day.wav") },
            // Hip-hop songs slow
            { "HipHop1_California_Love-0.5x.wav", ("HipHop1_California_Love-1.5x.wav", "HipHop1_California_Love-0.5x.wav", "HipHop1_California_Love.wav") },
            { "HipHop2_50_Cent_In_Da_Club-0.5x.wav", ("HipHop2_50_Cent_In_Da_Club-1.5x.wav", "HipHop2_50_Cent_In_Da_Club-0.5x.wav", "HipHop2_50_Cent_In_Da_Club.wav") },
            { "HipHop3_Ice_Cube_It_Was_A_Good_Day-0.5x.wav", ("HipHop3_Ice_Cube_It_Was_A_Good_Day-1.5x.wav", "HipHop3_Ice_Cube_It_Was_A_Good_Day-0.5x.wav", "HipHop3_Ice_Cube_It_Was_A_Good_Day.wav") }
        };
        private readonly Dictionary<string, (string heavy, string light, string normal)> songMappingsSpecial = new Dictionary<string, (string fast, string slow, string normal)>
        {
            // Pop songs normal
            { "Pop1_Jack_Harlow_Lovin_on_Me.wav", ("Pop1_Jack_Harlow_Lovin_on_Me_heavy.wav", "Pop1_Jack_Harlow_Lovin_on_Me_light.wav", "Pop1_Jack_Harlow_Lovin_on_Me.wav") },
            { "Pop2_Dua_Lipa_Houdini.wav", ("Pop2_Dua_Lipa_Houdini_heavy.wav", "Pop2_Dua_Lipa_Houdini_light.wav", "Pop2_Dua_Lipa_Houdini.wav") },
            { "Pop3_Miley_Cyrus_Flowers.wav", ("Pop3_Miley_Cyrus_Flowers_heavy.wav", "Pop3_Miley_Cyrus_Flowers_light.wav", "Pop3_Miley_Cyrus_Flowers.wav") },
            // Pop songs heavy
            { "Pop1_Jack_Harlow_Lovin_on_Me_heavy.wav", ("Pop1_Jack_Harlow_Lovin_on_Me_heavy.wav", "Pop1_Jack_Harlow_Lovin_on_Me_light.wav", "Pop1_Jack_Harlow_Lovin_on_Me.wav") },
            { "Pop2_Dua_Lipa_Houdini_heavy.wav", ("Pop2_Dua_Lipa_Houdini_heavy.wav", "Pop2_Dua_Lipa_Houdini_light.wav", "Pop2_Dua_Lipa_Houdini.wav") },
            { "Pop3_Miley_Cyrus_Flowers_heavy.wav", ("Pop3_Miley_Cyrus_Flowers_heavy.wav", "Pop3_Miley_Cyrus_Flowers_light.wav", "Pop3_Miley_Cyrus_Flowers.wav") },
            // Pop songs light
            { "Pop1_Jack_Harlow_Lovin_on_Me_light.wav", ("Pop1_Jack_Harlow_Lovin_on_Me_heavy.wav", "Pop1_Jack_Harlow_Lovin_on_Me_light.wav", "Pop1_Jack_Harlow_Lovin_on_Me.wav") },
            { "Pop2_Dua_Lipa_Houdini_light.wav", ("Pop2_Dua_Lipa_Houdini_heavy.wav", "Pop2_Dua_Lipa_Houdini_light.wav", "Pop2_Dua_Lipa_Houdini.wav") },
            { "Pop3_Miley_Cyrus_Flowers_light.wav", ("Pop3_Miley_Cyrus_Flowers_heavy.wav", "Pop3_Miley_Cyrus_Flowers_light.wav", "Pop3_Miley_Cyrus_Flowers.wav") },
    
            // Rock songs normal
            { "Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine.wav", ("Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine_heavy.wav", "Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine_light.wav", "Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine.wav") },
            { "Rock2_U2_One.wav", ("Rock2_U2_One_heavy.wav", "Rock2_U2_One_light.wav", "Rock2_U2_One.wav") },
            { "Rock3_The_Rolling_Stones_Miss_You.wav", ("Rock3_The_Rolling_Stones_Miss_You_heavy.wav", "Rock3_The_Rolling_Stones_Miss_You_light.wav", "Rock3_The_Rolling_Stones-Miss_You.wav") },
            // Rock songs heavy
            { "Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine_heavy.wav", ("Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine_heavy.wav", "Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine_light.wav", "Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine.wav") },
            { "Rock2_U2_One_heavy.wav", ("Rock2_U2_One_heavy.wav", "Rock2_U2_One_light.wav", "Rock2_U2_One.wav") },
            { "Rock3_The_Rolling_Stones_Miss_You_heavy.wav", ("Rock3_The_Rolling_Stones_Miss_You_heavy.wav", "Rock3_The_Rolling_Stones_Miss_You_light.wav", "Rock3_The_Rolling_Stones-Miss_You.wav") },
            // Rock songs light
            { "Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine_light.wav", ("Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine_heavy.wav", "Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine_light.wav", "Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine.wav") },
            { "Rock2_U2_One_light.wav", ("Rock2_U2_One_heavy.wav", "Rock2_U2_One_light.wav", "Rock2_U2_One.wav") },
            { "Rock3_The_Rolling_Stones_Miss_You_light.wav", ("Rock3_The_Rolling_Stones_Miss_You_heavy.wav", "Rock3_The_Rolling_Stones_Miss_You_light.wav", "Rock3_The_Rolling_Stones_Miss_You.wav") },
    
            // Hip-hop songs normal
            { "HipHop1_California_Love.wav", ("HipHop1_California_Love_heavy.wav", "HipHop1_California_Love_light.wav", "HipHop1_California_Love.wav") },
            { "HipHop2_50_Cent_In_Da_Club.wav", ("HipHop2_50_Cent_In_Da_Club_heavy.wav", "HipHop2_50_Cent_In_Da_Club_light.wav", "HipHop2_50_Cent_In_Da_Club.wav") },
            { "HipHop3_Ice_Cube_It_Was_A_Good_Day.wav", ("HipHop3_Ice_Cube_It_Was_A_Good_Day_heavy.wav", "HipHop3_Ice_Cube_It_Was_A_Good_Day_light.wav", "HipHop3_Ice_Cube_It_Was_A_Good_Day.wav") },
            // Hip-hop songs heavy
            { "HipHop1_California_Love_heavy.wav", ("HipHop1_California_Love_heavy.wav", "HipHop1_California_Love_light.wav", "HipHop1_California_Love.wav") },
            { "HipHop2_50_Cent_In_Da_Club_heavy.wav", ("HipHop2_50_Cent_In_Da_Club_heavy.wav", "HipHop2_50_Cent_In_Da_Club_light.wav", "HipHop2_50_Cent_In_Da_Club.wav") },
            { "HipHop3_Ice_Cube_It_Was_A_Good_Day_heavy.wav", ("HipHop3_Ice_Cube_It_Was_A_Good_Day_heavy.wav", "HipHop3_Ice_Cube_It_Was_A_Good_Day_light.wav", "HipHop3_Ice_Cube_It_Was_A_Good_Day.wav") },
            // Hip-hop songs light
            { "HipHop1_California_Love_light.wav", ("HipHop1_California_Love_heavy.wav", "HipHop1_California_Love_light.wav", "HipHop1_California_Love.wav") },
            { "HipHop2_50_Cent_In_Da_Club_light.wav", ("HipHop2_50_Cent_In_Da_Club_heavy.wav", "HipHop2_50_Cent_In_Da_Club_light.wav", "HipHop2_50_Cent_In_Da_Club.wav") },
            { "HipHop3_Ice_Cube_It_Was_A_Good_Day_light.wav", ("HipHop3_Ice_Cube_It_Was_A_Good_Day_heavy.wav", "HipHop3_Ice_Cube_It_Was_A_Good_Day_light.wav", "HipHop3_Ice_Cube_It_Was_A_Good_Day.wav") }
        };
        private enum DeckState
        {
            None,
            InputBPM,
            InputOrder,
            InputGerne,
            InputColor,
            InputSpecialEffects,
            InputRequestedSong,
            InputRating,
        }
        private enum PlaylistState
        {
            None,
            Pop,
            Rock,
            Hiphop,
            Fast,
            Slow,
            Audience
        }
        public DJDeck(UserRole role, string username, UserTicket currentUserTicket, int money)
        {
            InitializeComponent();
            this.username = username;
            this.currentUserRole = role;
            this.currentUserTicket = currentUserTicket;
            this.money = money;
            
            InitializeBox();
            HideInputs();
            InitializePlaylist();
            currentState = DeckState.None;
            playlistState = PlaylistState.None;

            LoadRoomState();

            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(PlayButton, "Play Song");
            toolTip.SetToolTip(ChangeBPMButton, "Change BPM");
            toolTip.SetToolTip(ChangeOrderButton, "Change Order");
            toolTip.SetToolTip(ChangeGenreButton, "Change Genre");
            toolTip.SetToolTip(ChangeColorButton, "Change Light Color");
            toolTip.SetToolTip(BackupButton, "Backup Settings");
            toolTip.SetToolTip(AudienceRequestButton, "Audience Request");
            toolTip.SetToolTip(KaraokeModeButton, "Toggle Karaoke Mode");
            toolTip.SetToolTip(KaraokeRatingButton, "Add Karaoke Rating");
            toolTip.SetToolTip(SpecialEffectButton, "Apply Special Effect");
            toolTip.SetToolTip(buttonHelp, "Help");
        }
        //
        // Initialize functions
        //
        private void InitializeBox()
        {
            comboBoxBPM.Text = "None";
            comboBoxBPM.Items.Add("50 BPM");
            comboBoxBPM.Items.Add("100 BPM");
            comboBoxBPM.Items.Add("150 BPM");

            comboBoxSpecialEffects.Text = "None";
            comboBoxSpecialEffects.Items.Add("Heavy");
            comboBoxSpecialEffects.Items.Add("Light");
            comboBoxSpecialEffects.Items.Add("Normal");

            comboBoxColor.Text = "None";
            comboBoxColor.Items.Add("Red");
            comboBoxColor.Items.Add("Green");
            comboBoxColor.Items.Add("Blue");
            comboBoxColor.Items.Add("None");

            comboBoxGerne.Text = "None";
            comboBoxGerne.Items.Add("Pop");
            comboBoxGerne.Items.Add("Rock");
            comboBoxGerne.Items.Add("Hiphop");

            comboBoxOrder.Text = "None";
            comboBoxOrder.Items.Add("Fast");
            comboBoxOrder.Items.Add("Slow");
            
            numericUpDownBPM.Text = "100";
            numericUpDownRating.Text = string.Empty;
        }
        private void HideInputs()
        {
            comboBoxBPM.Visible = false;
            comboBoxGerne.Visible = false;
            comboBoxColor.Visible = false;
            comboBoxSpecialEffects.Visible = false;
            comboBoxOrder.Visible = false;
            numericUpDownRating.Visible = false;
            numericUpDownBPM.Visible = false;
            comboBoxAudienceRequest.Visible = false;
            listBoxPlaylistPop.Visible = false;
            listBoxPlaylistRock.Visible = false;
            listBoxPlaylistHiphop.Visible = false;
            listBoxPlaylistFast.Visible = false;
            listBoxPlaylistSlow.Visible = false;
            listBoxPlaylistAudience.Visible = false;
        }
        //songs
        private void InitializePlaylist()
        {
            // Pop
            playlistPop.Add("Pop1_Jack_Harlow_Lovin_on_Me.wav");
            playlistPop.Add("Pop2_Dua_Lipa_Houdini.wav");
            playlistPop.Add("Pop3_Miley_Cyrus_Flowers.wav");
            // Rock
            playlistRock.Add("Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine.wav");
            playlistRock.Add("Rock2_U2_One.wav");
            playlistRock.Add("Rock3_The_Rolling_Stones_Miss_You.wav");
            // Hiohop
            playlistHiphop.Add("HipHop1_California_Love.wav");
            playlistHiphop.Add("HipHop2_50_Cent_In_Da_Club.wav");
            playlistHiphop.Add("HipHop3_Ice_Cube_It_Was_A_Good_Day.wav");
            // Fast
            playlistFast.Add("Pop1_Jack_Harlow_Lovin_on_Me.wav");
            playlistFast.Add("Rock3_The_Rolling_Stones_Miss_You.wav");
            playlistFast.Add("HipHop1_California_Love.wav");
            // Slow
            playlistSlow.Add("Pop3_Miley_Cyrus_Flowers.wav");
            playlistSlow.Add("Rock2_U2_One.wav");
            playlistSlow.Add("HipHop3_Ice_Cube_It_Was_A_Good_Day.wav");
            // Audience
            playlistAudience.Add("Pop1_Jack_Harlow_Lovin_on_Me.wav");
            playlistAudience.Add("Pop2_Dua_Lipa_Houdini.wav");
            playlistAudience.Add("Pop3_Miley_Cyrus_Flowers.wav");
            playlistAudience.Add("Rock1_Guns_N'_Roses_Sweet_Child_O'_Mine.wav");
            playlistAudience.Add("Rock2_U2_One.wav");
            playlistAudience.Add("Rock3_The_Rolling_Stones_Miss_You.wav");
            playlistAudience.Add("HipHop1_California_Love.wav");
            playlistAudience.Add("HipHop2_50_Cent_In_Da_Club.wav");
            playlistAudience.Add("HipHop3_Ice_Cube_It_Was_A_Good_Day.wav");
            UpdatePlaylist();
        }
        //
        // Methods
        //
        private void UpdatePlaylist()
        {
            listBoxPlaylistPop.Items.Clear();
            foreach (string song in playlistPop)
            {
                listBoxPlaylistPop.Items.Add(song);
            }

            listBoxPlaylistRock.Items.Clear();
            foreach (string song in playlistRock)
            {
                listBoxPlaylistRock.Items.Add(song);
            }

            listBoxPlaylistHiphop.Items.Clear();
            foreach (string song in playlistHiphop)
            {
                listBoxPlaylistHiphop.Items.Add(song);
            }

            listBoxPlaylistFast.Items.Clear();
            foreach (string song in playlistFast)
            {
                listBoxPlaylistFast.Items.Add(song);
            }

            listBoxPlaylistSlow.Items.Clear();
            foreach (string song in playlistSlow)
            {
                listBoxPlaylistSlow.Items.Add(song);
            }

            listBoxPlaylistAudience.Items.Clear();
            foreach (string song in playlistAudience)
            {
                listBoxPlaylistAudience.Items.Add(song);
            }
        }
        private void PlaySong(string song)
        {
            richTextBoxLog.AppendText($"Playing song: '{song}'\n");
        }
        private void ChangeOrder(string order)
        {
            richTextBoxLog.AppendText($"Changing playback order to {order}\n");
            if (order == "Fast")
            {
                playlistState = PlaylistState.Fast;
                listBoxPlaylistPop.Visible = false;
                listBoxPlaylistHiphop.Visible = false;
                listBoxPlaylistRock.Visible = false;
                listBoxPlaylistFast.Visible = true;
                listBoxPlaylistSlow.Visible = false;

            }
            else if (order == "Slow")
            {
                playlistState = PlaylistState.Slow;
                listBoxPlaylistRock.Visible = false;
                listBoxPlaylistPop.Visible = false;
                listBoxPlaylistHiphop.Visible = false;
                listBoxPlaylistFast.Visible = false;
                listBoxPlaylistSlow.Visible = true;
            }
            else
            {
                MessageBox.Show("Please enter a valid order value.");
            }
        }
        private void ChangeBPM(string BPM)
        {
            richTextBoxLog.AppendText($"Changing playback speed to {BPM}\n");
        }
        private void ChangeMusicGenre(string genre)
        {
            richTextBoxLog.AppendText($"Changing music genre to {genre}\n");
            if (genre == "Pop")
            {
                playlistState = PlaylistState.Pop;
                listBoxPlaylistPop.Visible = true;
                listBoxPlaylistHiphop.Visible = false;
                listBoxPlaylistRock.Visible = false;
                listBoxPlaylistFast.Visible = false;
                listBoxPlaylistSlow.Visible = false;
            }
            else if (genre == "Rock")
            {
                playlistState = PlaylistState.Rock;
                listBoxPlaylistRock.Visible = true;
                listBoxPlaylistHiphop.Visible = false;
                listBoxPlaylistPop.Visible = false;
                listBoxPlaylistFast.Visible = false;
                listBoxPlaylistSlow.Visible = false;
            }
            else if (genre == "Hiphop")
            {
                playlistState = PlaylistState.Hiphop;
                listBoxPlaylistHiphop.Visible = true;
                listBoxPlaylistPop.Visible = false;
                listBoxPlaylistRock.Visible = false;
                listBoxPlaylistFast.Visible = false;
                listBoxPlaylistSlow.Visible = false;
            }
            else
            {
                MessageBox.Show("Please enter a valid playlist.");
            }
        }
        private void ChangeLightColor(string color)
        {
            richTextBoxLog.AppendText($"Changing light color to {color}\n");
        }
        private void BackupSettings()
        {
            richTextBoxLog.AppendText("Creating backup of settings\n");
        }
        private void RespondToAudienceRequest(string songTitle)
        {
            richTextBoxLog.AppendText($"Playing audience-requested song: '{songTitle}'\n");
        }
        private void EnableKaraokeMode()
        {
            richTextBoxLog.AppendText("Enabling karaoke mode\n");
        }
        private void DisableKaraokeMode()
        {
            richTextBoxLog.AppendText("Disabling karaoke mode\n");
        }
        private void AddKaraokeRating(int rating)
        {
            richTextBoxLog.AppendText($"Karaoke rating added: {rating}\n");
        }
        private void ApplySpecialEffect(string effect)
        {
            richTextBoxLog.AppendText($"Applying special effect: {effect}\n");
        }
        private void Deselect()
        {
            richTextBoxLog.AppendText($"Applying deselect\n");
        }
        private void DisableButtonsForDeckState()
        {
            switch (currentState)
            {
                case DeckState.None:
                    SetControlProperties(PlayButton, true);
                    SetControlProperties(ChangeGenreButton, true);
                    SetControlProperties(ChangeBPMButton, true);
                    SetControlProperties(ChangeOrderButton, true);
                    SetControlProperties(ChangeColorButton, true);
                    SetControlProperties(AudienceRequestButton, true);
                    SetControlProperties(SpecialEffectButton, true);
                    SetControlProperties(KaraokeRatingButton, true);
                    SetControlProperties(BackupButton, true);
                    SetControlProperties(KaraokeModeButton, true);
                    break;
                case DeckState.InputBPM:
                    SetControlProperties(PlayButton, false);
                    SetControlProperties(ChangeGenreButton, false);
                    SetControlProperties(ChangeBPMButton, true);
                    SetControlProperties(ChangeOrderButton, false);
                    SetControlProperties(ChangeColorButton, false);
                    SetControlProperties(AudienceRequestButton, false);
                    SetControlProperties(SpecialEffectButton, false);
                    SetControlProperties(KaraokeRatingButton, false);
                    SetControlProperties(BackupButton, false);
                    SetControlProperties(KaraokeModeButton, false);
                    break;
                case DeckState.InputOrder:
                    SetControlProperties(PlayButton, false);
                    SetControlProperties(ChangeGenreButton, false);
                    SetControlProperties(ChangeBPMButton, false);
                    SetControlProperties(ChangeOrderButton, true);
                    SetControlProperties(ChangeColorButton, false);
                    SetControlProperties(AudienceRequestButton, false);
                    SetControlProperties(SpecialEffectButton, false);
                    SetControlProperties(KaraokeRatingButton, false);
                    SetControlProperties(BackupButton, false);
                    SetControlProperties(KaraokeModeButton, false);
                    break;
                case DeckState.InputGerne:
                    SetControlProperties(PlayButton, false);
                    SetControlProperties(ChangeGenreButton, true);
                    SetControlProperties(ChangeBPMButton, false);
                    SetControlProperties(ChangeOrderButton, false);
                    SetControlProperties(ChangeColorButton, false);
                    SetControlProperties(AudienceRequestButton, false);
                    SetControlProperties(SpecialEffectButton, false);
                    SetControlProperties(KaraokeRatingButton, false);
                    SetControlProperties(BackupButton, false);
                    SetControlProperties(KaraokeModeButton, false);
                    break;
                case DeckState.InputColor:
                    SetControlProperties(PlayButton, false);
                    SetControlProperties(ChangeGenreButton, false);
                    SetControlProperties(ChangeBPMButton, false);
                    SetControlProperties(ChangeOrderButton, false);
                    SetControlProperties(ChangeColorButton, true);
                    SetControlProperties(AudienceRequestButton, false);
                    SetControlProperties(SpecialEffectButton, false);
                    SetControlProperties(KaraokeRatingButton, false);
                    SetControlProperties(BackupButton, false);
                    SetControlProperties(KaraokeModeButton, false);
                    break;
                case DeckState.InputSpecialEffects:
                    SetControlProperties(PlayButton, false);
                    SetControlProperties(ChangeGenreButton, false);
                    SetControlProperties(ChangeBPMButton, false);
                    SetControlProperties(ChangeOrderButton, false);
                    SetControlProperties(ChangeColorButton, false);
                    SetControlProperties(AudienceRequestButton, false);
                    SetControlProperties(SpecialEffectButton, true);
                    SetControlProperties(KaraokeRatingButton, false);
                    SetControlProperties(BackupButton, false);
                    SetControlProperties(KaraokeModeButton, false);
                    break;
                case DeckState.InputRequestedSong:
                    SetControlProperties(PlayButton, true);
                    SetControlProperties(ChangeGenreButton, false);
                    SetControlProperties(ChangeBPMButton, false);
                    SetControlProperties(ChangeOrderButton, false);
                    SetControlProperties(ChangeColorButton, false);
                    SetControlProperties(AudienceRequestButton, false);
                    SetControlProperties(SpecialEffectButton, false);
                    SetControlProperties(KaraokeRatingButton, false);
                    SetControlProperties(BackupButton, false);
                    SetControlProperties(KaraokeModeButton, false);
                    break;
                case DeckState.InputRating:
                    SetControlProperties(PlayButton, false);
                    SetControlProperties(ChangeGenreButton, false);
                    SetControlProperties(ChangeBPMButton, false);
                    SetControlProperties(ChangeOrderButton, false);
                    SetControlProperties(ChangeColorButton, false);
                    SetControlProperties(AudienceRequestButton, false);
                    SetControlProperties(SpecialEffectButton, false);
                    SetControlProperties(KaraokeRatingButton, true);
                    SetControlProperties(BackupButton, false);
                    SetControlProperties(KaraokeModeButton, false);
                    break;
            }
        }
        private void SetControlProperties(Button button, bool enabled, Color? borderColor = null)
        {
            button.Enabled = enabled;

            if (enabled)
            {
                button.FlatAppearance.BorderColor = defaultButtonColor;
            }
            else
            {
                button.FlatAppearance.BorderColor = Color.Black;
            }
        }
        //
        // Buttons
        //
        private void ChangeSongButton_Click(object sender, EventArgs e)
        {
            if (currentState == DeckState.None)
            {
                if (playlistState == PlaylistState.Pop)
                {
                    if (listBoxPlaylistPop.SelectedIndex != -1)
                    {
                        string selectedSong = playlistPop[listBoxPlaylistPop.SelectedIndex];
                        soundPlayer.SoundLocation = selectedSong;
                        soundPlayer.Play();
                        PlaySong(selectedSong);
                    }
                }
                else if (playlistState == PlaylistState.Rock)
                {
                    if (listBoxPlaylistRock.SelectedIndex != -1)
                    {
                        string selectedSong = playlistRock[listBoxPlaylistRock.SelectedIndex];
                        soundPlayer.SoundLocation = selectedSong;
                        soundPlayer.Play();
                        PlaySong(selectedSong);
                    }
                }
                else if (playlistState == PlaylistState.Hiphop)
                {
                    if (listBoxPlaylistHiphop.SelectedIndex != -1)
                    {
                        string selectedSong = playlistHiphop[listBoxPlaylistHiphop.SelectedIndex];
                        soundPlayer.SoundLocation = selectedSong;
                        soundPlayer.Play();
                        PlaySong(selectedSong);
                    }
                }
                else if (playlistState == PlaylistState.Fast)
                {
                    if (listBoxPlaylistFast.SelectedIndex != -1)
                    {
                        string selectedSong = playlistFast[listBoxPlaylistFast.SelectedIndex];
                        soundPlayer.SoundLocation = selectedSong;
                        soundPlayer.Play();
                        PlaySong(selectedSong);
                    }
                }
                else if (playlistState == PlaylistState.Slow)
                {
                    if (listBoxPlaylistSlow.SelectedIndex != -1)
                    {
                        string selectedSong = playlistSlow[listBoxPlaylistSlow.SelectedIndex];
                        soundPlayer.SoundLocation = selectedSong;
                        soundPlayer.Play();
                        PlaySong(selectedSong);
                    }
                }
                else
                {
                    _ = MessageBox.Show("Please select a song from the playlist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (currentState == DeckState.InputRequestedSong)
            {
                if (listBoxPlaylistAudience.SelectedIndex != -1)
                {
                    string selectedSong = playlistAudience[listBoxPlaylistAudience.SelectedIndex];
                    soundPlayer.SoundLocation = selectedSong;
                    soundPlayer.Play();
                    PlaySong(selectedSong);
                    RespondToAudienceRequest(selectedSong);
                    // Reset
                    listBoxPlaylistAudience.Visible = false;
                    currentState = DeckState.None;
                    DisableButtonsForDeckState();
                }
                else
                {
                    _ = MessageBox.Show("Please select a song from the playlist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void BtnStop_Click(object sender, EventArgs e)
        {
            soundPlayer.Stop();
        }
        private void ChangeBPMButton_Click(object sender, EventArgs e)
        {
            if (currentState == DeckState.None)
            {
                ChangeBPMButton.FlatAppearance.BorderColor = Color.Black;
                comboBoxBPM.Visible = true;
                currentState = DeckState.InputBPM;
                DisableButtonsForDeckState();
            }
            else if (currentState == DeckState.InputBPM)
            {
                string orderInput = comboBoxBPM.Text;

                if (!string.IsNullOrWhiteSpace(orderInput))
                {
                    ChangeBPM(orderInput);

                    if (songMappingsSpeed.TryGetValue(soundPlayer.SoundLocation, out var songs))
                    {
                        string selectedSong;
                        switch (orderInput)
                        {
                            case "50 BPM":
                                selectedSong = songs.slow;
                                break;
                            case "100 BPM":
                                selectedSong = songs.normal;
                                break;
                            case "150 BPM":
                                selectedSong = songs.fast;
                                break;
                            default:
                                throw new NotImplementedException();
                        };
                        soundPlayer.SoundLocation = selectedSong;
                        soundPlayer.Play();
                        PlaySong(selectedSong);
                    }
                    else if (songMappingsSpecial.TryGetValue(soundPlayer.SoundLocation, out var specialSongs))
                    {
                        // For special songs, play the selected special version
                        string specialSong = specialSongs.normal;
                        if (songMappingsSpeed.TryGetValue(specialSong, out var mapping))
                        {
                            string selectedSongNormal;
                            switch (orderInput)
                            {
                                case "50 BPM":
                                    selectedSongNormal = mapping.slow;
                                    break;
                                case "100 BPM":
                                    selectedSongNormal = mapping.normal;
                                    break;
                                case "150 BPM":
                                    selectedSongNormal = mapping.fast;
                                    break;
                                default:
                                    throw new NotImplementedException();
                            };
                            soundPlayer.SoundLocation = selectedSongNormal;
                            soundPlayer.Play();
                            PlaySong(selectedSongNormal);
                        }
                    }
                    else
                    {
                        _ = MessageBox.Show("Τhis song is not available with this BPM value. Τry it in the simple version.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid BPM value.");
                }
                // Reset
                ChangeBPMButton.FlatAppearance.BorderColor = defaultButtonColor;
                currentState = DeckState.None;
                DisableButtonsForDeckState();
                comboBoxBPM.Visible = false;
            }
        }
        private (string heavy, string light, string normal) GetSpecialMapping(string songTitle)
        {
            if (songMappingsSpecial.TryGetValue(songTitle, out var mapping))
            {
                return mapping;
            }

            return default; // Return default values if song title is not found
        }
        private void ChangeOrderButton_Click(object sender, EventArgs e)
        {
            if (currentState == DeckState.None)
            {
                ChangeOrderButton.FlatAppearance.BorderColor = Color.Black;
                comboBoxOrder.Visible = true;
                currentState = DeckState.InputOrder;
                DisableButtonsForDeckState();
            }
            else if (currentState == DeckState.InputOrder)
            {
                string orderInput = comboBoxOrder.Text;

                if (!string.IsNullOrWhiteSpace(orderInput))
                {
                    ChangeOrder(orderInput);
                }
                else
                {
                    MessageBox.Show("Please enter a valid order value.");
                }
                // Reset
                ChangeOrderButton.FlatAppearance.BorderColor = defaultButtonColor;
                currentState = DeckState.None;
                DisableButtonsForDeckState();
                comboBoxOrder.Visible = false;
            }
        }
        private void ChangeGenreButton_Click(object sender, EventArgs e)
        {
            if (currentState == DeckState.None)
            {
                ChangeGenreButton.FlatAppearance.BorderColor = Color.Black;
                comboBoxGerne.Visible = true;
                currentState = DeckState.InputGerne;
                DisableButtonsForDeckState();
            }
            else if (currentState == DeckState.InputGerne)
            {
                string genreInput = comboBoxGerne.Text;

                if (!string.IsNullOrWhiteSpace(genreInput))
                {
                    ChangeMusicGenre(genreInput);
                }
                else
                {
                    MessageBox.Show("Please enter a valid music genre.");
                }
                // Reset
                ChangeGenreButton.FlatAppearance.BorderColor = defaultButtonColor;
                currentState = DeckState.None;
                DisableButtonsForDeckState();
                comboBoxGerne.Visible = false;
            }
        }
        private void ChangeColorButton_Click(object sender, EventArgs e)
        {
            if (currentState == DeckState.None)
            {
                ChangeColorButton.FlatAppearance.BorderColor = Color.Black;
                comboBoxColor.Visible = true;
                currentState = DeckState.InputColor;
                DisableButtonsForDeckState();
            }
            else if (currentState == DeckState.InputColor)
            {
                string colorInput = comboBoxColor.Text;

                if (!string.IsNullOrWhiteSpace(colorInput))
                {
                    lightState = (LightState)Enum.Parse(typeof(LightState), colorInput, true);
                    ChangeLightColor(colorInput);
                }
                else
                {
                    MessageBox.Show("Please enter a valid color.");
                }
                // Reset
                ChangeColorButton.FlatAppearance.BorderColor = defaultButtonColor;
                currentState = DeckState.None;
                DisableButtonsForDeckState();
                comboBoxColor.Visible = false;
            }
        }
        private void BackupButton_Click(object sender, EventArgs e)
        {
            if (currentState == DeckState.None)
            {
                BackupSettings();
            }
        }
        private void AudienceRequestButton_Click(object sender, EventArgs e)
        {
            if (currentState == DeckState.None)
            {
                listBoxPlaylistAudience.Visible = true;
                listBoxPlaylistAudience.BringToFront();
                currentState = DeckState.InputRequestedSong;
                DisableButtonsForDeckState();
            }
        }
        private void KaraokeModeButton_Click(object sender, EventArgs e)
        {
            if (karaokeState == KaraokeState.Off)
            {
                EnableKaraokeMode();
                karaokeState = KaraokeState.On;
            }
            else
            {
                DisableKaraokeMode();
                karaokeState = KaraokeState.Off;
            }
        }
        private void KaraokeRatingButton_Click(object sender, EventArgs e)
        {
            if (currentState == DeckState.None)
            {
                KaraokeRatingButton.FlatAppearance.BorderColor = Color.Black;
                numericUpDownRating.Visible = true;
                currentState = DeckState.InputRating;
                DisableButtonsForDeckState();
            }
            else if (currentState == DeckState.InputRating)
            {
                string ratingInput = numericUpDownRating.Text;

                if (!string.IsNullOrWhiteSpace(ratingInput) && int.TryParse(ratingInput, out int rating))
                {
                    AddKaraokeRating(rating);
                }
                else
                {
                    MessageBox.Show("Please enter a valid rating for karaoke.");
                }
                // Reset
                KaraokeRatingButton.FlatAppearance.BorderColor = defaultButtonColor;
                currentState = DeckState.None;
                DisableButtonsForDeckState();
                numericUpDownRating.Visible = false;
            }
        }
        private void SpecialEffectButton_Click(object sender, EventArgs e)
        {
            if (currentState == DeckState.None)
            {
                SpecialEffectButton.FlatAppearance.BorderColor = Color.Black;
                comboBoxSpecialEffects.Visible = true;
                currentState = DeckState.InputSpecialEffects;
                DisableButtonsForDeckState();
            }
            else if (currentState == DeckState.InputSpecialEffects)
            {
                string selectedEffect = comboBoxSpecialEffects.Text;

                if (!string.IsNullOrWhiteSpace(selectedEffect))
                {
                    ApplySpecialEffect(selectedEffect);

                    if (songMappingsSpecial.TryGetValue(soundPlayer.SoundLocation, out var songs))
                    {
                        string selectedSong;
                        switch (selectedEffect)
                        {
                            case "Heavy":
                                selectedSong = songs.heavy;
                                break;
                            case "Light":
                                selectedSong = songs.light;
                                break;
                            case "Normal":
                                selectedSong = songs.normal;
                                break;
                            default:
                                throw new NotImplementedException();
                        };
                        soundPlayer.SoundLocation = selectedSong;
                        soundPlayer.Play();
                        PlaySong(selectedSong);
                    }
                    else if (songMappingsSpeed.TryGetValue(soundPlayer.SoundLocation, out var specialSongs))
                    {
                        string specialSong = specialSongs.normal;
                        if (songMappingsSpecial.TryGetValue(specialSong, out var mapping))
                        {
                            string selectedSongNormal;
                            switch (selectedEffect)
                            {
                                case "Heavy":
                                    selectedSongNormal = mapping.heavy;
                                    break;
                                case "Light":
                                    selectedSongNormal = mapping.light;
                                    break;
                                case "Normal":
                                    selectedSongNormal = mapping.normal;
                                    break;
                                default:
                                    throw new NotImplementedException();
                            };
                            soundPlayer.SoundLocation = selectedSongNormal;
                            soundPlayer.Play();
                            PlaySong(selectedSongNormal);
                        }
                    }
                    else
                    {
                        _ = MessageBox.Show("Τhis song is not available with this effect. Τry it in the simple version.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid special effect value.");
                }
                // Reset
                SpecialEffectButton.FlatAppearance.BorderColor = defaultButtonColor;
                currentState = DeckState.None;
                DisableButtonsForDeckState();
                comboBoxSpecialEffects.Visible = false;
            }
        }
        private void ButtonDeselect_Click(object sender, EventArgs e)
        {
            Deselect();
            currentState = DeckState.None;
            HideInputs();
            DisableButtonsForDeckState();
            switch (playlistState)
            {
                case PlaylistState.Pop:
                    listBoxPlaylistPop.Visible = true;
                    break;
                case PlaylistState.Rock:
                    listBoxPlaylistRock.Visible = true;
                    break;
                case PlaylistState.Hiphop:
                    listBoxPlaylistHiphop.Visible = true;
                    break;
                case PlaylistState.Fast:
                    listBoxPlaylistFast.Visible = true;
                    break;
                case PlaylistState.Slow:
                    listBoxPlaylistSlow.Visible = true;
                    break;
            }
        }
        private void ButtonBack_Click(object sender, EventArgs e)
        {
            SaveRoomState();    
            DJ dj = new DJ(currentUserRole, username, currentUserTicket, money);
            dj.Show();
            this.Close();
        }
        private void ButtonHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "file://C:\\Users\\aggel\\Desktop\\On-Line help.chm");
        }
        //
        // Room State
        //
        private void SaveRoomState()
        {
            try
            {
                djState.KaraokeOn = karaokeState;
                djState.ColorLight = lightState;

                using (FileStream fs = new FileStream("roomStateDJ.dat", FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, djState);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving room state: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadRoomState()
        {
            try
            {
                if (File.Exists("roomStateDJ.dat"))
                {
                    using (FileStream fs = new FileStream("roomStateDJ.dat", FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        djState = (DJState)formatter.Deserialize(fs);
                    }
                }
                else
                {
                    djState = new DJState();
                }
                ApplyRoomState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading room state: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ApplyRoomState()
        {
            karaokeState = djState.KaraokeOn;
            lightState = djState.ColorLight;
            comboBoxColor.Text = Enum.GetName(typeof(LightState), lightState);
        }     
    }
}