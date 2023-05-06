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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    

    public partial class MainWindow : Window
    {
        private Levels levels;
        
        public MainWindow()
        {
            InitializeComponent();
            levels = new Levels();
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            levels.LevelsSwitch("default");
            CreateLayout();
        }


        

        private void CreateLayout()
        {
            double gridWidth = gameArea.ActualWidth;
            double gridHeight = gameArea.ActualHeight;


            gameArea.Children.Clear();
            gameArea.RowDefinitions.Clear();
            gameArea.ColumnDefinitions.Clear();
            gameArea.IsEnabled = true;

            int gridRow = levels.Rows;
            int gridColumn = levels.Columns;
            double buttonWidth = gridWidth / gridColumn;
            double buttonHeight = gridHeight / gridRow;

            for (int i = 0; i < gridRow; i++)
            {
                gameArea.RowDefinitions.Add(new RowDefinition());
                
            }

            for (int i = 0; i < gridColumn; i++)
            {
                gameArea.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < gridRow * gridColumn; i++)
            {
                Button rect = new Button()
                {
                    Width = buttonWidth,
                    Height = buttonHeight,
                    Content = "",
                    Background = Brushes.Gray
                };
                // which column x row 
                int row = i / gridColumn; // 
                int col = i % gridColumn;// 
                gameArea.Children.Add(rect);
                Grid.SetRow(rect, row);
                Grid.SetColumn(rect, col);
            }
            // all buttons event
            EventManager.RegisterClassHandler(typeof(Button), Button.ClickEvent, new RoutedEventHandler(Button_Click));


            List<(int, int)> mines = Randomgen.RandomBombs(levels.Rows, levels.Columns, levels.Mines);
            foreach (Button button in gameArea.Children)
            {
                int row = Grid.GetRow(button);
                int col = Grid.GetColumn(button);
                if (mines.Contains((row, col)))
                {
                    button.Tag = "mine";
                    
                }
            }


        } 



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (gameArea.Children.Contains(button) && button.Tag == null)
            {
                NearbyMines(button);

            }

            else if (gameArea.Children.Contains(button) && (string)button.Tag == "mine") {

                AssignMine(button);
                RevealAllMines();
                EndGame();


            }

        }

        public void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ComboBoxItem selectedItem = (ComboBoxItem)e.AddedItems[0];
            // when ComboBox is clicked adds it to list
            string selectedLevel = selectedItem.Name;

            levels.LevelsSwitch(selectedLevel);
            CreateLayout();


        }
        private void NearbyMines(Button button)
        {   //finding where button is in map
            int row = Grid.GetRow(button);
            int col = Grid.GetColumn(button);

            // if button is already clicked 
            // deal with here 
            if (!button.IsEnabled)
            {
                return;
            }
            button.IsEnabled = false;
            button.Background = Brushes.LightGray;


            int count = 0;
           

            // returns larger value row above or 0
            //returns row below or last row 
            for (int i = Math.Max(row - 1, 0);
                i <= Math.Min(row + 1, levels.Rows - 1); i++)
            {   // 3 iteration made from row above to below
                for (int j = Math.Max(col - 1, 0);  // 3 iteration across
                    j <= Math.Min(col + 1, levels.Columns - 1); j++)
                {   // checks column , column before or same

                    Button nextButton = GetButton(i, j);

                    if ((string)nextButton.Tag == "mine")
                    {
                        count++;
                    }


                }

            }
            if (count > 0)
            {
                button.Content = (count > 0) ? count.ToString() : "";
            }
            else
            {
                button.Content = "";
            }
            if (count == 0)
            {
                for (int i = Math.Max(row - 1, 0); i <= Math.Min(row + 1, levels.Rows - 1); i++)
                {
                    for (int j = Math.Max(col - 1, 0); j <= Math.Min(col + 1, levels.Columns - 1); j++)
                    {       
                        if (i == row && j == col)
                        {
                            continue;
                        }
                        Button neighborButton = GetButton(i, j);
                                    // grabs next button r-1 c-1
                                    //inside this loop checks all 6 gives them numbers
                                    // recursively 
                        NearbyMines(neighborButton);
                    }
                }

            
           }
            if (CheckWin())
            {
                MessageBox.Show("Congratulations, you won!");
                gameArea.IsEnabled = false;
            }
        }



            private Button GetButton(int row, int col)
             {
            foreach (Button button in gameArea.Children)
            {
            if (Grid.GetRow(button) == row && Grid.GetColumn(button) == col)
            {
                return button;
            }
            }

            return null;


            }

          

        private void EndGame()
        {
        
        MessageBox.Show("Sorry but you lost");
            gameArea.IsEnabled = false;
        }


        private void ResetButton(object sender, RoutedEventArgs e)
        {
            gameArea.IsEnabled = true;
            CreateLayout();

            }

        // method to implement picture into it.
        private void AssignMine(Button button)
        {
            string full; //  provide path for image to use as mine
            ImageBrush mineImage = new ImageBrush();
            mineImage.ImageSource = new BitmapImage(new Uri(full, UriKind.Relative));
            button.Background = mineImage;
        }

        private void RevealAllMines()
        {
            foreach (Button button in gameArea.Children)
            {
                if ((string)button.Tag == "mine")
                {
                    AssignMine(button);
                }
            }
        }

        private bool CheckWin()
        {
            foreach (Button button in gameArea.Children)
            {
                if ((string)button.Tag != "mine" && button.IsEnabled)
                {
                    return false;
                }
            }
            return true;
        }

    }

}








  


