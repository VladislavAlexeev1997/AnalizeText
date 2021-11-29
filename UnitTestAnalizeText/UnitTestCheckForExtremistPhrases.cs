using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnalizeText;

namespace UnitTestAnalizeText
{
    /// <summary>
    /// Класс для проверки ввода на наличие экстремистских фраз и выражений
    /// </summary>
    [TestClass]
    public class UnitTestCheckForExtremistPhrases
    {
        /// <summary>
        /// Проверка на ввод пустого текста
        /// </summary>
        [TestMethod]
        public void TestMethodEmptyInput()
        {
            ExtremistPhraseVocabulary vocabulary = new ExtremistPhraseVocabulary();
            Text textAnalize = new Text("");
            ExtremistAnalysisText analysisText = new ExtremistAnalysisText(textAnalize);
            AvailabilityExtremistPhrasesResult testResult = new AvailabilityExtremistPhrasesResult(new double[] {0, 0, 0, 0},
                new List<int>(), new List<int>());
            AvailabilityExtremistPhrasesResult test = analysisText.AvailabilityExtremistPhrases(vocabulary);
            Assert.AreEqual(testResult, test);
        }
        /// <summary>
        /// Проверка на ввод нейтрального текста, не содержащего экстремисткие фразы и выражения
        /// </summary>
        [TestMethod]
        public void TestMethodNeutralText()
        {
            ExtremistPhraseVocabulary vocabulary = new ExtremistPhraseVocabulary();
            Text textAnalize = new Text("Человек и природа неразделимы. Мы неразрывно связаны с окружающим нас животным и растительным миром и в немалой степени зависим от него.");
            ExtremistAnalysisText analysisText = new ExtremistAnalysisText(textAnalize);
            AvailabilityExtremistPhrasesResult testResult = new AvailabilityExtremistPhrasesResult(new double[] { 0, 0, 0, 0 },
                new List<int>(), new List<int>());
            AvailabilityExtremistPhrasesResult test = analysisText.AvailabilityExtremistPhrases(vocabulary);
            Assert.AreEqual(testResult, test);
        }
        /// <summary>
        /// Проверка на ввод политического характера текста
        /// </summary>
        [TestMethod]
        public void TestMethodPoliticalText()
        {
            ExtremistPhraseVocabulary vocabulary = new ExtremistPhraseVocabulary();
            Text textAnalize = new Text("Либеральная пропаганда – это мутировавшая логико-риторическая субстанция, " +
                "образовавшаеся в результате слияния социал-демократических ценностей с практикой фашизма. Но, мы стоим перед фактом: наша власть завела страну в тупик.");
            ExtremistAnalysisText analysisText = new ExtremistAnalysisText(textAnalize);
            AvailabilityExtremistPhrasesResult testResult = new AvailabilityExtremistPhrasesResult(new double[] {100, 0, 0, 0},
                new List<int>() {0, 190}, new List<int>() {22, 202});
            AvailabilityExtremistPhrasesResult test = analysisText.AvailabilityExtremistPhrases(vocabulary);
            Assert.AreEqual(testResult, test);
        }
        /// <summary>
        /// Проверка на ввод национального характера текста
        /// </summary>
        [TestMethod]
        public void TestMethodNationalText()
        {
            ExtremistPhraseVocabulary vocabulary = new ExtremistPhraseVocabulary();
            Text textAnalize = new Text("Лично меня очень обижает, когда меня называют 'хохол'. Будучи русским в четвертом поколении, я слово 'хохол', сказанное в свой адрес, воспринимаю как оскорбление.");
            ExtremistAnalysisText analysisText = new ExtremistAnalysisText(textAnalize);
            AvailabilityExtremistPhrasesResult testResult = new AvailabilityExtremistPhrasesResult(new double[] {0, 0, 100, 0},
                new List<int>() {45, 100}, new List<int>() {54, 109});
            AvailabilityExtremistPhrasesResult test = analysisText.AvailabilityExtremistPhrases(vocabulary);
            Assert.AreEqual(testResult, test);
        }

        /// <summary>
        /// Проверка на ввод социального характера текста
        /// </summary>
        [TestMethod]
        public void TestMethodSocialText()
        {
            ExtremistPhraseVocabulary vocabulary = new ExtremistPhraseVocabulary();
            Text textAnalize = new Text("'Их даже можно приручить и кормить с руки', - говорит потенциальный крокодиловый магнат." +
                " В еде кайманы тоже не особо притязательны - для нормальной полноценной жизни им хватает одного кормления в неделю.");
            ExtremistAnalysisText analysisText = new ExtremistAnalysisText(textAnalize);
            AvailabilityExtremistPhrasesResult testResult = new AvailabilityExtremistPhrasesResult(new double[] {0, 0, 0, 100},
                new List<int>() {67}, new List<int>() {88});
            AvailabilityExtremistPhrasesResult test = analysisText.AvailabilityExtremistPhrases(vocabulary);
            Assert.AreEqual(testResult, test);
        }

        /// <summary>
        /// Проверка на ввод регилиозного и социального характера текста
        /// </summary>
        [TestMethod]
        public void TestMethodReligiousAndNationalText()
        {
            ExtremistPhraseVocabulary vocabulary = new ExtremistPhraseVocabulary();
            Text textAnalize = new Text("По словам бывшего раввина Израиля, пропитанная семитизмом религия ... Наша чаша терпения переполнена, несмотря на то, что она чрезвычайно глубока.");
            ExtremistAnalysisText analysisText = new ExtremistAnalysisText(textAnalize);
            AvailabilityExtremistPhrasesResult testResult = new AvailabilityExtremistPhrasesResult(new double[] {0, 50, 50, 0},
                new List<int>() {34, 46}, new List<int>() {65, 57});
            AvailabilityExtremistPhrasesResult test = analysisText.AvailabilityExtremistPhrases(vocabulary);
            Assert.AreEqual(testResult, test);
        }

        /// <summary>
        /// Проверка на ввод текста. содержащего все виды экстремизма
        /// </summary>
        [TestMethod]
        public void TestMethodAllTypeExtremistText()
        {
            ExtremistPhraseVocabulary vocabulary = new ExtremistPhraseVocabulary();
            Text textAnalize = new Text("Наша власть завела страну в тупик..." +
                "По словам бывшего раввина Израиля, пропитанная семитизмом религия ..." +
                "...крокодиловый магнат.");
            ExtremistAnalysisText analysisText = new ExtremistAnalysisText(textAnalize);
            AvailabilityExtremistPhrasesResult testResult = new AvailabilityExtremistPhrasesResult(new double[] {25, 25, 25, 25},
                new List<int>() {0, 70, 82, 101}, new List<int>() {11, 101, 93, 128});
            AvailabilityExtremistPhrasesResult test = analysisText.AvailabilityExtremistPhrases(vocabulary);
            Assert.AreEqual(testResult, test);
        }
    }
}
