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

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        int СчетВопросов; // Счет вопросов
        int ПравилОтветов; // Количество правильных ответов
        int НеПравилОтветов; // Количество не правильных ответов
        int a, m, s;
        String[] НеПравилОтветы;
        StreamReader Читатель;

        int НомерПравОтвета; // Номер правильного ответа
        int ВыбранОтвет; // Номер ответа, выбранный студентом
        string path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));

        void НачалоТеста()
        {
            System.Text.Encoding Кодировка = System.Text.Encoding.GetEncoding(1251);
            try
            {
                Читатель = new StreamReader(Directory.GetCurrentDirectory() + @"\test1.txt", Кодировка);

                this.Text = Читатель.ReadLine(); // Название предмета
                                                 // Обнуление всех счетчиков :
                СчетВопросов = 0;
                ПравилОтветов = 0;
                НеПравилОтветов = 0;
                НеПравилОтветы = new String[100];
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                radioButton4.Checked = false;
                ЧитатьСледВопрос();

            }
            catch (Exception Ситуация)
            { // Отчет о всех ошибках
                MessageBox.Show(Ситуация.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (label7.Visible)
            {
                if (s < 59)
                {
                    s++;
                    if (s < 10) label6.Text = "0" + s.ToString();
                    else label6.Text = s.ToString();
                }
                else
                {
                    if (m < 59)
                    {
                        m++;
                        if (m < 10) label5.Text = "0" + m.ToString();
                        else label5.Text = m.ToString();
                        s = 0;
                        label6.Text = "00";
                    }
                    else
                    {
                        m = 0;
                        label5.Text = "00";
                    }
                }
                label7.Visible = false;
            }
            else
                label7.Visible = true;

        }

        private void button9_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text == "") || (textBox2.Text == ""))
            {
                MessageBox.Show("Введено пустое значение!!! Повторите ввод", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                label2.Visible = false;
                label1.Visible = false;

                button11.Visible = true;

                button10.Visible = true;
                button9.Visible = false;
                label16.Visible = true;
                label17.Visible = false;
                label5.Visible = true;
                label6.Visible = true;
                label7.Visible = true;

                label19.Visible = false;

                radioButton1.Visible = true;
                radioButton2.Visible = true;
                radioButton3.Visible = true;
                radioButton4.Visible = true;

                textBox1.Visible = false;
                textBox2.Visible = false;

                button10.Text = "Следующий вопрос";


                // Подписка на событие "изменение состояния
                // переключателей RadioButton:"
                radioButton1.CheckedChanged += new EventHandler(ИзмСостПерекл);
                radioButton2.CheckedChanged += new EventHandler(ИзмСостПерекл);
                radioButton3.CheckedChanged += new EventHandler(ИзмСостПерекл);
                radioButton4.CheckedChanged += new EventHandler(ИзмСостПерекл);

                НачалоТеста();
                label5.Text = "00";
                label6.Text = "00";
                timer1.Enabled = true;

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            // Щелчок на кнопке
            // "Следующий вопрос/Завершить/Начать тестирование снач"
            // Счет правильных ответов:
            if (ВыбранОтвет == НомерПравОтвета)
                ПравилОтветов = ПравилОтветов + 1;
            if (ВыбранОтвет != НомерПравОтвета)
            { // Счет неправильных ответов:
                НеПравилОтветов = НеПравилОтветов + 1;
                // Запоминаем вопросы с неправильными ответами:
                НеПравилОтветы[НеПравилОтветов] = label16.Text;
            }
            if (button10.Text == "Cначала")
            {
                label5.Visible = true; label7.Visible = true;
                label6.Visible = true;

                m = 0;
                s = 0;
                label5.Text = "00";
                label6.Text = "00";
                timer1.Enabled = true;

                button10.Text = "Следующий вопрос";
                // Переключатели становятся видимыми, доступными для выбора:
                radioButton1.Visible = true;
                radioButton2.Visible = true;
                radioButton3.Visible = true;
                radioButton4.Visible = true;

                // Переход к началу файла:
                НачалоТеста(); return;
            }
            if (button10.Text == "Завершить")
            { // Закрываем текстовый файл:
                Читатель.Close();
                // Переключатели делаем невидимыми:
                radioButton1.Visible = false;
                radioButton2.Visible = false;
                radioButton3.Visible = false;
                radioButton4.Visible = false;
                timer1.Enabled = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                // Формируем оценку за тест:
                string a;
                a = String.Format("Правильных ответов: {0} из {1}.\n" +
                    "Оценка в пятибальной системе: {2:F2}.", ПравилОтветов,
                    СчетВопросов, (ПравилОтветов * 5.0F) / СчетВопросов);

                label16.Text = String.Format("Тестирование завершено.\n" +
                    "Правильных ответов: {0} из {1}.\n" +
                    "Оценка в пятибальной системе: {2:F2}.", ПравилОтветов,
                    СчетВопросов, (ПравилОтветов * 5.0F) / СчетВопросов);
                // 5F - это максимальная оценка
                button10.Text = "Cначала";
                // Вывод вопросов, на которые вы дали неправильный ответ
                String Str = "СПИСОК ВОПРОСОВ, НА КОТОРЫЕ ВЫ ДАЛИ " +
                    "НЕПРАВИЛЬНЫЙ ОТВЕТ:\n\n";
                for (int i = 1; i <= НеПравилОтветов; i++)
                    Str = Str + НеПравилОтветы[i] + "\n";
                // Если есть неправильные ответы, то вывести через
                // MessageBox список соответствующих вопросов:
                if (НеПравилОтветов != 0)
                    MessageBox.Show(Str, "Тестирование завершено");

                StreamWriter sw = new StreamWriter(path + "\\Test.txt", true, System.Text.Encoding.Default);
                //Write a line of text
                sw.WriteLine("Тест 1" + "\n" + "Фамилия: " + textBox1.Text + "\n" + "Группа: " + textBox2.Text + "\n" + a + "\n" + "Дата и время прохождения теста: " + DateTime.Now + "\n\n");
                sw.Close();
            }
            if (button10.Text == "Следующий вопрос") ЧитатьСледВопрос();

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        void ИзмСостПерекл(System.Object sender, System.EventArgs e)
        { // Кнопка "Следующий вопрос" становится активной и ей передаем фокус:
            button10.Enabled = true; button10.Focus();
            RadioButton Переключатель = (RadioButton)sender;
            String tmp = Переключатель.Name;
            // Выясняем номер ответа, выбранный студентом:
            ВыбранОтвет = int.Parse(tmp.Substring(11));

        }



        void ЧитатьСледВопрос()
        {
            label16.Text = Читатель.ReadLine();
            // Считывание вариантов ответа:
            radioButton1.Text = Читатель.ReadLine();
            radioButton2.Text = Читатель.ReadLine();
            radioButton3.Text = Читатель.ReadLine();
            radioButton4.Text = Читатель.ReadLine();

            // Выясняем, какой ответ - правильный:
            НомерПравОтвета = int.Parse(Читатель.ReadLine());
            // Переводим все переключатели в состояние "выключено":
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;

            // Первую кнопку задаем не активной, пока студент не выберет
            // вариант ответа:
            button10.Enabled = false;
            СчетВопросов = СчетВопросов + 1;
            // Проверка, конец ли файла:
            if (Читатель.EndOfStream == true) button10.Text = "Завершить";
        }


    }
}
