using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnalizeText;

namespace UnitTestAnalizeText
{
    /// <summary>
    /// Класс для проверки ввода на наличие текста из списка
    /// </summary>
    [TestClass]
    public class UnitTestCheckForExtremistText
    {
        /// <summary>
        /// Проверка на ввод пустого текста
        /// </summary>
        [TestMethod]
        public void TestMethodEmptyInput()
        {
            ExtremistTextVocabulary vocabulary = new ExtremistTextVocabulary();
            Text textAnalize = new Text("");
            ExtremistAnalysisText analysisText = new ExtremistAnalysisText(textAnalize);
            List<AvailabilityExtremistTextResult> listresult = new List<AvailabilityExtremistTextResult>();
            List<AvailabilityExtremistTextResult> list = analysisText.AvailabilityExtremistText(vocabulary);
            CollectionAssert.AreEqual(listresult, list);

        }
        /// <summary>
        /// Проверка на ввод нейтрального текста, не содержащего экстремисткие тексты из списка
        /// </summary>
        [TestMethod]
        public void TestMethodNeutralText()
        {
            ExtremistTextVocabulary vocabulary = new ExtremistTextVocabulary();
            Text textAnalize = new Text("Верность, доверие, любовь, уважение, поддержка – основные понятия," +
                " из которых складывается большинство вечных человеческих ценностей." +
                " Но даже в таком, казалось бы, достаточно неоспоримом вопросе," +
                " в зависимости от условий и сложившихся обстоятельств могут возникнуть определенные противоречия.");
            ExtremistAnalysisText analysisText = new ExtremistAnalysisText(textAnalize);
            List<AvailabilityExtremistTextResult> listresult = new List<AvailabilityExtremistTextResult>();          
            List<AvailabilityExtremistTextResult> list = analysisText.AvailabilityExtremistText(vocabulary);
            CollectionAssert.AreEqual(listresult, list);
        }
        /// <summary>
        /// Проверка на ввод текста, полностью совпадающего с текстом из списка
        /// </summary>
        [TestMethod]
        public void TestMethodUseOneExtremisstTextCompleteCoincidence()
        {
            ExtremistTextVocabulary vocabulary = new ExtremistTextVocabulary();
            Text textAnalize = new Text("Девид Лейн – известен за бессмертных 14 слов. Одно из них он выделил пропитанную семитизмом религию, противоречащая законам природы");
            ExtremistAnalysisText analysisText = new ExtremistAnalysisText(textAnalize);
            List<AvailabilityExtremistTextResult> listresult = new List<AvailabilityExtremistTextResult>();
            listresult.Add(new AvailabilityExtremistTextResult(100, vocabulary.texts[2], new List<int>() {0}, new List<int>() {131}));
            List<AvailabilityExtremistTextResult> list = analysisText.AvailabilityExtremistText(vocabulary);
            CollectionAssert.AreEqual(listresult, list);
        }

        /// <summary>
        /// Проверка на ввод текста, частично совпадающего с текстом из списка
        /// </summary>
        [TestMethod]
        public void TestMethodUseOneExtremisstTextIncompleteMatch()
        {
            ExtremistTextVocabulary vocabulary = new ExtremistTextVocabulary();
            Text textAnalize = new Text("Данная машина либеральной пропаганды, которая прикидывается нашей властью, ненавидит нас и препятствует строить будущее наших родных и близких!!!");
            ExtremistAnalysisText analysisText = new ExtremistAnalysisText(textAnalize);
            List<AvailabilityExtremistTextResult> listresult = new List<AvailabilityExtremistTextResult>();
            listresult.Add(new AvailabilityExtremistTextResult(73.33, vocabulary.texts[1], new List<int>() {6, 45, 111}, new List<int>() {37, 74, 145}));
            List<AvailabilityExtremistTextResult> list = analysisText.AvailabilityExtremistText(vocabulary);
            CollectionAssert.AreEqual(listresult, list);
        }

        /// <summary>
        /// Проверка на ввод текста, частично содержащего 2 текста из списка
        /// </summary>
        [TestMethod]
        public void TestMethodUseTwoExtremisstText()
        {
            ExtremistTextVocabulary vocabulary = new ExtremistTextVocabulary();
            Text textAnalize = new Text("Известный американский ученый Девид Лейн, который высказал бессмертные 14 слов, " +
                "про одно из них считал, что пропитанная семитизмом религия, которая противоречит законам природы естественного отбора, "+
                "одурманивает мозг человека. Данная машина либеральной пропаганды, прикидывающаяся нашей властью и религией, "+
                "мешает строить наше и наших родных будущее.");
            ExtremistAnalysisText analysisText = new ExtremistAnalysisText(textAnalize);
            List<AvailabilityExtremistTextResult> listresult = new List<AvailabilityExtremistTextResult>();
            listresult.Add(new AvailabilityExtremistTextResult(60, vocabulary.texts[1], new List<int>() {233, 306}, new List<int>() {294, 326}));
            listresult.Add(new AvailabilityExtremistTextResult(66.67, vocabulary.texts[2], new List<int>() {58, 83, 107, 147}, new List<int>() {79, 95, 139, 176}));
            List<AvailabilityExtremistTextResult> list = analysisText.AvailabilityExtremistText(vocabulary);
            CollectionAssert.AreEqual(listresult, list);
        }
        
    }
}
