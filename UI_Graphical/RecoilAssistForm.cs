using RecoilAssist.DatabaseModule;
using RecoilAssist.Patterns;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace RecoilAssist.UI_Graphical
{
    /// <summary>
    /// Formatka do zarz¹dzania kontrolerem RecoilAssist
    /// </summary>
    public partial class RecoilAssistForm : Form
    {
        private RecoilAssistController recoilAssist;

        public IDatabaseProvider DatabaseProvider { get; private set; }

        /// <summary>
        /// Lista wzorców, s³u¿¹ca do przypiêcia DataSource do DataGridView
        /// </summary>
        public BindingList<MovementPattern> PatternsList { get; private set; }

        /// <summary>
        /// true - jeœli program nas³uchuje zdarzenia i koryguje ruchy myszki, false - w.p.p.
        /// </summary>
        public bool IsActive { get; set; }

        public RecoilAssistForm()
        {
            DatabaseProvider = new DatabaseProvider();
            //DatabaseProvider = new MemoryDatabaseProvider();

            recoilAssist = new RecoilAssistController();
            recoilAssist.AttachedPatterns.AddRange(DatabaseProvider.Patterns);
            recoilAssist.SetActive(false);

            InitializeComponent();

            // Dodaj do ComboBox typy funkcji
            foreach (FunctionType fType in Enum.GetValues(typeof(FunctionType)))
            {
                PatternTypeComboBox.Items.Add(fType);
            }

            // Podepnij listê do DataGridView i dodaj Event Handler'y
            PatternsList = new BindingList<MovementPattern>(DatabaseProvider.Patterns);
            Patterns.DataSource = PatternsList;
            PatternsList.ListChanged += PatternsList_ListChanged;
            PatternGridView.RowsAdded += PatternGridView_RowsAdded;
        }

        #region Zdarzenia obs³uguj¹ce maniuplacjê danych dotycz¹cych wzorców
        // Ta metoda wywo³ywana jest przy dodawaniu nowych wzorców do tabelki
        private void PatternGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < e.RowCount; i++)
            {
                var row = PatternGridView.Rows[e.RowIndex + i];
                var pattern = row.DataBoundItem as MovementPattern;

                // dopnij obrazek ¿eby ³adnie siê prezentowa³o
                row.Cells["Function"].Value = pattern.Type.Icon();
            }
        }

        // Ta metoda wywo³ywana jest przy zmianach na danych w DataGridView (u¿ytkownik zmodyfikowa³ tabelkê)
        private void PatternsList_ListChanged(object sender, ListChangedEventArgs e)
        {
            // Podepnij wzorce z tabelki do kontrolera
            recoilAssist.AttachedPatterns.Clear();
            recoilAssist.AttachedPatterns.AddRange(PatternsList);
        }
        #endregion

        #region Zdarzenia obs³uguj¹ce kliniêcia w kontrolki
        private void AddButton_Click(object sender, EventArgs e)
        {
            if (PatternTypeComboBox.SelectedItem != null)
            {
                var type = (FunctionType)PatternTypeComboBox.SelectedItem;
                PatternsList.Add(MovementPattern.Create(type, new double[] { 0, 0, 0 }, MovementDirection.Vertical, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(0)));
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (PatternGridView.SelectedRows.Count > 0)
            {
                var rowsToDelete = new List<DataGridViewRow>();
                foreach (DataGridViewRow row in PatternGridView.SelectedRows)
                    rowsToDelete.Add(row);

                foreach (var row in rowsToDelete)
                    PatternGridView.Rows.Remove(row);
            }
        }

        private void ActiveCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            recoilAssist.SetActive(ActiveCheckbox.Checked);
            RefreshControls();
        }
        #endregion

        /// <summary>
        /// Ustawia mo¿liwoœæ edycji kontrolek (w zale¿noœci czy kontroller "nas³uchuje")
        /// </summary>
        private void RefreshControls()
        {
            PatternGridView.Enabled = !ActiveCheckbox.Checked;
            PatternTypeComboBox.Enabled = !ActiveCheckbox.Checked;
            AddButton.Enabled = !ActiveCheckbox.Checked;
            RemoveButton.Enabled = !ActiveCheckbox.Checked;
        }
        
        // Ta metoda wywo³ywana jest przy zamykaniu okna
        private void RecoilAssistForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Zapisz zmiany
            DatabaseProvider.CommitChanges();
            // Zatrzymaj recoilAssist'a
            recoilAssist.Dispose();
        }
    }
}
