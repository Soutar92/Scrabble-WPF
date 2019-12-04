using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;


namespace Scrabble
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {   
        
        static List<LeaderBoard> PlayerScores = new List<LeaderBoard>();
        public MainWindow()
        {
            InitializeComponent();
            Initialise();
        }

        private void Initialise()
        {
            using(var db = new Player_ScoresEntities1())
            {
                PlayerScores = db.LeaderBoards.ToList();
                LeaderBoard.ItemsSource = PlayerScores;
            }
        }


        private void Start_button_Click(object sender, RoutedEventArgs e)
        {
            if (Player1_Textbox.Text == "" || Player2_Textbox.Text == "")
            {
                MessageBox.Show("Please Enter Player Names");
            }
            else if(Player1_Textbox.Text.Length > 10 || Player2_Textbox.Text.Length > 10)
            {
                MessageBox.Show("Names can't be longer than 10 characters");
            }
            
            else
            {
                string p1Name = Player1_Textbox.Text;
                string p2Name = Player2_Textbox.Text;

                int p1Score = 0;
                int p2Score = 0;

                using (var db = new Player_ScoresEntities1())
                {
                    LeaderBoard newPlayer = new LeaderBoard();
                    newPlayer.Player_Name = p1Name;
                    newPlayer.Score = p1Score;
                    db.LeaderBoards.Add(newPlayer);

                    LeaderBoard newPlayer2 = new LeaderBoard();
                    newPlayer2.Player_Name = p2Name;
                    newPlayer2.Score = p2Score;
                    db.LeaderBoards.Add(newPlayer2);
                    db.SaveChanges();

                    Page gameWindow = new GameWindow();
                    this.Content = gameWindow;
            }
            

            }
            
        }
    }
}
