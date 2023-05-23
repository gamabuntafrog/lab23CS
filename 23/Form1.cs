using System.Text;
using System.Windows.Forms;

namespace filestream
{
    public partial class Form1 : Form
    {
        private List<double> numbers;
        
        public Form1()
        {
            InitializeComponent();
            numbers = new List<double>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int positiveCount = numbers.Count(x => x > 0);
            int negativeCount = numbers.Count(x => x < 0);

            if (positiveCount > negativeCount)
            {
                label1.Text = "Більше додатніх елементів";
            }
            else if (positiveCount < negativeCount)
            {
                label1.Text = "Більше від'ємних елементів";
            }
            else
            {
                label1.Text = "Рівна кількість додатних та від'ємних елементів";
            }
        }

        private void відкритиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|Binary Files (*.bin)|*.bin";
            openFileDialog.Title = "Виберіть текстовий або бінарний файл";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                try
                {
                    numbers = LoadNumbersFromFile(filePath);
                    UpdateDataGridView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при зчитуванні файлу: {ex.Message}");
                }
            }
        }
        private List<double> LoadNumbersFromFile(string filePath)
        {
            List<double> numbers = new List<double>();

            if (Path.GetExtension(filePath) == ".txt")
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    if (double.TryParse(line, out double number))
                    {
                        numbers.Add(number);
                    }
                }
            }
            else
            {
                using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                {
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        double number = reader.ReadDouble();
                        numbers.Add(number);
                    }
                }
            }

            return numbers;
        }
        private void UpdateDataGridView()
        {
            dataGridView1.Rows.Clear();

            for (int i = 0; i < numbers.Count; i++)
            {
                dataGridView1.Rows.Add(numbers[i]);
            }
        }

        private void зберегтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|Binary Files (*.bin)|*.bin";
            saveFileDialog.Title = "Зберегти у текстовий або бінарний файл";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    SaveNumbersToFile(filePath);
                    MessageBox.Show("Дані успішно збережено у файлі.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при збереженні файлу: {ex.Message}");
                }
            }
        }
        private void SaveNumbersToFile(string filePath)
        {
            if (Path.GetExtension(filePath) == ".txt")
            {
                using StreamWriter writer = new StreamWriter(filePath);
                foreach (double number in numbers)
                {
                    writer.WriteLine(number);
                }
            } else
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs, Encoding.Default))
                    {
                        foreach (double number in numbers)
                        {
                            bw.Write(number);
                        }
                    }
                }
            }
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = 1;
        }
    }
}
