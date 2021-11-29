using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace AnalizeText
{
    public partial class FormAnalizeText : Form
    {
        ExtremistTextVocabulary textVocabulary;
        ExtremistPhraseVocabulary phraseVocabulary;
        List<AvailabilityExtremistTextResult> textsResult;
        AvailabilityExtremistPhrasesResult phrasesResult;
        List<Color> extremistPhrasesColor;

        public FormAnalizeText()
        {
            InitializeComponent();
            textVocabulary = new ExtremistTextVocabulary();
            phraseVocabulary = new ExtremistPhraseVocabulary();
        }

        private void FormAnalizeText_Load(object sender, EventArgs e)
        {
            radioText.Checked = false;
            splitContainerBD.Panel1.Enabled = false;
            splitContainerBD.Panel2.Enabled = false;
            ToolTip toolTip1 = new ToolTip();
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(this.buttonCancell, "При нажатии позволяет отредактировать текст для дальнейшего анализа");
            toolTip1.SetToolTip(this.buttonClear, "При нажатии позволяет очистить поле ввода и полученный результат");
            ColorTextRichTextBox("Введите текст для добавления в базу данных", richTextBoxTextBD);
            ColorTextRichTextBox("Введите слово(фразу)", richTextBoxWord);
            comboBoxExtrimism.Text = "Политический";
        }

        private void radioText_CheckedChanged(object sender, EventArgs e)
        {
            if (radioText.Checked)
            {
                splitContainerBD.Panel1.Enabled = true;
                ColorTextRichTextBox("Введите слово(фразу)", richTextBoxWord);
                comboBoxExtrimism.SelectedIndex = 0;
                splitContainerBD.Panel2.Enabled = false;
                UpdateTableTextVocubulary();
            }
        }


        private void radioExpressions_CheckedChanged(object sender, EventArgs e)
        {
            if (radioExpressions.Checked)
            {
                splitContainerBD.Panel2.Enabled = true;
                ColorTextRichTextBox("Введите текст для добавления в базу данных", richTextBoxTextBD);
                splitContainerBD.Panel1.Enabled = false;
                UpdateTablePhraseVocabulary();
            }
        }


        private void AddColumnsDataGridView(int columnCount)
        {
            dataGridViewSourse.Columns.Clear();

            if (columnCount == 2)
            {               
                dataGridViewSourse.ColumnCount = columnCount;
                dataGridViewSourse.ColumnHeadersVisible = true;
                dataGridViewSourse.Columns[0].Name = "Номер";
                dataGridViewSourse.Columns[0].Width = 45;
                dataGridViewSourse.Columns[1].Name = "Комментарий";
                dataGridViewSourse.Columns[1].Width = 255;
            }
            else
            {
                dataGridViewSourse.ColumnCount = 3;
                dataGridViewSourse.ColumnHeadersVisible = true;
                dataGridViewSourse.Columns[0].Name = "Номер";
                dataGridViewSourse.Columns[0].Width = 45;
                dataGridViewSourse.Columns[1].Name = "Фраза (слово)";
                dataGridViewSourse.Columns[1].Width = 160;
                dataGridViewSourse.Columns[2].Name = "Тип экстремизма";
                dataGridViewSourse.Columns[2].Width = 95;
            }

        }

        /// <summary>
        /// Событие для обработки выбора одной из вкладок компонента tabControlProcesing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlProcessing_SelectedIndexChanged(object sender, EventArgs e)
        {
            string textString = "Ввод данных недоступен. Для дальнейшей работы выберите вкладки <<Базовая обработка>> или <<Дополнительная обработка>>";

            switch (tabControlProcessing.SelectedIndex)
            {
                case 0:

                    if(richTextBoxText.Text== textString)
                    {
                        radioText.Checked = false;
                        radioExpressions.Checked = false;
                        richTextBoxText.Text = "";
                        ColorTextRichTextBox("Введите текст для анализа.", richTextBoxText);
                        richTextBoxText.Enabled = true;
                        richTextBoxText.ReadOnly = false;
                        buttonCancell.Enabled = false;
                        buttonClear.Enabled = true;
                        buttonCheck.Enabled = true;
                        AddTextCheckBox.Checked = false;
                        AddPhraseCheckBox.Checked = false;
                        richTextBoxTextBD.Text = "";
                        richTextBoxWord.Text = "";
                        splitContainerBD.Panel1.Enabled = false;
                        splitContainerBD.Panel2.Enabled = false;
                        dataGridViewExtraResult.Rows.Clear();
                        dataGridViewBasicResult.Rows.Clear();
                        dataGridViewSourse.Columns.Clear();
                        dataGridViewSourse.Rows.Clear(); 
                    }
                    else
                    {
                        richTextBoxText.SelectAll();
                        richTextBoxText.SelectionColor = Color.Gray;
                        if (textsResult != null)
                        {
                            for (int resultIndex = 0; resultIndex < textsResult.Count; resultIndex++)
                            {
                                for (int phraseIndex = 0; phraseIndex < textsResult[resultIndex].usingWordsStartLocation.Count; phraseIndex++)
                                {
                                    richTextBoxText.SelectionStart = textsResult[resultIndex].usingWordsStartLocation[phraseIndex];
                                    richTextBoxText.SelectionLength = textsResult[resultIndex].usingWordsLastLocation[phraseIndex] -
                                        textsResult[resultIndex].usingWordsStartLocation[phraseIndex];
                                    richTextBoxText.SelectionColor = extremistPhrasesColor[resultIndex];
                                }
                            }
                        }
                    }
                        break;

                case 1:

                    if (richTextBoxText.Text == textString)
                    {
                        radioText.Checked = false;
                        radioExpressions.Checked = false;
                        richTextBoxText.Text = "";
                        ColorTextRichTextBox("Введите текст для анализа.", richTextBoxText);
                        richTextBoxText.Enabled = true;
                        buttonCancell.Enabled = false;
                        richTextBoxText.ReadOnly = false;
                        buttonClear.Enabled = true;
                        buttonCheck.Enabled = true;
                        AddTextCheckBox.Checked = false;
                        AddPhraseCheckBox.Checked = false;
                        richTextBoxTextBD.Text = "";
                        richTextBoxWord.Text = "";
                        splitContainerBD.Panel1.Enabled = false;
                        splitContainerBD.Panel2.Enabled = false;
                        dataGridViewExtraResult.Rows.Clear();
                        dataGridViewBasicResult.Rows.Clear();
                        dataGridViewSourse.Columns.Clear();
                        dataGridViewSourse.Rows.Clear();
                    }
                    else
                    {
                        richTextBoxText.SelectAll();
                        richTextBoxText.SelectionColor = Color.Gray;
                        if (phrasesResult != null)
                        {
                            for (int phraseIndex = 0; phraseIndex < phrasesResult.indexSpacesOfStartUsingPhrases.Count; phraseIndex++)
                            {
                                richTextBoxText.SelectionStart = phrasesResult.indexSpacesOfStartUsingPhrases[phraseIndex];
                                richTextBoxText.SelectionLength = phrasesResult.indexSpacesOfEndUsingPhrases[phraseIndex] -
                                    phrasesResult.indexSpacesOfStartUsingPhrases[phraseIndex];
                                richTextBoxText.SelectionColor = Color.Blue;
                            }
                        }
                        
                    }
                        break;

                case 2:
                        richTextBoxText.Text = "";
                        richTextBoxText.Enabled = false;
                        buttonCheck.Enabled = false;
                        buttonCancell.Enabled = false;
                        buttonClear.Enabled = false;
                        richTextBoxText.Text = textString;
                        dataGridViewExtraResult.Rows.Clear();
                        dataGridViewBasicResult.Rows.Clear();
                        break;
            }
        }

        private void richTextBoxText_Leave(object sender, EventArgs e)
        {
            
            if (richTextBoxText.Text =="")
            {
                string hint = "Введите текст для анализа.";
                ColorTextRichTextBox(hint, richTextBoxText);
            }
            
        }

        private void ColorTextRichTextBox(string text, RichTextBox richText)
        {
            richText.Text = text;
            richText.SelectAll();
            richText.SelectionColor = Color.Gray;
        }


        private void richTextBoxText_Enter(object sender, EventArgs e)
        {
            string text = "Введите текст для анализа.";
            if (richTextBoxText.Text == text)
            {
                richTextBoxText.Text = "";
                richTextBoxText.SelectionColor = Color.Black;
            }
            
        }

        private void InputCheckText(string str, object sender, KeyPressEventArgs e)
        {
            if (str == "Text")
            {
                string pattern = @"[A-Za-z]|[А-Яа-яёЁ]|[-(),.:;!?""\s\b]";

                if (!(Regex.Match(e.KeyChar.ToString(), pattern).Success))
                {
                    MessageBox.Show("Ошибка! Неверно введенный символ, попробуйте еще раз.", "Сообщение об ошибке");
                    e.Handled = true;
                }
            }
            else
            {
                string pattern = @"[a-z]|[а-яё]|[\s\b-(),.:;""]";

                if (!(Regex.Match(e.KeyChar.ToString(), pattern).Success))
                {
                    MessageBox.Show("Ошибка! Неверно введенный символ, попробуйте еще раз.", "Сообщение об ошибке");
                    e.Handled = true;
                }
            }
        }

        private void richTextBoxText_KeyPress(object sender, KeyPressEventArgs e)
        {
            InputCheckText("Text", sender, e);

        }


        private void richTextBoxWord_KeyPress(object sender, KeyPressEventArgs e)
        {
            InputCheckText("Word", sender, e);
            
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Очистить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClear_Click(object sender, EventArgs e)
        {
            richTextBoxText.Text = "Введите текст для анализа.";
            richTextBoxText.SelectionColor = Color.Black;
            richTextBoxText.ReadOnly = false;
            dataGridViewBasicResult.Rows.Clear();
            dataGridViewExtraResult.Rows.Clear();
            buttonCancell.Enabled = false;
            buttonCheck.Enabled = true;
            textsResult = null;
            phrasesResult = null;
        }

        private void richTextBoxTextBD_KeyPress(object sender, KeyPressEventArgs e)
        {
            InputCheckText("Text", sender, e);
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Отмена"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancell_Click(object sender, EventArgs e)
        {
            richTextBoxText.SelectAll();
            richTextBoxText.SelectionColor = Color.Black;
            richTextBoxText.ReadOnly = false;
            buttonCancell.Enabled = false;
            buttonCheck.Enabled = true;
            textsResult = null;
            phrasesResult = null;
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Проверить текст"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCheck_Click(object sender, EventArgs e)
        {
            dataGridViewBasicResult.Rows.Clear();
            dataGridViewExtraResult.Rows.Clear();
            string text = "Введите текст для анализа.";
            if (richTextBoxText.Text == text)
            {
                MessageBox.Show("Ошибка анализа! Введите в пустое поле текст и попробуйте еще раз.", "Сообщение об ошибке");
            }
            else
            {
                Text enterText = new Text(richTextBoxText.Text);
                richTextBoxText.Text = enterText.fullText;
                ExtremistAnalysisText analysis = new ExtremistAnalysisText(enterText);
                textsResult = analysis.AvailabilityExtremistText(textVocabulary);
                phrasesResult = analysis.AvailabilityExtremistPhrases(phraseVocabulary);
                extremistPhrasesColor = RandomColors(textsResult.Count);
                if (textsResult.Count > 0 || phrasesResult.indexSpacesOfStartUsingPhrases.Count > 0)
                {
                    buttonCancell.Enabled = true;
                    buttonCheck.Enabled = false;
                    if (textsResult.Count == 0)
                    {
                        MessageBox.Show("Совпадений с экстремистскими текстами из базы данных не найдено, но были найдены экстремистские фразы.", "Результат анализа");
                        tabControlProcessing.SelectedIndex = 1;
                    }
                    else if (phrasesResult.indexSpacesOfStartUsingPhrases.Count == 0)
                    {
                        MessageBox.Show("Совпадений с экстремистскими фразами из базы данных не найдено, но были найдены совпадения с экстремистскими текстами.", "Результат анализа");
                        tabControlProcessing.SelectedIndex = 0;
                    }
                    for (int indexResult = 0; indexResult < textsResult.Count; indexResult++)
                    {
                        dataGridViewBasicResult.Rows.Add();
                        dataGridViewBasicResult.Rows[indexResult].Cells[0].Value = 
                            textVocabulary.texts.IndexOf(textsResult[indexResult].usingExtremistText) + 1;
                        dataGridViewBasicResult.Rows[indexResult].Cells[1].Value = "Начало использованного текста - \"" + 
                            textsResult[indexResult].usingExtremistText.startWords + "...\", конец текста - \"..." + 
                            textsResult[indexResult].usingExtremistText.endWords + "\".";
                        dataGridViewBasicResult.Rows[indexResult].Cells[2].Value = Convert.ToString(textsResult[indexResult].usindProbability) + " %";
                        dataGridViewBasicResult.Rows[indexResult].Cells[3].Style.BackColor = extremistPhrasesColor[indexResult];
                    }
                    for (int indexExtrType = 0; indexExtrType < phrasesResult.probabilityUsingExtremistPhrases.Length; indexExtrType++)
                    {
                        dataGridViewExtraResult.Rows.Add();
                        dataGridViewExtraResult.Rows[indexExtrType].Cells[0].Value = phraseVocabulary.ExtremistTypes()[indexExtrType];
                        dataGridViewExtraResult.Rows[indexExtrType].Cells[1].Value = 
                            Convert.ToString(phrasesResult.probabilityUsingExtremistPhrases[indexExtrType]) + " %";
                    }
                    tabControlProcessing_SelectedIndexChanged(this, EventArgs.Empty);
                    richTextBoxText.ReadOnly = true;
                }
                else
                {
                    MessageBox.Show("Совпадений с экстремистскими текстами и фразами из базы данных не найдено.", "Результат анализа");
                }
            }
        }

        private void richTextBoxTextBD_Enter(object sender, EventArgs e)
        {
            string text = "Введите текст для добавления в базу данных";
            if (richTextBoxTextBD.Text == text)
            {
                richTextBoxTextBD.Text = "";
                richTextBoxTextBD.SelectionColor = Color.Black;
            }
        }

        private void richTextBoxTextBD_Leave(object sender, EventArgs e)
        {
            string hint = "Введите текст для добавления в базу данных";
            if (richTextBoxTextBD.Text == "")
            {
                ColorTextRichTextBox(hint, richTextBoxTextBD);
            }
            
        }

        private void richTextBoxWord_Enter(object sender, EventArgs e)
        {
            string text = "Введите слово(фразу)";
            if (richTextBoxWord.Text == text)
            {
                richTextBoxWord.Text = "";
                richTextBoxWord.SelectionColor = Color.Black;
            }
        }

        private void richTextBoxWord_Leave(object sender, EventArgs e)
        {
            string hint = "Введите слово(фразу)";
            if (richTextBoxWord.Text =="")
            {
                ColorTextRichTextBox(hint, richTextBoxWord);
            }
            
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Добавить" для добавления экстремисткого текста в базу данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAddTextBD_Click(object sender, EventArgs e)
        {
            string text = "Введите текст для добавления в базу данных";
            if (richTextBoxTextBD.Text==text)
            {
                MessageBox.Show("Ошибка добавления в базу данных! Введите в пустое поле текст и попробуйте еще раз.", "Сообщение об ошибке");  
            }
            else
            {
                Text enterText = new Text(richTextBoxTextBD.Text);
                if (enterText.TextWords().Length > 5)
                {
                    string startText = enterText.fullText.Substring(0, enterText.SpaceElementLocationTextIndex(5));
                    string lastText = enterText.fullText.Substring(
                        enterText.SpaceElementLocationTextIndex(enterText.TextWords().Length - 5) + 1,
                        enterText.fullText.Length - enterText.SpaceElementLocationTextIndex(enterText.TextWords().Length - 5) - 1);
                    ExtremistText inputText = new ExtremistText(startText, lastText, richTextBoxTextBD.Text);
                    if (textVocabulary.AddExtremistText(inputText))
                    {
                        UpdateTableTextVocubulary();
                        AddTextCheckBox.Checked = false;
                        MessageBox.Show("Текст успешно добавлен в базу данных.", "Сообщение об добавлении текста");
                    }
                    else
                    {
                        MessageBox.Show("Текст уже существует в базе данных.", "Сообщение об добавлении текста");
                    }
                }
                else
                {
                    MessageBox.Show("Введите не менее 5 слов,чтобы добавить текст в базу данных", "Сообщение об добавлении текста");
                }
               
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Добавить" для добавления экстремисткого выражения в базу данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAddWordBD_Click(object sender, EventArgs e)
        {
            string text = "Введите слово(фразу)";
            if (richTextBoxWord.Text == text)
            {
                MessageBox.Show("Ошибка добавления в базу данных! Введите в пустое поле фразу и попробуйте еще раз.", "Сообщение об ошибке");
            }
            else
            {
                ExtremistPhrase phrase = new ExtremistPhrase(richTextBoxWord.Text, 
                    phraseVocabulary.ExtremistTypes()[comboBoxExtrimism.SelectedIndex]);
                if (phraseVocabulary.AddExtremistPhrase(phrase))
                {
                    UpdateTablePhraseVocabulary();
                    AddPhraseCheckBox.Checked = false;
                    MessageBox.Show("Фраза успешно добавлена в базу данных.", "Сообщение об добавлении фразы");
                }
                else
                {
                    MessageBox.Show("Фраза уже существует в базе данных.", "Сообщение об добавлении фразы");
                }
            }
        }

        private List<Color> RandomColors(int countColor)
        {
            List<Color> randomColors = new List<Color>();
            Random colorRandom = new Random((int)DateTime.Now.Ticks);
            for (int indexColor = 0; indexColor < countColor; indexColor++)
            {
                randomColors.Add(Color.FromArgb(colorRandom.Next(10, 245), colorRandom.Next(10, 245), colorRandom.Next(10, 245)));
            }
            return randomColors;
        }

        private void dataGridViewBasicResult_MouseDown(object sender, MouseEventArgs e)
        {
            ((DataGridView)sender).CurrentCell = null;
        }

        private void dataGridViewBasicResult_SelectionChanged(object sender, EventArgs e)
        {
            if (MouseButtons != MouseButtons.None)
            {
                ((DataGridView)sender).CurrentCell = null;
            }
        }

        private void AddTextCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (AddTextCheckBox.Checked)
            {
                buttonAddTextBD.Enabled = true;
                buttonUpdateTextDB.Enabled = false;
                buttonDeleteTextDB.Enabled = false;
                richTextBoxTextBD.Text = "";
                ColorTextRichTextBox("Введите текст для добавления в базу данных", richTextBoxTextBD);
            }
            else
            {
                buttonAddTextBD.Enabled = false;
                buttonUpdateTextDB.Enabled = true;
                buttonDeleteTextDB.Enabled = true;
                richTextBoxTextBD.Text = textVocabulary.texts[dataGridViewSourse.CurrentRow.Index].fullText;
            }
        }

        private void AddPhraseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (AddPhraseCheckBox.Checked)
            {
                buttonAddWordBD.Enabled = true;
                buttonUpdateWordDB.Enabled = false;
                buttonDeleteWordDB.Enabled = false;
                richTextBoxWord.Text = "";
                ColorTextRichTextBox("Введите слово(фразу)", richTextBoxWord);
                comboBoxExtrimism.SelectedIndex = 0;
            }
            else
            {
                buttonAddWordBD.Enabled = false;
                buttonUpdateWordDB.Enabled = true;
                buttonDeleteWordDB.Enabled = true;
                SelectRowPhraseVocabulary();
            }
        }

        private void dataGridViewSourse_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (radioText.Checked)
            {
                if (!AddTextCheckBox.Checked)
                {
                    richTextBoxTextBD.Text = textVocabulary.texts[dataGridViewSourse.CurrentRow.Index].fullText;
                }
            }
            else if (radioExpressions.Checked)
            {
                if (!AddPhraseCheckBox.Checked)
                {
                    SelectRowPhraseVocabulary();
                }
            }
        }

        private void SelectRowPhraseVocabulary()
        {
            richTextBoxWord.Text = phraseVocabulary.phrases[dataGridViewSourse.CurrentRow.Index].phrase;
            comboBoxExtrimism.SelectedIndex =
                phraseVocabulary.ExtremistTypes().IndexOf(phraseVocabulary.phrases[dataGridViewSourse.CurrentRow.Index].typePhrase);
        }

        private void UpdateTableTextVocubulary()
        {
            AddColumnsDataGridView(2);
            for (int textIndex = 0; textIndex < textVocabulary.texts.Count; textIndex++)
            {
                dataGridViewSourse.Rows.Add();
                dataGridViewSourse.Rows[textIndex].Cells[0].Value = textIndex + 1;
                dataGridViewSourse.Rows[textIndex].Cells[1].Value = "Начало текста - \"" +
                        textVocabulary.texts[textIndex].startWords + "...\", конец текста - \"..." +
                        textVocabulary.texts[textIndex].endWords + "\".";
            }
            richTextBoxTextBD.Text = textVocabulary.texts[0].fullText;
        }

        private void UpdateTablePhraseVocabulary()
        {
            AddColumnsDataGridView(3);
            for (int phraseIndex = 0; phraseIndex < phraseVocabulary.phrases.Count; phraseIndex++)
            {
                dataGridViewSourse.Rows.Add();
                dataGridViewSourse.Rows[phraseIndex].Cells[0].Value = phraseIndex + 1;
                dataGridViewSourse.Rows[phraseIndex].Cells[1].Value = phraseVocabulary.phrases[phraseIndex].phrase;
                dataGridViewSourse.Rows[phraseIndex].Cells[2].Value = phraseVocabulary.phrases[phraseIndex].typePhrase;
            }
            richTextBoxWord.Text = phraseVocabulary.phrases[0].phrase;
            comboBoxExtrimism.SelectedIndex =
                phraseVocabulary.ExtremistTypes().IndexOf(phraseVocabulary.phrases[0].typePhrase);
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Изменить" для изменения экстремисткого выражения в базе данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUpdateWordDB_Click(object sender, EventArgs e)
        {
            if (richTextBoxWord.Text == "")
            {
                MessageBox.Show("Ошибка изменения записи в базе данных! Введите в пустое поле фразу и попробуйте еще раз.", "Сообщение об ошибке");
            }
            else
            {
                ExtremistPhrase enterPhrase = new ExtremistPhrase(richTextBoxWord.Text,
                    phraseVocabulary.ExtremistTypes()[comboBoxExtrimism.SelectedIndex]);
                ExtremistPhrase updatePhrase = phraseVocabulary.phrases[dataGridViewSourse.CurrentRow.Index];
                if (phraseVocabulary.UpdateExtremistPhrase(enterPhrase, updatePhrase))
                {
                    UpdateTablePhraseVocabulary();
                    MessageBox.Show("Фраза успешно изменена в базе данных.", "Сообщение об изменении фразы");
                }
                else
                {
                    MessageBox.Show("Невозможно изменить фразу.", "Сообщение об изменении фразы");
                }
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Удалить" для удаления экстремисткого выражения из базы данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDeleteWordDB_Click(object sender, EventArgs e)
        {
            ExtremistPhrase phrase = phraseVocabulary.phrases[dataGridViewSourse.CurrentRow.Index];
            if (phraseVocabulary.DeleteExtremistPhrase(phrase))
            {
                UpdateTablePhraseVocabulary();
                MessageBox.Show("Фраза успешно удалена из базы данных.", "Сообщение об удалении фразы");
            }
            else
            {
                MessageBox.Show("Невозможно удалить фразу.", "Сообщение об удалении фразы");
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Изменить" для изменения экстремисткого текста в базе данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUpdateTextDB_Click(object sender, EventArgs e)
        {
            if (richTextBoxTextBD.Text == "")
            {
                MessageBox.Show("Ошибка изменения записи в базе данных! Введите в пустое поле текст и попробуйте еще раз.", "Сообщение об ошибке");
            }
            else
            {
                Text text = new Text(richTextBoxTextBD.Text);
                if (text.TextWords().Length > 5)
                {
                    string startText = text.fullText.Substring(0, text.SpaceElementLocationTextIndex(5));
                    string lastText = text.fullText.Substring(
                        text.SpaceElementLocationTextIndex(text.TextWords().Length - 5) + 1,
                        text.fullText.Length - text.SpaceElementLocationTextIndex(text.TextWords().Length - 5) - 1);
                    ExtremistText enterText = new ExtremistText(startText, lastText, richTextBoxTextBD.Text);
                    ExtremistText updateText = textVocabulary.texts[dataGridViewSourse.CurrentRow.Index];
                    if (textVocabulary.UpdateExtremistText(enterText, updateText))
                    {
                        UpdateTableTextVocubulary();
                        MessageBox.Show("Текст успешно изменен в базе данных.", "Сообщение об изменении текста");
                    }
                    else
                    {
                        MessageBox.Show("Невозможно изменить текст.", "Сообщение об изменении текста");
                    }
                }
                else
                {
                    MessageBox.Show("Введите не менее 5 слов,чтобы изменить текст в базе данных", "Сообщение об изменении текста");
                }
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Удалить" для удаления экстремисткого текста из базы данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDeleteTextDB_Click(object sender, EventArgs e)
        {
            ExtremistText text = textVocabulary.texts[dataGridViewSourse.CurrentRow.Index];
            if (textVocabulary.DeleteExtremistText(text))
            {
                UpdateTableTextVocubulary();
                MessageBox.Show("Текст успешно удален из базы данных.", "Сообщение об удалении текста");
            }
            else
            {
                MessageBox.Show("Невозможно удалить текст.", "Сообщение об удалении текста");
            }
        }
    }
}
