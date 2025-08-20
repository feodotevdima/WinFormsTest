using Application.Interfaces;
using Core;
using Persistence.Dtos;

namespace WinFormsTest
{
    public partial class Form1 : Form
    {
        private Panel filtersPanel;
        private Panel employeesPanel;
        private Label lblStatusFilter;
        private ComboBox cmbStatusFilter;
        private Label lblDepartmentFilter;
        private ComboBox cmbDepartmentFilter;
        private Label lblPostFilter;
        private ComboBox cmbPostFilter;
        private Label lblLastNameFilter;
        private TextBox txtLastNameFilter;
        private Button btnApplyFilter;
        private Button btnClearFilters;
        private DataGridView dataGridView;
        private Panel statisticsPanel;
        private Label lblStatStatus;
        private ComboBox cmbStatStatus;
        private Label lblStartDate;
        private DateTimePicker dtpStartDate;
        private Label lblEndDate;
        private DateTimePicker dtpEndDate;
        private RadioButton rbEmployed;
        private RadioButton rbFired;
        private Button btnShowStatistics;
        private DataGridView statisticsDataGridView;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblConnectionStatus;
        private ToolStripStatusLabel lblRecordsCount;

        private readonly IEmployeeRepository _repository;
        private List<Person> _persons;

        public Form1(IEmployeeRepository repository)
        {
            _repository = repository;
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Система управления сотрудниками";
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new Size(1200, 700);
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Font = new Font("Segoe UI", 9);

            CreateMainLayout();
            LoadReferenceDataAsync();
        }

        private void CreateMainLayout()
        {
            var mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                Padding = new Padding(5),
                BackColor = Color.Transparent
            };

            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            mainTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            filtersPanel = CreateFiltersPanel();
            employeesPanel = CreateEmployeesPanel();
            statisticsPanel = CreateStatisticsPanel();
            statusStrip = CreateStatusStrip();

            mainTable.Controls.Add(filtersPanel, 0, 0);
            mainTable.SetColumnSpan(filtersPanel, 2);

            mainTable.Controls.Add(employeesPanel, 0, 1);
            mainTable.Controls.Add(statisticsPanel, 1, 1);

            mainTable.Controls.Add(statusStrip, 0, 2);
            mainTable.SetColumnSpan(statusStrip, 2);

            this.Controls.Add(mainTable);
        }

        private Panel CreateFiltersPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 110,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            var flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoSize = true
            };

            var lblTitle = new Label
            {
                Text = "Фильтры сотрудников:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 8, 15, 0),
                ForeColor = Color.DarkSlateBlue
            };

            lblStatusFilter = new Label
            {
                Text = "Статус:",
                AutoSize = true,
                Margin = new Padding(0, 8, 3, 0),
                TextAlign = ContentAlignment.MiddleRight
            };

            cmbStatusFilter = new ComboBox
            {
                Width = 140,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 5, 15, 0)
            };

            lblDepartmentFilter = new Label
            {
                Text = "Отдел:",
                AutoSize = true,
                Margin = new Padding(0, 8, 3, 0),
                TextAlign = ContentAlignment.MiddleRight
            };

            cmbDepartmentFilter = new ComboBox
            {
                Width = 140,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 5, 15, 0)
            };

            lblPostFilter = new Label
            {
                Text = "Должность:",
                AutoSize = true,
                Margin = new Padding(0, 8, 3, 0),
                TextAlign = ContentAlignment.MiddleRight
            };

            cmbPostFilter = new ComboBox
            {
                Width = 140,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 5, 15, 0)
            };

            lblLastNameFilter = new Label
            {
                Text = "Фамилия:",
                AutoSize = true,
                Margin = new Padding(0, 8, 3, 0),
                TextAlign = ContentAlignment.MiddleRight
            };

            txtLastNameFilter = new TextBox
            {
                Width = 120,
                Margin = new Padding(0, 5, 15, 0)
            };

            btnApplyFilter = new Button
            {
                Text = "Применить",
                Width = 100,
                Height = 28,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Margin = new Padding(0, 5, 5, 0)
            };

            btnClearFilters = new Button
            {
                Text = "Очистить",
                Width = 100,
                Height = 28,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.LightGray,
                Font = new Font("Segoe UI", 9),
                Margin = new Padding(0, 5, 0, 0)
            };

            btnApplyFilter.Click += async (s, e) => await LoadEmployeesAsync();
            btnClearFilters.Click += ClearFilters;

            flowPanel.Controls.Add(lblTitle);
            flowPanel.Controls.Add(lblStatusFilter);
            flowPanel.Controls.Add(cmbStatusFilter);
            flowPanel.Controls.Add(lblDepartmentFilter);
            flowPanel.Controls.Add(cmbDepartmentFilter);
            flowPanel.Controls.Add(lblPostFilter);
            flowPanel.Controls.Add(cmbPostFilter);
            flowPanel.Controls.Add(lblLastNameFilter);
            flowPanel.Controls.Add(txtLastNameFilter);
            flowPanel.Controls.Add(btnApplyFilter);
            flowPanel.Controls.Add(btnClearFilters);

            panel.Controls.Add(flowPanel);
            return panel;
        }

        private Panel CreateEmployeesPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5),
                BackColor = Color.White
            };

            var lblTitle = new Label
            {
                Text = "Список сотрудников",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 25,
                ForeColor = Color.DarkSlateBlue,
                TextAlign = ContentAlignment.MiddleLeft
            };

            dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                Font = new Font("Segoe UI", 9),
                RowHeadersVisible = false,
                Margin = new Padding(0)
            };

            dataGridView.ColumnHeadersVisible = true;
            dataGridView.ColumnHeadersHeight = 70;

            dataGridView.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Padding = new Padding(3)
            };

            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
            dataGridView.DefaultCellStyle.Padding = new Padding(3);
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView.RowTemplate.Height = 25;

            dataGridView.CellDoubleClick += DataGridView_CellDoubleClick;

            ConfigureDataGridViewColumns();

            panel.Controls.Add(lblTitle);
            panel.Controls.Add(dataGridView);

            return panel;
        }

        private void ConfigureDataGridViewColumns()
        {
            dataGridView.AutoGenerateColumns = false;
            dataGridView.Columns.Clear();

            var columns = new[]
            {
                new { DataProperty = "FullName", Header = "ФИО", Width = 200 },
                new { DataProperty = "StatusName", Header = "Статус", Width = 120 },
                new { DataProperty = "DepartmentName", Header = "Отдел", Width = 150 },
                new { DataProperty = "PostName", Header = "Должность", Width = 150 },
                new { DataProperty = "DateEmploy", Header = "Дата приема", Width = 110 },
                new { DataProperty = "DateUnemploy", Header = "Дата увольнения", Width = 110 }
            };

            foreach (var col in columns)
            {
                var column = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = col.DataProperty,
                    HeaderText = col.Header,
                    Width = col.Width,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Alignment = DataGridViewContentAlignment.MiddleLeft,
                        Padding = new Padding(5, 0, 0, 0)
                    },
                    Name = col.DataProperty
                };

                dataGridView.Columns.Add(column);
            }

            SetColumnFormatSafe("DateEmploy", "dd.MM.yyyy");
            SetColumnFormatSafe("DateUnemploy", "dd.MM.yyyy");

            SetColumnNullValueSafe("DateEmploy", "—");
            SetColumnNullValueSafe("DateUnemploy", "—");
        }

        private void SetColumnFormatSafe(string columnName, string format)
        {
            if (dataGridView.Columns.Contains(columnName))
            {
                dataGridView.Columns[columnName].DefaultCellStyle.Format = format;
            }
        }

        private void SetColumnNullValueSafe(string columnName, object nullValue)
        {
            if (dataGridView.Columns.Contains(columnName))
            {
                dataGridView.Columns[columnName].DefaultCellStyle.NullValue = nullValue;
            }
        }

        private Panel CreateStatisticsPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            var controlsPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 170,
                BackColor = Color.Lavender,
                Padding = new Padding(15)
            };

            lblStatStatus = new Label
            {
                Text = "Статус:",
                Top = 15,
                Left = 10,
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };

            cmbStatStatus = new ComboBox
            {
                Top = 12,
                Left = 70,
                Width = 160,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };

            lblStartDate = new Label
            {
                Text = "С:",
                Top = 50,
                Left = 10,
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };

            dtpStartDate = new DateTimePicker
            {
                Top = 47,
                Left = 70,
                Width = 160,
                Value = DateTime.Now.AddMonths(-1),
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 9)
            };

            lblEndDate = new Label
            {
                Text = "По:",
                Top = 85,
                Left = 10,
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };

            dtpEndDate = new DateTimePicker
            {
                Top = 82,
                Left = 70,
                Width = 160,
                Value = DateTime.Now,
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 9)
            };

            rbEmployed = new RadioButton
            {
                Text = "Принятые",
                Top = 115,
                Left = 10,
                AutoSize = true,
                Checked = true,
                Font = new Font("Segoe UI", 9)
            };

            rbFired = new RadioButton
            {
                Text = "Уволенные",
                Top = 115,
                Left = 120,
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };

            btnShowStatistics = new Button
            {
                Text = "Показать Статистику",
                Top = 135,
                Left = 10,
                Width = 100,
                Height = 28,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Enabled = false
            };

            btnShowStatistics.Click += async (s, e) => await ShowStatisticsAsync();

            controlsPanel.Controls.AddRange(new Control[] {
                lblStatStatus, cmbStatStatus, lblStartDate, dtpStartDate,
                lblEndDate, dtpEndDate, rbEmployed, rbFired, btnShowStatistics
            });

            panel.Controls.Add(controlsPanel);
            return panel;
        }

        private StatusStrip CreateStatusStrip()
        {
            var statusStrip = new StatusStrip
            {
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9)
            };

            lblConnectionStatus = new ToolStripStatusLabel
            {
                Text = "Подключение: OK",
                Spring = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            lblRecordsCount = new ToolStripStatusLabel
            {
                Text = "Записей: 0",
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            statusStrip.Items.AddRange(new ToolStripItem[] {
                lblConnectionStatus, lblRecordsCount
            });

            return statusStrip;
        }

        private async void LoadReferenceDataAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                lblConnectionStatus.Text = "Загрузка справочников...";

                var statuses = await _repository.GetStatusesAsync();
                var departments = await _repository.GetDepartmentsAsync();
                var posts = await _repository.GetPostsAsync();

                cmbStatusFilter.DataSource = new List<Status> { new Status { Id = -1, Name = "Все" } }
                    .Concat(statuses).ToList();
                cmbStatusFilter.DisplayMember = "Name";
                cmbStatusFilter.ValueMember = "Id";

                cmbDepartmentFilter.DataSource = new List<Department> { new Department { Id = -1, Name = "Все" } }
                    .Concat(departments).ToList();
                cmbDepartmentFilter.DisplayMember = "Name";
                cmbDepartmentFilter.ValueMember = "Id";

                cmbPostFilter.DataSource = new List<Post> { new Post { Id = -1, Name = "Все" } }
                    .Concat(posts).ToList();
                cmbPostFilter.DisplayMember = "Name";
                cmbPostFilter.ValueMember = "Id";

                cmbStatStatus.DataSource = statuses;
                cmbStatStatus.DisplayMember = "Name";
                cmbStatStatus.ValueMember = "Id";

                btnShowStatistics.Enabled = true;

                await LoadEmployeesAsync();
            }
            catch (Exception ex)
            {
                lblConnectionStatus.Text = "Ошибка подключения";
                MessageBox.Show($"Ошибка загрузки справочников: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async Task LoadEmployeesAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                lblConnectionStatus.Text = "Загрузка данных...";

                var filter = new EmployeeFilter
                {
                    StatusId = (int)cmbStatusFilter.SelectedValue == -1 ? null : (int?)cmbStatusFilter.SelectedValue,
                    DepartmentId = (int)cmbDepartmentFilter.SelectedValue == -1 ? null : (int?)cmbDepartmentFilter.SelectedValue,
                    PostId = (int)cmbPostFilter.SelectedValue == -1 ? null : (int?)cmbPostFilter.SelectedValue,
                    LastNameFilter = txtLastNameFilter.Text
                };

                _persons = await _repository.GetEmployeesAsync(filter);

                dataGridView.DataSource = _persons;

                lblRecordsCount.Text = $"Записей: {_persons.Count}";
                lblConnectionStatus.Text = "Подключение: OK";
            }
            catch (Exception ex)
            {
                lblConnectionStatus.Text = "Ошибка подключения";
                MessageBox.Show($"Ошибка загрузки сотрудников: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async Task ShowStatisticsAsync()
        {
            try
            {
                if (cmbStatStatus.SelectedValue == null)
                {
                    MessageBox.Show("Выберите статус для статистики", "Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Cursor = Cursors.WaitCursor;
                lblConnectionStatus.Text = "Загрузка статистики...";

                var statistics = await _repository.GetStatisticsAsync(
                    (int)cmbStatStatus.SelectedValue,
                    dtpStartDate.Value.Date,
                    dtpEndDate.Value.Date.AddDays(1).AddSeconds(-1),
                    rbEmployed.Checked ? "employ" : "unemploy"
                );

                lblConnectionStatus.Text = "Подключение: OK";

                if (statistics == null || statistics.Count == 0)
                {
                    MessageBox.Show("Нет данных для отображения за выбранный период", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string message = "";
                    foreach (var item in statistics)
                    {
                        message += ($"День: {item.StatDate}, Колличество сотрудников: {item.EmployeeCount}\n");
                    }

                    MessageBox.Show(message, "Статистика");
                }
            }
            catch (Exception ex)
            {
                lblConnectionStatus.Text = "Ошибка загрузки статистики";
                MessageBox.Show($"Ошибка загрузки статистики: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }


        private void ClearFilters(object sender, EventArgs e)
        {
            cmbStatusFilter.SelectedIndex = 0;
            cmbDepartmentFilter.SelectedIndex = 0;
            cmbPostFilter.SelectedIndex = 0;
            txtLastNameFilter.Text = string.Empty;
        }

        private void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && _persons != null && e.RowIndex < _persons.Count)
            {
                var person = _persons[e.RowIndex];
                MessageBox.Show(
                    $"Детальная информация о сотруднике:\n\n" +
                    $"ФИО: {person.FullName}\n" +
                    $"Статус: {person.StatusName}\n" +
                    $"Отдел: {person.DepartmentName}\n" +
                    $"Должность: {person.PostName}\n" +
                    $"Дата приема: {person.DateEmploy?.ToString("dd.MM.yyyy") ?? "—"}\n" +
                    $"Дата увольнения: {person.DateUnemploy?.ToString("dd.MM.yyyy") ?? "—"}",
                    "Информация о сотруднике",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}